using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabsController : MonoBehaviour
{

    [SerializeField] private GameObject[] tabs;

    public void SwitchTab(int index)
    {
        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[index].SetActive(true);
    }
    
}
