using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine;

namespace Services {
    public class ObjectPoolService<T>: IObjectPoolService<T> where T : MonoBehaviour {
        private readonly Queue<T> pool = new Queue<T>();
        private readonly Transform cellsParent;
        private readonly GameObject prefab;

        public ObjectPoolService(Transform cellsParent, GameObject prefab) {
            this.cellsParent = cellsParent;
            this.prefab = prefab;
        }
        
        /// <summary>
        /// Get a cell from the pool or instantiate a new one if none is available
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public T Get(int x, int y) {
            Vector3 position = new Vector3(x, y, 0);
            T t;
            
            if (this.pool.Count > 0) {
                t = this.pool.Dequeue();
                t.transform.position = position;
                t.gameObject.SetActive(true);
                return t;
            }

            GameObject gameObject = Object.Instantiate(this.prefab, position, Quaternion.identity, this.cellsParent);
            gameObject.name = $"Cell ({x}, {y})";
            return gameObject.GetComponent<T>();
        }

        /// <summary>
        /// Return a cell to the pool
        /// </summary>
        /// <param name="t"></param>
        public void Release(T t) {
            t.gameObject.SetActive(false);
            this.pool.Enqueue(t);
        }

        /// <summary>
        /// Clear the pool
        /// </summary>
        public void Clear() {
            this.pool.Clear();
        }
    }
}