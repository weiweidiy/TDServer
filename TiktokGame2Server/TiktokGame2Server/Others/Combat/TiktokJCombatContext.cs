using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class TiktokJCombatContext : IJCombatContext
    {
        public TiktokJCombatContext(
            IJCombatTurnBasedEventRecorder? eventRecorder = null,
            JFramework.ILogger? logger = null)
        {
            EventRecorder = eventRecorder;
            Logger = logger;
        }

        public JFramework.ILogger? Logger { get; private set; }

        public IJCombatTurnBasedEventRecorder? EventRecorder { get; private set; }
    }
}

