using UnityEngine;

namespace RPG.EventBus
{
    public static class Bus <T> where T : IEvent
    {
        public delegate void Event(T args);
        public static event Event OnEvent;  //event Event means only the Bus can invoke events

        public static void Raise(T evt)
        {
            OnEvent?.Invoke(evt); //OnEvent?. is a short hand way of checking if OnEvent != null
        }
    }
}

