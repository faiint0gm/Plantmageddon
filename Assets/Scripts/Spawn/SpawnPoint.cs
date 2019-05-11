using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    UnitType unitType;
    [SerializeField]
    int amount;
    [SerializeField]
    float randomPositionRange = 3;

    void Start()
    {
       GameManager.Instance.OnInitialized += Spawn;
    }

    void Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomVector = new Vector3(Random.Range(-randomPositionRange, randomPositionRange), 
                Random.Range(-randomPositionRange, randomPositionRange), 0);
            if (amount == 1)
            { randomVector = Vector3.zero; }
            Instantiate(GameManager.Instance.unitsPrefabs[unitType],
                transform.position + randomVector,
                Quaternion.identity,
                GameManager.Instance.unitsParent);
        }
    }

}
