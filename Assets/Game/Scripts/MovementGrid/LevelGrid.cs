
using RPG.Movement;
using RPGCharacterAnims.Actions;
using UnityEngine;
using System.Collections.Generic;

namespace RPG.MovementGrid
{
    public class LevelGrid : MonoBehaviour
    {
        public static LevelGrid Instance { get; private set;  }

        [SerializeField] private Transform gridDebugObjectPrefab;
        private GridSystem gridSystem;

        void Awake()
        {
            if(Instance != null)
            {
                Debug.LogError("There is more than one LevelGrid! " + transform + " + " + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this;

            gridSystem = new GridSystem(10, 10, 2f);
            gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        }


        public void SetUnitAtGridPosition(GridPosition gridPosition, Mover mover)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.AddMover(mover);
        }

        public List<Mover> GetUnitsAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            return gridObject.GetMovers();
        }

        public void ClearUnitAtGridPosition(GridPosition gridPosition, Mover mover)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.RemoveMover(mover);
        }

        public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    }

}


