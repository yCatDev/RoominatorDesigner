using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Action;
using Logic.Controls;
using Logic.Core;
using Logic.Designer;
using Newtonsoft.Json;
using UnityEngine;

namespace Logic
{
    public class CoreManager : MonoBehaviour
    {
        public Room SelectedRoom;
        [SerializeField] private Factory m_factory;
        public ActionsManager ActionsManager = new ActionsManager();
        public static CoreManager Instance;
        [SerializeField] private string focusName;
        public string UserId;
        public UserRoom UserRoom;
        public bool Loaded = false;

        private void Awake()
        {
            Instance = this;
        }
        
        
        private void Start()
        {
            if (SelectedRoom==null) 
                SelectedRoom = new Room();
        }
        

        public WallControl CreateWall(Vector3 startPosition)
        {
            startPosition.z = 0;
            var wall = new Wall()
            {
                StartPoint = new WallPoint(startPosition, false),
            };
            wall.EndPoint = new WallPoint(startPosition, true);
            SelectedRoom.Walls.Add(wall);
            return m_factory.CreateWallControl(wall);
        }


        public WindowControl CreateWindow(WallControl wc, Vector2 position)
        {
            var wall = wc.GetWall();
            var w = 0f;
            var h = 0f;
            var dir = Mathf.Abs((wall.StartPoint.Value - wall.EndPoint.Value).x) <= 0.1f ? Direction.Horizontal : Direction.Vertical;
            if (dir==Direction.Horizontal)
            {
                w = 5;
                h = 1;
            }
            else
            {
                w = 1;
                h = 5;
            }

            var window = new Window()
            {
                Width = w,
                Height = h,
                Direction = dir,
                WallIndex = SelectedRoom.Walls.IndexOf(wall)
            };
            SelectedRoom.Windows.Add(window);
            var contrl = m_factory.CreateWindowControl(window);
            var clamped = ((Vector3)position).ClampVector(wall.StartPoint.Value, wall.EndPoint.Value);
            clamped.z = -1;
            contrl.transform.position = clamped;
            return contrl;
        }
        
        public DoorControl CreateDoor(WallControl wc, Vector2 position)
        {
            var wall = wc.GetWall();
            var w = 0f;
            var h = 0f;
            var dir = Mathf.Abs((wall.StartPoint.Value - wall.EndPoint.Value).x) <= 0.1f ? Direction.Horizontal : Direction.Vertical;
           // Debug.Log( Mathf.Abs((wall.StartPoint.Value - wall.EndPoint.Value).x)+" "+dir.ToString());
            if (dir==Direction.Horizontal)
            {
                w = 5;
                h = 1;
            }
            else
            {
                w = 1;
                h = 5;
            }

            var door = new Door()
            {
                Width = w,
                Height = h,
                Direction = dir,
                WallIndex = SelectedRoom.Walls.IndexOf(wall)
            };
            SelectedRoom.Doors.Add(door);
            var contrl = m_factory.CreateDoorControl(door);
            var clamped = ((Vector3)position).ClampVector(wall.StartPoint.Value, wall.EndPoint.Value);
            clamped.z = -1;
            contrl.transform.position = clamped;
            return contrl;
        }

        public void RestoreRoom()
        {
            SelectedRoom = JsonConvert.DeserializeObject<Room>(UserRoom.Json);
            Debug.Log($"Gotten json of room: {UserRoom.Json}");
            foreach (var wall in SelectedRoom.Walls)
            {
                CreateWallFrom(wall);
            }

            foreach (var door in SelectedRoom.Doors)    
            {
                CreateDoorFrom(door);
            }

            foreach (var window in SelectedRoom.Windows)
            {
                CreateWindowFrom(window);
            }

            foreach (var furniture in SelectedRoom.Furnitures)
            {
                CreateFurnitureFrom(furniture);
            }
        }

        private void CreateFurnitureFrom(Furniture furniture)
        {
            var prefab = m_factory.FindFurniture(furniture.Name);
            if (prefab==null) return;
            var obj = Instantiate(prefab);
            obj.transform.position = furniture.Position;
            obj.transform.localScale = furniture.Scale;
            obj.transform.localEulerAngles = furniture.Rotation;
            obj.GetComponent<FurnitureControl>().SetFurniture(furniture);
        }

        private void CreateWallFrom(Wall wall)
        {
            m_factory.CreateWallControl(wall);
        }

        private void CreateWindowFrom(Window window)
        {
            //window.Wall.UpdateList(SelectedRoom.Walls);
            var contrl = m_factory.CreateWindowControl(window);
            var wall = SelectedRoom.Walls[window.WallIndex];
            var clamped = ((Vector3)window.Position).ClampVector(wall.StartPoint.Value, wall.EndPoint.Value);
            clamped.z = -1;
            contrl.transform.position = clamped;
        }

        private void CreateDoorFrom(Door door)
        {
            //door.Wall.UpdateList(SelectedRoom.Walls);
            var contrl = m_factory.CreateDoorControl(door);
            var wall = SelectedRoom.Walls[door.WallIndex];
            var clamped = ((Vector3)door.Position).ClampVector(wall.StartPoint.Value, wall.EndPoint.Value);
            clamped.z = -1;
            contrl.transform.position = clamped;
        }
        
    }
}