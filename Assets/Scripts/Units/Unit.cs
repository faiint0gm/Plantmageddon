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
    protected GameObject targetObject;
    protected Vector3 target;

    protected AIDestinationSetter destinationSetter;
    protected AIPath aiPath;

    public UnitState UnitState
    {
        get { return unitState; }
    }

    private void Awake()
    {
        ResetHp(this);
    }

    public void TakeDamage(int dmg, bool isKilling = false)
    {
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

    public void ResetHp(Unit unit)
    {
        unit.currentHp = hp;
        unit.killingHp = hp;
    }

    public void GiveDamage(Unit unit,bool killable = false)
    {
        unit.TakeDamage(damage, killable);
    }

    public void ChangeForm()
    {
        int randomNumber = 0;
        if (unitType.ToString().Contains("player"))
        {
            randomNumber = Random.Range(4, 6);
        }
        else
        {
            randomNumber = Random.Range(0, 3);
        }
        GameObject go = Instantiate(GameManager.Instance.unitsPrefabs[(UnitType)randomNumber], transform.position,
                        Quaternion.identity, GameManager.Instance.unitsParent);
        if(randomNumber < 4)
        {
            GameManager.Instance.playerUnits.Add(go.GetComponent<PlayerUnit>());
        }
        else
        {
            GameManager.Instance.enemyUnits.Add(go.GetComponent<EnemyUnit>());
        }
        Die();
    }

    public virtual void Die()
    {
        GameManager.Instance.allUnits.Remove(this);
        Destroy(this);
    }

    public IEnumerator TakeOver(Unit unit)
    {
        while (unit.hp > 0)
        {
            yield return new WaitForSeconds(damageTime);
            GiveDamage(unit);
            if (unitState != UnitState.TAKING_OVER)
            {
                ResetHp(unit);
                break;
            }
        }
    }

    public IEnumerator Kill(Unit unit)
    {
        while (unit.hp > 0)
        {
            yield return new WaitForSeconds(damageTime);
            GiveDamage(unit,true);
            if (unitState != UnitState.KILLING)
            {
                ResetHp(unit);
                break;
            }
        }
    }

    public void TakeOverSelectedTarget(GameObject targetObj)
    {
        unitState = UnitState.TAKING_OVER;
        targetObject = targetObj;
    }

    protected void FollowAndTakeOver()
    {
        target = targetObject.transform.position;
        target = new Vector3(target.x, target.y, transform.position.z);
        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack)
        {
            StartCoroutine(TakeOver(targetObject.GetComponent<Unit>()));
        }
    }

    protected void FollowAndKill()
    {
        target = targetObject.transform.position;
        target = new Vector3(target.x, target.y, transform.position.z);
        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack)
        {
            StartCoroutine(Kill(targetObject.GetComponent<Unit>()));
        }
    }
}
