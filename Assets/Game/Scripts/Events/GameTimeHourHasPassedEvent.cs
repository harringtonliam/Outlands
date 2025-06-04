using RPG.EventBus;
using RPG.GameTime;


namespace RPG.Events
{
    public class GameTimeHourHasPassedEvent: IEvent
    {
        public GameTimeContoller GameTimeContoller { get; private set; }

        public GameTimeHourHasPassedEvent(GameTimeContoller gameTimeContoller)
        {
            GameTimeContoller = gameTimeContoller;
        }
    }
}


