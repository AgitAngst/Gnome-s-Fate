using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsSpawner : MonoBehaviour
{
    public GameObject[] objects;
    public bool enableEpicObjects = false;

    [Range(0, 1)] public float epicObjectsSpawnRate = 0.1f;
    private float epicSpawnChance;
    public GameObject[] epicObjects;
    public GameObject[] spawnPoints;
    private float timeLeft;
    public Vector2 timeToSpawnObjects;
    public bool randomRotation;
    public Vector2 rotationDegrees;

    public GameObject parentGround;
    private Character states;
    public bool randomMaterials = false;
    public Material[] materials;
    private GameObject spawn;
    private GameObject epicSpawn;
    [HideInInspector] public ObstacleHp obstacleHp;
    private bool isCanSpawn = true;

    void Start()
    {
        states = FindObjectOfType<Character>();
        isCanSpawn = states != (states.characterStates == States.CharacterState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch (states.characterStates)
        {
            case States.CharacterState.Idle:
                isCanSpawn = false;
                break;
            case States.CharacterState.Running:
                isCanSpawn = true;
                break;
            case States.CharacterState.Dead:
                isCanSpawn = false;
                break;
        }

        if (isCanSpawn)
        {
            SpawnObjects();
        }
    }

    void Spawn()
    {
        epicSpawnChance = Random.Range(0f, 1f);
        var spawnPoint = Random.Range(0, spawnPoints.Length);
        if (randomRotation)
        {
            spawn = Instantiate(objects[Random.Range(0, objects.Length)], spawnPoints[spawnPoint].transform.position,
                Quaternion.Euler(90, 0, Random.Range(rotationDegrees.x, rotationDegrees.y)));
            if (enableEpicObjects && epicSpawnChance <= epicObjectsSpawnRate)
            {
                var position = new Vector3(spawnPoints[spawnPoint].transform.position.x,
                    spawnPoints[spawnPoint].transform.position.y, spawnPoints[spawnPoint].transform.position.z - 1);
                epicSpawn = Instantiate(epicObjects[Random.Range(0, epicObjects.Length)],
                    position,
                Quaternion.Euler(180, 180, Random.Range(rotationDegrees.x, rotationDegrees.y)));
            }
        }
        else
        {
            spawn = Instantiate(objects[Random.Range(0, objects.Length)], spawnPoints[spawnPoint].transform.position,
                Quaternion.Euler(90, 0, 0));

            if (enableEpicObjects && epicSpawnChance <= epicObjectsSpawnRate)
            {
                var position = new Vector3(spawnPoints[spawnPoint].transform.position.x,
                    spawnPoints[spawnPoint].transform.position.y, spawnPoints[spawnPoint].transform.position.z - 1);
                epicSpawn = Instantiate(epicObjects[Random.Range(0, epicObjects.Length)],
                    position,
                    Quaternion.Euler(180, 180, 0));
            }
        }

        spawn.transform.parent = parentGround.transform;

        if (enableEpicObjects && epicSpawnChance <= epicObjectsSpawnRate)
        {
            epicSpawn.transform.parent = parentGround.transform;
        }

        if (randomMaterials)
        {
            spawn.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }

    void SpawnObjects()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Spawn();
            timeLeft = Random.Range(timeToSpawnObjects.x, timeToSpawnObjects.y);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}