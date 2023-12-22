﻿using System;
using Model.Physics;
using Model.Sources.Model.Movement;
using UnityEngine;

namespace Model.Stickmen
{
	public class StickmanMovement
	{
		public readonly Entity Model;
		
		private readonly SurfaceSliding _surfaceSliding;
		private readonly float _distanceBetweenBounds;

		private InertialMovement _inertialMovement;
		private Vector3 _startMovePosition;
		
		public StickmanMovement(Entity model, SurfaceSliding surfaceSliding, InertialMovement inertialMovement, float distanceBetweenBounds)
		{
			Model = model;
			_surfaceSliding = surfaceSliding;
			_inertialMovement = inertialMovement;
			_distanceBetweenBounds = distanceBetweenBounds;
		}

		public bool OnRightBound => Math.Abs(Model.Position.x - DistanceToBound) < 0.1f;

		public bool OnLeftBound => Math.Abs(Model.Position.x - -DistanceToBound) < 0.1f;

		public float Acceleration => _inertialMovement.Acceleration;
		
		private float DistanceToBound => _distanceBetweenBounds / 2.0f;

		public void Bind(InertialMovement movement)
		{
			_inertialMovement = movement;
		}
		
		public void StartMovingRight()
		{
			_startMovePosition = Model.Position;
		}

		public void MoveRight(float axis)
		{
			Vector3 position = new Vector3
			{
				x = _distanceBetweenBounds * axis + _startMovePosition.x,
				y = Model.Position.y,
				z = Model.Position.z
			};

			position.x = Mathf.Clamp(position.x, -DistanceToBound, DistanceToBound);
			
			Model.Move(position);
		}

		public void Accelerate(float deltaTime)
		{
			_inertialMovement.Accelerate(deltaTime);

			MoveForward(deltaTime);
		}

		public void Slowdown(float deltaTime)
		{
			_inertialMovement.Slowdown(deltaTime);
			
			MoveForward(deltaTime);
		}

		private void MoveForward(float deltaTime)
		{
			Vector3 directionAlongSurface = _surfaceSliding.DirectionAlongSurface(Model.Forward);
			Vector3 delta = directionAlongSurface * (_inertialMovement.Acceleration * deltaTime);

			Model.Move(Model.Position + delta);
		}
	}
}