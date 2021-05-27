using System;
using Drawing;
using Logic.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.Controls
{
    public class FurnitureControl : ControlBehaviour
    {
        private float m_width, m_height;

        private Vector2[] m_dragDots = new Vector2[4];
        private Vector2 m_rotateDot;
        private FitterID m_fitterID;

        [SerializeField] private bool isSideMatter = false;
        private float m_scale = 3;
        [SerializeField] private Mesh sphere;
        private Vector3 m_offset, m_dragOffset;
        private bool m_captured;
        private Vector3 m_baseScale = new Vector3(3, 3, 3);
        private SpriteRenderer m_spr;
        private Furniture m_furniture;
        private BoxCollider m_collider;

        private void Start()
        {
            m_fitterID = GetComponent<FitterID>();
            Setup();
            m_collider = GetComponent<BoxCollider>();
        }

        protected override bool CanBeUnSelected()
        {
            return m_captured;
        }

        protected override void OnSelectedFirstTime()
        {
        }

        protected override void OnClick()
        {
            m_offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }

        protected override void OnDragEnd()
        {
            m_captured = false;
        }

        public override void Setup()
        {
            m_spr = GetComponent<SpriteRenderer>();
        }

        private void UpdateCoords()
        {
            m_furniture.UpSide = transform.position + transform.up*100;
            m_furniture.DownSide = transform.position - transform.up*100;
            m_furniture.RightSide = transform.position + transform.right*100;
            m_furniture.LeftSide = transform.position - transform.right*100;

            m_width = m_spr.bounds.size.x / 2f;
            m_height = m_spr.bounds.size.y / 2f;
            var x = m_width;
            var y = m_height;


            m_dragDots[0] = transform.position + new Vector3(x, y);
            m_dragDots[1] = transform.position + new Vector3(-x, -y);
            m_dragDots[2] = transform.position + new Vector3(-x, y);
            m_dragDots[3] = transform.position + new Vector3(x, -y);
            m_rotateDot = transform.position + new Vector3(0, y);
        }

        public override void Delete()
        {
            CoreManager.Instance.SelectedRoom.Furnitures.Remove(m_furniture);
            Destroy(gameObject);
        }

        public override void UpdateOnSelected()
        {
            var t = new GameObject();
            t.transform.localScale *= 2;


            t.transform.position = m_dragDots[0];
            using (Draw.ingame.InLocalSpace(t.transform))
                Draw.ingame.SolidMesh(sphere, Color.blue);
            t.transform.position = m_dragDots[1];
            using (Draw.ingame.InLocalSpace(t.transform))
                Draw.ingame.SolidMesh(sphere, Color.blue);
            t.transform.position = m_dragDots[2];
            using (Draw.ingame.InLocalSpace(t.transform))
                Draw.ingame.SolidMesh(sphere, Color.blue);
            t.transform.position = m_dragDots[3];
            using (Draw.ingame.InLocalSpace(t.transform))
                Draw.ingame.SolidMesh(sphere, Color.blue);
            t.transform.position = m_rotateDot;
            using (Draw.ingame.InLocalSpace(t.transform))
            {
                Draw.ingame.SolidMesh(sphere, Color.red);
            }

            Destroy(t);
        }


        /*
        private void OnDrawGizmosSelected()
        {
            Debug.Log(m_furniture.Bounds.size);
        }
        */

        private void Update()
        {
            m_furniture.Update(transform.position, transform.localScale, transform.localEulerAngles);
            m_furniture.Bounds = m_spr.bounds;
            m_furniture.Bounds.size = new Vector2(m_furniture.Bounds.size.y, m_furniture.Bounds.size.x);
            UpdateCoords();
            //if (!Input.GetMouseButtonDown(0)) return;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && Vector2.Distance(mousePosition, m_rotateDot) <= 1)
            {
                transform.localEulerAngles += Vector3.forward * 90;
            }

            if (!Input.GetMouseButton(0))
            {
                m_captured = false;
                return;
            }


            if (m_captured)
            {
                m_scale = Helpers.Diff(m_dragOffset, mousePosition);
                transform.localScale = m_baseScale + m_baseScale * (m_scale / 10f);
            }
            else
            {
                foreach (Vector2 mDragDot in m_dragDots)
                {
                    if (Vector2.Distance(mousePosition, mDragDot) <= 1)
                    {
                        m_dragOffset = mousePosition;
                        m_captured = true;
                        m_baseScale = transform.localScale;
                        return;
                    }
                }
            }
        }


        public override void UpdateOnDrag(Vector3 mousePosition)
        {
            if (m_captured) return;
            var pos = mousePosition - m_offset;
            pos.z = 0;
            transform.position = pos;
        }


        public void SetFurniture(Furniture furniture)
        {
            m_furniture = furniture;
            m_furniture.Name = transform.name;
            m_furniture.SideMatter = isSideMatter;
            m_fitterID = GetComponent<FitterID>();
            m_fitterID.Init(furniture);
        }
    }
}