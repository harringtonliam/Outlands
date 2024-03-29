using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Dragging
{
    public interface IDragSource<T> where T : class
    {
        T GetItem();

        int GetNumber();

        int GetNumberOfUses();

        void RemoveItems(int number);
    }
}


