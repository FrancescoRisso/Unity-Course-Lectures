using UnityEngine;
using UnityEngine.Assertions;

namespace c04.risso.francesco {

	public enum BuildingType { Park, University, Office, Library, House }


	public abstract class Building : MonoBehaviour {
		private Component _ground;

		private float maxX, maxZ, minX, minZ;

		protected virtual void Start() {
			MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>(true);
			if(children.Length == 1)
				_ground = children[0];
			else
				foreach(MeshRenderer child in children)
					if(child.gameObject.name == "ground") _ground = child;


			Assert.IsNotNull(_ground, $"{name} cannot find its ground");

			Vector3 topRigth = gameObject.transform.TransformPoint(new Vector3(5, 0, 5));
			Vector3 topLeft = gameObject.transform.TransformPoint(new Vector3(-5, 0, 5));
			Vector3 bottomLeft = gameObject.transform.TransformPoint(new Vector3(-5, 0, -5));
			Vector3 bottomRight = gameObject.transform.TransformPoint(new Vector3(5, 0, -5));

			maxX = Mathf.Max(topLeft.x, bottomRight.x, topRigth.x, bottomLeft.x);
			maxZ = Mathf.Max(topLeft.z, bottomRight.z, topRigth.z, bottomLeft.z);
			minX = Mathf.Min(topLeft.x, bottomRight.x, topRigth.x, bottomLeft.x);
			minZ = Mathf.Min(topLeft.z, bottomRight.z, topRigth.z, bottomLeft.z);
		}

		public Vector3 SelectRandomPoint() {
			return new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
		}
	}

}
