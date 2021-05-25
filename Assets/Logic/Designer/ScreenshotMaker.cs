using System;
using System.IO;
using UnityEngine;

namespace Logic.Designer
{
    public class ScreenshotMaker : MonoBehaviour
    {

        private Camera m_camera;

        private void Awake()
        {
            m_camera = GetComponent<Camera>();
        }

        public void ScreenshotRoom()
        {
            FitToRoom();
            
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = m_camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 1);
 
            m_camera.Render();
 
            Texture2D Image = new Texture2D(m_camera.targetTexture.width, m_camera.targetTexture.height);
            Image.ReadPixels(new Rect(0, 0, m_camera.targetTexture.width, m_camera.targetTexture.height), 0, 0);
            Image.Apply();
            RenderTexture.active = currentRT;
 
            var Bytes = Image.EncodeToPNG();
            Destroy(Image);
 
            File.WriteAllBytes(Application.dataPath + "/" + "test" + ".png", Bytes);
        }

        private void FitToRoom()
        {
            var bounds = new Bounds();
            foreach (var wall in CoreManager.Instance.SelectedRoom.Walls)           
            {
                bounds.Encapsulate(wall.StartPoint.Value);
                bounds.Encapsulate(wall.EndPoint.Value);
            }

            float orthographicSize = m_camera.orthographicSize;
            Vector3 topRight = new Vector3(bounds.max.x, bounds.max.y, 0f);
            Vector3 topRightAsViewport = m_camera.WorldToViewportPoint(topRight);
        
            if (topRightAsViewport.x >= topRightAsViewport.y)
                orthographicSize = Mathf.Abs(bounds.size.x) / m_camera.aspect / 2f;
            else
                orthographicSize = Mathf.Abs(bounds.size.y) / 2f;

            m_camera.orthographicSize = orthographicSize+3;
            m_camera.transform.position = bounds.center+Vector3.back*5;
        }
        
    }
}