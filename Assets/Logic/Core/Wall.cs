using System;
using UnityEngine;

namespace Logic.Core
{
    [Serializable]
    public class Wall
    {
        public WallPoint StartPoint;
        public WallPoint EndPoint;

        public Wall()
        {
            
        }
        
        public Wall(Wall wall)
        {
            StartPoint = wall.StartPoint;
            EndPoint = wall.EndPoint;
        }
    }

    [Serializable]
    public class WallPoint
    {
        public Vector2 Value;
        public WallPoint[] NextPoints;
        public bool IsFixed = true;

        public WallPoint(Vector2 value, bool isFixed, params WallPoint[] points)
        {
            Value = value;
            NextPoints = points;
            IsFixed = isFixed;
        }
    }
    
}