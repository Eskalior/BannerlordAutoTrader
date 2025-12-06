using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;
using AutoTrader.Warsails;

namespace AutoTrader
{
    class AutoTraderMenuViewModel : ViewModel
    {
		private string _configMenuText;
		private string _doneText;
		private string _cancelText;
		private string _lockHintText;
		private string _buyText;
		private string _sellText;
		private string _defaultText;
		private string _infText;

		private string _presetsText;
		private string _presetsDefaultText;
		private string _presetsSellerText;
		private string _presetsGlobalText;
		private string _presetsLocalText;

		private string _headerBaseText;
		private string _headerCustomText;
		private string _headerSpecialText;
		private string _headerTradeText;

		private string _useWeightedText;
		private string _simpleAIText;
		private string _buyThresholdText;
		private string _sellThresholdText;
		private string _maxCapacityText;
		private string _keepGrainsMinText;
		private string _keepGrainsMaxText;
		private string _keepConsumablesMinText;
		private string _keepConsumablesMaxText;
		private string _useInventorySpaceText;
		private string _useMaxFleetCapacityText;
		private string _keepWagesText;
		private string _searchRadiusText;
		private string _weaponsArmorTierText;

		private string _useAltATText;
		private string _junkCattleText;
		private string _sellSmithingText;
		private string _keepSmeltingText;
		private string _resupplyHardwoodText;
		private string _resupplyText;

		private string _tradeHorsesText;
		private string _tradeWeaponsText;
		private string _tradeArmorText;
		private string _tradeGoodsText;
		private string _tradeConsumablesText;
		private string _tradeLivestockText;


		private float _buyThresholdValue;
		private string _buyThresholdValueAsString;

		private float _sellThresholdValue;
		private string _sellThresholdValueAsString;

		private float _maxCapacityValue;
		private string _maxCapacityValueAsString;

		private float _keepGrainsMinValue;
		private float _keepGrainsMaxValue;
		private string _keepGrainsMinValueAsString;
		private string _keepGrainsMaxValueAsString;

		private float _keepConsumablesMinValue;
		private float _keepConsumablesMaxValue;
		private string _keepConsumablesMinValueAsString;
		private string _keepConsumablesMaxValueAsString;

		private float _useInventorySpaceValue;
		private string _useInventorySpaceValueAsString;

		private bool _useMaxFleetCapacityValue;
		private bool _isFleetCapacityVisible;

		private float _keepWagesValue;
		private string _keepWagesValueAsString;

		private float _searchRadiusValue;
		private string _searchRadiusValueAsString;

		private float _weaponsArmorTierValue;
		private string _weaponsArmorTierValueAsString;

		private bool _useWeightedValue;
		private bool _simpleAIValue;

		private bool _useAltATValue;
		private bool _junkCattleValue;
		private bool _sellSmithingValue;
		private bool _keepSmeltingValue;
		private bool _resupplyHardwoodValue;
		private bool _resupplyValue;

		private bool _buyHorsesValue;
		private bool _sellHorsesValue;
		private bool _buyWeaponsValue;
		private bool _sellWeaponsValue;
		private bool _buyArmorValue;
		private bool _sellArmorValue;
		private bool _buyGoodsValue;
		private bool _sellGoodsValue;
		private bool _buyConsumablesValue;
		private bool _sellConsumablesValue;
		private bool _buyLivestockValue;
		private bool _sellLivestockValue;
		private bool _isWeightedActive;
		private bool _isSearchRadiusActive;

		private Action _closeMenuScreen;

		public AutoTraderMenuViewModel(Action closeMenuScreen)
        {
			this._closeMenuScreen = closeMenuScreen;

			this._configMenuText = new TextObject("{=ATOptions}AutoTrader Configuration", null).ToString();
			this._doneText = new TextObject("{=ATDone}Done", null).ToString();
			this._cancelText = new TextObject("{=ATCancel}Cancel", null).ToString();
			this._lockHintText = new TextObject("{=ATLockHint}Note: Lock items to prevent them from being sold.", null).ToString();
			this._buyText = new TextObject("{=ATBuy}Buy", null).ToString();
			this._sellText = new TextObject("{=ATSell}Sell", null).ToString();
			this._defaultText = new TextObject("{=ATDefault}Default: ", null).ToString();
			this._infText = new TextObject("{=ATInf}Inf", null).ToString();

			this._presetsText = new TextObject("{=ATPresets}Presets", null).ToString();
			this._presetsDefaultText = new TextObject("{=ATPreDefault}Default", null).ToString();
			this._presetsSellerText = new TextObject("{=ATPreSeller}Loot Seller", null).ToString();
			this._presetsGlobalText = new TextObject("{=ATPreGlobal}Global Trade", null).ToString();
			this._presetsLocalText = new TextObject("{=ATPreLocal}Local Trade", null).ToString();

			this._headerBaseText = new TextObject("{=ATHeaderBase}Basic Options", null).ToString();
			this._headerCustomText = new TextObject("{=ATHeaderCustom}User Preferences", null).ToString();
			this._headerSpecialText = new TextObject("{=ATHeaderSpecial}Special Options", null).ToString();
			this._headerTradeText = new TextObject("{=ATHeaderTrade}Buy / Sell Options", null).ToString();

			this._useWeightedText = new TextObject("{=ATWeightedRadius}Weight average price by distance", null).ToString();
			this._simpleAIText = new TextObject("{=ATSimpleAI}Green/red-based trading (improves with trade rumors)", null).ToString();
			this._buyThresholdText = new TextObject("{=ATBuyThreshold}Buy under X% of the average price", null).ToString();
			this._sellThresholdText = new TextObject("{=ATSellThreshold}Sell above X% of the average price", null).ToString();
			this._maxCapacityText = new TextObject("{=ATMaxCapacity}Maximum % of inventory filled by the same good", null).ToString();
			this._keepGrainsMinText = new TextObject("{=ATKeepGrainsMin}Minimum amount of grain to keep", null).ToString();
			this._keepGrainsMaxText = new TextObject("{=ATKeepGrainsMax}Maximum amount of grain to keep", null).ToString();
			this._keepConsumablesMinText = new TextObject("{=ATKeepConsumablesMin}Minimum amount of other consumables to keep", null).ToString();
			this._keepConsumablesMaxText = new TextObject("{=ATKeepConsumablesMax}Maximum amount of other consumables to keep", null).ToString();
			this._useInventorySpaceText = new TextObject("{=ATUseInventorySpace}Total % of inventory space the mod may use", null).ToString();
			this._useMaxFleetCapacityText = new TextObject("{=ATUseMaxFleetCapacity}Use fleet capacity instead of inventory capacity", null).ToString();
			this._keepWagesText = new TextObject("{=ATKeepWages}Keep enough gold for X days of troop wages", null).ToString();
			this._searchRadiusText = new TextObject("{=ATSearchRadius}Average price search radius", null).ToString();
			this._weaponsArmorTierText = new TextObject("{=ATWeaponsArmorTier}Sell weapons and armor of tier X and below", null).ToString();

			this._useAltATText = new TextObject("{=ATAltAT}Use hotkey <Alt + A + T> instead of <Alt + A>", null).ToString();
			this._junkCattleText = new TextObject("{=ATJunkCattle}Always sell all cattle", null).ToString();
			this._sellSmithingText = new TextObject("{=ATSellSmithing}Sell charcoal and ingots", null).ToString();
			this._keepSmeltingText = new TextObject("{=ATKeepSmelting}Keep smeltable weapons", null).ToString();
			this._resupplyHardwoodText = new TextObject("{=ATResupplyHardwood}Resupply hardwood to 'keep consumables' value", null).ToString();
			this._resupplyText = new TextObject("{=ATResupply}Resupply food to 'keep consumables' value", null).ToString();

			this._tradeHorsesText = new TextObject("{=ATTradeHorses}Trade Horses (buys only carry horses)", null).ToString();
			this._tradeWeaponsText = new TextObject("{=ATTradeWeapons}Trade Weapons (sells below Tier X)", null).ToString();
			this._tradeArmorText = new TextObject("{=ATTradeArmor}Trade Armor (sells below Tier X)", null).ToString();
			this._tradeGoodsText = new TextObject("{=ATTradeGoods}Trade Goods", null).ToString();
			this._tradeConsumablesText = new TextObject("{=ATTradeConsumables}Trade Consumables", null).ToString();
			this._tradeLivestockText = new TextObject("{=ATTradeLivestock}Trade Livestock", null).ToString();

			this._useWeightedValue = AutoTraderConfig.UseWeightedValue;
			this._simpleAIValue = AutoTraderConfig.SimpleTradingAI;
			this._buyThresholdValue = AutoTraderConfig.BuyThresholdValue;
			this._sellThresholdValue = AutoTraderConfig.SellThresholdValue;
			this._maxCapacityValue = AutoTraderConfig.MaxCapacityValue;
			this._keepGrainsMinValue = AutoTraderConfig.KeepGrainsMinValue;
			this._keepGrainsMaxValue = AutoTraderConfig.KeepGrainsMaxValue;
			this._keepConsumablesMinValue = AutoTraderConfig.KeepConsumablesMinValue;
			this._keepConsumablesMaxValue = AutoTraderConfig.KeepConsumablesMaxValue;
			this._useInventorySpaceValue = AutoTraderConfig.UseInventorySpaceValue;
			this._useMaxFleetCapacityValue = AutoTraderConfig.UseMaxFleetCapacityValue;
			this._keepWagesValue = AutoTraderConfig.KeepWagesValue;
			this._searchRadiusValue = AutoTraderConfig.SearchRadiusValue;
			this._weaponsArmorTierValue = AutoTraderConfig.WeaponsArmorTierValue;

			this._buyHorsesValue = AutoTraderConfig.BuyHorsesValue;
			this._buyWeaponsValue = AutoTraderConfig.BuyWeaponsValue;
			this._buyArmorValue = AutoTraderConfig.BuyArmorValue;
			this._buyGoodsValue = AutoTraderConfig.BuyGoodsValue;
			this._buyConsumablesValue = AutoTraderConfig.BuyConsumablesValue;
			this._buyLivestockValue = AutoTraderConfig.BuyLivestockValue;

			this._useAltATValue = AutoTraderConfig.UseAltATValue;
			this._junkCattleValue = AutoTraderConfig.JunkCattleValue;
			this._sellSmithingValue = AutoTraderConfig.SellSmithingValue;
			this._keepSmeltingValue = AutoTraderConfig.KeepSmeltingValue;
			this._resupplyHardwoodValue = AutoTraderConfig.ResupplyHardwoodValue;
			this._resupplyValue = AutoTraderConfig.ResupplyValue;

			this._sellHorsesValue = AutoTraderConfig.SellHorsesValue;
			this._sellWeaponsValue = AutoTraderConfig.SellWeaponsValue;
			this._sellArmorValue = AutoTraderConfig.SellArmorValue;
			this._sellGoodsValue = AutoTraderConfig.SellGoodsValue;
			this._sellConsumablesValue = AutoTraderConfig.SellConsumablesValue;
			this._sellLivestockValue = AutoTraderConfig.SellLivestockValue;

			this._isWeightedActive = !this._simpleAIValue;
			this._isSearchRadiusActive = (!this._simpleAIValue && !this._useWeightedValue);

			// Check at runtime if Warsails DLC is available
			this._isFleetCapacityVisible = WarsailsDetector.IsWarsailsDLCAvailable();

			this.RefreshValues();
		}

        public override void RefreshValues()
        {
            base.RefreshValues();
			this.BuyThresholdValueAsString = this.BuyThresholdValue.ToString();
			this.SellThresholdValueAsString = this.SellThresholdValue.ToString();
			this.MaxCapacityValueAsString = this.MaxCapacityValue.ToString();
			this.KeepGrainsMinValueAsString = this.KeepGrainsMinValue.ToString();
			if (this.KeepGrainsMaxValue >= AutoTraderConfig.MaxKeepGrainsValue)
				this.KeepGrainsMaxValueAsString = new TextObject("{=ATAll}All", null).ToString();
			else
				this.KeepGrainsMaxValueAsString = this.KeepGrainsMaxValue.ToString();
			this.KeepConsumablesMinValueAsString = this.KeepConsumablesMinValue.ToString();
			this.KeepConsumablesMaxValueAsString = this.KeepConsumablesMaxValue.ToString();
			this.UseInventorySpaceValueAsString = this.UseInventorySpaceValue.ToString();
			this.KeepWagesValueAsString = this.KeepWagesValue.ToString();
			if (this.SearchRadiusValue > 999.0f)
				this.SearchRadiusValueAsString = this.InfText;
			else
				this.SearchRadiusValueAsString = this.SearchRadiusValue.ToString();
			this.WeaponsArmorTierValueAsString = this.WeaponsArmorTierValue.ToString();

			this.IsWeightedActive = (!this._simpleAIValue);
			this.IsSearchRadiusActive = (!this._simpleAIValue && !this._useWeightedValue);
		}

		private void ExecuteDone()
		{
			AutoTraderConfig.UseWeightedValue = this._useWeightedValue;
			AutoTraderConfig.SimpleTradingAI = this._simpleAIValue;
			AutoTraderConfig.BuyThresholdValue = (int)this.BuyThresholdValue;
			AutoTraderConfig.SellThresholdValue = (int)this.SellThresholdValue;
			AutoTraderConfig.MaxCapacityValue = (int)this.MaxCapacityValue;
			AutoTraderConfig.KeepGrainsMinValue = (int)this.KeepGrainsMinValue;
			AutoTraderConfig.KeepGrainsMaxValue = (int)this.KeepGrainsMaxValue;
			AutoTraderConfig.KeepConsumablesMinValue = (int)this.KeepConsumablesMinValue;
			AutoTraderConfig.KeepConsumablesMaxValue = (int)this.KeepConsumablesMaxValue;
			AutoTraderConfig.UseInventorySpaceValue = (int)this.UseInventorySpaceValue;
			AutoTraderConfig.UseMaxFleetCapacityValue = this.UseMaxFleetCapacityValue;
			AutoTraderConfig.KeepWagesValue = (int)this.KeepWagesValue;
			AutoTraderConfig.SearchRadiusValue = (int)this.SearchRadiusValue;
			AutoTraderConfig.WeaponsArmorTierValue = (int)this.WeaponsArmorTierValue;

			AutoTraderConfig.BuyHorsesValue = this.BuyHorsesValue;
			AutoTraderConfig.BuyArmorValue = this.BuyArmorValue;
			AutoTraderConfig.BuyWeaponsValue = this.BuyWeaponsValue;
			AutoTraderConfig.BuyGoodsValue = this.BuyGoodsValue;
			AutoTraderConfig.BuyConsumablesValue = this.BuyConsumablesValue;
			AutoTraderConfig.BuyLivestockValue = this.BuyLivestockValue;

			AutoTraderConfig.UseAltATValue = this.UseAltATValue;
			AutoTraderConfig.JunkCattleValue = this.JunkCattleValue;
			AutoTraderConfig.SellSmithingValue = this.SellSmithingValue;
			AutoTraderConfig.KeepSmeltingValue = this.KeepSmeltingValue;
			AutoTraderConfig.ResupplyHardwoodValue = this.ResupplyHardwoodValue;
			AutoTraderConfig.ResupplyValue = this.ResupplyValue;

			AutoTraderConfig.SellHorsesValue = this.SellHorsesValue;
			AutoTraderConfig.SellArmorValue = this.SellArmorValue;
			AutoTraderConfig.SellWeaponsValue = this.SellWeaponsValue;
			AutoTraderConfig.SellGoodsValue = this.SellGoodsValue;
			AutoTraderConfig.SellConsumablesValue = this.SellConsumablesValue;
			AutoTraderConfig.SellLivestockValue = this.SellLivestockValue;

			AutoTraderConfig.Save();

			if (this._closeMenuScreen != null)
				this._closeMenuScreen();
			else
				ScreenManager.PopScreen();
		}

		private void ExecuteCancel()
		{
			if (this._closeMenuScreen != null)
				this._closeMenuScreen();
			else
				ScreenManager.PopScreen();
		}

		private void ExecutePresetDefault()
		{
			this.SimpleAIValue = true;

			this.BuyThresholdValue = 85.0f;
			this.SellThresholdValue = 95.0f;
			this.UseWeightedValue = false;
			this.MaxCapacityValue = 15.0f;
			this.KeepGrainsMinValue = 10.0f;
			this.KeepGrainsMaxValue = 100.0f;
			this.KeepConsumablesMinValue = 4.0f;
			this.KeepConsumablesMaxValue = 20.0f;
			this.UseInventorySpaceValue = 90.0f;
			this.KeepWagesValue = 5.0f;
			this.SearchRadiusValue = 300.0f;
			this.WeaponsArmorTierValue = 2.0f;

			this.JunkCattleValue = false;
			this.SellSmithingValue = false;
			this.KeepSmeltingValue = false;
			this.ResupplyHardwoodValue = false;
			this.ResupplyValue = true;

			this.BuyHorsesValue = true;
			this.SellHorsesValue = false;
			this.BuyWeaponsValue = false;
			this.SellWeaponsValue = true;
			this.BuyArmorValue = false;
			this.SellArmorValue = true;
			this.BuyGoodsValue = true;
			this.SellGoodsValue = true;
			this.BuyConsumablesValue = true;
			this.SellConsumablesValue = true;
			this.BuyLivestockValue = false;
			this.SellLivestockValue = true;

			RefreshValues();
		}

		private void ExecutePresetSeller()
		{
			this.WeaponsArmorTierValue = 3.0f;
			this.SellSmithingValue = false;
			this.KeepSmeltingValue = false;
			this.BuyHorsesValue = false;
			this.SellHorsesValue = true;
			this.BuyWeaponsValue = false;
			this.SellWeaponsValue = true;
			this.BuyArmorValue = false;
			this.SellArmorValue = true;
			this.BuyGoodsValue = false;
			this.SellGoodsValue = false;
			this.BuyConsumablesValue = false;
			this.SellConsumablesValue = false;
			this.BuyLivestockValue = false;
			this.SellLivestockValue = false;
			this.JunkCattleValue = true;

			RefreshValues();
		}

		private void ExecutePresetGlobal()
		{
			this.SimpleAIValue = false;

			this.SearchRadiusValue = 1000.0f;
			this.UseWeightedValue = false;

			this.BuyHorsesValue = true;
			this.BuyGoodsValue = true;
			this.SellGoodsValue = true;
			this.BuyConsumablesValue = true;
			this.SellConsumablesValue = true;

			RefreshValues();
		}

		private void ExecutePresetLocal()
		{
			this.SimpleAIValue = false;

			this.SearchRadiusValue = 300.0f;
			this.UseWeightedValue = true;

			this.BuyHorsesValue = true;
			this.BuyGoodsValue = true;
			this.SellGoodsValue = true;
			this.BuyConsumablesValue = true;
			this.SellConsumablesValue = true;

			RefreshValues();
		}

		[DataSourceProperty]
		public string UseWeightedText
		{
			get
			{
				return this._useWeightedText;
			}
		}

		[DataSourceProperty]
		public string ResupplyHardwoodText
		{
			get
			{
				return this._resupplyHardwoodText;
			}
		}

		[DataSourceProperty]
		public string SimpleAIText
		{
			get
			{
				return this._simpleAIText;
			}
		}

		[DataSourceProperty]
		public string ResupplyText
		{
			get
			{
				return this._resupplyText;
			}
		}

		[DataSourceProperty]
		public string InfText
		{
			get
			{
				return this._infText;
			}
		}

		[DataSourceProperty]
		public string ConfigMenuText
		{
			get
			{
				return this._configMenuText;
			}
		}

		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
		}

		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
		}

		[DataSourceProperty]
		public string LockHintText
		{
			get
			{
				return this._lockHintText;
			}
		}

		[DataSourceProperty]
		public string BuyText
		{
			get
			{
				return this._buyText;
			}
		}

		[DataSourceProperty]
		public string SellText
		{
			get
			{
				return this._sellText;
			}
		}

		[DataSourceProperty]
		public string DefaultText
		{
			get
			{
				return this._defaultText;
			}
		}

		[DataSourceProperty]
		public string PresetsText
		{
			get
			{
				return this._presetsText;
			}
		}

		[DataSourceProperty]
		public string PresetsDefaultText
		{
			get
			{
				return this._presetsDefaultText;
			}
		}

		[DataSourceProperty]
		public string PresetsSellerText
		{
			get
			{
				return this._presetsSellerText;
			}
		}

		[DataSourceProperty]
		public string PresetsGlobalText
		{
			get
			{
				return this._presetsGlobalText;
			}
		}

		[DataSourceProperty]
		public string PresetsLocalText
		{
			get
			{
				return this._presetsLocalText;
			}
		}

		[DataSourceProperty]
		public string HeaderBaseText
		{
			get
			{
				return this._headerBaseText;
			}
		}

		[DataSourceProperty]
		public string HeaderCustomText
		{
			get
			{
				return this._headerCustomText;
			}
		}

		[DataSourceProperty]
		public string HeaderSpecialText
		{
			get
			{
				return this._headerSpecialText;
			}
		}

		[DataSourceProperty]
		public string HeaderTradeText
		{
			get
			{
				return this._headerTradeText;
			}
		}

		[DataSourceProperty]
		public string BuyThresholdText
		{
			get
			{
				return this._buyThresholdText;
			}
		}

		[DataSourceProperty]
		public string SellThresholdText
		{
			get
			{
				return this._sellThresholdText;
			}
		}

		[DataSourceProperty]
		public string MaxCapacityText
		{
			get
			{
				return this._maxCapacityText;
			}
		}

		[DataSourceProperty]
		public string UseInventorySpaceText
		{
			get
			{
				return this._useInventorySpaceText;
			}
		}

		[DataSourceProperty]
		public string UseMaxFleetCapacityText
		{
			get
			{
				return this._useMaxFleetCapacityText;
			}
		}

		[DataSourceProperty]
		public string KeepWagesText
		{
			get
			{
				return this._keepWagesText;
			}
		}

		[DataSourceProperty]
		public string SearchRadiusText
		{
			get
			{
				return this._searchRadiusText;
			}
		}

		[DataSourceProperty]
		public string WeaponsArmorTierText
		{
			get
			{
				return this._weaponsArmorTierText;
			}
		}

		[DataSourceProperty]
		public string UseAltATText
		{
			get
			{
				return this._useAltATText;
			}
		}

		[DataSourceProperty]
		public string JunkCattleText
		{
			get
			{
				return this._junkCattleText;
			}
		}

		[DataSourceProperty]
		public string SellSmithingText
		{
			get
			{
				return this._sellSmithingText;
			}
		}

		[DataSourceProperty]
		public string KeepSmeltingText
		{
			get
			{
				return this._keepSmeltingText;
			}
		}

		[DataSourceProperty]
		public string TradeHorsesText
		{
			get
			{
				return this._tradeHorsesText;
			}
		}

		[DataSourceProperty]
		public string TradeWeaponsText
		{
			get
			{
				return this._tradeWeaponsText;
			}
		}

		[DataSourceProperty]
		public string TradeArmorText
		{
			get
			{
				return this._tradeArmorText;
			}
		}

		[DataSourceProperty]
		public string TradeGoodsText
		{
			get
			{
				return this._tradeGoodsText;
			}
		}

		[DataSourceProperty]
		public string TradeConsumablesText
		{
			get
			{
				return this._tradeConsumablesText;
			}
		}

		[DataSourceProperty]
		public string TradeLivestockText
		{
			get
			{
				return this._tradeLivestockText;
			}
		}

		[DataSourceProperty]
		public bool IsSearchRadiusActive
		{
			get
			{
				return this._isSearchRadiusActive;
			}
			set
			{
				if (value != this._isSearchRadiusActive)
				{
					this._isSearchRadiusActive = value;
					base.OnPropertyChanged("IsSearchRadiusActive");
				}
			}
		}

		[DataSourceProperty]
		public bool IsFleetCapacityVisible
		{
			get
			{
				return this._isFleetCapacityVisible;
			}
			set
			{
				if (value != this._isFleetCapacityVisible)
				{
					this._isFleetCapacityVisible = value;
					base.OnPropertyChanged("IsFleetCapacityVisible");
				}
			}
		}

		[DataSourceProperty]
		public bool IsWeightedActive
		{
			get
			{
				return this._isWeightedActive;
			}
			set
			{
				if (value != this._isWeightedActive)
				{
					this._isWeightedActive = value;
					RefreshValues();
					base.OnPropertyChanged("IsWeightedActive");
				}
			}
		}

		[DataSourceProperty]
		public bool UseWeightedValue
		{
			get
			{
				return this._useWeightedValue;
			}
			set
			{
				if (value != this._useWeightedValue)
				{
					this._useWeightedValue = value;
					RefreshValues();
					base.OnPropertyChanged("UseWeightedValue");
				}
			}
		}

		[DataSourceProperty]
		public bool SimpleAIValue
		{
			get
			{
				return this._simpleAIValue;
			}
			set
			{
				if (value != this._simpleAIValue)
				{
					this._simpleAIValue = value;
					RefreshValues();

					if (this._simpleAIValue)
					{
						this.SellThresholdValue = 125.0f;
						this.BuyThresholdValue = 80.0f;
					}
					else
					{
						this.SellThresholdValue = 95.0f;
						this.BuyThresholdValue = 70.0f;
					}

					base.OnPropertyChanged("SimpleAIValue");
				}
			}
		}

		[DataSourceProperty]
		public bool UseAltATValue
		{
			get
			{
				return this._useAltATValue;
			}
			set
			{
				if (value != this._useAltATValue)
				{
					this._useAltATValue = value;
					base.OnPropertyChanged("UseAltATValue");
				}
			}
		}

		[DataSourceProperty]
		public bool JunkCattleValue
		{
			get
			{
				return this._junkCattleValue;
			}
			set
			{
				if (value != this._junkCattleValue)
				{
					this._junkCattleValue = value;
                    if (value)
                    {
						this.BuyLivestockValue = false;
						this.SellLivestockValue = true;
                    }
					base.OnPropertyChanged("JunkCattleValue");
				}
			}
		}

		[DataSourceProperty]
		public bool SellSmithingValue
		{
			get
			{
				return this._sellSmithingValue;
			}
			set
			{
				if (value != this._sellSmithingValue)
				{
					this._sellSmithingValue = value;
					base.OnPropertyChanged("SellSmithingValue");
				}
			}
		}

		[DataSourceProperty]
		public bool KeepSmeltingValue
		{
			get
			{
				return this._keepSmeltingValue;
			}
			set
			{
				if (value != this._keepSmeltingValue)
				{
					this._keepSmeltingValue = value;
					base.OnPropertyChanged("KeepSmeltingValue");
				}
			}
		}

		[DataSourceProperty]
		public bool ResupplyHardwoodValue
		{
			get
			{
				return this._resupplyHardwoodValue;
			}
			set
			{
				if (value != this._resupplyHardwoodValue)
				{
					this._resupplyHardwoodValue = value;
					if (value)
						this.BuyGoodsValue = true;
					base.OnPropertyChanged("ResupplyHardwoodValue");
				}
			}
		}

		[DataSourceProperty]
		public bool ResupplyValue
		{
			get
			{
				return this._resupplyValue;
			}
			set
			{
				if (value != this._resupplyValue)
				{
					this._resupplyValue = value;
					if (value)
						this.BuyConsumablesValue = true;
					base.OnPropertyChanged("ResupplyValue");
				}
			}
		}

        [DataSourceProperty]
        public bool BuyHorsesValue
        {
            get
            {
                return this._buyHorsesValue;
            }
            set
            {
                if (value != this._buyHorsesValue)
                {
                    this._buyHorsesValue = value;
                    base.OnPropertyChanged("BuyHorsesValue");
                }
            }
        }

        [DataSourceProperty]
        public bool BuyWeaponsValue
        {
            get
            {
                return this._buyWeaponsValue;
            }
            set
            {
                if (value != this._buyWeaponsValue)
                {
                    this._buyWeaponsValue = value;
                    base.OnPropertyChanged("BuyWeaponsValue");
                }
            }
        }

        [DataSourceProperty]
        public bool BuyArmorValue
        {
            get
            {
                return this._buyArmorValue;
            }
            set
            {
                if (value != this._buyArmorValue)
                {
                    this._buyArmorValue = value;
                    base.OnPropertyChanged("BuyArmorValue");
                }
            }
        }

        [DataSourceProperty]
        public bool BuyGoodsValue
        {
            get
            {
                return this._buyGoodsValue;
            }
            set
            {
                if (value != this._buyGoodsValue)
                {
                    this._buyGoodsValue = value;
                    if (!value)
                        this.ResupplyHardwoodValue = false;
                    base.OnPropertyChanged("BuyGoodsValue");
                }
            }
        }

        [DataSourceProperty]
        public bool BuyConsumablesValue
        {
            get
            {
                return this._buyConsumablesValue;
            }
            set
            {
                if (value != this._buyConsumablesValue)
                {
                    this._buyConsumablesValue = value;
                    if (!value)
                        this.ResupplyValue = false;
                    base.OnPropertyChanged("BuyConsumablesValue");
                }
            }
        }

        [DataSourceProperty]
        public bool BuyLivestockValue
        {
            get
            {
                return this._buyLivestockValue;
            }
            set
            {
                if (value != this._buyLivestockValue)
                {
                    this._buyLivestockValue = value;
                    if (value)
                        this.JunkCattleValue = false;
                    base.OnPropertyChanged("BuyLivestockValue");
                }
            }
        }

        [DataSourceProperty]
        public bool SellHorsesValue
        {
            get
            {
                return this._sellHorsesValue;
            }
            set
            {
                if (value != this._sellHorsesValue)
                {
                    this._sellHorsesValue = value;
                    base.OnPropertyChanged("SellHorsesValue");
                }
            }
        }

        [DataSourceProperty]
        public bool SellWeaponsValue
        {
            get
            {
                return this._sellWeaponsValue;
            }
            set
            {
                if (value != this._sellWeaponsValue)
                {
                    this._sellWeaponsValue = value;
                    base.OnPropertyChanged("SellWeaponsValue");
                }
            }
        }

        [DataSourceProperty]
        public bool SellArmorValue
        {
            get
            {
                return this._sellArmorValue;
            }
            set
            {
                if (value != this._sellArmorValue)
                {
                    this._sellArmorValue = value;
                    base.OnPropertyChanged("SellArmorValue");
                }
            }
        }

        [DataSourceProperty]
        public bool SellGoodsValue
        {
            get
            {
                return this._sellGoodsValue;
            }
            set
            {
                if (value != this._sellGoodsValue)
                {
                    this._sellGoodsValue = value;
                    base.OnPropertyChanged("SellGoodsValue");
                }
            }
        }

        [DataSourceProperty]
        public bool SellConsumablesValue
        {
            get
            {
                return this._sellConsumablesValue;
            }
            set
            {
                if (value != this._sellConsumablesValue)
                {
                    this._sellConsumablesValue = value;
                    base.OnPropertyChanged("SellConsumablesValue");
                }
            }
        }

        [DataSourceProperty]
        public bool SellLivestockValue
        {
            get
            {
                return this._sellLivestockValue;
            }
            set
            {
                if (value != this._sellLivestockValue)
                {
                    this._sellLivestockValue = value;
                    base.OnPropertyChanged("SellLivestockValue");
                }
            }
        }

		[DataSourceProperty]
		public string KeepGrainsMinText
		{
			get
			{
				return this._keepGrainsMinText;
			}
		}

		[DataSourceProperty]
		public string KeepGrainsMaxText
		{
			get
			{
				return this._keepGrainsMaxText;
			}
		}

		[DataSourceProperty]
		public string KeepConsumablesMinText
		{
			get
			{
				return this._keepConsumablesMinText;
			}
		}

		[DataSourceProperty]
		public string KeepConsumablesMaxText
		{
			get
			{
				return this._keepConsumablesMaxText;
			}
		}


		[DataSourceProperty]
		public float BuyThresholdValue
		{
			get
			{
				return this._buyThresholdValue;
			}
			set
			{
				if (value != this._buyThresholdValue)
				{
					this._buyThresholdValue = value;
					this.BuyThresholdValueAsString = this.BuyThresholdValue.ToString();
					base.OnPropertyChanged("BuyThresholdValue");
				}
			}
		}

		[DataSourceProperty]
		public string BuyThresholdValueAsString
		{
			get
			{
				return this._buyThresholdValueAsString;
			}
			set
			{
				if (value != this._buyThresholdValueAsString)
				{
					this._buyThresholdValueAsString = value;
					base.OnPropertyChanged("BuyThresholdValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public float SellThresholdValue
		{
			get
			{
				return this._sellThresholdValue;
			}
			set
			{
				if (value != this._sellThresholdValue)
				{
					this._sellThresholdValue = value;
					this.SellThresholdValueAsString = this.SellThresholdValue.ToString();
					base.OnPropertyChanged("SellThresholdValue");
				}
			}
		}

		[DataSourceProperty]
		public string SellThresholdValueAsString
		{
			get
			{
				return this._sellThresholdValueAsString;
			}
			set
			{
				if (value != this._sellThresholdValueAsString)
				{
					this._sellThresholdValueAsString = value;
					base.OnPropertyChanged("SellThresholdValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public float MaxCapacityValue
		{
			get
			{
				return this._maxCapacityValue;
			}
			set
			{
				if (value != this._maxCapacityValue)
				{
					this._maxCapacityValue = value;
					this.MaxCapacityValueAsString = this.MaxCapacityValue.ToString();
					base.OnPropertyChanged("MaxCapacityValue");
				}
			}
		}

		[DataSourceProperty]
		public string MaxCapacityValueAsString
		{
			get
			{
				return this._maxCapacityValueAsString;
			}
			set
			{
				if (value != this._maxCapacityValueAsString)
				{
					this._maxCapacityValueAsString = value;
					base.OnPropertyChanged("MaxCapacityValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public float KeepGrainsMinValue
		{
			get
			{
				return this._keepGrainsMinValue;
			}
			set
			{
				if (value != this._keepGrainsMinValue)
				{
					this._keepGrainsMinValue = value;
					this.KeepGrainsMinValueAsString = this.KeepGrainsMinValue.ToString();
					base.OnPropertyChanged("KeepGrainsMinValue");
				}
			}
		}

		[DataSourceProperty]
		public float KeepGrainsMaxValue
		{
			get
			{
				return this._keepGrainsMaxValue;
			}
			set
			{
				if (value != this._keepGrainsMaxValue)
				{
					this._keepGrainsMaxValue = value;
					if (this._keepGrainsMaxValue >= AutoTraderConfig.MaxKeepGrainsValue)
						this.KeepGrainsMaxValueAsString = new TextObject("{=ATAll}All", null).ToString();
					else
						this.KeepGrainsMaxValueAsString = this.KeepGrainsMaxValue.ToString();
					base.OnPropertyChanged("KeepGrainsMaxValue");
				}
			}
		}

		[DataSourceProperty]
		public string KeepGrainsMinValueAsString
		{
			get
			{
				return this._keepGrainsMinValueAsString;
			}
			set
			{
				if (value != this._keepGrainsMinValueAsString)
				{
					this._keepGrainsMinValueAsString = value;
					base.OnPropertyChanged("KeepGrainsMinValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public string KeepGrainsMaxValueAsString
		{
			get
			{
				return this._keepGrainsMaxValueAsString;
			}
			set
			{
				if (value != this._keepGrainsMaxValueAsString)
				{
					this._keepGrainsMaxValueAsString = value;
					base.OnPropertyChanged("KeepGrainsMaxValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public float KeepConsumablesMinValue
		{
			get
			{
				return this._keepConsumablesMinValue;
			}
			set
			{
				if (value != this._keepConsumablesMinValue)
				{
					this._keepConsumablesMinValue = value;
					this.KeepConsumablesMinValueAsString = this.KeepConsumablesMinValue.ToString();
					base.OnPropertyChanged("KeepConsumablesMinValue");
				}
			}
		}

		[DataSourceProperty]
		public float KeepConsumablesMaxValue
		{
			get
			{
				return this._keepConsumablesMaxValue;
			}
			set
			{
				if (value != this._keepConsumablesMaxValue)
				{
					this._keepConsumablesMaxValue = value;
					this.KeepConsumablesMaxValueAsString = this.KeepConsumablesMaxValue.ToString();
					base.OnPropertyChanged("KeepConsumablesMaxValue");
				}
			}
		}

		[DataSourceProperty]
		public string KeepConsumablesMinValueAsString
		{
			get
			{
				return this._keepConsumablesMinValueAsString;
			}
			set
			{
				if (value != this._keepConsumablesMinValueAsString)
				{
					this._keepConsumablesMinValueAsString = value;
					base.OnPropertyChanged("KeepConsumablesMinValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public string KeepConsumablesMaxValueAsString
		{
			get
			{
				return this._keepConsumablesMaxValueAsString;
			}
			set
			{
				if (value != this._keepConsumablesMaxValueAsString)
				{
					this._keepConsumablesMaxValueAsString = value;
					base.OnPropertyChanged("KeepConsumablesMaxValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public float UseInventorySpaceValue
		{
			get
			{
				return this._useInventorySpaceValue;
			}
			set
			{
				if (value != this._useInventorySpaceValue)
				{
					this._useInventorySpaceValue = value;
					this.UseInventorySpaceValueAsString = this.UseInventorySpaceValue.ToString();
					base.OnPropertyChanged("UseInventorySpaceValue");
				}
			}
		}

		[DataSourceProperty]
		public string UseInventorySpaceValueAsString
		{
			get
			{
				return this._useInventorySpaceValueAsString;
			}
			set
			{
				if (value != this._useInventorySpaceValueAsString)
				{
					this._useInventorySpaceValueAsString = value;
					base.OnPropertyChanged("UseInventorySpaceValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public bool UseMaxFleetCapacityValue
		{
			get
			{
				return this._useMaxFleetCapacityValue;
			}
			set
			{
				if (value != this._useMaxFleetCapacityValue)
				{
					this._useMaxFleetCapacityValue = value;
					base.OnPropertyChanged("UseMaxFleetCapacityValue");
				}
			}
		}

		[DataSourceProperty]
		public float KeepWagesValue
		{
			get
			{
				return this._keepWagesValue;
			}
			set
			{
				if (value != this._keepWagesValue)
				{
					this._keepWagesValue = value;
					this.KeepWagesValueAsString = this.KeepWagesValue.ToString();
					base.OnPropertyChanged("KeepWagesValue");
				}
			}
		}

		[DataSourceProperty]
		public string KeepWagesValueAsString
		{
			get
			{
				return this._keepWagesValueAsString;
			}
			set
			{
				if (value != this._keepWagesValueAsString)
				{
					this._keepWagesValueAsString = value;
					base.OnPropertyChanged("KeepWagesValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public float SearchRadiusValue
		{
			get
			{
				return this._searchRadiusValue;
			}
			set
			{
				if (value != this._searchRadiusValue)
				{
					this._searchRadiusValue = value;
					if (this._searchRadiusValue > 999.0f)
						this.SearchRadiusValueAsString = this.InfText;
					else
						this.SearchRadiusValueAsString = this.SearchRadiusValue.ToString();
					base.OnPropertyChanged("SearchRadiusValue");
				}
			}
		}

		[DataSourceProperty]
		public string SearchRadiusValueAsString
		{
			get
			{
				return this._searchRadiusValueAsString;
			}
			set
			{
				if (value != this._searchRadiusValueAsString)
				{
					this._searchRadiusValueAsString = value;
					base.OnPropertyChanged("SearchRadiusValueAsString");
				}
			}
		}

		[DataSourceProperty]
		public float WeaponsArmorTierValue
		{
			get
			{
				return this._weaponsArmorTierValue;
			}
			set
			{
				if (value != this._weaponsArmorTierValue)
				{
					this._weaponsArmorTierValue = value;
					this.WeaponsArmorTierValueAsString = this.WeaponsArmorTierValue.ToString();
					base.OnPropertyChanged("WeaponsArmorTierValue");
				}
			}
		}

		[DataSourceProperty]
		public string WeaponsArmorTierValueAsString
		{
			get
			{
				return this._weaponsArmorTierValueAsString;
			}
			set
			{
				if (value != this._weaponsArmorTierValueAsString)
				{
					this._weaponsArmorTierValueAsString = value;
					base.OnPropertyChanged("WeaponsArmorTierValueAsString");
				}
			}
		}


	}
}
