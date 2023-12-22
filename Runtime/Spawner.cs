using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

        private List<SpawnClass> activeList = new List<SpawnClass>();
		private List<SpawnClass> inactiveList = new List<SpawnClass>();

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
			return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, null);
		}

		public SpawnClass Spawn(Transform transform) {
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, null);
		}

		public SpawnClass Spawn(Transform transform, Transform parentObject) {
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, parentObject);
		}

		public SpawnClass Spawn(Vector3 position) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, null);
		}

		public SpawnClass Spawn(Vector3 position, Transform parentObject) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, parentObject);
		}

		public SpawnClass Spawn(Vector3 position, Quaternion rotation) {
			return this.Spawn(this.prefabDefault, position, rotation, null);
		}

		public SpawnClass Spawn(Vector3 position, Quaternion rotation, Transform parentObject) {
			return this.Spawn(this.prefabDefault, position, rotation, parentObject);
		}

		public SpawnClass Spawn(SpawnClass prefab) {
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Transform transform) {
			return this.Spawn(prefab, transform.position, transform.rotation, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Transform transform, Transform parentObject) {
			return this.Spawn(prefab, transform.position, transform.rotation, parentObject);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position) {
			return this.Spawn(prefab, position, Quaternion.identity, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Transform parentObject) {
			return this.Spawn(prefab, position, Quaternion.identity, parentObject);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Quaternion rotation) {
			return this.Spawn(prefab, position, rotation, null);
		}

		public SpawnClass Spawn(SpawnClass prefab, Vector3 position, Quaternion rotation, Transform parentObject) {
			SpawnClass spawnling;

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
			}
			else {
				spawnling = GameObject.Instantiate(prefab, position, rotation);
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
				Object.Destroy(spawnling.gameObject);
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