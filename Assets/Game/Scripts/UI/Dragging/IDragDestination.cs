using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Dragging
{
    public interface IDragDestination<T> where T : class 
    {
        int MaxAcceptable(T item);

        bool AddItems(T item, int number, int numberOfUses);
    }
}


