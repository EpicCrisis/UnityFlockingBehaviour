using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float accelerationX = 1.0f;
    [SerializeField] private float accelerationY = 1.0f;
    [SerializeField] private float velocityX;
    [SerializeField] private float velocityY;
    [SerializeField] private float mass;

    void Start()
    {
        accelerationX /= mass;
        accelerationY /= mass;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += new Vector3(velocityX, velocityY, 0.0f);

        if (transform.position.x < target.position.x)
        {
            velocityX += accelerationX;
        }
        else if (transform.position.x > target.position.x)
        {
            velocityX -= accelerationX;
        }

        if (transform.position.y < target.position.y)
        {
            velocityY += accelerationY;
        }
        else if (transform.position.y > target.position.y)
        {
            velocityY -= accelerationY;
        }
    }
}
