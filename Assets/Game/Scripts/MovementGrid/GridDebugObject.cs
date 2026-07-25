using UnityEngine;
using TMPro;

namespace RPG.MovementGrid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] TextMeshPro textMeshPro;
        private GridObject gridObject;

        public void SetGridObject(GridObject gridObject)
        {
            this.gridObject = gridObject;
        }

        private void Update()
        {
            this.textMeshPro.text = this.gridObject.ToString(); 
        }

    }


}

