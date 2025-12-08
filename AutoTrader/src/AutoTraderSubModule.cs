using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Library;
using TaleWorlds.InputSystem;
using AutoTrader.GUI;

namespace AutoTrader
{
    public class AutoTraderSubModule : MBSubModuleBase
    {
		private AutoTraderLogic _autoTraderLogic;

		public AutoTraderSubModule()
		{
			//var harmony = new Harmony("eskalior.autotrader");
			//harmony.PatchAll(typeof(AutoTraderSubModule).Assembly);
		}

		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			AutoTraderConfig.Initialize();
			AutoTraderHelpers.Initialize();
			_autoTraderLogic = new AutoTraderLogic(new AutoTraderLogicConnector());
		}

		public override void OnGameInitializationFinished(Game game)
		{
			string t_text = AutoTraderConfig.UseAltATValue ? " + T" : "";
			AutoTraderHelpers.PrintMessage(new TextObject("{=ATStartup01}Thanks for using AutoTrader! Press <ALT + A", null)
				+ t_text
				+ new TextObject("{=ATStartup02}> to open the settings menu", null).ToString());

			string version = ApplicationVersion.FromParametersFile(null).ToString().Substring(0, 7);

			if (!version.Equals(AutoTraderConfig.AutoTraderGameVersion)){
				AutoTraderHelpers.PrintMessage(new TextObject("{=ATVersionMismatch01}You are using AutoTrader for ", null).ToString()
					+ AutoTraderConfig.AutoTraderGameVersion
					+ new TextObject("{=ATVersionMismatch02} with Bannerlord ", null).ToString() 
					+ version
					+ new TextObject("{=ATVersionMismatch03}. If you encounter issues please check the mod page for a fitting version.", null).ToString());
			}

			if (AutoTraderConfig.DebugMode)
			{
				AutoTraderHelpers.PrintMessage("WARNING: AutoTrader debug mode is active. This will make autotrading drastically slower.");
			}
		}

		private void OpenSettingsMenu()
		{
			Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<AutoTraderState>(), 0);
		}


		protected override void OnApplicationTick(float dt)
		{
			if(Game.Current != null && !_autoTraderLogic.IsTradingActive)
			{
				if(Input.IsKeyDown(InputKey.LeftAlt) 
					&& Input.IsKeyDown(InputKey.A) 
					&& (!AutoTraderConfig.UseAltATValue || Input.IsKeyDown(InputKey.T))
					&& Game.Current.GameStateManager.ActiveState.GetType() == typeof(MapState)
					&& Game.Current.GameStateManager.ActiveState.IsMenuState == false
					&& Game.Current.GameStateManager.ActiveState.GetType() != typeof(MissionState)
					)
                {
					OpenSettingsMenu();
				}
			}
		}

		protected override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
		{
			if (game.GameType is Campaign)
			{
				CampaignGameStarter gameInitializer = (CampaignGameStarter)gameStarterObject;
				this.AddBehaviors(gameInitializer);
			}
		}

		private void AddBehaviors(CampaignGameStarter gameStarterObject)
		{
			gameStarterObject.AddBehavior(new TradeBehavior(_autoTraderLogic));
		}

	}
}
