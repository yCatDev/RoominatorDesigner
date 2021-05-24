using System;
using Logic.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logic.Core
{
    [Serializable]
    public class Factory
    {
        [SerializeField] private GameObject wallPrefab;
        
        
        public WallControl CreateWallControl(Wall wall)
        {
            var obj = Object.Instantiate(wallPrefab).GetComponent<WallControl>();
            obj.transform.position = Vector3.zero;
            obj.SetWall(wall);
            obj.Setup();
            return obj;
        }
    }
}