﻿using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class LocalTargetSensor : LocalTargetSensorBase
    {
        public override void Created()
        {
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent)
        {
            return default;
        }
    }
}