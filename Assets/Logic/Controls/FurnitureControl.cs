using System;
using Drawing;
using Logic.Core;
using UnityEngine;

namespace Logic.Controls
{
    public class FurnitureControl : ControlBehaviour
    {
        private float m_width, m_height;

        private Vector2[] m_dragDots = new Vector2[4];
        private Vector2 m_rotateDot;

        private float m_scale = 3;
        [SerializeField] private Mesh sphere;
        private Vector3 m_offset, m_dragOffset;
        private bool m_captured;
        private Vector3 m_baseScale = new Vector3(3,3,3);
        private SpriteRenderer m_spr;
        private Furniture m_furniture;

        private void Start()
        {
            Setup();
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


        private float Diff(Vector2 a, Vector2 b)
        {
            var num1 = b.x - a.x;
            var num2 = a.y - b.y;
            return  num1 + num2;
        }
        
        private void Update()
        {
            UpdateCoords();
            //if (!Input.GetMouseButtonDown(0)) return;
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0) && Vector2.Distance(mousePosition, m_rotateDot)<=1)
            {
                transform.localEulerAngles += Vector3.forward*90;
            }
            
            if (!Input.GetMouseButton(0))
            {
                m_captured = false;
                return;
            }

            
            if (m_captured)
            {
                m_scale = Diff(m_dragOffset,mousePosition);
                transform.localScale = m_baseScale+m_baseScale * (m_scale / 10f);
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
    }
}