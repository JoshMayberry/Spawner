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

	public class UnitySpawner<T> : IEnumerable<T> where T : Component, ISpawnable {
        [SerializeField] private T prefabDefault;

		private List<T> activeList = new List<T>();
		private List<T> inactiveList = new List<T>();

		public UnitySpawner() {
		}

		public UnitySpawner(T prefab) {
			this.prefabDefault = prefab;
		}

		public void Initialize(List<T> existingList) {
			foreach (T spawnling in existingList) {
				spawnling.gameObject.SetActive(true);
				this.activeList.Add(spawnling);
				spawnling.OnSpawn(this);
			}
		}

		public void SetPrefabDefault(T prefabDefault) {
			this.prefabDefault= prefabDefault;
		}

		public T Spawn() {
			return this.Spawn(this.prefabDefault, Vector3.zero, Quaternion.identity, null);
		}

		public T Spawn(Transform transform) {
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, null);
		}

		public T Spawn(Transform transform, Transform parentObject) {
			return this.Spawn(this.prefabDefault, transform.position, transform.rotation, parentObject);
		}

		public T Spawn(Vector3 position) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, null);
		}

		public T Spawn(Vector3 position, Transform parentObject) {
			return this.Spawn(this.prefabDefault, position, Quaternion.identity, parentObject);
		}

		public T Spawn(Vector3 position, Quaternion rotation) {
			return this.Spawn(this.prefabDefault, position, rotation, null);
		}

		public T Spawn(Vector3 position, Quaternion rotation, Transform parentObject) {
			return this.Spawn(this.prefabDefault, position, rotation, parentObject);
		}

		public T Spawn(T prefab) {
			return this.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
		}

		public T Spawn(T prefab, Transform transform) {
			return this.Spawn(prefab, transform.position, transform.rotation, null);
		}

		public T Spawn(T prefab, Transform transform, Transform parentObject) {
			return this.Spawn(prefab, transform.position, transform.rotation, parentObject);
		}

		public T Spawn(T prefab, Vector3 position) {
			return this.Spawn(prefab, position, Quaternion.identity, null);
		}

		public T Spawn(T prefab, Vector3 position, Transform parentObject) {
			return this.Spawn(prefab, position, Quaternion.identity, parentObject);
		}

		public T Spawn(T prefab, Vector3 position, Quaternion rotation) {
			return this.Spawn(prefab, position, rotation, null);
		}

		public T Spawn(T prefab, Vector3 position, Quaternion rotation, Transform parentObject) {
			T spawnling;

			if (this.inactiveList.Count > 0) {
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

			spawnling.gameObject.SetActive(true);
			this.activeList.Add(spawnling);
			spawnling.OnSpawn(this);
			return spawnling;
		}

		public void Despawn(T spawnling) {
			this.activeList.Remove(spawnling);
			this.inactiveList.Add(spawnling);
			spawnling.OnDespawn(this);
			spawnling.gameObject.SetActive(false);
		}

		public void DespawnAll() {
			foreach (T spawnling in this.activeList) {
				this.inactiveList.Add(spawnling);
				spawnling.OnDespawn(this);
				spawnling.gameObject.SetActive(false);
			}

			this.activeList = new List<T>();
        }

        public bool ShouldBeActive(T spawnling) {
            if (this.activeList.Contains(spawnling)) {
                return false;
            }

            if (this.inactiveList.Contains(spawnling)) {
                this.inactiveList.Remove(spawnling);
            }

            spawnling.gameObject.SetActive(true);
            this.activeList.Add(spawnling);
            spawnling.OnSpawn(this);
            return true;
        }

        public bool ShouldBeInactive(T spawnling) {
            if (this.inactiveList.Contains(spawnling)) {
                return false;
            }

            if (this.activeList.Contains(spawnling)) {
                this.activeList.Remove(spawnling);
            }

            this.inactiveList.Add(spawnling);
            spawnling.OnDespawn(this);
            spawnling.gameObject.SetActive(true);
            return true;
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (T spawnling in this.activeList) {
                yield return spawnling;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

	public class CodeSpawner<T> : IEnumerable<T> where T : ISpawnable, new() {
		private List<T> activeList = new List<T>();
		private List<T> inactiveList = new List<T>();

		public void Initialize(List<T> existingList) {
			foreach (T spawnling in existingList) {
				activeList.Add(spawnling);
				spawnling.OnSpawn(this);
			}
		}

		public T Spawn() {
			T spawnling;

			if (inactiveList.Count > 0) {
				spawnling = inactiveList[inactiveList.Count - 1];
				inactiveList.RemoveAt(inactiveList.Count - 1);
			}
			else {
				spawnling = new T();
			}

			activeList.Add(spawnling);
			spawnling.OnSpawn(this);
			return spawnling;
		}

		public void Despawn(T spawnling) {
			activeList.Remove(spawnling);
			inactiveList.Add(spawnling);
			spawnling.OnDespawn(this);
		}

		public void DespawnAll() {
			foreach (T spawnling in activeList) {
				inactiveList.Add(spawnling);
				spawnling.OnDespawn(this);
			}

			this.activeList = new List<T>();
        }

        public bool ShouldBeActive(T spawnling) {
            if (this.activeList.Contains(spawnling)) {
                return false;
            }

            if (this.inactiveList.Contains(spawnling)) {
                this.inactiveList.Remove(spawnling);
            }

            this.activeList.Add(spawnling);
            spawnling.OnSpawn(this);
            return true;
        }

        public bool ShouldBeInactive(T spawnling) {
            if (this.inactiveList.Contains(spawnling)) {
                return false;
            }

            if (this.activeList.Contains(spawnling)) {
                this.activeList.Remove(spawnling);
            }

            this.inactiveList.Add(spawnling);
            spawnling.OnDespawn(this);
            return true;
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (T spawnling in this.activeList) {
                yield return spawnling;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}