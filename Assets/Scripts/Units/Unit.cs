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
    protected int damageTime;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected UnitType unitType;
    
    protected int currentHp;
}
