using AutoTrader.GUI;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;
using TaleWorlds.MountAndBlade.View.Screens;

namespace AutoTrader
{
	[GameStateScreen(typeof(AutoTraderState))]
	class AutoTradeMenuGauntletScreen : ScreenBase, IGameStateListener, IAutoTraderStateHandler
    {
		private GauntletLayer _gauntletLayer;
		private AutoTraderMenuViewModel _viewModel;
		private AutoTraderState _autoTraderState;

		public AutoTradeMenuGauntletScreen(AutoTraderState autoTraderState)
		{
			this._autoTraderState = autoTraderState;
			this._autoTraderState.Handler = this;
		}

		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			LoadingWindow.DisableGlobalLoadingWindow();
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				this.CloseAutoTraderScreen();
			}
		}

		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
			this._viewModel = new AutoTraderMenuViewModel(new System.Action(this.CloseAutoTraderScreen));

			// Add and configure layers
			this._gauntletLayer = new GauntletLayer("GauntletLayer", 1);

			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._gauntletLayer.IsFocusLayer = true;
			this._gauntletLayer.LoadMovie("AutoTraderConfigScreen", this._viewModel);

			base.AddLayer(this._gauntletLayer);
		}

		private void CloseAutoTraderScreen()
		{
			Game.Current.GameStateManager.PopState(0);
		}

		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
			base.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
		}

		void IGameStateListener.OnInitialize()
		{
		}

		void IGameStateListener.OnFinalize()
		{
			this._gauntletLayer = null;
			this._viewModel = null;
		}
	}
}
