using System;
using System.Linq;
using Logic.Controls;
using Logic.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Action
{
    public class WallCreateAction : ActionBehaviour
    {
        private int m_phase = 0;
        private WallControl wc;
        [SerializeField] private Toggle toggle;


        protected override void OnStart()
        {
           
            CoreManager.Instance.ActionsManager.RegisterBehaviour(this);
        }

        protected override void OnActivate()
        {
        }

        protected override void OnDeactivate()
        {
            toggle.isOn = false;
            m_phase = 0;
            if (wc != null && wc.NotFixed())
                wc.Delete();
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                OnDeactivate();
        }

        protected override void OnClick(Vector3 position)
        {
            wc?.AlignDots();
            if (CoreManager.Instance.SelectedRoom.IsRoomShapeCorrect)
            {
                toggle.isOn = false;
                m_phase = 0;
                Active = false;
                return;
            }

            if (CoreManager.Instance.SelectedRoom.Walls.Count > 0)
                wc = CoreManager.Instance.CreateWall(CoreManager.Instance.SelectedRoom.Walls.Last().StartPoint
                    .Value);
            else
                wc = CoreManager.Instance.CreateWall(position);
            wc.ForceSelect();
            wc.ForceFollow();
        }
    }
}