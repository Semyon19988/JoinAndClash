﻿using UnityEngine;

namespace Model.Sources.Model.Movement
{
	public class InertialMovement
	{
		private IMovementStatsProvider _provider;

		public InertialMovement(IMovementStatsProvider provider)
		{
			_provider = provider;
		}

		public float Acceleration { get; private set; }

		private MovementStats Stats => _provider.Stats();

		public void Bind(IMovementStatsProvider provider)
		{
			_provider = provider;
		}

		public void Accelerate(float deltaTime)
		{
			Acceleration += Stats.MaxSpeed * (deltaTime / Stats.AccelerationTime);
			Acceleration = Mathf.Clamp(Acceleration, 0.0f, Stats.MaxSpeed);
		}

		public void Slowdown(float deltaTime)
		{
			Acceleration -= Acceleration * (deltaTime / Stats.AccelerationTime);
		}
	}
}