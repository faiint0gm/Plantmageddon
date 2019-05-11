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

    public static bool followingMouse;

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
        CheckFollowingMouse();
        if (GameManager.Instance.selectedUnits.Count > 0 && !followingMouse)
            FollowUnit();
        else
            FollowMouseOnEdge();
    }

    void FollowUnit()
    {
        Vector3 desiredPosition = GameManager.Instance.selectedUnits[0].transform.position
            + GameManager.Instance.cameraToUnitOffset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, lerpTime * Time.deltaTime);
        transform.position = position;
    }

    void FollowMouseOnEdge()
    {
        if (Input.mousePosition.x > screenWidth - xStartMoving && Input.mousePosition.x <= screenWidth)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime,0,0);
        }
        if (Input.mousePosition.x < 0 + xStartMoving && Input.mousePosition.x >= 0)
        {
            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.mousePosition.y > screenHeight - yStartMoving && Input.mousePosition.y <= screenHeight)
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
        if (Input.mousePosition.y < 0 + yStartMoving && Input.mousePosition.y >= 0)
        {
            transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }

    void CheckFollowingMouse()
    {
        if ((Input.mousePosition.x > screenWidth - xStartMoving && Input.mousePosition.x <= screenWidth) ||
            (Input.mousePosition.x < 0 + xStartMoving && Input.mousePosition.x >= 0) ||
            (Input.mousePosition.y > screenHeight - yStartMoving && Input.mousePosition.y <= screenHeight) ||
            (Input.mousePosition.y < 0 + yStartMoving && Input.mousePosition.y >= 0))
        {
            followingMouse = true;
        }
        else followingMouse = false;

    }
}
