using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector2 acceleration;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float mass;
    [SerializeField] private float magnitude;
    [SerializeField] private float speed;

    [SerializeField] private List<Transform> nearNeighbours;
    [SerializeField] private List<Vector2> externalForces;
    [SerializeField] private float repelRadius = 2.0f;
    [SerializeField] private float repelAmplification = 2.0f;
    [SerializeField] private Vector2 distance;
    [SerializeField] private Vector2 externalForce;

    [SerializeField] private bool isLimitSpeed = false;

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

        CheckDirection();

        RepelNeighbourCheck();
    }

    private void FixedUpdate()
    {
        CheckMove();
    }

    //Check for neigbours entering the repel radius.
    public void RepelNeighbourCheck()
    {
        List<Transform> allNeighbours = SpawnManager.Instance.GetAllNeighbours();
        
        externalForces.Clear();
        nearNeighbours.Clear();

        for (int i = 0; i < allNeighbours.Count; ++i)
        {
            if (allNeighbours[i] == transform)
            {
                continue;
            }
            
            distance = CheckDistance(transform.position, allNeighbours[i].position);

            Movement neighbourMove = allNeighbours[i].GetComponent<Movement>();

            if (neighbourMove.distance.sqrMagnitude <= repelRadius * repelRadius)
            {
                externalForces.Add(distance);

                //Debug to check if circle is near neighbour.
                nearNeighbours.Add(allNeighbours[i]);
            }
        }

        externalForce = Vector2.zero;
        for (int i = 0; i < externalForces.Count; ++i)
        {
            externalForce += externalForces[i] * repelAmplification;
        }
    }

    //Check distance between this and target.
    public Vector2 CheckDistance(Vector2 thisPos, Vector2 targetPos)
    {
        distance = thisPos - targetPos;

        return distance;
    }

    //Reset this position and randomize velocity so it won't bunch up.
    void ResetPosition()
    {
        transform.position = target.position;

        acceleration = Vector2.zero;
        velocity = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
    }

    //Find difference between this and target, then adding force to make it move.
    void CheckDirection()
    {
        direction = GetDirection(transform.position, target.position);

        AddForce2D(magnitude, (direction + externalForce).normalized);
    }

    //Strictly used for changing acceleration and velocity, FixedUpdate()
    void CheckMove()
    {
        velocity += acceleration;

        if (velocity.sqrMagnitude > speed * speed && isLimitSpeed)
        {
            velocity.Normalize();

            velocity *= speed;
        }

        transform.position += new Vector3(velocity.x, velocity.y, 0.0f);
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

        acceleration = (direction * magnitude) / mass;
    }

    public Vector2 GetDistance()
    {
        return distance;
    }
}
