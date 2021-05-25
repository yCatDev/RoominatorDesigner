using Logic.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.Action
{
    public class FurnitureCreateAction: ActionBehaviour
    {

        [SerializeField] private GameObject furniturePrefab;
        [SerializeField] private Toggle toggle;

        protected override void OnStart()
        {
            
        }

        protected override void OnActivate()
        {
            
        }

        protected override void OnDeactivate()
        {
            toggle.isOn = false;
        }

        protected override void OnClick(Vector3 position)
        {
            var obj = Instantiate(furniturePrefab);
            obj.transform.position = position;
            var fc = obj.GetComponent<FurnitureControl>();
            var furniture = fc.GetFurniture();
            CoreManager.Instance.SelectedRoom.Furnitures.Add(furniture);
            toggle.isOn = false;
        }
    }
}