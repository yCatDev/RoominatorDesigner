using System;
using Logic.Controls;
using Logic.Core;
using UnityEngine;

namespace Logic.Action
{
    public abstract class ActionBehaviour: MonoBehaviour
    {
        private void Start()
        {
            OnStart();
        }

        protected abstract void OnStart();

        public bool Active = false;
        [SerializeField] protected LayerMask targetLayer;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && Active && !ControlsManager.IsPointerOnUI)
            {
                var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(screenRay.origin, screenRay.direction, out var hit, float.PositiveInfinity, targetLayer))
                    if (hit.transform!=null)
                        OnClick(hit.point);
            }
        }

        public void Activate()
        {
            if (Active)
            {
                Active = false;
                return;
            }

            Active = true;
            OnActivate();
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();

        protected abstract void OnClick(Vector3 position);
    }
}