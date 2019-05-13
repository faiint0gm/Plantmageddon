using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class PlayerUnit : Unit,IPointerClickHandler
{
    Material material;
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
    protected override void Start()
    {
        base.Start();
        material = GetComponent<SpriteRenderer>().material;
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

    public void MoveToTarget(Vector3 thisTarget)
    {
        target = thisTarget;
        target = new Vector3(target.x, target.y, transform.position.z);
        destinationSetter.TargetPositionSet(target);
    }

    protected override void Update()
    {
        base.Update();
        HandleAnimations();
        if(GameManager.Instance.selectedUnits.Contains(this))
        {
            material.SetFloat("_IsOutlineEnabled", 1);
        }
        else 
        {
           material.SetFloat("_IsOutlineEnabled", 0);
        }
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
            if(unitState == UnitState.TAKING_OVER)
            {
                unitState = UnitState.IDLE;
                aiPath.canMove = true;
            }

            if (aiPath != null)
            {
                aiPath.endReachedDistance = endReachedDistance;
            }
        }

        if(killingHp<=0)
        {
            Die();
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

    protected override void HandleAnimations()
    {
        base.HandleAnimations();
       

    }
}
