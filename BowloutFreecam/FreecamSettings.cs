using BowloutModManager.BowloutMod.Interfaces;
using System;
using UnityEngine;

namespace BowloutFreecam
{
    [Serializable]
    public class FreecamSettings : IBowloutConfiguration
    {
        public int Version { get; set; } = 1;

        public bool ScrollingHandlesSpeed = true;
        [Range(5.0f, 100.0f)]
        public float ScrollingSpeed = 40;
    }
}
