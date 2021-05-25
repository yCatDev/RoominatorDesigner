using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Logic.Core
{
    
    [Serializable]
    public enum Direction{
        Horizontal = 0,
        Vertical = 1
    }
    
    [Serializable]
    public class Window: RoomElement
    {
        public float Width;
        public float Height;
        public Direction Direction = 0;
        internal const float BaseWidthConstant = 10;
        [JsonProperty] internal int WallIndex;
    }
}