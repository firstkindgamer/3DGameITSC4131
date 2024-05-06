using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class clickTower : MonoBehaviour, IPointerClickHandler
{
    public UIScript script;
    public void OnPointerClick(PointerEventData eventData)
    {
        //script.clickHandle(eventData);
    }
}
