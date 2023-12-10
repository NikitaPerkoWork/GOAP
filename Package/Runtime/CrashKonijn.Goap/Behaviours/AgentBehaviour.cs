using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Exceptions;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        public GoapSetBehaviour goapSetBehaviour;

        [field: SerializeField]
        public float DistanceMultiplier { get; set; } = 1f;
        public AgentState State { get; private set; } = AgentState.NoAction;
        public AgentMoveState MoveState { get; set; } = AgentMoveState.Idle;

        private IGoapSet _goapSet;
        public IGoapSet GoapSet
        {
            get => _goapSet;
            set
            {
                _goapSet = value;
                value.Register(this);
            }
        }

        public IGoalBase CurrentGoal { get; private set; }
        public IActionBase CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public List<IActionBase> CurrentActionPath { get; private set; } = new List<IActionBase>();
        public IWorldData WorldData { get; } = new LocalWorldData();
        public IAgentEvents Events { get; } = new AgentEvents();
        public IDataReferenceInjector Injector { get; private set; }
        public IAgentDistanceObserver DistanceObserver { get; set; } = new VectorDistanceObserver();

        private ITarget _currentTarget;
        private IMonoAgent _monoAgentImplementation;

        private void Awake()
        {
            Injector = new DataReferenceInjector(this);
            
            if (goapSetBehaviour != null)
            {
                GoapSet = goapSetBehaviour.GoapSet;
            }
        }

        private void Start()
        {
            if (GoapSet == null)
            {
                throw new GoapException($"There is no GoapSet assigned to the agent '{name}'! Please assign one in the inspector or through code in the Awake method.");
            }
        }

        private void OnEnable()
        {
            if (GoapSet != null)
            {
                GoapSet.Register(this);
            }
        }

        private void OnDisable()
        {
            EndAction(false);
            
            if (GoapSet != null)
            {
                GoapSet.Unregister(this);
            }
        }

        public void Run()
        {
            if (CurrentAction == null)
            {
                State = AgentState.NoAction;
                return;
            }
            
            UpdateTarget();

            switch (CurrentAction.Config.MoveMode)
            {
                case ActionMoveMode.MoveBeforePerforming:
                    RunMoveBeforePerforming();
                    break;
                case ActionMoveMode.PerformWhileMoving:
                    RunPerformWhileMoving();
                    break;
            }
        }

        private void RunPerformWhileMoving()
        {
            if (IsInRange())
            {
                State = AgentState.PerformingAction;
                SetMoveState(AgentMoveState.InRange);
                PerformAction();
                return;
            }
                
            State = AgentState.MovingWhilePerformingAction;
            SetMoveState(AgentMoveState.OutOfRange);
            Move();
            PerformAction();
        }

        private void RunMoveBeforePerforming()
        {
            if (IsInRange())
            {
                State = AgentState.PerformingAction;
                SetMoveState(AgentMoveState.InRange);
                PerformAction();
                return;
            }

            State = AgentState.MovingToTarget;
            SetMoveState(AgentMoveState.OutOfRange);
            Move();
        }

        private void UpdateTarget()
        {
            if (_currentTarget == CurrentActionData?.Target)
            {
                return;
            }

            _currentTarget = CurrentActionData?.Target;
            Events.TargetChanged(_currentTarget, IsInRange());
        }

        private void SetMoveState(AgentMoveState state)
        {
            if (MoveState == state)
            {
                return;
            }

            MoveState = state;
            
            switch (state)
            {
                case AgentMoveState.InRange:
                    Events.TargetInRange(_currentTarget);
                    break;
                case AgentMoveState.OutOfRange:
                    Events.TargetOutOfRange(_currentTarget);
                    break;
            }
        }

        private void Move()
        {
            if (_currentTarget == null)
            {
                return;
            }

            Events.Move(_currentTarget);
        }

        private void PerformAction()
        {
            var result = CurrentAction.Perform(this, CurrentActionData, new ActionContext
            {
                DeltaTime = Time.deltaTime,
            });

            if (result == ActionRunState.Continue)
            {
                return;
            }

            EndAction();
        }

        private bool IsInRange()
        {
            var distance = DistanceObserver.GetDistance(this, CurrentActionData?.Target, Injector);
            
            return CurrentAction.IsInRange(this, distance, CurrentActionData, Injector);
        }

        public void SetGoal<TGoal>(bool endAction)
            where TGoal : IGoalBase
        {
            SetGoal(GoapSet.ResolveGoal<TGoal>(), endAction);
        }

        public void SetGoal(IGoalBase goal, bool endAction)
        {
            if (goal == CurrentGoal)
            {
                return;
            }

            CurrentGoal = goal;
            
            if (CurrentAction == null)
            {
                GoapSet.Agents.Enqueue(this);
            }

            Events.GoalStart(goal);
            
            if (endAction)
            {
                EndAction();
            }
        }

        public void ClearGoal()
        {
            CurrentGoal = null;
        }

        public void SetAction(IActionBase action, List<IActionBase> path, ITarget target)
        {
            if (CurrentAction != null)
            {
                EndAction(false);
            }

            CurrentAction = action;

            var data = action.GetData();
            Injector.Inject(data);
            CurrentActionData = data;
            CurrentActionData.Target = target;
            CurrentActionPath = path;
            CurrentAction.Start(this, CurrentActionData);
            Events.ActionStart(action);
        }
        
        public void EndAction(bool enqueue = true)
        {
            var action = CurrentAction;
            
            CurrentAction?.End(this, CurrentActionData);
            CurrentAction = null;
            CurrentActionData = null;
            _currentTarget = null;
            MoveState = AgentMoveState.Idle;
            
            Events.ActionStop(action);
            
            if (enqueue)
            {
                GoapSet.Agents.Enqueue(this);
            }
        }

        public void SetDistanceMultiplierSpeed(float speed)
        {
            DistanceMultiplier = 1f / speed;
        }
    }
}