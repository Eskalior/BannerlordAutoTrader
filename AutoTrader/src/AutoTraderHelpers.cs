using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace AutoTrader
{
    public static class AutoTraderHelpers
    {
        private static PlatformFilePath debugLogFilePath = new PlatformFilePath(EngineFilePaths.ConfigsPath, "AutoTrader.log");

        public static void Initialize()
        {
            if (FileHelper.FileExists(AutoTraderHelpers.debugLogFilePath))
            {
                FileHelper.DeleteFile(AutoTraderHelpers.debugLogFilePath);
            }
            FileHelper.SaveFileString(debugLogFilePath, "AutoTrader log");
            FileHelper.AppendLineToFileString(debugLogFilePath, "------------------------------------------------------------------------------");
            FileHelper.AppendLineToFileString(debugLogFilePath, "For detailed info, add <debugMode>True</debugMode> to the AutoTraderConfig.xml");
            FileHelper.AppendLineToFileString(debugLogFilePath, "WARNING: Debug mode is will drastically slow autotrading!");
            FileHelper.AppendLineToFileString(debugLogFilePath, "------------------------------------------------------------------------------");
        }

        public static void PrintMessage(string message)
        {
            InformationManager.DisplayMessage(new InformationMessage(message, Color.FromUint(16753958U)));
            FileHelper.AppendLineToFileString(debugLogFilePath, message);
        }

        public static void PrintDebugMessage(string message)
        {
            if (AutoTraderConfig.DebugMode)
            {
                FileHelper.AppendLineToFileString(debugLogFilePath, message);
            }
                
        }

        public static bool IsArmor(ItemObject itemObject)
        {
            if (itemObject.ItemType == ItemObject.ItemTypeEnum.HeadArmor
                || itemObject.ItemType == ItemObject.ItemTypeEnum.BodyArmor
                || itemObject.ItemType == ItemObject.ItemTypeEnum.LegArmor
                || itemObject.ItemType == ItemObject.ItemTypeEnum.HandArmor
                || itemObject.ItemType == ItemObject.ItemTypeEnum.ChestArmor
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Cape
                || itemObject.ItemType == ItemObject.ItemTypeEnum.HorseHarness)
                return true;
            return false;
        }

        public static bool IsWeapon(ItemObject itemObject)
        {
            if (itemObject.ItemType == ItemObject.ItemTypeEnum.OneHandedWeapon
                || itemObject.ItemType == ItemObject.ItemTypeEnum.TwoHandedWeapon
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Polearm
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Arrows
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Bolts
                || itemObject.ItemType == ItemObject.ItemTypeEnum.SlingStones
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Shield
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Bow
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Crossbow
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Sling
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Thrown
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Pistol
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Musket
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Bullets)
                return true;
            return false;
        }

        public static bool IsHorse(ItemObject itemObject)
        {
            if (itemObject.ItemType == ItemObject.ItemTypeEnum.Horse)
                return true;
            return false;
        }

        public static bool IsTradeGood(ItemObject itemObject)
        {
            if (itemObject.ItemType == ItemObject.ItemTypeEnum.Goods
                || itemObject.ItemType == ItemObject.ItemTypeEnum.Animal)
                return true;
            return false;
        }

        public static bool IsConsumable(ItemObject itemObject)
        {
            if (itemObject.IsFood)
                return true;
            return false;
        }

        public static bool IsSmithingMaterial(ItemObject itemObject)
        {
            if (itemObject == DefaultItems.Charcoal
                || itemObject == DefaultItems.HardWood
                || itemObject == DefaultItems.IronOre
                || itemObject == DefaultItems.IronIngot1
                || itemObject == DefaultItems.IronIngot2
                || itemObject == DefaultItems.IronIngot3
                || itemObject == DefaultItems.IronIngot4
                || itemObject == DefaultItems.IronIngot5
                || itemObject == DefaultItems.IronIngot6)
                return true;
            return false;
        }

        public static bool IsLivestock(ItemObject itemObject)
        {
            if (itemObject.IsAnimal)
                return true;
            return false;
        }
    }
}
