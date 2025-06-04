using RPG.EventBus;
using RPG.GameTime;
using UnityEngine;

public class GameTimeTimeUpdateEvent: IEvent
{
    public GameTimeContoller GameTimeContoller { get; private set; }

    public GameTimeTimeUpdateEvent(GameTimeContoller gameTimeContoller)
    {
        GameTimeContoller = gameTimeContoller;
    }
}
