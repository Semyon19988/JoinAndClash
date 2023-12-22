namespace Model.Sources.Model.Movement
{
	public interface IMovementStatsProvider
	{
		MovementStats Stats();
		
		public class None : IMovementStatsProvider
		{
			public MovementStats Stats()
			{
				return new MovementStats(0.0f, 1.0f);
			}
		}
	}
}