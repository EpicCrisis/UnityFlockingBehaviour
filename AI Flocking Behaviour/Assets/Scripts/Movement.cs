using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector2 acceleration;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float mass;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float fixedAcceleration = 0.1f;
    [SerializeField] private float minScale = 0.0001f;

    [SerializeField] private List<Transform> nearNeighbours;
    [SerializeField] private List<Vector2> externalForces;

    [SerializeField] private float circleRepelRadius = 0.5f;
    [SerializeField] private float circleRepelAmplification = 5.0f;
    [SerializeField] private float obstacleRepelRadius = 0.5f;
    [SerializeField] private float obstacleRepelAmplification = 5.0f;

    [SerializeField] private Vector2 distance;
    [SerializeField] private Vector2 externalForce;

    [SerializeField] private bool isLimitSpeed = false;
    [SerializeField] private bool isUseFixedAcceleration = false;
    [SerializeField] private bool isUseSpecialEffect = false;
    [SerializeField] private bool isIgnoreMass = false;
    [SerializeField] private bool isCheckObstacle = false;
    [SerializeField] private bool isCheckNeighbor = false;

    void Start()
    {
        target = SpawnManager.Instance.GetTarget();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            ResetPosition();
        }

        if (Input.GetButton("Fire2"))
        {
            ResetMovement();
        }

        RepelCheck();

        CheckDirection();
    }

    private void FixedUpdate()
    {
        CheckMove();
    }

    //Check for neigbours entering the repel radius.
    public void RepelCheck()
    {
        List<Transform> allNeighbours = SpawnManager.Instance.GetAllNeighbours();
        List<Transform> allObstacles = SpawnManager.Instance.GetAllObstacles();
        
        externalForce = Vector2.zero;

        externalForces.Clear();
        nearNeighbours.Clear();

        if (isCheckNeighbor)
        {
            for (int i = 0; i < allNeighbours.Count; ++i)
            {
                if (allNeighbours[i] == transform)
                {
                    continue;
                }

                distance = CheckDistance(transform.position, allNeighbours[i].position);

                float totalRepelRadius = circleRepelRadius + (transform.localScale.x / 2.0f) + (allNeighbours[i].localScale.x / 2.0f);

                if (distance.sqrMagnitude <= totalRepelRadius * totalRepelRadius)
                {
                    externalForces.Add(distance * circleRepelAmplification);

                    //Debug to check if circle is near neighbour.
                    nearNeighbours.Add(allNeighbours[i]);
                }
            }
        }

        if (isCheckObstacle)
        {
            for (int i = 0; i < allObstacles.Count; ++i)
            {
                distance = CheckDistance(transform.position, allObstacles[i].position);

                float totalRepelRadius = circleRepelRadius + (transform.localScale.x / 2.0f) + (allObstacles[i].localScale.x / 2.0f);

                if (distance.sqrMagnitude <= totalRepelRadius * totalRepelRadius)
                {
                    externalForces.Add(distance * obstacleRepelAmplification);

                    //Debug to check if circle is near neighbour.
                    nearNeighbours.Add(allNeighbours[i]);
                }
            }
        }

        for (int i = 0; i < externalForces.Count; ++i)
        {
            externalForce += externalForces[i];
        }
    }

    //Find difference between this and target, then adding force to make it move.
    void CheckDirection()
    {
        Vector2 direction = GetDirection(transform.position, target.position);

        float totalMagnitude = (direction + externalForce).magnitude;

        if (isUseFixedAcceleration)
        {
            totalMagnitude = fixedAcceleration;
        }

        Vector2 totalDirection = direction.normalized + externalForce;

        AddForce2D(totalMagnitude, totalDirection.normalized);

        if (isUseSpecialEffect)
        {
            transform.localScale = new Vector3(acceleration.sqrMagnitude + minScale, acceleration.sqrMagnitude + minScale, 1.0f) * mass;
        }
    }

    //Strictly used for changing acceleration and velocity, FixedUpdate()
    void CheckMove()
    {
        velocity += acceleration;

        if (isLimitSpeed)
        {
            if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                velocity.Normalize();

                velocity *= maxSpeed;
            }
        }

        transform.position += new Vector3(velocity.x, velocity.y, 0.0f);
    }

    //Check distance between this and target.
    public Vector2 CheckDistance(Vector2 thisPos, Vector2 targetPos)
    {
        distance = thisPos - targetPos;

        return distance;
    }

    //Checking the direction variable by comparing transform between this and target.
    public Vector2 GetDirection(Vector2 thisPos, Vector2 targetPos)
    {
        Vector2 direction = targetPos - thisPos;

        return direction;
    }

    //Using force equals to mass times acceleration as formula.
    public void AddForce2D(float magnitude, Vector2 direction)
    {
        //Normalize to remove external variables and improve reliability.
        direction.Normalize();

        acceleration = (direction * magnitude);

        if (!isIgnoreMass)
        {
            acceleration /= mass;
        }
    }

    //Reset this position and randomize velocity so it won't bunch up.
    void ResetPosition()
    {
        transform.position = target.position;
    }

    void ResetMovement()
    {
        acceleration = Vector2.zero;
        velocity = Vector2.zero;
    }

    public Vector2 GetDistance()
    {
        return distance;
    }
}
