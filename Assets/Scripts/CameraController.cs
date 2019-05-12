using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 pos;
    public float lerpTime = 0.5f;

    [SerializeField]
    private int xStartMoving = 50;
    [SerializeField]
    private int yStartMoving = 50;
    [SerializeField]
    private float moveSpeed = 2f;

    int screenWidth;
    int screenHeight;
    [HideInInspector]
    public float lockYmin, lockYmax, lockXmin, lockXmax;

    public static bool followingMouse;
    public static bool followUnit;
    private void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void Start()
    {
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.drawMultipleSelectionBox)
        {
            CheckFollowingMouse();

           if (followingMouse && !followUnit)
                FollowMouseOnEdge();
        }
    }

    void FollowUnit()
    {
        if (GameManager.Instance.selectedUnits[0] != null)
        {
            Vector3 desiredPosition = GameManager.Instance.selectedUnits[0].transform.position;
            desiredPosition = new Vector3(desiredPosition.x, desiredPosition.y, transform.position.z);
            Vector3 position = Vector3.Lerp(transform.position, desiredPosition, lerpTime * Time.deltaTime);
            transform.position = position;
        }
    }

    void FollowMouseOnEdge()
    {
        if (Input.mousePosition.x > screenWidth - xStartMoving && Input.mousePosition.x <= screenWidth && !LockXMaxMovement())
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime,0,0);
        }
        if (Input.mousePosition.x < 0 + xStartMoving && Input.mousePosition.x >= 0 && !LockXMinMovement())
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.y > screenHeight - yStartMoving && Input.mousePosition.y <= screenHeight && !LockYMaxMovement())
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
        if (Input.mousePosition.y < 0 + yStartMoving && Input.mousePosition.y >= 0 && !LockYMinMovement())
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }

    void CheckFollowingMouse()
    {
        if ((Input.mousePosition.x > Screen.width - xStartMoving && Input.mousePosition.x <= Screen.width) ||
            (Input.mousePosition.x < 0 + xStartMoving && Input.mousePosition.x >= 0) ||
            (Input.mousePosition.y > Screen.height - yStartMoving && Input.mousePosition.y <= Screen.height) ||
            (Input.mousePosition.y < 0 + yStartMoving && Input.mousePosition.y >= 0))
        {
            followingMouse = true;
            followUnit = false;
        }
        else 
        {
            followingMouse = false;
        }

    }

    bool LockXMinMovement()
    {
        if (transform.position.x <= lockXmin)
            return true;
        else
            return false;
    }

    bool LockXMaxMovement()
    {
        if (transform.position.x >= lockXmax)
            return true;
        else
            return false;
    }

    bool LockYMinMovement()
    {
        if (transform.position.y <= lockYmin)
            return true;
        else
            return false;
    }

    bool LockYMaxMovement()
    {
        if (transform.position.y >= lockYmax)
            return true;
        else
            return false;
    }
}
