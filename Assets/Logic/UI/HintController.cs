using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HintController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hint;
    [SerializeField] private Toggle toggle;

    public void HideHint()
    {
        hint.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggle.isOn) return;
        hint.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (toggle.isOn) return;
        hint.SetActive(false);
    }
}
