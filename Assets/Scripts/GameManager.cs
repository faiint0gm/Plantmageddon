using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitsPrefabs
{
    public UnitType unitType;
    public GameObject unitPrefab;
}
public class GameManager : MonoBehaviour
{
    private bool initialized;

    static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if (_instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                go.AddComponent<GameManager>();
                _instance = go.GetComponent<GameManager>();
            }
            return _instance; }
    }
    int unitsAmount;

    public Camera mainCamera;

    [HideInInspector]
    public List<Unit> allUnits = new List<Unit>();
    [HideInInspector]
    public List<PlayerUnit>playerUnits = new List<PlayerUnit>();
    [HideInInspector]
    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    [HideInInspector]
    public List<PlayerUnit> selectedUnits = new List<PlayerUnit>();

    [SerializeField]
    UnitsPrefabs [] unitPrefabs;
        public Vector3 cameraToUnitOffset;

    public Transform unitsParent;

    public Dictionary<UnitType, GameObject> unitsPrefabs = new Dictionary<UnitType, GameObject>();

    public delegate void InitAction();
    public event InitAction OnInitialized;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        if(initialized)
        {
            OnInitialized?.Invoke();
            initialized = false;
        }

        if(selectedUnits.Count >0)
        {
            if(Input.GetMouseButton(1))
            {
                selectedUnits.Clear();
            }
        }
    }

    private void Init()
    {
        foreach (UnitsPrefabs prefab in unitPrefabs)
        {
            unitsPrefabs.Add(prefab.unitType, prefab.unitPrefab);
        }
        initialized = true;
    }
}
