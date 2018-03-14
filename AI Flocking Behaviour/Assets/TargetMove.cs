using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    Vector3 mousePosition;

    void Start()
    {

    }

    void Update()
    {
        Crosshair();
    }

    void Crosshair()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0.0f);
    }
}
