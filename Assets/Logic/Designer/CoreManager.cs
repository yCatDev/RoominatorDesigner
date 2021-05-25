using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Action;
using Logic.Controls;
using Logic.Core;
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

        private void Awake()
        {
            Instance = this;
        }
        
        
        private void Start()
        {
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
            Debug.Log( Mathf.Abs((wall.StartPoint.Value - wall.EndPoint.Value).x)+" "+dir.ToString());
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
                Wall = wall
            };
            SelectedRoom.Windows.Add(window);
            var contrl = m_factory.CreateWindowControl(window);
            var clamped = ((Vector3)position).ClampVector(window.Wall.StartPoint.Value, window.Wall.EndPoint.Value);
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
            Debug.Log( Mathf.Abs((wall.StartPoint.Value - wall.EndPoint.Value).x)+" "+dir.ToString());
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
                Wall = wall
            };
            SelectedRoom.Doors.Add(door);
            var contrl = m_factory.CreateDoorControl(door);
            var clamped = ((Vector3)position).ClampVector(door.Wall.StartPoint.Value, door.Wall.EndPoint.Value);
            clamped.z = -1;
            contrl.transform.position = clamped;
            return contrl;
        }
    }
}