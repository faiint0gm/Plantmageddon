using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class PlayerUnit : Unit,IPointerClickHandler
{
    [SerializeField]
    protected int killHP;
    protected AIDestinationSetter destinationSetter;

    bool moving;
    Vector3 target;

    public void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        if (destinationSetter != null)
        {
            destinationSetter.playerTarget = transform.position;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.selectedUnits.Add(this);
        Debug.Log("PlayerUnit : "+unitType.ToString()+ " Clicked");
    }

    public void MoveToSelectedTarget()
    {
        target = GameManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, target.y, transform.position.z);
        destinationSetter.TargetPositionSet(target);
    }

    void Update()
    {

    }

}
