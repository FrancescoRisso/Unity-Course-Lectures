using UnityEngine;
using UnityEngine.Assertions;

namespace c04.risso.francesco {


	public class House : Building {
		[SerializeField]
		private Vector2Int _min_max_npcs;

		private int _min_npcs, _max_npcs;

		protected override void Start() {
			base.Start();

			_min_npcs = _min_max_npcs.x;
			_max_npcs = _min_max_npcs.y;
			Assert.IsTrue(
				_min_npcs <= _max_npcs, $"{name} cannot have a number of NPCs between {_min_npcs} (min) and {_max_npcs} (max): invalid range");
			Assert.IsTrue(_min_npcs >= 0, $"{name} cannot have less than 0 NPCs");

			int num_NPCs = Random.Range(_min_npcs, _max_npcs);

			NPCcontroller npc_spawner = FindObjectOfType<NPCcontroller>();
			Assert.IsNotNull(npc_spawner, $"{name} could not find the NPC controller");

			for(int i = 0; i < num_NPCs; i++) npc_spawner.Spawn(this);
		}
	}

}
