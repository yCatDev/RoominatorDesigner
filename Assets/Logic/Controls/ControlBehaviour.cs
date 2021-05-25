using System;
using Logic.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Logic.Controls
{
    public abstract class ControlBehaviour : MonoBehaviour
    {
        protected bool m_selected = false, m_dragable = false, m_firstClick = false;
        private bool m_pointerOnNonUIitem;

        protected void OnMouseDown()
        {
            if (!m_selected && !ControlsManager.IsPointerOnUI && CanBeSelected())
            {
                OnSelectedFirstTime();
                ForceSelect();
            }

            OnClick();
        }

        protected virtual bool CanBeSelected()
        {
            return true;
        }
        
        protected virtual bool CanBeDragged()
        {
            return true;
        }
        
        protected virtual bool CanBeUnSelected()
        {
            return true;
        }

        protected abstract void OnSelectedFirstTime();


        protected void OnMouseDrag()
        {
            if (ControlsManager.IsPointerOnUI || !CanBeDragged()) return;
            m_dragable = true;
            m_selected = true;
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            UpdateOnDrag(position);
        }

        protected void OnMouseUp()
        {
            if (ControlsManager.IsPointerOnUI || !CanBeUnSelected()) return;
            m_dragable = false;
            m_firstClick = false;
            
            OnDragEnd();
        }

        public void ForceSelect()
        {
            m_selected = true;
            m_firstClick = true;
            ControlsManager.Instance.SelectThisControl(this);
        }

        protected abstract void OnDragEnd();

        protected void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Delete) && m_selected)
                Delete();
            if (m_selected)
            {
                UpdateOnSelected();
            }

            if (Input.GetMouseButtonDown(0) && !m_firstClick)
            {
                var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hits = Physics2D.RaycastAll(screenRay.origin, screenRay.direction);
                if (hits.Length == 0)
                {
                    UnSelect();
                }
            }
        }


        public abstract void Setup();
        public abstract void Delete();
        public abstract void UpdateOnSelected();
        public abstract void UpdateOnDrag(Vector3 mousePosition);

        protected virtual void OnDeSelecting()
        {
        }

        internal void UnSelect()
        {
            m_selected = false;
            OnDeSelecting();
        }

        protected virtual void OnClick()
        {
            
        }
    }
}