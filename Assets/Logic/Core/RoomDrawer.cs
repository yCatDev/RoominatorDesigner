using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Drawing;
using Logic.Controls;
using Logic.Designer;
using Logic.Dijkstra;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.ShortestPath;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Algorithm;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Core
{
    public class RoomDrawer : MonoBehaviour
    {
        private MeshRenderer m_meshRenderer;
        private MeshFilter m_meshFilter;
        private Mesh m_mesh;
        private MeshCollider m_meshCollider;


        private void Start()
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            m_meshFilter = GetComponent<MeshFilter>();
            m_meshCollider = GetComponent<MeshCollider>();
            m_mesh = new Mesh();
            m_meshFilter.mesh = m_mesh;
        }


        private Vector2 GetClosestPoint(IEnumerable<Vector2> points)
        {
            var rect = new Bounds();
            foreach (var point in points)
            {
                rect.Encapsulate(point);
            }

            var dist = float.PositiveInfinity;
            var closest = Vector2.zero;
            var target = rect.min;
            foreach (var vector2 in points)
            {
                var tmp = Vector2.Distance(vector2, target);
                if (tmp < dist)
                {
                    dist = tmp;
                    closest = vector2;
                }
            }

            return closest;
        }

        
        
        private void Update()
        {
            //var points = new HashSet<Vector2>();
            var uniPoints = new HashSet<Vector2>();
            var points = new List<Vector2>();

            if (!CoreManager.Instance.SelectedRoom.IsRoomShapeCorrect) return;

            foreach (var wall in CoreManager.Instance.SelectedRoom.Walls)
            {
                //if (!uniPoints.Contains(wall.StartPoint.Value))
                uniPoints.Add(wall.StartPoint.Value);
                //if (!uniPoints.Contains(wall.EndPoint.Value))
                uniPoints.Add(wall.EndPoint.Value);
            }

            var targetPoint = GetClosestPoint(uniPoints);
            var walls = CoreManager.Instance.SelectedRoom.Walls;
            for (int i = 0; i < walls.Count; i++)
            {
                foreach (var wall in walls)
                {
                    if (wall.StartPoint.Value == targetPoint)
                    {
                        points.Add(targetPoint);
                        targetPoint = wall.EndPoint.Value;
                        break;
                    }
                }
            }

            var polygon = new Polygon(points.Count);
            polygon.Add(points.ToList());
            var triangleNetMesh = (TriangleNetMesh) polygon.Triangulate();


            m_mesh = triangleNetMesh.GenerateUnityMesh();
            m_mesh.RecalculateBounds();
            m_meshFilter.mesh = m_mesh;
            m_meshCollider.sharedMesh = m_mesh;
        }
    }
}