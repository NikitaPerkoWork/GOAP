namespace CrashKonijn.Goap.Interfaces
{
    public interface ILocalTargetSensor : ITargetSensor
    {
        public void Update();
        
        public ITarget Sense(IMonoAgent agent);
    }
}