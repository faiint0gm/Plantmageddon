using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float damageTime;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected UnitType unitType;
    
    protected int currentHp;
    protected int killingHp;

    private void Awake()
    {
        ResetHp();
    }

    public void TakeDamage(int dmg, bool isKilling = false)
    {
        currentHp -= dmg;
        if(isKilling)
        {
            killingHp -= dmg;
        }
    }

    public void ResetHp()
    {
        currentHp = hp;
        killingHp = hp;
    }

    public void GiveDamage(Unit unit,bool killable = false)
    {
        unit.TakeDamage(damage, killable);
    }

    public void ChangeForm()
    {

    }

    public virtual void Die()
    {
        GameManager.Instance.allUnits.Remove(this);
        Destroy(this);
    }
}
