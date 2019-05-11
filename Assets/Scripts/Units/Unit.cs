using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float damageTime;
    [SerializeField]
    protected float rangeToAttack;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected UnitType unitType;

    protected UnitState unitState;
    protected int currentHp;
    protected int killingHp;
    protected Unit targetUnit;
    protected Vector3 target;

    protected AIDestinationSetter destinationSetter;
    protected AIPath aiPath;
    protected float endReachedDistance;

    public bool isChangingForm;
    bool takingOverStarted;
    bool killingStarted;
    RaycastHit2D hit2d;

    public UnitState UnitState
    {
        get { return unitState; }
        set { unitState = value; }
    }

    public UnitType UnitType
    {
        get { return unitType; }
        
    }

    private void Start()
    {
        InitHp();
        unitState = UnitState.IDLE;
    }

    public void TakeDamage(int dmg, bool isKilling = false)
    {
        unitState = UnitState.BEING_ATTACKED;
        currentHp -= dmg;
        if(isKilling)
        {
            killingHp -= dmg;
        }
        if(currentHp <= 0)
        {
            currentHp = 0;
        }
        if(killingHp <= 0)
        {
            killingHp = 0;
        }
    }

    public void ResetCurrentHp(Unit unit)
    {
        unit.currentHp = unit.hp;
    }

    void InitHp()
    {
        currentHp = hp;
        killingHp = hp;
    }

    public void GiveDamage(Unit unit,bool killable = false)
    {
        unit.TakeDamage(damage, killable);
    }

    public void ChangeForm(UnitType attackerType)
    {
        isChangingForm = true;
        InterruptPathFollowing();
        int randomNumber = 0;
        if (attackerType.ToString().Contains("enemy"))
        {
            randomNumber = Random.Range(4, 6);
        }
        else
        {
            randomNumber = (int)attackerType;
        }
        Debug.Log("Change form! Called from: " + attackerType.ToString());
        Instantiate(GameManager.Instance.unitsPrefabs[(UnitType)randomNumber], transform.position,
                        Quaternion.identity, GameManager.Instance.unitsParent);
        Die();
    }

    public virtual void Die()
    {
        GameManager.Instance.allUnits.Remove(this); 
        Destroy(gameObject);
    }

    public void BlowOver()
    {
        foreach(EnemyUnit e in GameManager.Instance.enemyUnits)
        {
            if(Vector3.Distance(transform.position,e.transform.position)<rangeToAttack)
            {
                StartCoroutine(TakeOver(e));
            }
        }
    }

    public IEnumerator TakeOver(Unit unit)
    {
        unit.InterruptPathFollowing();
        unit.aiPath.canMove = false;
        while (unit.currentHp > 0)
        {
            yield return new WaitForSeconds(damageTime);
            GiveDamage(unit);
            unitState = UnitState.TAKING_OVER;
            if (unitState != UnitState.TAKING_OVER)
            {
                ResetCurrentHp(unit);
                unit.unitState = UnitState.IDLE;
                unit.aiPath.canMove = true;
                break;
            }
        }
        if(unit.currentHp <=0)
        {
            unitState = UnitState.IDLE;
            if (unit != null && !unit.isChangingForm)
            {
                unit.ChangeForm(unitType);
            }
            takingOverStarted = false;
            unit.aiPath.canMove = false;
        }
    }

    public IEnumerator Kill(Unit unit)
    {
        while (unit.killingHp > 0)
        {
            yield return new WaitForSeconds(damageTime);
            if (unit == null)
            {
                killingStarted = false;
                unitState = UnitState.IDLE;
                break;
            }
            GiveDamage(unit,true);
            if (unitState != UnitState.KILLING)
            {
                killingStarted = false;
                yield break;
            }
            if(Vector3.Distance(transform.position,unit.transform.position)>rangeToAttack)
            {
                killingStarted = false;
                yield break;
            }

        }
        if (unit != null)
        {
            if (unit.killingHp <= 0)
            {
                unitState = UnitState.IDLE;
                if (unit != null)
                {
                    unit.Die();
                }
                killingStarted = false;
            }
        }
        else
        {
            yield break;
        }
    }

    public void TakeOverSelectedTarget(Unit targetObj)
    {
        unitState = UnitState.TAKING_OVER;
        targetUnit = targetObj;
    }

    protected virtual void FollowAndTakeOver()
    {
        if (takingOverStarted)
        {
            target = transform.position;
        }
        else
        {
            target = targetUnit.transform.position;
            target = new Vector3(target.x, target.y, transform.position.z);
        }
        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack && !takingOverStarted)
        {
            takingOverStarted = true;
            StartCoroutine(TakeOver(targetUnit));
        }
    }

    protected void FollowAndBlowOver()
    {
        if(takingOverStarted)
        {
            target = transform.position;
        }
        else
        {
            target = targetUnit.transform.position;
            target = new Vector3(target.x, target.y, transform.position.z);
        }
        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack && !takingOverStarted)
        {
            takingOverStarted = true;
            BlowOver();
            Invoke("Die",0.2f);
        }
    }

    protected void FollowAndKill()
    {
        target = targetUnit.transform.position;
        target = new Vector3(target.x, target.y, transform.position.z);
        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack && !killingStarted)
        {
            Debug.DrawRay(transform.position, (targetUnit.transform.position-transform.position).normalized, Color.red,3f);
            hit2d = Physics2D.Raycast(transform.position, (targetUnit.transform.position - transform.position).normalized,
                rangeToAttack);

            Debug.Log("Raycast hit : " + hit2d.collider.gameObject.layer.ToString());

            killingStarted = true;
            target = transform.position;
            StartCoroutine(Kill(targetUnit));
        }
    }

    public void InterruptPathFollowing()
    {
        StopAllCoroutines();
        target = transform.position;
        targetUnit = null;
    }

    private void Update()
    {
        if (unitState == UnitState.IDLE && !aiPath.canMove)
        {
            aiPath.canMove = true;
        }
        if (takingOverStarted)
        {
            InterruptPathFollowing();
        }
    }
}
