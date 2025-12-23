using UnityEngine;
using Views;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Variables", menuName = "Variables")]
    public class Variables : ScriptableObject
    {
        [Header("Required")]
        public CellView cellPrefab;

        [Header("Simulation Settings")]
        public int width;
        public int height;
        [Range(0.1f, 100f)]
        public float speed;

        [Header("Grid Settings")]
        public Color mainColor;
        public Color subColor;
        public bool showMainGrid;
        public bool showSubGrid;
        public float smallStep;
        public float largeStep;
    }
}