using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public override void Die()
    {
        GameManager.Instance.enemyUnits.Remove(this);
        base.Die();
    }
}
