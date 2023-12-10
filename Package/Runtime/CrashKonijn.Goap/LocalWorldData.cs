using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap
{
    public class LocalWorldData : WorldDataBase
    {
        public void Apply(IWorldData globalWorldData)
        {
            foreach (var (key, value) in globalWorldData.States)
            {
                SetState(key, value);
            }
            
            foreach (var (key, value) in globalWorldData.Targets)
            {
                SetTarget(key, value);
            }
        }
    }
}