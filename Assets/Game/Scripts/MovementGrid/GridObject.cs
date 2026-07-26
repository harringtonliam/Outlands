using NUnit.Framework;
using UnityEngine;
using RPG.Movement;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using System.Text;


namespace RPG.MovementGrid
{

    public class GridObject
    {
        private GridSystem gridSystem;
        private GridPosition gridPosition;
        private List<Mover> movers;

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
            movers = new List<Mover>();
        }

        public override string ToString()
        {
            StringBuilder desc= new StringBuilder(gridPosition.ToString());
            foreach (Mover mover in movers)
            {
                desc.Append("\n");
                desc.Append(mover.name.ToString());
            }


            return desc.ToString();
        }

        public void AddMover(Mover mover)
        {
            if (!movers.Contains(mover))
            {
                {
                    movers.Add(mover);
                }
            }
        }

        public List<Mover> GetMovers()
        {
            return movers;
        }

        public void RemoveMover(Mover mover)
        {
            if (movers.Contains(mover))
            {
                movers.Remove(mover);
            }
        }

    }
}
