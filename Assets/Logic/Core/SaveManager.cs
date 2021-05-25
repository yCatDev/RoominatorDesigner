using Newtonsoft.Json;
using UnityEngine;

namespace Logic.Core
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private string json;
        
        public void Save()
        {
            Debug.Log(JsonConvert.SerializeObject(CoreManager.Instance.SelectedRoom));
        }

        public void Load()
        {
            CoreManager.Instance.SelectedRoom = JsonConvert.DeserializeObject<Room>(json);
            CoreManager.Instance.RestoreRoom();
        }
        
    }
}