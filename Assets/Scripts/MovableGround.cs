using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableGround : MonoBehaviour, IPointerClickHandler
{
    List<Vector3> targets = new List<Vector3>();
    float radius = 5f;
    float cx = 0f;
    float cy = 0f;
    float x = 0f;
    float y = 0f;
    float spacingRads;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.selectedUnits.Count == 1)
        {
            if (GameManager.Instance.selectedUnits[0] != null && !GameManager.Instance.lockMovement)
            {
                if (GameManager.Instance.selectedUnits[0].aiPath.canMove)
                {
                    GameManager.Instance.selectedUnits[0].InterruptPathFollowing();
                    GameManager.Instance.selectedUnits[0].MoveToSelectedTarget();
                }
            }

        }
        else if (GameManager.Instance.selectedUnits.Count > 1)
        {
            targets.Clear();
            targets.Add(GameManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition));
            cx = targets[0].x;
            cy = targets[0].y;
            spacingRads = 360 / (GameManager.Instance.selectedUnits.Count - 1) * Mathf.Deg2Rad;

            for (int i = 0; i < GameManager.Instance.selectedUnits.Count-1; i++)
            {
                x = cx + (radius * Mathf.Cos(spacingRads * i));
                y = cy + (radius * Mathf.Cos(spacingRads * i));
                targets.Add(new Vector3(x, y, 0));
            }


            for (int i = 0; i <GameManager.Instance.selectedUnits.Count;i++)
            {
                if (GameManager.Instance.selectedUnits[i] != null && !GameManager.Instance.lockMovement)
                {
                    if (GameManager.Instance.selectedUnits[i].aiPath.canMove)
                    {
                        GameManager.Instance.selectedUnits[i].InterruptPathFollowing();
                        GameManager.Instance.selectedUnits[i].MoveToTarget(targets[i]);
                    }
                }
            }

        }
    }
}
