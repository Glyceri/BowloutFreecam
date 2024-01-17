using BowloutModManager.BowloutMod.Interfaces;
using System;

namespace BowloutFreecam
{
    [Serializable]
    public class FreecamSettings : IBowloutConfiguration
    {
        public int Version { get; set; } = 1;

        public bool ScrollingHandlesSpeed = true;
    }
}
