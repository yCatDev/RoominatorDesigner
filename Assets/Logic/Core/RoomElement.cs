using System;
using UnityEngine;

namespace Logic.Core
{
    [Serializable]
    public abstract class RoomElement
    {
        public Vector2 Position;
        public Vector2 Scale;
        public Vector3 Rotation;

        public void Update(Vector2 position, Vector2 scale, Vector3 rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }
    }
}