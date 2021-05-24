using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMover : MonoBehaviour
{
    private Vector3 m_mouse, m_offset;
    private Vector3 m_last;
    float mouseX;
    float mouseY;
    Vector3 cameraPos;
    [SerializeField] private float sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(2))
        {
            m_last = m_mouse;
        }

        if (Input.GetMouseButton(2))
        {
            mouseX = Input.GetAxis ("MouseX");
            mouseY = Input.GetAxis ("MouseY");
            cameraPos += transform.right * (mouseX * -1) * sensitivity;
            cameraPos += transform.up * (mouseY * -1) * sensitivity;
            transform.position = cameraPos;
        }
    }
}
