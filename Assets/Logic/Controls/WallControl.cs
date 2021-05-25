using System;
using System.Collections.Generic;
using System.Linq;
using Drawing;
using UnityEngine;
using Logic.Core;

namespace Logic.Controls
{
    public class WallControl : ControlBehaviour
    {
        private Wall m_wall;
        //private EdgeCollider2D m_edgeCollider;
        private BoxCollider m_collider;
        private LineRenderer m_lineRenderer;
        [SerializeField] private Mesh sphere;
        [SerializeField] private float minimumAlignDiff = 1f;
        [SerializeField] private float magneticAlign = 1f;
        private int m_PointSelectedIndex = 0;
        private bool m_forceFollow;

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_collider = GetComponent<BoxCollider>();
        }

        protected override bool CanBeSelected()
        {
            return !CoreManager.Instance.ActionsManager.IsAnyActionActive();
        }

        protected override bool CanBeDragged()
        {
            return !CoreManager.Instance.ActionsManager.IsAnyActionActive();
        }

        private void UpdateCollider()
        {
            var points = new[]
            {
                m_wall.StartPoint.Value, m_wall.EndPoint.Value
            };
         

            /*var size = bounds.size;
            size.z = 10;*/
            var size = m_lineRenderer.bounds.size;
            var dir = points[0]-points[1];

            if (dir.x == 0)
            {
                size.x *= 5f;
            }
            else
            {
                size.y *= 5f;
            }

            m_collider.size = size;
            m_collider.center = Helpers.FinalMiddleVector(points[0], points[1]);
        }

        public void SetWall(Wall wall)
        {
            m_wall = wall;
        }

        protected override void OnSelectedFirstTime()
        {
        }

        protected override void OnDragEnd()
        {
            AlignLines();
            AlignDots();
            CheckForDestroy();
            //CoreManager.Instance.SelectedRoom.AlignRoom();
            //RegenerateLinks();
        }

        private void CheckForDestroy()
        {
            if (m_wall.StartPoint == m_wall.EndPoint)
                Delete();
        }


        public void AlignDotsPointerless()
        {
            var wallControls = ControlsManager.Instance.GetControls<WallControl>();
            var points = new List<WallPoint>();
            foreach (var wallControl in wallControls)
            {
                if (this == wallControl) continue;

                points.Add(wallControl.m_wall.StartPoint);
                points.Add(wallControl.m_wall.EndPoint);
            }

            foreach (var dot in points)
            {
                if (Vector3.Distance(dot.Value, m_wall.StartPoint.Value) < magneticAlign)
                    m_wall.StartPoint.Value = dot.Value;
                if (Vector3.Distance(dot.Value, m_wall.EndPoint.Value) < magneticAlign)
                    m_wall.EndPoint.Value = dot.Value;
            }
        }

        public void AlignDots()
        {
            var wallControls = ControlsManager.Instance.GetControls<WallControl>();
            var points = new List<WallPoint>();
            foreach (var wallControl in wallControls)
            {
                if (this == wallControl) continue;

                if (wallControl.m_wall.StartPoint.IsFixed)
                    points.Add(wallControl.m_wall.StartPoint);
                if (wallControl.m_wall.EndPoint.IsFixed)
                    points.Add(wallControl.m_wall.EndPoint);
            }

            foreach (var dot in points)
            {
                if (Vector3.Distance(dot.Value, m_wall.StartPoint.Value) < magneticAlign && dot.IsFixed)
                    m_wall.StartPoint = dot;
                else if (Vector3.Distance(dot.Value, m_wall.EndPoint.Value) < magneticAlign && dot.IsFixed)
                    m_wall.EndPoint = dot;
            }
        }


        private void RegenerateLinks()
        {
            var allWalls = CoreManager.Instance.SelectedRoom.Walls;
            var startPoint = allWalls[0].StartPoint;
            Link(startPoint);

            void Link(WallPoint point)
            {
                var next = new List<WallPoint>();
                foreach (var wall in allWalls)
                {
                    if (wall.StartPoint.Value == point.Value)
                    {
                        next.Add(wall.EndPoint);
                        if (wall.EndPoint != startPoint)
                            Link(wall.EndPoint);
                    }
                    /*if (wall.EndPoint.Value == point.Value)
                    {
                        next.Add(wall.StartPoint);
                        if (wall.StartPoint != startPoint)
                            Link(wall.StartPoint);
                    }*/
                }

                point.NextPoints = next.ToArray();
            }
        }

        private void AlignLines()
        {
            var xDiff = Mathf.Abs(m_wall.StartPoint.Value.x - m_wall.EndPoint.Value.x);
            var yDiff = Mathf.Abs(m_wall.StartPoint.Value.y - m_wall.EndPoint.Value.y);

            if (xDiff - minimumAlignDiff < yDiff - minimumAlignDiff)
            {
                if (m_PointSelectedIndex == 1)
                    m_wall.StartPoint.Value.x = m_wall.EndPoint.Value.x;
                else
                    m_wall.EndPoint.Value.x = m_wall.StartPoint.Value.x;
            }
            else
            {
                if (m_PointSelectedIndex == 1)
                {
                    m_wall.StartPoint.Value.y = m_wall.EndPoint.Value.y;
                }
                else
                    m_wall.EndPoint.Value.y = m_wall.StartPoint.Value.y;
            }

            m_PointSelectedIndex = 0;
        }

        public override void Setup()
        {
            ControlsManager.Instance.AddControl(this);
            m_lineRenderer.SetPosition(0, m_wall.StartPoint.Value);
            m_lineRenderer.SetPosition(1, m_wall.EndPoint.Value);
            UpdateCollider();
        }

        public override void Delete()
        {
            ControlsManager.Instance.RemoveControl(this);
            CoreManager.Instance.SelectedRoom.Walls.Remove(m_wall);
            Destroy(gameObject);
        }

        public override void UpdateOnSelected()
        {
            var t = new GameObject();
            t.transform.localScale *= 2;
            t.transform.position = m_wall.StartPoint.Value;
            using (Draw.ingame.InLocalSpace(t.transform))
            {
                Draw.ingame.SolidMesh(sphere, Color.blue);
            }

            t.transform.position = m_wall.EndPoint.Value;
            using (Draw.ingame.InLocalSpace(t.transform))
            {
                Draw.ingame.SolidMesh(sphere, Color.blue);
            }

            Destroy(t);
        }

        public void ForceFollow()
        {
            m_forceFollow = true;
        }

        private void Update()
        {
            m_lineRenderer.SetPosition(0, m_wall.StartPoint.Value);
            m_lineRenderer.SetPosition(1, m_wall.EndPoint.Value);
            UpdateCollider();
            

            if (!m_forceFollow) return;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            m_wall.StartPoint.Value = mousePosition;


            var diff = m_wall.EndPoint.Value - m_wall.StartPoint.Value;

            if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
                m_wall.StartPoint.Value.x = m_wall.EndPoint.Value.x;
            else
                m_wall.StartPoint.Value.y = m_wall.EndPoint.Value.y;


            m_lineRenderer.SetPosition(0, m_wall.StartPoint.Value);
            m_PointSelectedIndex = -1;
            if (Input.GetMouseButtonDown(0) && !ControlsManager.IsPointerOnUI)
            {
                m_forceFollow = false;
                m_wall.StartPoint.IsFixed = true;
            }
        }

        protected override void OnDeSelecting()
        {
            //CoreManager.Instance.UnFocus();
            UpdateCollider();
            base.OnDeSelecting();
        }

        public override void UpdateOnDrag(Vector3 mousePosition)
        {
            if (Vector3.Distance(mousePosition, m_wall.StartPoint.Value) <= 2 && m_PointSelectedIndex != -1)
            {
                var diff = (Vector2) mousePosition - m_wall.StartPoint.Value;
                //m_wall.StartPoint.Value = mousePosition;
                if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
                    m_wall.StartPoint.Value.y = ((Vector2) mousePosition).y;
                else
                    m_wall.StartPoint.Value.x = ((Vector2) mousePosition).x;

                m_PointSelectedIndex = 1;
            }
            else if (Vector3.Distance(mousePosition, m_wall.EndPoint.Value) <= 2 && m_PointSelectedIndex != 1)
            {
                var diff = (Vector2) mousePosition - m_wall.EndPoint.Value;
                //m_wall.EndPoint.Value = mousePosition;
                if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
                    m_wall.EndPoint.Value.y = ((Vector2) mousePosition).y;

                else
                    m_wall.EndPoint.Value.x = ((Vector2) mousePosition).x;
                m_PointSelectedIndex = -1;
            }
        }


        public MonoBehaviour Behavior => this;
        public bool Following => m_forceFollow;

        public bool NotFixed()
        {
            return m_wall.StartPoint.IsFixed || m_wall.EndPoint.IsFixed;
        }

        public Wall GetWall()
        {
            return m_wall;
        }
    }
}