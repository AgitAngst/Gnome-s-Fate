using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectsSpawner : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject[] spawnPoints;
    private float timeLeft;
    public Vector2 timeToSpawnObjects;
    public bool randomRotation;
    public Vector2 rotationDegrees;

    public GameObject parentGround;
    private States.CharacterState states;
    public bool randomMaterials = false;
    public Material[] materials;
    private GameObject spawn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (states)
        {
            case States.CharacterState.Idle:
                break;
            case States.CharacterState.Running:
                Debug.Log("work");
                SpawnObjects();
                break;
            case States.CharacterState.Dead:
                break;
        }
        SpawnObjects();
    }

    void Spawn()
    {
        var spawnPoint = Random.Range(0, spawnPoints.Length);
        if (randomRotation)
        {
            spawn = Instantiate(objects[Random.Range(0, objects.Length)],spawnPoints[spawnPoint].transform.position ,
                Quaternion.Euler(90, 0, Random.Range(rotationDegrees.x,rotationDegrees.y)));
        }
        else
        {
            spawn = Instantiate(objects[Random.Range(0, objects.Length)],spawnPoints[spawnPoint].transform.position ,
                Quaternion.Euler(90, 0, 0));
        }
        
        spawn.transform.parent = parentGround.transform;
        
        if (randomMaterials)
        {
            spawn.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }

   void SpawnObjects()
    {
        timeLeft -= Time.deltaTime;
        if ( timeLeft <= 0 )
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
