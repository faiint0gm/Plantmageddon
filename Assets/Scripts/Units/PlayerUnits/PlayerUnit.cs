using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class PlayerUnit : Unit,IPointerClickHandler
{


    public void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        if (destinationSetter != null)
        {
            destinationSetter.playerTarget = transform.position;
        }
        if (aiPath != null)
        {
            aiPath.maxSpeed = moveSpeed;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.selectedUnits.Add(this);
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
        if (targetObject != null)
        {
            if (targetObject.GetComponent<Unit>().UnitState != UnitState.BEING_ATTACKED)
            {
                if (unitState == UnitState.TAKING_OVER)
                {
                    FollowAndTakeOver();
                }
                if (unitState == UnitState.KILLING)
                {
                    FollowAndKill();
                }
            }

        }
    }

    public override void Die()
    {
        GameManager.Instance.playerUnits.Remove(this);
        base.Die();
    }
}
