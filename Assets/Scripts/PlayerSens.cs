using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSens : MonoBehaviour
{
    public Transform cam;
    private Vector2 turn;
    private float valConst = 0.14f;

    private void Awake()
    {
        GameManager.instance.playerRef = gameObject;
    }
    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.visible = !Cursor.visible;
        turn.y -= Input.GetAxis("Mouse Y") * GameManager.instance.playerSens * valConst;
        turn.y = Mathf.Clamp(turn.y, -90, 90);
        cam.localEulerAngles = new Vector3(turn.y, 0, 0);
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * GameManager.instance.playerSens * valConst);
    }
}
