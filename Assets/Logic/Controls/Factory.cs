using System;
using Logic.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logic.Core
{
    [Serializable]
    public class Factory
    {
        [SerializeField] private GameObject wallPrefab, windowPrefab, doorPrefab;
        [SerializeField] private FurnitureControl[] furnitures;


        public GameObject FindFurniture(string name)
        {
            foreach (var furniture in furnitures)
            {
                if (furniture.name == name)
                    return furniture.gameObject;
            }

            return null;
        }
        
        public WindowControl CreateWindowControl(Window window)
        {
            var obj = Object.Instantiate(windowPrefab).GetComponent<WindowControl>();
            obj.transform.position = Vector3.zero;
            obj.SetWindow(window);
            obj.Setup();
            return obj;
        }
        
        public WallControl CreateWallControl(Wall wall)
        {
            var obj = Object.Instantiate(wallPrefab).GetComponent<WallControl>();
            obj.transform.position = Vector3.zero;
            obj.SetWall(wall);
            obj.Setup();
            return obj;
        }

        public DoorControl CreateDoorControl(Door door)
        {
            var obj = Object.Instantiate(doorPrefab).GetComponent<DoorControl>();
            obj.transform.position = Vector3.zero;
            obj.SetDoor(door);
            obj.Setup();
            return obj;
        }
    }
}