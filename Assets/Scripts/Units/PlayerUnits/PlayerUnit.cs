using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class PlayerUnit : Unit,IPointerClickHandler
{

    public void Awake()
    {
        GameManager.Instance.allUnits.Add(this);
        GameManager.Instance.playerUnits.Add(this);
        Debug.Log("PlayerUnit Awake: " + gameObject.name);
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        if (destinationSetter != null)
        {
            destinationSetter.playerTarget = transform.position;
        }
        if (aiPath != null)
        {
            aiPath.maxSpeed = moveSpeed;
            endReachedDistance = aiPath.endReachedDistance;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.selectedUnits.Count>0)
        {
            GameManager.Instance.selectedUnits.Clear();
        }
        GameManager.Instance.selectedUnits.Add(this);
        CameraController.followUnit = true;
        Debug.Log("PlayerUnit : "+unitType.ToString()+ " Clicked");
    }

    public void MoveToSelectedTarget()
    {
        target = GameManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, target.y, transform.position.z);
        destinationSetter.TargetPositionSet(target);
    }

    void Update()
    {
        if (targetUnit != null)
        {
            if (unitState == UnitState.TAKING_OVER)
            {
                aiPath.endReachedDistance = rangeToAttack;
                if (unitType == UnitType.playerBlower)
                {
                    FollowAndBlowOver();
                }
                else
                {
                    FollowAndTakeOver();
                }
            }
        }
        else
        {
            if (aiPath != null)
            {
                aiPath.endReachedDistance = endReachedDistance;
            }
        }
    }

    public override void Die()
    {
        if (GameManager.Instance.selectedUnits.Contains(this))
        {
            GameManager.Instance.selectedUnits.Remove(this);
        }
        GameManager.Instance.playerUnits.Remove(this);
        base.Die();
    }
}
