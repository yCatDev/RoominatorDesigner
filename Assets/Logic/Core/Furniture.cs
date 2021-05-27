using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Logic.Core
{
    
    [Serializable]
    public class Furniture: RoomElement
    {
        public string Name;
        public Bounds Bounds;
        public string ID;
        public bool SideMatter;
        [JsonIgnore]
        public Vector2 UpSide;
        [JsonIgnore]
        public Vector2 DownSide;
        [JsonIgnore]
        public Vector2 LeftSide;
        [JsonIgnore]
        public Vector2 RightSide;

        public List<Furniture> linkedFurnitures = new List<Furniture>();
    }
}