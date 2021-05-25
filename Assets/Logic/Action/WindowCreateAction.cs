using Logic.Controls;
using Logic.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Action
{
    public class WindowCreateAction: ActionBehaviour
    {
        
        [SerializeField] private Toggle toggle;
        private WindowControl m_window;
        
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
            if (m_window != null)
                m_window.Delete();
        }

        protected override void OnClick(Vector3 position)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray.origin, ray.direction, out var hit))
            {
                if (hit.transform.gameObject.CompareTag("Wall"))
                {
                    m_window = CoreManager.Instance.CreateWindow(hit.transform.GetComponent<WallControl>(), position);
                }
            }
        }
    }
}