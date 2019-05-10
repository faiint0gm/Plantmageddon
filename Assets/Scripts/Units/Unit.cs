using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField]
    int hp;
    [SerializeField]
    int damage;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    UnitType unitType;
}
