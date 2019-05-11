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
            if (player != null && !GameManager.Instance.lockMovement)
            {
                player.InterruptPathFollowing();
                player.MoveToSelectedTarget();
            }
        }
    }
}
