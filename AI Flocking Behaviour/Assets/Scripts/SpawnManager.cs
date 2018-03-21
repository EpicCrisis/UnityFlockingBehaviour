using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> allNeighbours;
    [SerializeField] private int amount = 10;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Transform target;

    public static SpawnManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < amount; ++i)
        {
            Vector3 randomPos = new Vector3(Random.Range(0.0f, Screen.width), Random.Range(0.0f, Screen.height), 0.0f);
            randomPos = Camera.main.ScreenToWorldPoint(randomPos);
            randomPos.z = 0.0f;

            GameObject GO = Instantiate(circlePrefab, randomPos, Quaternion.identity, transform);

            allNeighbours.Add(GO.GetComponent<Transform>());
        }
    }

    public List<Transform> GetAllNeighbours()
    {
        return allNeighbours;
    }

    public Transform GetTarget()
    {
        return target;
    }
}
