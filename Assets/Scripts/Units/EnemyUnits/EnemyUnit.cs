using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Pathfinding;
public class EnemyUnit : Unit,IPointerClickHandler
{
    [SerializeField]
    private float safeDistance = 30;

    float smallestDistance = 100000;
    float enemySmallestDistance = 100000;
    PlayerUnit closestPlayer;
    Vector3 direction;
    Vector3 unsafeTarget;

    void Awake()
    {
        GameManager.Instance.allUnits.Add(this);
        GameManager.Instance.enemyUnits.Add(this);
        Debug.Log("EnemyUnit Awake: " + gameObject.name);
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

    public override void Die()
    {
        if (targetUnit != null)
        {
            targetUnit.UnitState = UnitState.IDLE;
        }
        GameManager.Instance.enemyUnits.Remove(this);
        base.Die();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.selectedUnits.Count > 0)
        {
            foreach(PlayerUnit player in GameManager.Instance.selectedUnits)
            {
                player.TakeOverSelectedTarget(this);
            }
        }
    }

    void FindAndSetClosestTarget()
    {
        smallestDistance = 100000;
        enemySmallestDistance = 100000;
        if (unitState != UnitState.BEING_ATTACKED)
        {
            foreach (PlayerUnit player in GameManager.Instance.playerUnits)
            {
                if (player != null)
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < smallestDistance)
                    {
                        smallestDistance = Vector3.Distance(player.transform.position, transform.position);
                        closestPlayer = player;
                        if (smallestDistance < safeDistance)
                        {
                            if (unitType == UnitType.enemyAttacker)
                            {
                                targetUnit = player;
                                unitState = UnitState.KILLING;
                            }
                            if (unitType == UnitType.enemyHealer)
                            {
                                targetUnit = player;
                                unitState = UnitState.TAKING_OVER;
                            }
                        }
                    }

                }
            }

            if(unitType==UnitType.enemyRunner)
            {
                foreach (EnemyUnit enemy in GameManager.Instance.enemyUnits)
                {
                    if (enemy.unitType == UnitType.enemyAttacker && smallestDistance<safeDistance)
                    {
                        if (Vector3.Distance(enemy.transform.position, transform.position) < enemySmallestDistance)
                        {
                            enemySmallestDistance = Vector3.Distance(enemy.transform.position, transform.position);
                            targetUnit = enemy;
                            unitState = UnitState.RUNNING;
                        }
                    }
                }
            }
            if (smallestDistance > safeDistance)
            {
                unitState = UnitState.IDLE;
                targetUnit = null;
                target = transform.position;
            }
        }
    }

    private void Update()
    {
        FindAndSetClosestTarget();

        if (targetUnit != null)
        {
            if (unitState == UnitState.TAKING_OVER)
            {
                aiPath.endReachedDistance = rangeToAttack;
                FollowAndTakeOver();
            }
            else if (unitState == UnitState.KILLING)
            {
                aiPath.endReachedDistance = rangeToAttack;
                FollowAndKill();
            }
            else if (unitState == UnitState.RUNNING)
            {
                aiPath.endReachedDistance = rangeToAttack;
                FollowAttacker();
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

    void FollowAttacker()
    {
        target = targetUnit.transform.position;
        target = new Vector3(target.x + Random.Range(-15,15), target.y + Random.Range(-15, 15), transform.position.z);
        destinationSetter.TargetPositionSet(target);
    }

}
