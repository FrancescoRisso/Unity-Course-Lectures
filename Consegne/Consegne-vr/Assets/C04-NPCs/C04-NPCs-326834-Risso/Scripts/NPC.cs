using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace c04.risso.francesco {

	public enum NPCtype { Student, Adult, Eleder }

	enum Place { Home, Place, Park }


	public class NPC : MonoBehaviour {
		private House _house;

		private float _timeInHouse, _timeInPlace, _timeInPark;

		private Place _currentPlace = Place.Home;

		private NavMeshAgent _navAgent;

		[SerializeField]
		private Vector2 _minMaxWaitTime;

		[SerializeField]
		private NPCtype _type;

		private bool _waiting = false;

		public void setHouse(House house) {
			_house = house;
		}

		void Start() {
			float min = _minMaxWaitTime.x;
			float max = _minMaxWaitTime.y;

			Assert.IsTrue(min >= 0, "NPC wait times cannot be negative");
			Assert.IsTrue(min <= max, $"NPC max stop time ({max}) cannot be less than the min stop time ({min})");

			_timeInHouse = UnityEngine.Random.Range(min, max);
			_timeInPlace = UnityEngine.Random.Range(min, max);
			_timeInPark = UnityEngine.Random.Range(min, max);

			_navAgent = GetComponent<NavMeshAgent>();
			Assert.IsNotNull(_navAgent, $"NPC {name} could not find its own NavMeshAgent");

			_currentPlace = GetNextPlace();
			_navAgent.SetDestination(GetTargetBuilding().SelectRandomPoint());
		}

		void Update() {
			if(!_waiting && Arrived()) StartCoroutine(WaitAndSetNextPoint());
		}

		Building GetOwnPlaceBuilding() {
			switch(_type) {
				case NPCtype.Student: return FindAnyObjectByType<University>();
				case NPCtype.Adult: return FindAnyObjectByType<Office>();
				case NPCtype.Eleder: return FindAnyObjectByType<Library>();
			};
			throw new Exception($"_type = {_type} is not valid");
		}

		Building GetTargetBuilding() {
			switch(_currentPlace) {
				case Place.Home: return _house;
				case Place.Park: return FindAnyObjectByType<Park>();
				case Place.Place: return GetOwnPlaceBuilding();
			};
			throw new Exception($"_currentPlace = {_currentPlace} is not valid");
		}
		Place GetNextPlace() {
			switch(_currentPlace) {
				case Place.Home: return Place.Place;
				case Place.Place: return Place.Park;
				case Place.Park: return Place.Home;
			};
			throw new Exception($"_currentPlace = {_currentPlace} is not valid");
		}

		IEnumerator WaitAndSetNextPoint() {
			_waiting = true;

			switch(_currentPlace) {
				case Place.Home: yield return new WaitForSeconds(_timeInHouse); break;
				case Place.Place: yield return new WaitForSeconds(_timeInPlace); break;
				case Place.Park: yield return new WaitForSeconds(_timeInPark); break;
			}

			_currentPlace = GetNextPlace();
			_navAgent.SetDestination(GetTargetBuilding().SelectRandomPoint());

			_waiting = false;
		}

		bool Arrived() {
			return (!_navAgent.pathPending) && (_navAgent.remainingDistance <= _navAgent.stoppingDistance) &&
				   (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0f);
		}
	}

}

// TODO: controllare perchè ogni tanto si muovono a caso nello stesso posto, oppure saltano gli step
