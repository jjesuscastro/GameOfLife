using Models;
using UnityEngine;

namespace Views {
    public class CellView : MonoBehaviour {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public void SetColor(Color color) {
            this.spriteRenderer.color = color;
        }

        public void SetAlive(int isAlive) {
            this.spriteRenderer.enabled = isAlive == 1;
        }
    }
}