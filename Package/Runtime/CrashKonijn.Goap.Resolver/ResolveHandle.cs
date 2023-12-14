using System.Collections.Generic;
using CrashKonijn.Goap.Resolver.Interfaces;
using Unity.Collections;
using Unity.Jobs;

namespace CrashKonijn.Goap.Resolver
{
    public class ResolveHandle : IResolveHandle
    {
        private readonly GraphResolver graphResolver;
        private JobHandle handle;
        private GraphResolverJob job;
        private RunData runData;

#if UNITY_COLLECTIONS_2_1
        public ResolveHandle(GraphResolver graphResolver, NativeParallelMultiHashMap<int, int> nodeConditions, NativeParallelMultiHashMap<int, int> conditionConnections, RunData runData)
        {
            this.graphResolver = graphResolver;
            this.job = new GraphResolverJob
            {
                NodeConditions = nodeConditions,
                ConditionConnections = conditionConnections,
                RunData = runData,
                Result = new NativeList<NodeData>(Allocator.TempJob)
            };
        
            this.handle = this.job.Schedule();
        }
#else
        public ResolveHandle(GraphResolver graphResolver, NativeMultiHashMap<int, int> nodeConditions, NativeMultiHashMap<int, int> conditionConnections, RunData runData)
        {
            this.graphResolver = graphResolver;
            job = new GraphResolverJob
            {
                NodeConditions = nodeConditions,
                ConditionConnections = conditionConnections,
                RunData = runData,
                Result = new NativeList<NodeData>(Allocator.TempJob)
            };
        
            handle = job.Schedule();
        }
#endif

        public IAction[] Complete()
        {
            handle.Complete();
        
            var results = new IAction[job.Result.Length];

            for (var i = 0; i < job.Result.Length; i++)
            {
                NodeData data = job.Result[i];
                results[i] =graphResolver.GetAction(data.Index);
            }

            job.Result.Dispose();
            job.RunData.IsExecutable.Dispose();
            job.RunData.Positions.Dispose();
            job.RunData.Costs.Dispose();
            job.RunData.ConditionsMet.Dispose();

            return results;
        }
    }
}