using Drawing;
using Logic.Core;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;

namespace Logic.Controls
{
    public class DoorControl: ControlBehaviour
    {
        
         [SerializeField] private SpriteRenderer renderer;
        private BoxCollider m_collider;
        private Door m_door;
        private float m_width, m_height;
        private Vector3[] m_dragPoints = new Vector3[2];
        private bool m_captured;
        private Vector3 m_dragOffset;
        private float m_baseSize;
        private float m_scale;
        private int m_selectedDragPoint;
        [JsonIgnore]
        private Wall Wall => CoreManager.Instance.SelectedRoom.Walls[m_door.WallIndex];

        protected override void OnSelectedFirstTime()
        {
        }

        protected override void OnDragEnd()
        {
            m_captured = false;
        }

        public override void Setup()
        {
            m_collider = GetComponent<BoxCollider>();
        }

        public override void Delete()
        {
            CoreManager.Instance.SelectedRoom.Doors.Remove(m_door);
        }

        private void Update()
        {
            m_door.Update(transform.position, transform.localScale, transform.localEulerAngles);
            if (m_door == null) return;


            /*if (m_window.Direction == Direction.Vertical)
            {
                m_width = m_window.Width;
                m_height = m_window.Height;
            }
            else
            {
                m_width = m_window.Height;
                m_height = m_window.Width;
            }*/
            m_width = m_door.Width;
            m_height = m_door.Height;

            /*m_width /= 10;
            m_height /= 10;*/

            transform.localScale = new Vector3(m_height, m_width, 1);
            var size = renderer.bounds.size;
            size.z = 10f;
            m_collider.size = new Vector3(1.25f, 1.25f, 5);

            if (m_captured)
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                m_scale = Helpers.Diff(mousePosition, m_dragOffset);
                float res = 0;
                var compareTarget = m_door.Direction == Direction.Vertical ? 2 : 1; 
                if (m_selectedDragPoint==compareTarget)
                    res = (m_baseSize + m_baseSize * (m_scale / 10f));
                else
                    res = (m_baseSize - m_baseSize * (m_scale / 10f));

                if (m_door.Direction == Direction.Horizontal)
                    m_door.Width = res;
                else
                    m_door.Height = res;
            }
        }

        public override void UpdateOnSelected()
        {
            var t = new GameObject().transform;
            if (m_door.Direction == Direction.Horizontal)
            {
                m_dragPoints[0] = t.position = transform.position + Vector3.up * (m_width / 2f);
                using (Draw.ingame.InLocalSpace(t))
                {
                    Draw.ingame.SolidPlane(float3.zero, Vector3.forward, new float2(0.75f, m_height), Color.red);
                }

                m_dragPoints[1] = t.position = transform.position - Vector3.up * (m_width / 2f);
                using (Draw.ingame.InLocalSpace(t))
                {
                    Draw.ingame.SolidPlane(float3.zero, Vector3.forward, new float2(0.75f, m_height), Color.red);
                }
            }
            else
            {
                m_dragPoints[0] = t.position = transform.position + Vector3.right * (m_height / 2f);
                using (Draw.ingame.InLocalSpace(t))
                {
                    Draw.ingame.SolidPlane(float3.zero, Vector3.forward, new float2(m_width, 0.75f), Color.red);
                }

                m_dragPoints[1] = t.position = transform.position - Vector3.right * (m_height / 2f);
                using (Draw.ingame.InLocalSpace(t))
                {
                    Draw.ingame.SolidPlane(float3.zero, Vector3.forward, new float2(m_width, 0.75f), Color.red);
                }
            }


            Destroy(t.gameObject);
        }

        public override void UpdateOnDrag(Vector3 mousePosition)
        {
            for (var i = 0; i < m_dragPoints.Length; i++)
            {
                var mDragPoint = m_dragPoints[i];
                if (Vector2.Distance(mousePosition, mDragPoint) < 1f)
                {
                    m_captured = true;
                    m_dragOffset = mousePosition;
                    m_selectedDragPoint = i+1;
                    m_baseSize = m_door.Direction == Direction.Horizontal
                        ? m_width
                        : m_height;
                }
            }

            if (m_captured) return;
            var clamped = mousePosition.ClampVector(Wall.StartPoint.Value, Wall.EndPoint.Value);
            clamped.z = -1;
            transform.position = clamped;
        }

        public void SetDoor(Door door)
        {
            m_door = door;
        }
    }
}