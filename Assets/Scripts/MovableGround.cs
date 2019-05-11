using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableGround : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        
        foreach (PlayerUnit player in GameManager.Instance.selectedUnits)
        {
            player.MoveToSelectedTarget();    
        }
    }
}
