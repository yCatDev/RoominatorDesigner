using System;
using Newtonsoft.Json;

namespace Logic.Core
{
    
    [Serializable]
    public class Door: RoomElement
    {
        public float Width;
        public float Height;
        public Direction Direction = 0;
        [JsonProperty] internal int WallIndex;
        internal const float BaseWidthConstant = 10;
    }
}