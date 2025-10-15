using TaleWorlds.Core;

namespace AutoTrader.GUI
{
    internal class AutoTraderState : GameState
    {
		public IAutoTraderStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		private IAutoTraderStateHandler _handler;
	}
}
