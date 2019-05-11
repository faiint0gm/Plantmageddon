using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    CameraController cameraController;

   
    public List<Unit> allUnits = new List<Unit>();
    
    public List<PlayerUnit>playerUnits = new List<PlayerUnit>();
    
    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
   
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

    private Vector3 selectionStartPosition;
    private Vector3 selectionStartWorldPosition;
    private Vector3 selectionEndPosition;
    private Vector3 selectionEndWorldPosition;
    [HideInInspector]
    public bool drawMultipleSelectionBox;
    [HideInInspector]
    public bool lockMovement;
    [SerializeField]
    Texture selectionTexture;
    
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
        HandleMultipleUnitSelection();
        HandleLists();
    }
    private void HandleLists()
    {
        playerUnits = playerUnits.Where(item => item != null).ToList();
        selectedUnits = selectedUnits.Where(item => item != null).ToList();
        allUnits = allUnits.Where(item => item != null).ToList();
        enemyUnits = enemyUnits.Where(item => item != null).ToList();
    }
    private void Init()
    {
        cameraController = mainCamera.GetComponent<CameraController>();
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

    void HandleMultipleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectionStartPosition = Vector3.zero;
            selectionStartWorldPosition = Vector3.zero;
            selectionEndPosition = Vector3.zero;
            selectionEndWorldPosition = Vector3.zero;

            selectionStartPosition = Input.mousePosition;
            selectionStartWorldPosition = mainCamera.ScreenToWorldPoint(selectionStartPosition);
        }

        if(Input.GetMouseButton(0))
        {
            drawMultipleSelectionBox = true;
            selectionEndPosition = Input.mousePosition;
            selectionEndWorldPosition = mainCamera.ScreenToWorldPoint(selectionEndPosition);
        }

        if(Input.GetMouseButtonUp(0))
        {
            drawMultipleSelectionBox = false;
            SelectMultiple(selectionStartWorldPosition,selectionEndWorldPosition);
            selectionStartPosition = Vector3.zero;
            selectionStartWorldPosition = Vector3.zero;
            selectionEndPosition = Vector3.zero;
            selectionEndWorldPosition = Vector3.zero;
        }

    }

    void SelectMultiple(Vector3 startPos, Vector3 endPos)
    {
        List<PlayerUnit> players = new List<PlayerUnit>();

        float xLow, yLow, xHigh, yHigh;
        if(startPos.x <endPos.x)
        {
            xLow = startPos.x;
            xHigh = endPos.x;
        }
        else
        {
            xLow = endPos.x;
            xHigh = startPos.x;
        }

        if (startPos.y<endPos.y)
        {
            yLow = startPos.y;
            yHigh = endPos.y;
        }else
        {
            yLow = endPos.y;
            yHigh = startPos.y;
        }

        
        foreach (PlayerUnit player in playerUnits)
        {
            Vector3 pos = player.transform.position;
            if(pos.x>xLow && pos.x < xHigh && pos.y > yLow && pos.y < yHigh)
            {
                players.Add(player);
            }
        }
        if (players.Count != 0)
        {
            selectedUnits.Clear();
            foreach(PlayerUnit p in players)
            {
                selectedUnits.Add(p);
            }
        }
    }

    private void OnGUI()
    {
            if (drawMultipleSelectionBox)
            {
                Vector3 startScreenPos = selectionStartPosition;
                Vector3 endScreenPos = Input.mousePosition;
                float width, height;
                if (startScreenPos.x > endScreenPos.x)
                {
                    width = startScreenPos.x - endScreenPos.x;
                }
                else
                {
                    width = endScreenPos.x - startScreenPos.x;
                }
                
                if (startScreenPos.y > endScreenPos.y)
                {
                    height = startScreenPos.y - endScreenPos.y;
                }
                else
                {
                    height = endScreenPos.y - startScreenPos.y;
                }
                if (width > 0 || height > 0)
                {
                lockMovement = true;
                }
                else
                {
                lockMovement = false;
                }
                Rect posToDrawBox;

                if (endScreenPos.x > startScreenPos.x)
                {
                    if (endScreenPos.y > startScreenPos.y)
                    {
                        posToDrawBox = new Rect(startScreenPos.x, Screen.height - endScreenPos.y, width, height);
                    }
                    else
                    {
                        posToDrawBox = new Rect(startScreenPos.x, Screen.height - startScreenPos.y, width, height);
                    }
                }
                else
                {
                    if (endScreenPos.y > startScreenPos.y)
                    {
                        posToDrawBox = new Rect(endScreenPos.x, Screen.height - endScreenPos.y, width, height);
                    }
                    else
                    {
                        posToDrawBox = new Rect(endScreenPos.x, Screen.height - startScreenPos.y, width, height);
                    }
                }

                GUI.DrawTexture(posToDrawBox, selectionTexture);
            }

        }
}
