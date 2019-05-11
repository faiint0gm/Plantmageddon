using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if(_instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                go.AddComponent<GameManager>();
                _instance = go.GetComponent<GameManager>();
            }
            return _instance; }
    }
    int unitsAmount;


    List<PlayerUnit>playerUnits = new List<PlayerUnit>();

    public Camera mainCamera;
    public GameObject actionMenu;
    [HideInInspector]
    public bool playersMoving;
    [HideInInspector]
    public bool playersAttacking;

    public List<PlayerUnit> selectedUnits = new List<PlayerUnit>();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Update()
    {
        if(selectedUnits.Count >0)
        {
            if(Input.GetMouseButton(1))
            {
                selectedUnits.Clear();
            }
        }
    }

    public void MoveUnits()
    {
        playersMoving = true;
    }

    public void AttackUnits()
    {
        playersAttacking = true;
    }
}
