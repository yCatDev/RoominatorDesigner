using System.Collections.Generic;
using UnityEngine;

namespace Logic.Dijkstra
{
    public class GraphNode
    {
        public Vector2 Value;
        public Dictionary<GraphNode, float> Distances;
        public int Count => Distances.Count;
        public GraphNode(Vector2 vector3)
        {
            Value = vector3;
        }

    }
}