using System;
using Drawing;
using Logic.Core;
using Unity.Mathematics;
using UnityEngine;

namespace Logic.Controls
{
    public class WindowControl: ControlBehaviour
    {
        [SerializeField] private Mesh m_sphere;
        private Window m_window;
        private float m_width, m_height;

        protected override void OnSelectedFirstTime()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDragEnd()
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            throw new System.NotImplementedException();
        }

        public override void Delete()
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            if (m_window.Direction == Direction.Vertical)
            {
                m_width = m_window.Width;
                m_height = m_window.Height;
            }
            else
            {
                m_width = m_window.Height;
                m_height = m_window.Width;
            }
        }

        public override void UpdateOnSelected()
        {
            using (Draw.ingame.InLocalSpace(transform))
            {
                Draw.ingame.SolidPlane(transform.position, Vector3.up, 
                    new float2(m_width, m_height), Color.blue);
            }

            if (m_window.Direction == Direction.Vertical)
            {
            }
        }

        public override void UpdateOnDrag(Vector3 mousePosition)
        {
            
        }
    }
}