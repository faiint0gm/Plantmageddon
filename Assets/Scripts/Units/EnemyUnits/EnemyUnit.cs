using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyUnit : Unit,IPointerClickHandler
{
    public override void Die()
    {
        GameManager.Instance.enemyUnits.Remove(this);
        base.Die();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.selectedUnits.Count > 0)
        {
            foreach(PlayerUnit player in GameManager.Instance.selectedUnits)
            {
                player.TakeOverSelectedTarget(gameObject);
            }
        }
    }
}
