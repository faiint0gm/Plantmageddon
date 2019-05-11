using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct UnitsPrefabs
{
    public UnitType unitType;
    public GameObject unitPrefab;
}
public class GameManager : MonoBehaviour
{
    private bool initialized;
    public static GameManager Instance { get; private set; }
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
    [SerializeField]
    Image playerUnitsProgress;
    [SerializeField]
    TextMeshProUGUI playerAmountText;
    [SerializeField]
    TextMeshProUGUI enemyAmountText;

    public Transform unitsParent;

    public Dictionary<UnitType, GameObject> unitsPrefabs = new Dictionary<UnitType, GameObject>();

    public delegate void InitAction();
    public event InitAction OnInitialized;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

        UpdateAmounts();
    }

    private void Init()
    {
        foreach (UnitsPrefabs prefab in unitPrefabs)
        {
            unitsPrefabs.Add(prefab.unitType, prefab.unitPrefab);
        }
        initialized = true;
    }

    void UpdateAmounts()
    {
        playerAmountText.text = string.Format("Evil plants: {0}", playerUnits.Count);
        enemyAmountText.text = string.Format("Humans: {0}", enemyUnits.Count);
        float percent = playerUnits.Count / (float)allUnits.Count;
        playerUnitsProgress.fillAmount = percent;
    }
}
