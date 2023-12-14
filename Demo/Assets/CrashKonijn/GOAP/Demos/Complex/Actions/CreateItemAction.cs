using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Demos.Complex.Behaviours;
using Demos.Complex.Classes.Items;
using Demos.Complex.Goap;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Actions
{
    public class CreateItemAction<TCreatable> : ActionBase<CreateItemAction<TCreatable>.Data>, IInjectable
        where TCreatable : ItemBase, ICreatable
    {
        private ItemFactory _itemFactory;
        private InstanceHandler _instanceHandler;

        public void Inject(GoapInjector injector)
        {
            _itemFactory = injector.itemFactory;
            _instanceHandler = injector.instanceHandler;
        }

        public override void Created()
        {
        }
        
        public override void Start(IMonoAgent agent, Data data)
        {
            data.Timer = 5f;
            data.RequiredWood = GetRequiredWood();
            data.RequiredIron = GetRequiredIron();
        }

        public override ActionRunState Perform(IMonoAgent agent, Data data, ActionContext context)
        {
            if (data.State == State.NotStarted)
            {
                data.State = State.Started;
                RemoveRequiredResources(data);
            }
            
            data.Timer -= context.DeltaTime;
            
            if (data.Timer > 0)
                return ActionRunState.Continue;
            
            var item = _itemFactory.Instantiate<TCreatable>();
            item.transform.position = GetRandomPosition(agent);
            
            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, Data data)
        {
        }

        private void RemoveRequiredResources(Data data)
        {
            for (var i = 0; i < data.RequiredIron; i++)
            {
                var iron = data.Inventory.Get<Iron>().FirstOrDefault();
                data.Inventory.Remove(iron);
                _instanceHandler.QueueForDestroy(iron);
            }
            
            for (var j = 0; j < data.RequiredWood; j++)
            {
                var wood = data.Inventory.Get<Wood>().FirstOrDefault();
                data.Inventory.Remove(wood);
                _instanceHandler.QueueForDestroy(wood);
            }
        }

        private int GetRequiredWood()
        {
            return Config.Conditions.FirstOrDefault(x => x.WorldKey.Name == "IsHolding<Wood>")!.Amount;
        }

        private int GetRequiredIron()
        {
            return Config.Conditions.FirstOrDefault(x => x.WorldKey.Name == "IsHolding<Iron>")!.Amount;
        }
        
        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            var pos = Random.insideUnitCircle.normalized * Random.Range(1f, 2f);

            return agent.transform.position + new Vector3(pos.x, 0f, pos.y);
        }
        
        public class Data : IActionData
        {
            public ITarget Target { get; set; }
            public int RequiredWood { get; set; }
            public int RequiredIron { get; set; }
            public State State { get; set; } = State.NotStarted;
            public float Timer { get; set; }
            
            public ComplexInventoryBehaviour Inventory { get; set; }
        }

        public enum State
        {
            NotStarted,
            Started
        }
    }
}