using Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Localization;

namespace AutoTrader
{
    class TradeBehavior : CampaignBehaviorBase
    {
        private AutoTraderLogic _autoTraderLogic;

        public TradeBehavior(AutoTraderLogic autoTraderLogic)
        {
            _autoTraderLogic = autoTraderLogic;
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void AutoTradeGoodsConsequence(MenuCallbackArgs args)
        {
            _autoTraderLogic.PerformAutoTrade(false);
        }


        private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
        {
            if (campaignGameStarter != null)
                this.AddDialogAndGameMenus(campaignGameStarter);
        }

        private bool AutoTradeGoodsCondition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Trade;
            return MenuHelper.SetOptionProperties(args, true, false, new TextObject("", null));
        }

        private bool AutoTradeGoodsVillageCondition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Trade;
            return MenuHelper.SetOptionProperties(args, true, false, new TextObject("", null));
        }

        private bool AutoTradeGoodsCaravanCondition()
        {
            return true;
        }

        private void AutoTradeGoodsCaravanConsequence()
        {
            _autoTraderLogic.PerformAutoTrade(true);
            PlayerEncounter.LeaveEncounter = true;
        }

        private void AddDialogAndGameMenus(CampaignGameStarter campaignGameStarter)
        {
            if (campaignGameStarter != null)
            {
                campaignGameStarter.AddGameMenuOption("town", "autotrader_town", new TextObject("{=ATTrade}Automatically trade wares", null).ToString(),
                    new GameMenuOption.OnConditionDelegate(this.AutoTradeGoodsCondition),
                    new GameMenuOption.OnConsequenceDelegate(this.AutoTradeGoodsConsequence), false, 4, false);
                campaignGameStarter.AddGameMenuOption("village", "autotrader_village", new TextObject("{=ATTrade}Automatically trade wares", null).ToString(),
                    new GameMenuOption.OnConditionDelegate(this.AutoTradeGoodsVillageCondition),
                    new GameMenuOption.OnConsequenceDelegate(this.AutoTradeGoodsConsequence), false, 4, false);
                campaignGameStarter.AddPlayerLine("caravan_buy_products", "caravan_talk",
                    "close_window", "{=ATCaravan}I'd like to inspect your wares. (Autotrade)", 
                    new ConversationSentence.OnConditionDelegate(this.AutoTradeGoodsCaravanCondition),
                    new ConversationSentence.OnConsequenceDelegate(this.AutoTradeGoodsCaravanConsequence), 100, null, null);
            }
                
        }

    }
}
