using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Logic.Controls
{
    public enum ControlsState
    {
        Nothing, Drawing
    }
    public class ControlsManager : MonoBehaviour
    {

        private Dictionary<Type, List<Object>> m_controls;
        private ControlBehaviour m_selectedControl;
        
        public static ControlsManager Instance;
        public ControlsState CurrentState { get; internal set; }
        
        private void Awake()
        {
            m_controls = new Dictionary<Type, List<Object>>();
            Instance = this;
        }

        private void Update()
        {
            /*if (Input.GetMouseButtonDown(0))
            {
                var targetLayer = 1<<LayerMask.NameToLayer("Grid");
                var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(screenRay.origin, screenRay.direction, out var hit, float.PositiveInfinity, targetLayer))
                    if (hit.transform!=null)
                        foreach (var control in m_controls)
                        {
                            foreach (ControlBehaviour behaviour in control.Value)
                            {
                                behaviour.UnSelect();
                            }
                        }
            }*/
        }

        public static bool IsPointerOnUI
        {
            get
            {
                var data = new List<RaycastResult>();
                var pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;
                EventSystem.current.RaycastAll(pointer, data);
                /*data.ForEach((x) =>
                {
                    Debug.Log(x.gameObject.name);
                });*/
                return data.Count>0;
            }
        }

        public void ChangeState(ControlsState newState)
        {
            CurrentState = newState;
        }

        internal void AddControl<T>(T control) where T : ControlBehaviour
        {
            var t = typeof(T);
            if (!m_controls.ContainsKey(t))
            {
                m_controls.Add(t, new List<Object>(1));
            }
            m_controls[t].Add(control);
        }

        internal void RemoveControl<T>(T control) where T : ControlBehaviour
        {
            var t = typeof(T);
            m_controls[t].Remove(control);
        }

        public T[] GetControls<T>() where T : ControlBehaviour
        {
            var t = typeof(T);
            return m_controls.ContainsKey(t) ? m_controls[t].Select(x => x).Cast<T>().ToArray() : null;
        }

        public void SelectThisControl(ControlBehaviour behaviour)
        {
            if (m_selectedControl) m_selectedControl.UnSelect();
            m_selectedControl = behaviour;
        }

    }
}