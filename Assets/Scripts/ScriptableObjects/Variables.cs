using UnityEngine;
using Views;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "Variables", menuName = "Variables")]
    public class Variables : ScriptableObject {
        public CellView cellPrefab;
        public int width;
        public int height;
        [Range(0.1f, 100f)]
        public float speed;
    }
}