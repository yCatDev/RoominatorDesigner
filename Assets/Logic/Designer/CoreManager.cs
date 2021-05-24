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

       
    }
}