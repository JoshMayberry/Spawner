using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace jmayberry.Spawner {

	/**
	 * Example OnSpawn:
	 *  public void SomeMethod(object spawner) {
	 *      if (spawner is UnitySpawner<EnemyBase> mySpawner) {
	 *          this.mySpawner = mySpawner
	 *      }
	 *  }
	 */
	public interface ISpawnable {
		void OnSpawn(object spawner);
		void OnDespawn(object spawner);
	}

	public interface ISpawner<SpawnClass> where SpawnClass : ISpawnable {
		SpawnClass Spawn();

		void Despawn(SpawnClass spawnling);
	}

	public class UnitySpawner<SpawnClass> : IEnumerable<SpawnClass>, ISpawner<SpawnClass> where SpawnClass : Component, ISpawnable {
		[SerializeField] private SpawnClass prefabDefault;
		public bool usePooling = true;
		public bool destroyUnpooled = true;
		public int maxSpawns = 0; // If 0 will not cap the spawns
		public int nextSpawnlingIndex = 0;

		private List<SpawnClass> activeList = new List<SpawnClass>();
		private List<SpawnClass> inactiveList = new List<SpawnClass>();

		public delegate void SpawnConstructor(SpawnClass spawnling);

		public UnitySpawner() {
		}

		public UnitySpawner(SpawnClass prefab) {
			this.prefabDefault = prefab;
		}

		public void Initialize(List<SpawnClass> existingList) {
			foreach (SpawnClass spawnling in existingList) {
				this.moveToActiveList(spawnling);
			}
		}

		public void SetPrefabDefault(SpawnClass prefabDefault) {
			this.prefabDefault= prefabDefault;
		}

		public SpawnClass Spawn() {
			return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, null, null);
		}

		public SpawnClass Spawn(SpawnConstructor spawnConstructor) {
			return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, null, spawnConstructor);
		}

		public SpawnClass Spawn(Transform transform) {
			if (transform == null) {
				return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, null, null);
			}
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, null, null);
		}

		public SpawnClass Spawn(Transform transform, SpawnConstructor spawnConstructor) {
			if (transform == null) {
				return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, null, spawnConstructor);
			}
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, null, spawnConstructor);
		}

		public SpawnClass Spawn(Transform transform, Transform parentObject) {
			if (transform == null) {
				return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, parentObject, null);
			}
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, parentObject, null);
		}
		public SpawnClass Spawn(Transform transform, Transform parentObject, SpawnConstructor spawnConstructor) {
			if (transform == null) {
				return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, parentObject, spawnConstructor);
			}
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, parentObject, spawnConstructor);
		}

		public SpawnClass Spawn(Vector3 position) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, null, null);
		}

		public SpawnClass Spawn(Vector3 position, SpawnConstructor spawnConstructor) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, null, spawnConstructor);
		}

		public SpawnClass Spawn(Vector3 position, Transform parentObject) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, parentObject, null);
		}

		public SpawnClass Spawn(Vector3 position, Transform parentObject, SpawnConstructor spawnConstructor) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, parentObject, spawnConstructor);
		}

		public SpawnClass Spawn(Vector3 position, Quaternion rotation) {
			return this.Spawn(this.prefabDefault, position, rotation, null, null);
		}

		public SpawnClass Spawn(Vector3 position, Quaternion rotation, SpawnConstructor spawnConstructor) {
			return this.Spawn(this.prefabDefault, position, rotation, null, spawnConstructor);
		}

		public SpawnClass Spawn(Vector3 position, Quaternion rotation, Transform parentObject) {
			return this.Spawn(this.prefabDefault, position, rotation, parentObject, null);
		}

		public SpawnClass Spawn(Vector3 position, Quaternion rotation, Transform parentObject, SpawnConstructor spawnConstructor) {
			return this.Spawn(this.prefabDefault, position, rotation, parentObject, spawnConstructor);
		}

		public SpawnClass Spawn(SpawnClass prefab) {
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, SpawnConstructor spawnConstructor) {
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null, spawnConstructor);
		}

		public SpawnClass Spawn(SpawnClass prefab, Transform transform) {
			if (transform == null) {
				return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null, null);
			}
			return this.Spawn(prefab, transform.position, transform.rotation, null, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Transform transform, SpawnConstructor spawnConstructor) {
			if (transform == null) {
				return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null, spawnConstructor);
			}
			return this.Spawn(prefab, transform.position, transform.rotation, null, spawnConstructor);
		}

		public SpawnClass Spawn(SpawnClass prefab, Transform transform, Transform parentObject) {
			if (transform == null) {
				return this.Spawn(prefab, Vector3.zero, Quaternion.identity, parentObject, null);
			}
			return this.Spawn(prefab, transform.position, transform.rotation, parentObject, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Transform transform, Transform parentObject, SpawnConstructor spawnConstructor) {
			if (transform == null) {
				return this.Spawn(prefab, Vector3.zero, Quaternion.identity, parentObject, spawnConstructor);
			}
			return this.Spawn(prefab, transform.position, transform.rotation, parentObject, spawnConstructor);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position) {
			return this.Spawn(prefab, position, Quaternion.identity, null, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, SpawnConstructor spawnConstructor) {
			return this.Spawn(prefab, position, Quaternion.identity, null, spawnConstructor);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Transform parentObject) {
			return this.Spawn(prefab, position, Quaternion.identity, parentObject, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Transform parentObject, SpawnConstructor spawnConstructor) {
			return this.Spawn(prefab, position, Quaternion.identity, parentObject, spawnConstructor);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Quaternion rotation) {
			return this.Spawn(prefab, position, rotation, null, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Quaternion rotation, SpawnConstructor spawnConstructor) {
			return this.Spawn(prefab, position, rotation, null, spawnConstructor);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Quaternion rotation, Transform parentObject) {
			return this.Spawn(prefab, position, rotation, parentObject, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Quaternion rotation, Transform parentObject, SpawnConstructor spawnConstructor) {
			SpawnClass spawnling = null;
			if ((this.maxSpawns > 0) && (this.ActiveCount() >= this.maxSpawns)) {
				return spawnling;
			}

			if (prefab == null) {
				prefab = this.prefabDefault;
			}

			if (position == null) {
				position = Vector3.zero;
			}

			if (rotation == null) {
				rotation = Quaternion.identity;
			}

			if (this.usePooling && (this.inactiveList.Count > 0)) {
				spawnling = this.inactiveList[this.inactiveList.Count - 1];
				this.inactiveList.RemoveAt(this.inactiveList.Count - 1);
				spawnling.transform.position = position;
				spawnling.transform.rotation = rotation;

				if ((parentObject != null) && (spawnling.transform.parent != parentObject.transform)) {
					spawnling.transform.SetParent(parentObject.transform);
				}
			}
			else if (parentObject != null) {
				spawnling = GameObject.Instantiate(prefab, position, rotation, parentObject);
				if (spawnConstructor != null) {
					spawnConstructor(spawnling);
				}
			}
			else {
				spawnling = GameObject.Instantiate(prefab, position, rotation);
				if (spawnConstructor != null) {
					spawnConstructor(spawnling);
				}
			}

			this.moveToActiveList(spawnling);
			return spawnling;
		}

		public void Despawn(SpawnClass spawnling) {
			this.activeList.Remove(spawnling);
			this.moveToInactiveList(spawnling);
		}

		public void DespawnAll() {
			foreach (SpawnClass spawnling in this.activeList) {
				this.moveToInactiveList(spawnling);
			}

			this.activeList = new List<SpawnClass>();
		}

		public bool ShouldBeActive(SpawnClass spawnling) {
			if (this.activeList.Contains(spawnling)) {
				return false;
			}

			if (this.inactiveList.Contains(spawnling)) {
				this.inactiveList.Remove(spawnling);
			}

			this.moveToActiveList(spawnling);
			return true;
		}

		public bool ShouldBeInactive(SpawnClass spawnling) {
			if (this.inactiveList.Contains(spawnling)) {
				return false;
			}

			if (this.activeList.Contains(spawnling)) {
				this.activeList.Remove(spawnling);
			}

			this.moveToInactiveList(spawnling);
			return true;
		}

		public IEnumerator<SpawnClass> GetEnumerator() {
			foreach (SpawnClass spawnling in this.activeList) {
				yield return spawnling;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		private void moveToActiveList(SpawnClass spawnling) {
			spawnling.gameObject.SetActive(true);
			this.activeList.Add(spawnling);
			spawnling.OnSpawn(this);
		}

		private void moveToInactiveList(SpawnClass spawnling) {
			if (this.usePooling) {
				spawnling.OnDespawn(this);
				this.inactiveList.Add(spawnling);
				spawnling.gameObject.SetActive(false);
				return;
			}

			spawnling.OnDespawn(this);
			spawnling.gameObject.SetActive(false);

			if (this.destroyUnpooled) {
				UnityEngine.Object.Destroy(spawnling.gameObject);
			}
		}

		public bool IsActive(SpawnClass spawnling) {
			return this.activeList.Contains(spawnling);
		}

		public bool IsInactive(SpawnClass spawnling) {
			return this.inactiveList.Contains(spawnling);
		}

		public int ActiveCount() {
			return this.activeList.Count;
		}

		public int InactiveCount() {
			return this.inactiveList.Count;
		}

		public SpawnClass GetSpawnling(int index, Transform transform=null, Transform parentObject=null, SpawnConstructor spawnConstructor=null, bool forceIndex=false) {
			int activeCount = this.ActiveCount();

			if (activeCount > index) {
				return this.activeList[index];
			}

			if (!forceIndex) {
				return this.Spawn(transform, parentObject, spawnConstructor);
			}

			SpawnClass spawnling = null;
			for (int i = activeCount; i < activeCount + index; i++) {
				spawnling = this.Spawn(transform, parentObject, spawnConstructor);
			}
			return spawnling;
		}

		public void SetNextSpawnling(int index) {
			this.nextSpawnlingIndex = index;
		}

		public SpawnClass GetNextSpawnling(Transform transform=null, Transform parentObject=null, SpawnConstructor spawnConstructor=null, bool forceIndex=false) {
			SpawnClass spawnling = this.GetSpawnling(this.nextSpawnlingIndex, transform, parentObject, spawnConstructor, forceIndex);
			if (spawnling == null) {
				return null;
			}

			if (!forceIndex) {
				this.nextSpawnlingIndex = this.ActiveCount();
			}
			else {
				this.nextSpawnlingIndex++;
			}
			return spawnling;
		}
	}

	public class CodeSpawner<SpawnClass> : IEnumerable<SpawnClass>, ISpawner<SpawnClass> where SpawnClass : ISpawnable, new() {
		public bool usePooling = true;
		private List<SpawnClass> activeList = new List<SpawnClass>();
		private List<SpawnClass> inactiveList = new List<SpawnClass>();

		public void Initialize(List<SpawnClass> existingList) {
			foreach (SpawnClass spawnling in existingList) {
				this.moveToActiveList(spawnling);
			}
		}

		public SpawnClass Spawn() {
			SpawnClass spawnling;

			if (usePooling && (inactiveList.Count > 0)) {
				spawnling = inactiveList[inactiveList.Count - 1];
				inactiveList.RemoveAt(inactiveList.Count - 1);
			}
			else {
				spawnling = new SpawnClass();
			}

			this.moveToActiveList(spawnling);
			return spawnling;
		}

		public void Despawn(SpawnClass spawnling) {
			activeList.Remove(spawnling);
			this.moveToInactiveList(spawnling);
		}

		public void DespawnAll() {
			foreach (SpawnClass spawnling in activeList) {
				this.moveToInactiveList(spawnling);
			}

			this.activeList = new List<SpawnClass>();
		}

		public bool ShouldBeActive(SpawnClass spawnling) {
			if (this.activeList.Contains(spawnling)) {
				return false;
			}

			if (this.inactiveList.Contains(spawnling)) {
				this.inactiveList.Remove(spawnling);
			}

			this.moveToActiveList(spawnling);
			return true;
		}

		public bool ShouldBeInactive(SpawnClass spawnling) {
			if (this.inactiveList.Contains(spawnling)) {
				return false;
			}

			if (this.activeList.Contains(spawnling)) {
				this.activeList.Remove(spawnling);
			}

			this.moveToInactiveList(spawnling);
			return true;
		}

		public IEnumerator<SpawnClass> GetEnumerator() {
			foreach (SpawnClass spawnling in this.activeList) {
				yield return spawnling;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		private void moveToActiveList(SpawnClass spawnling) {
			this.activeList.Add(spawnling);
			spawnling.OnSpawn(this);
		}

		private void moveToInactiveList(SpawnClass spawnling) {
			if (this.usePooling) {
				spawnling.OnDespawn(this);
				this.inactiveList.Add(spawnling);
				return;
			}

			spawnling.OnDespawn(this);
			// It will be garbage collected?
		}

		public bool IsActive(SpawnClass spawnling) {
			return this.activeList.Contains(spawnling);
		}

		public bool IsInactive(SpawnClass spawnling) {
			return this.inactiveList.Contains(spawnling);
		}

		public int ActiveCount() {
			return this.activeList.Count;
		}

		public int InactiveCount() {
			return this.inactiveList.Count;
		}
	}
}