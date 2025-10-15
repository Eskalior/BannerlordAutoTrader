using System.Collections.Generic;
using TaleWorlds.Core;

namespace AutoTrader
{
    public interface ILogicConnector
    {
        bool IsCaravan { get; set; }
        bool IsBuying { get; set; }

        void SetCurrentElementById(int itemId);
        void SetCurrentElementByName(string itemName);
        int GetInitialGold();
        int GetTroopWage();
        float GetCurrentWeight();
        float GetInventoryCapacity();
        int GetPlayerItemRosterSize();
        List<string> GetPlayerItemRosterNames();
        int GetNumPartyMembers();
        int GetNumLivestockAnimals();
        int GetMerchantItemRosterSize();
        List<string> GetLocks();
        bool IsItemLocked();
        bool IsItemTradeGood();
        int GetItemAmount();
        int GetItemAmountInPlayerRoster();
        string GetItemName();
        float GetItemWeight();
        bool IsWeaponDesignEmpty();
        bool IsPackAnimal();
        bool IsItemGrain();
        bool IsItemHardwood();
        int GetPartyHardwoodIndex();
        float GetRosterElementWeight();
        bool InitInventory();
        int GetMerchantGold();
        bool IsItemTierLowerThan(ItemObject.ItemTiers tier);

        bool IsItemFiltered(List<string> doneItems = null);
        void TransferItem();
        int GetProjectedProfit(int buyoutPrice);
        int GetItemPrice();
        float GetAveragePriceFallback();
        int GetCostOfRosterElement();
        float GetAveragePriceFactorItemCategory();

        /// Towns
        int GetTownListSize();
        bool IsTownInRange(int townId, out float actualDistance);
        bool IsCurrentTown(int townId);
        float GetTownItemPrice(int townId, bool isSelling);
        float GetCurrentTownPriceFactor();

        /// Villages
        int GetVillageListSize();
        bool IsVillageInRange(int villageId, out float actualDistance);
        bool IsCurrentVillage(int townId);
        float GetVillageItemPrice(int townId, bool isSelling);

        /// Helper Wrapper
        bool IsArmor();
        bool IsWeapon();
        bool IsHorse();
        bool IsConsumable();
        bool IsLivestock();

    }
}
