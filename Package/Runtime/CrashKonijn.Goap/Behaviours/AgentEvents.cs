using System;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentEvents : IAgentEvents
    {
        // Actions
        public event Action<IActionBase> OnActionStart;
        public void ActionStart(IActionBase action)
        {
            OnActionStart?.Invoke(action);
        }
        
        public event Action<IActionBase> OnActionStop;
        public void ActionStop(IActionBase action)
        {
            OnActionStop?.Invoke(action);
        }

        public event Action<IGoalBase> OnNoActionFound;
        public void NoActionFound(IGoalBase goal)
        {
            OnNoActionFound?.Invoke(goal);
        }
        
        // Goals
        public event Action<IGoalBase> OnGoalStart;
        public void GoalStart(IGoalBase goal)
        {
            OnGoalStart?.Invoke(goal);
        }

        public event Action<IGoalBase> OnGoalCompleted;
        public void GoalCompleted(IGoalBase goal)
        {
            OnGoalCompleted?.Invoke(goal);
        }

        // Targets
        public event Action<ITarget> OnTargetInRange;
        public void TargetInRange(ITarget target)
        {
            OnTargetInRange?.Invoke(target);
        }

        public event Action<ITarget> OnTargetOutOfRange;
        public void TargetOutOfRange(ITarget target)
        {
            OnTargetOutOfRange?.Invoke(target);
        }

        public event Action<ITarget,bool> OnTargetChanged;
        public void TargetChanged(ITarget target, bool inRange)
        {
            OnTargetChanged?.Invoke(target, inRange);
        }

        public event Action<ITarget> OnMove;
        public void Move(ITarget target)
        {
            OnMove?.Invoke(target);
        }
    }
}