using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

[assembly: InternalsVisibleTo("AutoTraderTests")]
namespace AutoTrader
{
    public class AutoTraderLogic
    {
        public bool IsTradingActive { get; set; }

        private ILogicConnector _logicConnector;

        private float _availableInventoryCapacity;
        private int _availablePlayerGold;
        private int _availableMerchantGold;

        private List<string> _soldItems;
        private List<string> _boughtItems;

        public AutoTraderLogic(ILogicConnector logicConnector)
        {
            AutoTraderHelpers.PrintDebugMessage("####### AutoTrader Initialization #######");
            _logicConnector = logicConnector;
            
        }

        public void PerformAutoTrade(bool isCaravan = false)
        {
            AutoTraderHelpers.PrintDebugMessage("####### Performing AutoTrade #######");
            _logicConnector.IsCaravan = isCaravan;
            AutoTraderHelpers.PrintDebugMessage(" - isCaravan: " + isCaravan);
            int initialGold = _logicConnector.GetInitialGold();
            int troopWage = _logicConnector.GetTroopWage();
            _availablePlayerGold = initialGold - (AutoTraderConfig.KeepWagesValue * troopWage);
            AutoTraderHelpers.PrintDebugMessage(" - availablePlayerGold: " + _availablePlayerGold.ToString());

            _soldItems = new List<string>();
            _boughtItems = new List<string>();
            _availableMerchantGold = _logicConnector.GetMerchantGold();
            UpdateAvailableInventoryCapacity();

            // Set trading state
            IsTradingActive = true;
            try
            {
                if (!_logicConnector.InitInventory())
                {
                    AutoTraderHelpers.PrintDebugMessage("Failed to initialize the inventory!");
                    return;
                }

                AutoTraderHelpers.PrintDebugMessage("##### Restocking #####");
                BuyProcess(RestockFilter);
                BuyProcess(BuyFilter);
                Sell();
                BuyProcess(BuyFilter);
                //BuyHorses();
            } catch ( Exception e)
            {
                AutoTraderHelpers.PrintMessage("My Lord! Something terrible happened to our autotraders! The last we heard of them is:\n" + e.ToString());
            } finally
            {
                // Unset trading state
                IsTradingActive = false;
            }   
        }

        private void UpdateAvailableInventoryCapacity()
        {
            float currentWeight = _logicConnector.GetCurrentWeight();
            float inventoryCapacity = _logicConnector.GetInventoryCapacity();
           
            _availableInventoryCapacity = inventoryCapacity * ((float)AutoTraderConfig.UseInventorySpaceValue / 100.0f);
            _availableInventoryCapacity -= currentWeight;
            AutoTraderHelpers.PrintDebugMessage(" - actual availableInventoryCapacity: " + _availableInventoryCapacity.ToString());

            // Catch some cases where the user got a wrong configuration.
            if (AutoTraderConfig.UseMaxFleetCapacityValue && MobileParty.MainParty.Ships.Count <= 0)
            {
                AutoTraderHelpers.PrintMessage(new TextObject(
                    "{=ATNoFleet}My Lord! Your traders are at a loss, you specifically asked them to load the wares on our ships but we have no fleet! " +
                    "I instructed to load them on our carts but you may want to rethink your orders.").ToString()
                );
            }
        }

        private void Sell()
        {
            AutoTraderHelpers.PrintDebugMessage("##### Selling #####");
            _logicConnector.IsBuying = false;
            var itemSellList = new List<KeyValuePair<string, KeyValuePair<float, float>>>();

            foreach( string itemName in _logicConnector.GetPlayerItemRosterNames())
            {
                AutoTraderHelpers.PrintDebugMessage("-------------------------------------------------------------------");
                AutoTraderHelpers.PrintDebugMessage(" - current item: " + itemName);
                _logicConnector.SetCurrentElementByName(itemName);

                // Check if its filtered
                if (_logicConnector.IsItemFiltered(_boughtItems))
                {
                    AutoTraderHelpers.PrintDebugMessage(" - Item is filtered!");
                    continue;
                }

                int amount = _logicConnector.GetItemAmount();
                if (amount < 1)
                {
                    AutoTraderHelpers.PrintDebugMessage(" - skipping because amount is less than 1");
                    continue;
                }

                // TODO: Change on simple AI
                float averagePrice = GetAveragePrice();

                // Sell items one by one
                bool canSell = false;
                do
                {
                    int buyout_price = 0;
                    canSell = CanSell(averagePrice, amount, out buyout_price);
                    if (canSell)
                    {
                        // Update members
                        ProcessTransaction(buyout_price);
                        amount--;
                    }

                } while (canSell && amount > 0);
            }
        }

        private void BuyProcess(Func<bool> filterFunc)
        {
            AutoTraderHelpers.PrintDebugMessage("##### Executing buy process #####");

            _logicConnector.IsBuying = true;
            var itemBuyList = new List<KeyValuePair<string, KeyValuePair<float, float>>>();

            // Loop through all items of merchant
            for (int itemId = 0; itemId < _logicConnector.GetMerchantItemRosterSize(); itemId++)
            {
                AutoTraderHelpers.PrintDebugMessage("-------------------------------------------------------------------");
                AutoTraderHelpers.PrintDebugMessage(" - current item ID: " + itemId.ToString());
                _logicConnector.SetCurrentElementById(itemId);

                if (filterFunc())
                {
                    AutoTraderHelpers.PrintDebugMessage(" - skipping because item is filtered");
                    continue;
                }

                int buyoutPrice = _logicConnector.GetItemPrice();
                float averagePrice = GetAveragePrice();
                float profit = _logicConnector.GetProjectedProfit(buyoutPrice);
                AutoTraderHelpers.PrintDebugMessage(" - average price: " + averagePrice.ToString());
                AutoTraderHelpers.PrintDebugMessage(" - projected profit: " + profit.ToString());
                if (profit == buyoutPrice)
                    profit = averagePrice - (float)buyoutPrice;
                AutoTraderHelpers.PrintDebugMessage(" - final profit: " + profit.ToString());

                AutoTraderHelpers.PrintDebugMessage(" --> adding to buy list!");
                itemBuyList.Add(new KeyValuePair<string, KeyValuePair<float, float>>(_logicConnector.GetItemName(), new KeyValuePair<float, float>(averagePrice, profit)));
            }

            // Sort the list
            itemBuyList.Sort((pair1, pair2) => pair2.Value.Value.CompareTo(pair1.Value.Value));

            // Buy items in order of profit
            AutoTraderHelpers.PrintDebugMessage("### Actual buy loop ###");
            foreach (var element in itemBuyList)
            {
                AutoTraderHelpers.PrintDebugMessage("-------------------------------------------------------------------");

                string itemName = element.Key;
                float averagePrice = element.Value.Key;
                AutoTraderHelpers.PrintDebugMessage(" - current item Name: " + itemName.ToString());
                AutoTraderHelpers.PrintDebugMessage(" - averagePrice: " + averagePrice.ToString());

                _logicConnector.SetCurrentElementByName(itemName);                
                int amount = _logicConnector.GetItemAmount();
                int ownAmount = _logicConnector.GetItemAmountInPlayerRoster();

                // Buy items one by one
                bool canBuy = false;
                do
                {
                    int buyout_price = 0;
                    canBuy = CanBuy(averagePrice, amount, ownAmount, out buyout_price);

                    if (canBuy)
                    {
                        // Update members
                        ProcessTransaction(buyout_price);
                        amount--;
                        ownAmount++;
                    }

                } while (canBuy && amount > 0);
            }
        }

        private bool RestockFilter()
        {
            // Returns true if filtered
            if (!AutoTraderConfig.ResupplyValue)
            {
                AutoTraderHelpers.PrintDebugMessage("- Skipping resupply due to config");
                return true;
            }
            if (_logicConnector.GetItemAmount() <= 0)
            {
                AutoTraderHelpers.PrintDebugMessage(" - skipping because out of stock");
                return true;
            }
            if (!_logicConnector.IsConsumable())
            {
                AutoTraderHelpers.PrintDebugMessage(" - skipping because it is not a consumable");
                return true;
            }
            if (AutoTraderSpecialRules.CheckBuyResupplyRule(_logicConnector, _logicConnector.GetItemAmount()))
            {
                AutoTraderHelpers.PrintDebugMessage(" - skipping because of the resupply rule");
                return true; ;
            }
            return false;
        }

        private bool BuyFilter()
        {
            var result = _logicConnector.IsItemFiltered(_soldItems);
            if(result)
                AutoTraderHelpers.PrintDebugMessage(" - skipping because item is filtered");
            return result;
        }

        //private bool BuyHorseFilter()
        //{
        //    if (!_logicConnector.IsHorse())
        //    {
        //        AutoTraderHelpers.PrintDebugMessage(" - skipping because item is not horse");
        //        continue;
        //    }
        //}

        //private void BuyHorses()
        //{
        //    AutoTraderHelpers.PrintDebugMessage("### Buying Horses ###");
        //    _logicConnector.IsBuying = true;

        //    // Loop through items
        //    for (int itemId = 0; itemId < _logicConnector.GetMerchantItemRosterSize(); itemId++)
        //    {
        //        AutoTraderHelpers.PrintDebugMessage("-------------------------------------------------------------------");
        //        AutoTraderHelpers.PrintDebugMessage(" - current item ID: " + itemId.ToString());
        //        _logicConnector.SetCurrentElementById(itemId);
        //        // Check if its filtered
        //        if (!_logicConnector.IsHorse())
        //        {
        //            AutoTraderHelpers.PrintDebugMessage(" - skipping because item is not horse");
        //            continue;
        //        }

        //        int amount = _logicConnector.GetItemAmount();
        //        if (amount < 1)
        //        {
        //            AutoTraderHelpers.PrintDebugMessage(" - skipping because amount is less than 1");
        //            continue;
        //        }

        //        float averagePrice = GetAveragePrice();
        //        int buyoutPrice = 0;

        //        bool canBuy = false;
        //        do
        //        {
        //            canBuy = CanBuy(averagePrice, 1, out buyoutPrice);
        //            if (canBuy)
        //            {
        //                ProcessTransaction(buyoutPrice);
        //                amount -= 1;
        //            }
        //        } while (canBuy && amount > 0);
        //    }
        //}

        internal bool SimpleWorthCheck(int value, int buyoutPrice)
        {
            AutoTraderHelpers.PrintDebugMessage("### Simple Worth Check ###");
            /// Checks if the value is below or above threshold 
            var result = false;
            if (!_logicConnector.IsBuying && buyoutPrice >= ((float)AutoTraderConfig.SellThresholdValue / 100.0f) * (float)value)
            {
                result = true;
            }
            else if (_logicConnector.IsBuying && buyoutPrice < ((float)AutoTraderConfig.BuyThresholdValue / 100.0f) * (float)value)
            {
                result = true;
            }
            AutoTraderHelpers.PrintDebugMessage("- isItemWorth: " + result.ToString());
            return result;
        }

        private void ProcessTransaction(int buyoutPrice)
        {
            AutoTraderHelpers.PrintDebugMessage("### Processing transaction ###");
            // Update available gold
            _availablePlayerGold += _logicConnector.IsBuying ? -buyoutPrice : buyoutPrice;
            _availableMerchantGold += _logicConnector.IsBuying ? buyoutPrice : -buyoutPrice;

            // Mark the item
            string name = _logicConnector.GetItemName();
            if (_logicConnector.IsBuying)
            {
                if (!_boughtItems.Exists(x => x == name))
                    _boughtItems.Add(name);
            } else
            {
                if (!_soldItems.Exists(x => x == name))
                    _soldItems.Add(name);
            }

            // Update available weight
            UpdateAvailableInventoryCapacity();

            _logicConnector.TransferItem();
        }

        private bool CanBuy(float averagePrice, int amount, int ownAmount, out int buyoutPrice)
        {
            AutoTraderHelpers.PrintDebugMessage("### Can buy check ###");
            // TODO: Refactor, too many return statements

            // Retrieve price
            buyoutPrice = _logicConnector.GetCostOfRosterElement();

            // Special Rules
            // Horses
            if (_logicConnector.IsHorse())
            {
                if (AutoTraderSpecialRules.CheckBuyHorsesRules(_logicConnector, buyoutPrice, _availablePlayerGold))
                    return CheckBasicBuyRequirements(amount, buyoutPrice);
                AutoTraderHelpers.PrintDebugMessage(" - do not buy because we need no additional horses");
                return false; // buy no other horses
            }

            // Hardwood
            if (AutoTraderSpecialRules.CheckBuyResupplyHardwoodRule(_logicConnector, amount))
            {
                AutoTraderHelpers.PrintDebugMessage("- buying hardwood to resupply");
                return CheckBasicBuyRequirements(amount, buyoutPrice); ;
            }

            // Consumables
            if (_logicConnector.IsConsumable())
            {
                int maxAmount = _logicConnector.IsItemGrain() ? AutoTraderConfig.KeepGrainsMaxValue : AutoTraderConfig.KeepConsumablesMaxValue;

                if (ownAmount >= maxAmount)
                {
                    AutoTraderHelpers.PrintDebugMessage(" --> not buying because we have enough");
                    return false;
                }
                if (AutoTraderSpecialRules.CheckBuyConsumablesRules(_logicConnector, ownAmount))
                    return CheckBasicBuyRequirements(amount, buyoutPrice);

            }

            // Price niveau
            if (AutoTraderConfig.SimpleTradingAI)
            {
                // Case: buying
                float averagePriceFactorItemCategory = _logicConnector.GetAveragePriceFactorItemCategory();
                AutoTraderHelpers.PrintDebugMessage(" - average price factor item category: " + averagePriceFactorItemCategory.ToString());

                if (averagePriceFactorItemCategory != -99.0)
                {
                    float priceFactor = _logicConnector.GetCurrentTownPriceFactor();
                    AutoTraderHelpers.PrintDebugMessage(" - price factor: " + priceFactor.ToString());
                    if (priceFactor > averagePriceFactorItemCategory * 0.8f)
                    {
                        AutoTraderHelpers.PrintDebugMessage(" - do not buy because the price factor is too bad (" + priceFactor.ToString() + " > " + (averagePriceFactorItemCategory * 0.8f).ToString() + ")");
                        return false;
                    }
                        
                } else {
                    AutoTraderHelpers.PrintDebugMessage("- no average price found for " + _logicConnector.GetItemName());
                    return false;
                }
            }
            else
            {
                // Check threshold
                float priceFactor = (float)buyoutPrice / averagePrice;
                AutoTraderHelpers.PrintDebugMessage(" - price factor: " + priceFactor.ToString());
                if (priceFactor > (float)AutoTraderConfig.BuyThresholdValue / 100.0f)
                {
                    AutoTraderHelpers.PrintDebugMessage(" - do not buy because priceFactor is above threshold (" + priceFactor.ToString() + " > " + ((float)AutoTraderConfig.BuyThresholdValue / 100.0f).ToString() + ")");
                    return false;
                }
            }

            // Specials rules after price check
            // Check if we have enough cattle
            if (AutoTraderSpecialRules.CheckBuyCattleCondition(_logicConnector))
            {
                if (AutoTraderSpecialRules.CheckBuyCattleRule(_logicConnector))
                    return CheckBasicBuyRequirements(amount, buyoutPrice);
            }

            // Check weight
            if (AutoTraderSpecialRules.CheckBuyMaxCapacityRule(_logicConnector, (int)_logicConnector.GetInventoryCapacity(), ownAmount))
                return CheckBasicBuyRequirements(amount, buyoutPrice);
            else
            {
                AutoTraderHelpers.PrintDebugMessage("- do not buy because we are at capacity");
                return false;
            }
        }

        private bool CanSell(float averagePrice, int amount, out int buyoutPrice)
        {
            AutoTraderHelpers.PrintDebugMessage("### Can sell check ###");
            // Retrieve price
            buyoutPrice = _logicConnector.GetCostOfRosterElement();

            // Sell all Armor and Weapons
            if (_logicConnector.IsArmor())
            {
                if (_logicConnector.IsItemTierLowerThan((ItemObject.ItemTiers)AutoTraderConfig.WeaponsArmorTierValue))
                {
                    return CheckBasicSellRequirements(amount, buyoutPrice);
                }
                else
                {
                    AutoTraderHelpers.PrintDebugMessage("- do not sell because tier is too high");
                    return false;
                }
            } else if (_logicConnector.IsWeapon())
            {
                // Keep handmade weapons
                if (AutoTraderConfig.KeepSmeltingValue && !_logicConnector.IsWeaponDesignEmpty())
                {
                    AutoTraderHelpers.PrintDebugMessage("- do not sell because its crafted");
                    return false;
                }

                if (_logicConnector.IsItemTierLowerThan((ItemObject.ItemTiers)AutoTraderConfig.WeaponsArmorTierValue))
                {
                    return CheckBasicSellRequirements(amount, buyoutPrice);
                }
                else
                {
                    AutoTraderHelpers.PrintDebugMessage("- do not sell because tier is too high");
                    return false;
                }
            }
                
            // Special horse rule
            if (_logicConnector.IsHorse())
            {
                if (_logicConnector.IsPackAnimal() && AutoTraderConfig.SellHorsesValue)
                {
                    AutoTraderHelpers.PrintDebugMessage("- do not sell because its a pack animal");
                    return false;
                }
            }

            // Check amounts to keep
            if (_logicConnector.IsConsumable())
            {
                int maxAmountToKeep = _logicConnector.IsItemGrain() ?
                    AutoTraderConfig.KeepGrainsMaxValue : AutoTraderConfig.KeepConsumablesMaxValue;
                int minAmountToKeep = _logicConnector.IsItemGrain() ?
                    AutoTraderConfig.KeepGrainsMinValue : AutoTraderConfig.KeepConsumablesMinValue;

                if (minAmountToKeep > amount)
                {
                    AutoTraderHelpers.PrintDebugMessage("- do not sell because we dont have enough of this consumable");
                    return false;
                }
                if (maxAmountToKeep < amount)
                {
                    AutoTraderHelpers.PrintDebugMessage("- selling because we have too much of this consumable");
                    return CheckBasicSellRequirements(amount, buyoutPrice);
                }
            }

            // Livestock
            if (_logicConnector.IsLivestock())
            {
                // Sell if its treated as junk
                if (AutoTraderConfig.JunkCattleValue)
                    return CheckBasicSellRequirements(amount, buyoutPrice);
            }

            // Special hardwood rule
            if (AutoTraderSpecialRules.CheckBuyResupplyHardwoodRule(_logicConnector, amount))
            {
                AutoTraderHelpers.PrintDebugMessage("- do not sell because we dont have enough hardwood");
                return false;                
            }

            if (AutoTraderConfig.SimpleTradingAI)
            {
                int weighted_profit = buyoutPrice - _logicConnector.GetProjectedProfit(buyoutPrice);
                AutoTraderHelpers.PrintDebugMessage(" - weighted_profit: " + weighted_profit.ToString());
                // Check case of no trade rumours
                if (weighted_profit == 0)
                {
                    AutoTraderHelpers.PrintDebugMessage(" - no trade rumours!");
                    float priceFactor = (float)buyoutPrice / averagePrice;
                    if (priceFactor <= 1.2f)
                    {
                        AutoTraderHelpers.PrintDebugMessage("- do not sell because price factor is too low: " + priceFactor.ToString());
                        return false;
                    }

                }
                else if (weighted_profit > buyoutPrice * 0.8f)
                {
                    AutoTraderHelpers.PrintDebugMessage("- do not sell because weighted profit (" + weighted_profit.ToString() + ") is higher than " + (buyoutPrice * 0.8f).ToString());
                    return false;
                }
                    
            }
            else
            {
                // Check threshold
                float priceFactor = (float)buyoutPrice / averagePrice;
                if (priceFactor < (float)AutoTraderConfig.SellThresholdValue / 100.0f)
                {
                    AutoTraderHelpers.PrintDebugMessage("- do not sell because price factor is less than sell threshold: " + priceFactor.ToString());
                    return false;
                }
                    
            }
            
            return CheckBasicSellRequirements(amount, buyoutPrice);
        }

        private bool CheckBasicBuyRequirements(int amount, int price)
        {
            AutoTraderHelpers.PrintDebugMessage("### Checking basic buy requirements ###");
            if (price >= _availablePlayerGold)
            {
                AutoTraderHelpers.PrintDebugMessage("- do not buy because not enough gold: " + price.ToString() + " >= " + _availablePlayerGold.ToString());
                return false;
            }

            if (amount <= 0)
            {
                AutoTraderHelpers.PrintDebugMessage("- do not buy because not enough stock");
                return false;
            }


            // ToDo: Only for carry horses? Nop, also for livestock.
            if (!_logicConnector.IsHorse() && !_logicConnector.IsLivestock() && _logicConnector.GetItemWeight() > _availableInventoryCapacity)
            {
                AutoTraderHelpers.PrintDebugMessage("- do not buy because not enough capacity: " + _logicConnector.GetItemWeight().ToString() + " > " + _availableInventoryCapacity.ToString());
                return false;
            }

            AutoTraderHelpers.PrintDebugMessage("- fulfills basic buy requirements");
            return true;
        }

        private bool CheckBasicSellRequirements(int amount, int price)
        {
            AutoTraderHelpers.PrintDebugMessage("### Checking basic sell requirements ###");
            if (price >= _availableMerchantGold)
            {
                AutoTraderHelpers.PrintDebugMessage(" - not selling because not enough merchant gold");
                return false;
            }
                

            if (amount <= 0)
            {
                AutoTraderHelpers.PrintDebugMessage(" - not selling because not enough stock");
                return false;
            }

            AutoTraderHelpers.PrintDebugMessage("- fulfills basic sell requirements");
            return true;
        }

        private float GetAveragePrice()
        {
            AutoTraderHelpers.PrintDebugMessage("### Getting average price ###");
            float averagePrice = 0;
            float actualDistance = 0;
            float count = 0.0f;

            for (int townId = 0; townId < _logicConnector.GetTownListSize(); townId++)
            {
                bool isInRange = _logicConnector.IsTownInRange(townId, out actualDistance);
                if (AutoTraderConfig.UseWeightedValue // Consider weighted value
                    || AutoTraderConfig.SearchRadiusValue > 999 // Consider the maximum setting
                    || isInRange)
                {
                    if (AutoTraderConfig.UseWeightedValue)
                    {
                        // If its the current town, skip
                        if (_logicConnector.IsCurrentTown(townId))
                        {
                            AutoTraderHelpers.PrintDebugMessage("- continue because current town");
                            continue;
                        }

                        // Weight by distance
                        try
                        {
                            averagePrice += _logicConnector.GetTownItemPrice(townId, true) / actualDistance;
                            averagePrice += _logicConnector.GetTownItemPrice(townId, false) / actualDistance;
                            count += 2.0f / actualDistance;
                        }
                        catch (Exception e)
                        {
                            AutoTraderHelpers.PrintDebugMessage("ERROR: Could not retrieve average price for town: " + e.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            averagePrice += _logicConnector.GetTownItemPrice(townId, true);
                            averagePrice += _logicConnector.GetTownItemPrice(townId, false);
                            count += 2.0f;
                        }
                        catch (Exception e)
                        {
                            AutoTraderHelpers.PrintDebugMessage("ERROR: Could not retrieve average price for town: " + e.ToString());
                        }

                    }
                }
            }

            // ToDo: Why the restriction?
            if (_logicConnector.IsItemTradeGood())
            {
                for (int villageId = 0; villageId < _logicConnector.GetVillageListSize(); villageId++)
                {
                    bool isInRange = _logicConnector.IsVillageInRange(villageId, out actualDistance);
                    if (AutoTraderConfig.UseWeightedValue // Consider weighted value
                        || AutoTraderConfig.SearchRadiusValue > 999 // Consider the maximum setting
                        || isInRange)
                    {
                        if (AutoTraderConfig.UseWeightedValue)
                        {
                            // If its the current town, skip
                            if (_logicConnector.IsCurrentVillage(villageId))
                                continue;
                            // Weight by distance
                            try
                            {
                                averagePrice += _logicConnector.GetVillageItemPrice(villageId, true) / actualDistance;
                                averagePrice += _logicConnector.GetVillageItemPrice(villageId, false) / actualDistance;
                                count += 2.0f / actualDistance;
                            }
                            catch (Exception e)
                            {
                                AutoTraderHelpers.PrintDebugMessage("ERROR: Could not retrieve average price for village: " + e.ToString());
                            }
                        }
                        else
                        {
                            try
                            {
                                averagePrice += _logicConnector.GetVillageItemPrice(villageId, true);
                                averagePrice += _logicConnector.GetVillageItemPrice(villageId, false);
                                count += 2.0f;
                            }
                            catch (Exception e)
                            {
                                AutoTraderHelpers.PrintDebugMessage("ERROR: Could not retrieve average price for village: " + e.ToString());
                            }
                        }
                    }
                }
            }

            if (count == 0.0f)
                return _logicConnector.GetAveragePriceFallback();

            averagePrice /= count;
            AutoTraderHelpers.PrintDebugMessage(" - average price: " + averagePrice.ToString());
            return averagePrice > 0 ? averagePrice : _logicConnector.GetAveragePriceFallback();
        }

    }
}
