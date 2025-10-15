using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace AutoTrader
{
    class AutoTraderLogicConnector: ILogicConnector
    {

        public bool _isCaravan = false;
        public bool _isBuying = false;

        private ItemRosterElement _currentItemRosterElement;

        bool ILogicConnector.IsCaravan { get { return _isCaravan; } set { _isCaravan = value; } }
        bool ILogicConnector.IsBuying { get { return _isBuying; } set { _isBuying = value; } }

        public enum MerchantType
        {
            Town,
            Village,
            Caravan
        }

        public void SetCurrentElementById(int itemId)
        {
            _currentItemRosterElement = GetItemRoster()[itemId];
            AutoTraderHelpers.PrintDebugMessage(" - Current Item: " + _currentItemRosterElement.EquipmentElement.Item.Name.ToString());
        }

        public int GetInitialGold()
        {
            // TODO: Use "MobileParty" instead since 1.3?
            AutoTraderHelpers.PrintDebugMessage(" - InitialGold: " + PartyBase.MainParty.Owner.Gold.ToString());
            return PartyBase.MainParty.Owner.Gold;
        }
        public int GetTroopWage()
        {
            // ToDo: Whole daily wage
            // TODO: Use "MobileParty" instead since 1.3?
            AutoTraderHelpers.PrintDebugMessage(" - TroopWage: " + PartyBase.MainParty.MobileParty.TotalWage.ToString());
            return PartyBase.MainParty.MobileParty.TotalWage;
        }
        public float GetCurrentWeight()
        {
            AutoTraderHelpers.PrintDebugMessage(" - CurrentWeight: " + MobileParty.MainParty.TotalWeightCarried.ToString());
            return MobileParty.MainParty.TotalWeightCarried;
        }
        public float GetInventoryCapacity()
        {
            AutoTraderHelpers.PrintDebugMessage(" - InventoryCapacity: " + MobileParty.MainParty.InventoryCapacity.ToString());
            return MobileParty.MainParty.InventoryCapacity;
        }
        public int GetPlayerItemRosterSize()
        {
            // TODO: Use "MobileParty" instead since 1.3?
            AutoTraderHelpers.PrintDebugMessage(" - PartyItemRosterSize: " + PartyBase.MainParty.ItemRoster.Count.ToString());
            return PartyBase.MainParty.ItemRoster.Count;
        }
        public int GetNumPartyMembers()
        {
            // TODO: Use "MobileParty" instead since 1.3?
            AutoTraderHelpers.PrintDebugMessage(" - NumPartyMembers: " + PartyBase.MainParty.NumberOfAllMembers.ToString());
            return PartyBase.MainParty.NumberOfAllMembers;
        }
        public int GetNumLivestockAnimals()
        {
            // TODO: Use "MobileParty" instead since 1.3?
            AutoTraderHelpers.PrintDebugMessage(" - NumLivestockAnimals: " + PartyBase.MainParty.ItemRoster.NumberOfLivestockAnimals.ToString());
            return PartyBase.MainParty.ItemRoster.NumberOfLivestockAnimals;
        }
        public int GetMerchantItemRosterSize()
        {
            if (_isCaravan)
            {
                AutoTraderHelpers.PrintDebugMessage(" - MerchantItemRosterSize (Caravan): " + MobileParty.ConversationParty.ItemRoster.Count.ToString());
                return MobileParty.ConversationParty.ItemRoster.Count;
            }
            AutoTraderHelpers.PrintDebugMessage(" - MerchantItemRosterSize (Town): " + Settlement.CurrentSettlement.ItemRoster.Count.ToString());
            return Settlement.CurrentSettlement.ItemRoster.Count;
        }
        public List<string> GetLocks()
        {
            var locksEnumerable = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetInventoryLocks();
            if (locksEnumerable != null)
            {
                return locksEnumerable.ToList<string>();
            }
            return new List<string>();
        }

        public bool IsItemLocked()
        {
            var locks = GetLocks();
            var itemStringId = _currentItemRosterElement.EquipmentElement.Item.StringId;
            if (_currentItemRosterElement.EquipmentElement.ItemModifier != null)
            {
                itemStringId += _currentItemRosterElement.EquipmentElement.ItemModifier.StringId;
            }
            AutoTraderHelpers.PrintDebugMessage(" - IsLocked: " + locks.Contains(itemStringId).ToString());
            return locks.Contains(itemStringId);
        }
        public bool IsItemTradeGood()
        {
            AutoTraderHelpers.PrintDebugMessage(" - IsTradeGood: " + _currentItemRosterElement.EquipmentElement.Item.IsTradeGood.ToString());
            return _currentItemRosterElement.EquipmentElement.Item.IsTradeGood;
        }
        public int GetItemAmount()
        {
            try
            {
                AutoTraderHelpers.PrintDebugMessage(" - ItemAmount: " + _currentItemRosterElement.Amount.ToString());
                return _currentItemRosterElement.Amount;
            }
            catch (Exception e)
            {
                AutoTraderHelpers.PrintMessage("GetItemAmount crashed: " + e.ToString());
                return 0;
            }
        }
        public string GetItemName()
        {
            AutoTraderHelpers.PrintDebugMessage(" - ItemName: " + _currentItemRosterElement.EquipmentElement.Item.Name.ToString());
            return _currentItemRosterElement.EquipmentElement.Item.Name.ToString();
        }
        public float GetItemWeight()
        {
            AutoTraderHelpers.PrintDebugMessage(" - ItemWeight: " + _currentItemRosterElement.EquipmentElement.Item.Weight.ToString());
            return _currentItemRosterElement.EquipmentElement.Item.Weight;
        }

        public bool IsWeaponDesignEmpty()
        {
            var result = _currentItemRosterElement.EquipmentElement.Item.WeaponDesign == null;
            AutoTraderHelpers.PrintDebugMessage(" - IsWeaponDesignEmpty: " + result.ToString());
            return result;
        }
        public bool IsPackAnimal()
        {
            var result = _currentItemRosterElement.EquipmentElement.Item.HorseComponent.IsPackAnimal;
            AutoTraderHelpers.PrintDebugMessage(" - IsPackAnimal: " + result.ToString());
            return result;
        }
        public bool IsItemGrain()
        {
            var result = _currentItemRosterElement.EquipmentElement.Item == DefaultItems.Grain;
            AutoTraderHelpers.PrintDebugMessage(" - IsItemGrain: " + result.ToString());
            return result;
        }
        public bool IsItemHardwood()
        {
            var result = _currentItemRosterElement.EquipmentElement.Item == DefaultItems.HardWood;
            AutoTraderHelpers.PrintDebugMessage(" - IsItemHardwood: " + result.ToString());
            return result;
        }
        public int GetPartyHardwoodIndex()
        {
            var result = PartyBase.MainParty.ItemRoster.FindIndexOfItem(DefaultItems.HardWood);
            AutoTraderHelpers.PrintDebugMessage(" - HardwoodIndex: " + result.ToString());
            return result;
        }
        public float GetRosterElementWeight()
        {
            var result = _currentItemRosterElement.GetRosterElementWeight();
            if (!_isBuying)
                AutoTraderHelpers.PrintDebugMessage("GetRosterElementWeigth: Not buying!");
            AutoTraderHelpers.PrintDebugMessage(" - RosterElementWeight: " + result.ToString());
            return result;
        }
        private MerchantType GetMerchantType()
        {
            MerchantType merchantType;
            if (_isCaravan)
            {
                merchantType = MerchantType.Caravan;

                // Make sure its opened through conversation
                if (MobileParty.ConversationParty == null)
                {
                    AutoTraderHelpers.PrintDebugMessage("Caravan trading but not through a conversation!");
                }
            }
            else
                merchantType = Settlement.CurrentSettlement.IsTown ? MerchantType.Town : MerchantType.Village;
            return merchantType;
        }

        public bool InitInventory()
        {
            var merchantType = GetMerchantType();
            if (merchantType == MerchantType.Town)
            {
                InventoryScreenHelper.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.Town,
                    InventoryScreenHelper.InventoryCategoryType.None, null);
                return true;
            }
            else if (merchantType == MerchantType.Village)
            {
                InventoryScreenHelper.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.Village, InventoryScreenHelper.InventoryCategoryType.None, null);
                return true;
            }
            else if (merchantType == MerchantType.Caravan)
            {
                InventoryScreenHelper.OpenTradeWithCaravanOrAlleyParty(MobileParty.ConversationParty, InventoryScreenHelper.InventoryCategoryType.None);
                return true;
            }
            return false;
        }

        public int GetMerchantGold()
        {
            var result = 0;
            var merchantType = GetMerchantType();
            switch (merchantType)
            {
                case MerchantType.Town:
                    result = Settlement.CurrentSettlement.Town.Gold;
                    break;
                case MerchantType.Village:
                    result = Settlement.CurrentSettlement.Village.Gold;
                    break;
                case MerchantType.Caravan:
                    result = MobileParty.ConversationParty.PartyTradeGold;
                    break;
            }
            AutoTraderHelpers.PrintDebugMessage(" - MerchantGold: " + result.ToString());
            return result;
        }

        private ItemRoster GetItemRoster()
        {
            if (_isBuying)
            {
                if (GetMerchantType() == MerchantType.Caravan)
                    return MobileParty.ConversationParty.ItemRoster;
                return Settlement.CurrentSettlement.ItemRoster;
                
            } else
                return PartyBase.MainParty.ItemRoster;
        }

        public bool IsItemTierLowerThan(ItemObject.ItemTiers tier)
        {
            var result = _currentItemRosterElement.EquipmentElement.Item.Tier < tier;
            AutoTraderHelpers.PrintDebugMessage(" - IsItemTierLowerThan: " + result.ToString());
            return result;
        }

        public bool IsItemFiltered(List<string> doneItems=null)
        {
            var itemRosterElement = _currentItemRosterElement;
            ItemObject itemObject = itemRosterElement.EquipmentElement.Item;

            // Filter by amount
            if (_currentItemRosterElement.Amount <=0)
            {
                AutoTraderHelpers.PrintDebugMessage(" - filtered because out of stock");
                return true;
            }
            // Filter by lock
            if (IsItemLocked())
                return true;

            // Check if already bought / sold
            if (doneItems != null && doneItems.Exists(x => x == itemRosterElement.EquipmentElement.Item.Name.ToString()))
                return true;

            // Exclude horses when buying for now
            if (AutoTraderHelpers.IsHorse(itemObject) && _isBuying)
                return true;

            // Filter by type
            if (!_isBuying && AutoTraderHelpers.IsSmithingMaterial(itemObject))
            {
                AutoTraderHelpers.PrintDebugMessage(" - is smithing material");
                return AutoTraderConfig.SellSmithingValue ? false : true;
            }   
            if (AutoTraderHelpers.IsHorse(itemObject) && !(_isBuying ? AutoTraderConfig.BuyHorsesValue : AutoTraderConfig.SellHorsesValue))
                return true;
            if (AutoTraderHelpers.IsArmor(itemObject) && !(_isBuying ? AutoTraderConfig.BuyArmorValue : AutoTraderConfig.SellArmorValue))
                return true;
            if (AutoTraderHelpers.IsWeapon(itemObject) && !(_isBuying ? AutoTraderConfig.BuyWeaponsValue : AutoTraderConfig.SellWeaponsValue))
                return true;
            if (AutoTraderHelpers.IsLivestock(itemObject) && !(_isBuying ? AutoTraderConfig.BuyLivestockValue : AutoTraderConfig.SellLivestockValue))
                return true;
            if (AutoTraderHelpers.IsTradeGood(itemObject) && !(_isBuying ? AutoTraderConfig.BuyGoodsValue : AutoTraderConfig.SellGoodsValue))
            {
                if (!AutoTraderHelpers.IsConsumable(itemObject))
                    return true;
            }
            if (AutoTraderHelpers.IsConsumable(itemObject) && !(_isBuying ? AutoTraderConfig.BuyConsumablesValue : AutoTraderConfig.SellConsumablesValue))
                return true;

            return false;
        }

        public void TransferItem()
        {
            // Generate command
            TransferCommand transferCommand = TransferCommand.Transfer(1,
                _isBuying ? InventoryLogic.InventorySide.OtherInventory : InventoryLogic.InventorySide.PlayerInventory,
                _isBuying ? InventoryLogic.InventorySide.PlayerInventory : InventoryLogic.InventorySide.OtherInventory,
                _currentItemRosterElement, EquipmentIndex.None, EquipmentIndex.None, CharacterObject.PlayerCharacter);
            InventoryScreenHelper.GetActiveInventoryState().InventoryLogic.AddTransferCommand(transferCommand);
            AutoTraderHelpers.PrintDebugMessage(" - Transfer of item " + GetItemName() + " complete! (" + (_isBuying? "Buy" : "Sell") + ")");
        }

        public int GetProjectedProfit(int buyoutPrice)
        {
            IPlayerTradeBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IPlayerTradeBehavior>();
            var result = campaignBehavior.GetProjectedProfit(_currentItemRosterElement, buyoutPrice);
            AutoTraderHelpers.PrintDebugMessage(" - ProjectedProfit: " + result.ToString());
            return result;
        }

        public int GetItemPrice()
        {
            var result = InventoryScreenHelper.GetActiveInventoryState().InventoryLogic.GetItemPrice(_currentItemRosterElement.EquipmentElement, _isBuying);
            AutoTraderHelpers.PrintDebugMessage(" - ItemPrice: " + result.ToString());
            return result;
        }

        public float GetAveragePriceFallback()
        {
            var result = _currentItemRosterElement.EquipmentElement.Item.Value;
            AutoTraderHelpers.PrintDebugMessage(" - AveragePriceFallback: " + result.ToString());
            return result;
        }

        public int GetCostOfRosterElement()
        {
            var result = InventoryScreenHelper.GetActiveInventoryState().InventoryLogic.GetCostOfItemRosterElement(_currentItemRosterElement, _isBuying ? InventoryLogic.InventorySide.OtherInventory : InventoryLogic.InventorySide.PlayerInventory);
            AutoTraderHelpers.PrintDebugMessage(" - CostOfElement: " + result.ToString());
            return result;
        }
        public float GetAveragePriceFactorItemCategory()
        {
            var result = InventoryScreenHelper.GetActiveInventoryState().InventoryLogic.GetAveragePriceFactorItemCategory(_currentItemRosterElement.EquipmentElement.Item.ItemCategory);
            AutoTraderHelpers.PrintDebugMessage(" - AveragePriceFactorItemCategory: " + result.ToString());
            return result;
        }
        
        /// Towns
        public int GetTownListSize()
        {
            var result = Town.AllTowns.Count();
            return result;
        }

        private Town GetTownById(int townId)
        {
            return Town.AllTowns.ElementAt(townId);
        }

        public bool IsTownInRange(int townId, out float actualDistance)
        {
            var town = GetTownById(townId);
            float estimatedLandRatio;
            //TODO: take into consideration naval distance
            actualDistance = Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, town.Settlement, false, MobileParty.NavigationType.Default, out estimatedLandRatio);
            return actualDistance < (float)AutoTraderConfig.SearchRadiusValue;
        }
        public bool IsCurrentTown(int townId)
        {
            if (_isCaravan)
                return false;
            var town = GetTownById(townId);
            var result = Settlement.CurrentSettlement.IsTown && town == Settlement.CurrentSettlement.Town;
            AutoTraderHelpers.PrintDebugMessage(" - IsCurrentTown: " + result.ToString());
            return result;
        }
        public float GetTownItemPrice(int townId, bool isSelling)
        {
            var town = GetTownById(townId);
            var result = town.MarketData.GetPrice(_currentItemRosterElement.EquipmentElement.Item, PartyBase.MainParty.MobileParty, isSelling);
            return result;
        }
        public float GetCurrentTownPriceFactor()
        {
            Town town = Settlement.CurrentSettlement.IsVillage ? Settlement.CurrentSettlement.Village.Bound.Town : Settlement.CurrentSettlement.Town;
            var result = town.MarketData.GetPriceFactor(_currentItemRosterElement.EquipmentElement.Item.ItemCategory);
            AutoTraderHelpers.PrintDebugMessage(" - CurrentTownPriceFactor: " + result.ToString());
            return result;
        }

        /// Villages
        public int GetVillageListSize()
        {
            var result = Village.All.Count();
            return result;
        }

        private Village GetVillageById(int villageId)
        {
            return Village.All.ElementAt(villageId);
        }

        public bool IsVillageInRange(int villageId, out float actualDistance)
        {
            var village = GetVillageById(villageId);
            float estimatedLandRatio;

            // TODO: take into consideration naval distance
            actualDistance = Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, village.Settlement, false, MobileParty.NavigationType.Default, out estimatedLandRatio);
            return actualDistance < (float)AutoTraderConfig.SearchRadiusValue; ;
        }
        public bool IsCurrentVillage(int townId)
        {
            if (_isCaravan)
                return false;
            var village = GetVillageById(townId);
            var result = Settlement.CurrentSettlement.IsVillage && village == Settlement.CurrentSettlement.Village;
            AutoTraderHelpers.PrintDebugMessage(" - IsCurrentVillage: " + result.ToString());
            return result;
        }
        public float GetVillageItemPrice(int townId, bool isSelling)
        {
            var town = GetVillageById(townId);
            var result = town.MarketData.GetPrice(_currentItemRosterElement.EquipmentElement.Item, PartyBase.MainParty.MobileParty, isSelling, null);
            return result;
        }

        /// Helper Wrapper
        public bool IsArmor()
        {
            var itemRosterElement = _currentItemRosterElement;
            return AutoTraderHelpers.IsArmor(itemRosterElement.EquipmentElement.Item);
        }
        public bool IsWeapon()
        {
            var itemRosterElement = _currentItemRosterElement;
            return AutoTraderHelpers.IsWeapon(itemRosterElement.EquipmentElement.Item);
        }
        public bool IsHorse()
        {
            var itemRosterElement = _currentItemRosterElement;
            return AutoTraderHelpers.IsHorse(itemRosterElement.EquipmentElement.Item);
        }
        public bool IsConsumable()
        {
            var itemRosterElement = _currentItemRosterElement;
            return AutoTraderHelpers.IsConsumable(itemRosterElement.EquipmentElement.Item);
        }
        public bool IsLivestock()
        {
            var itemRosterElement = _currentItemRosterElement;
            return AutoTraderHelpers.IsLivestock(itemRosterElement.EquipmentElement.Item);
        }

        public void SetCurrentElementByName(string itemName)
        {
            bool found = false;
            foreach (ItemRosterElement element in GetItemRoster())
            {
                if (element.EquipmentElement.Item.Name.ToString().Equals(itemName))
                {
                    found = true;
                    AutoTraderHelpers.PrintDebugMessage("Found by name: " + element.EquipmentElement.Item.Name.ToString() + " == " + itemName);
                    _currentItemRosterElement = element;
                    break;
                }
            }
            if (!found)
            {
                AutoTraderHelpers.PrintDebugMessage("!!!!! Could not find element with name : " + itemName + " !!!!!");
            }
        }

        public List<string> GetPlayerItemRosterNames()
        {
            return PartyBase.MainParty.ItemRoster.Select(x => x.EquipmentElement.Item.Name.ToString()).ToList();
        }

        public int GetItemAmountInPlayerRoster()
        {
            int amount = 0;
            foreach (ItemRosterElement element in PartyBase.MainParty.ItemRoster)
            {
                if (element.EquipmentElement.Item.Name.ToString().Equals(GetItemName()))
                {
                    amount = element.Amount;
                    break;
                }
            }
            AutoTraderHelpers.PrintDebugMessage("- amount in own inventory: " + amount.ToString());
            return amount;
        }
    }
}
