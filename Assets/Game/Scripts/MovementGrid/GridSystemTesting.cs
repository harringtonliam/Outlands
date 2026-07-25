using UnityEngine;


namespace RPG.MovementGrid
{
    public class GridSystemTesting : MonoBehaviour
    {
        [SerializeField] private Transform gridDebugObjectPrefab;
        private GridSystem gridSystem;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gridSystem = new GridSystem(10, 10, 2f);
            gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        }

        // Update is called once per frame
        void Update()
        {
           // Debug.Log(gridSystem.GetGridPosition(MouseWorld));
        }
    }

}

