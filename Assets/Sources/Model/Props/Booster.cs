using Model.Sources.Model.Movement;
using Model.Timers;

namespace Model.Props
{
	public class Booster : MovementStatsDecorator, ITickable
	{
		[System.Serializable]
		public struct Preferences
		{
			public float Time;
			public float SpeedMultiplier;
		}

		private readonly Preferences _preferences;
		private readonly Timer _timer = new Timer();
		
		public Booster(Preferences preferences, IMovementStatsProvider wrappedEntity) : base(wrappedEntity)
		{
			_preferences = preferences;
		}

		public void Apply()
		{
			_timer.Start(_preferences.Time);
		}

		public void Tick(float deltaTime)
		{
			_timer.Tick(deltaTime);
		}

		protected override MovementStats Decorate(IMovementStatsProvider statsProvider)
		{
			MovementStats stats = statsProvider.Stats();

			return _timer.IsOver
				? statsProvider.Stats()
				: new MovementStats(stats.MaxSpeed * _preferences.SpeedMultiplier, stats.AccelerationTime);
		}
	}
}