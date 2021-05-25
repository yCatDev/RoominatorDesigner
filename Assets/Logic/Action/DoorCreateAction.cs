using Logic.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Action
{
    public class DoorCreateAction: ActionBehaviour
    {
        [SerializeField] private Toggle toggle;
        private DoorControl m_door;
        
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
            if (m_door != null)
                m_door.Delete();
        }

        protected override void OnClick(Vector3 position)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray.origin, ray.direction, out var hit))
            {
                if (hit.transform.gameObject.CompareTag("Wall"))
                {
                    m_door = CoreManager.Instance.CreateDoor(hit.transform.GetComponent<WallControl>(), position);
                }
            }
        }
    }
}