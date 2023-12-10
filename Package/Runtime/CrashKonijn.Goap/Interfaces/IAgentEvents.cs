using System;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IAgentEvents
    {
        // Actions
        event  Action<IActionBase> OnActionStart;
        void ActionStart(IActionBase action);
        
        event  Action<IActionBase> OnActionStop;
        void ActionStop(IActionBase action);
        
        event Action<IGoalBase> OnNoActionFound;
        void NoActionFound(IGoalBase goal);
        
        // Goals
        event Action<IGoalBase> OnGoalStart;
        void GoalStart(IGoalBase goal);
        
        event Action<IGoalBase> OnGoalCompleted;
        void GoalCompleted(IGoalBase goal);
        
        // Targets
        event Action<ITarget> OnTargetInRange;
        void TargetInRange(ITarget target);
        
        event Action<ITarget> OnTargetOutOfRange;
        void TargetOutOfRange(ITarget target);
        
        event Action<ITarget, bool> OnTargetChanged;
        void TargetChanged(ITarget target, bool inRange);

        event Action<ITarget> OnMove;
        void Move(ITarget target);
    }
}