using System.Collections.Generic;
using System.Linq;
using Model.Stickmen;
using Sources.CompositeRoot.Base;
using UnityEngine;

namespace Sources.CompositeRoot.Enemies
{
	public class EnemiesCompositionRoot : CompositionRoot
	{
		[SerializeField] private EnemyGroup[] _enemyGroups;

		private List<Entity> _entities;

		public override void Compose() =>
			_entities = _enemyGroups
				.SelectMany(x => x.Models())
				.ToList();

		public IEnumerable<Entity> Entities() => 
			_entities.Where(x => x.IsDead == false);
	}
}