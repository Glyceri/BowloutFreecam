using BowloutModManager.BowloutMod.Interfaces;
using System;

namespace BowloutFreecam
{
    [Serializable]
    public class FreecamSettings : IBowloutConfiguration
    {
        public int Version { get; set; } = 1;

        public bool ScrollingHandlesSpeed = true;
        public bool ScrollingHandlesSpeed2 = true;
        public bool ScrollingHandlesSpeed3 = true;
        public bool ScrollingHandlesSpeed4 = true;
        public bool ScrollingHandlesSpeed5 = true;
    }
}
