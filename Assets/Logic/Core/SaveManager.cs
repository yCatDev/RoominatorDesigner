using System.Runtime.InteropServices;
using Logic.Designer;
using Newtonsoft.Json;
using UnityEngine;

namespace Logic.Core
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private ScreenshotMaker screenshotMaker;

        [DllImport("__Internal")]
        private static extern void SaveRoom(string userId, string json);

        public void Save()
        {
            var userId = CoreManager.Instance.UserId;
            CoreManager.Instance.UserRoom.Json = JsonConvert.SerializeObject(CoreManager.Instance.SelectedRoom);
            CoreManager.Instance.UserRoom.Preview = screenshotMaker.ScreenshotForPreview();
            var json =  JsonConvert.SerializeObject(CoreManager.Instance.UserRoom);
            Debug.Log(json);
            
            SaveRoom(userId, json);
        }

        public void SetUser(string userId)
        {
            CoreManager.Instance.UserId = userId;
        }
        
        public void LoadRoom(string json)
        {
            if (CoreManager.Instance.Loaded) return;
            CoreManager.Instance.UserRoom = JsonConvert.DeserializeObject<UserRoom>(json);
            Debug.Log($"Gotten json: {json}");
            CoreManager.Instance.RestoreRoom();
            CoreManager.Instance.Loaded = true;
        }
        
    }
}