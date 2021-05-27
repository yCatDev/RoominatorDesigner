using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logic.Core
{
    [Serializable]
    public class Room
    {
        public List<Wall> Walls;
        public List<Furniture> Furnitures;
        public List<Window> Windows;
        public List<Door> Doors;

     

        public Room()
        {
            Walls = new List<Wall>();
            Furnitures = new List<Furniture>();
            Windows = new List<Window>();
            Doors = new List<Door>();
        }


        public void AlignRoomWalls()
        {
            foreach (var wall in Walls)
            {
                var diff = wall.EndPoint.Value - wall.StartPoint.Value;

                if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
                    wall.StartPoint.Value.x = wall.EndPoint.Value.x;
                else
                    wall.StartPoint.Value.y = wall.EndPoint.Value.y;
            }
        }

        public void AlignRoom()
        {
            if (Walls.Count < 2)
                return;
            var set = new HashSet<Vector2>();
            foreach (var wall in Walls)
            {
                if (wall.EndPoint.IsFixed)
                    set.Add(wall.EndPoint.Value);
                if (wall.StartPoint.IsFixed)
                    set.Add(wall.StartPoint.Value);
            }

            foreach (var vector2 in set)
            {
                var np = new WallPoint(vector2, true);
                foreach (var wall in Walls)
                {
                    if (wall.StartPoint.Value == vector2)
                        wall.StartPoint = np;
                    else if (wall.EndPoint.Value == vector2)
                        wall.EndPoint = np;
                }
            }
        }

        public bool IsRoomShapeCorrect
        {
            get
            {
                if (Walls.Count < 2)
                    return false;
                var set = new Dictionary<Vector2, int>();
                foreach (var wall in Walls)
                {
                    if (wall.StartPoint.IsFixed)
                        Add(wall.StartPoint.Value);
                    if (wall.EndPoint.IsFixed)
                        Add(wall.EndPoint.Value);
                }

                void Add(Vector2 point)
                {
                    if (set.ContainsKey(point))
                    {
                        set[point]++;
                    }
                    else
                    {
                        set.Add(point, 1);
                    }
                }

                var arr = set.Values.ToArray();

                foreach (var i in arr)
                {
                    if (i % 2 > 0)
                        return false;
                }

                return true;
            }
        }

        static bool IsPrime(int n)
        {
            // Corner case
            if (n <= 1)
                return false;

            // Check from 2 to n-1
            for (int i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
    }
}