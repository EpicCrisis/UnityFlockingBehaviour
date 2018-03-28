using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private List<Transform> nearNeighbours;
    [SerializeField] private List<Vector2> externalForces;
    [SerializeField] private float repelRadius = 2.0f;
    [SerializeField] private float repelAmplification = 2.0f;
    [SerializeField] private Vector2 distance;
    [SerializeField] private Vector2 externalForce;

    void Start()
    {

    }

    void Update()
    {
        RepelNeighbourCheck();
    }

    private void FixedUpdate()
    {

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

            if (neighbourMove.GetDistance().sqrMagnitude <= repelRadius * repelRadius)
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
}
