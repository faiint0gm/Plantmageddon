using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    UnitType unitType;
    [SerializeField]
    int amount;

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomVector = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            if(amount == 1)
            { randomVector = Vector3.zero; }
            Instantiate(GameManager.Instance.unitsPrefabs[unitType], 
                transform.position + randomVector,
                Quaternion.identity,
                GameManager.Instance.unitsParent);
        }
    }


}
