using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace c04.risso.francesco {


	public class NPCcontroller : MonoBehaviour {
		protected House _house;

		[SerializeField]
		private List<GameObject> _prefabs;

		public NPC Spawn(House house) {
			int childType = UnityEngine.Random.Range(0, _prefabs.Count);
			Quaternion rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);
			GameObject npc_obj = Instantiate(_prefabs[childType], house.SelectRandomPoint(), rotation);

			NPC npc = npc_obj.GetComponent<NPC>();
			npc.setHouse(house);

			return npc;
		}

		void Start() {
			Assert.AreNotEqual(0, _prefabs.Count, "The NPC controller does not have any prefabs to spawn");
		}
	}

}
