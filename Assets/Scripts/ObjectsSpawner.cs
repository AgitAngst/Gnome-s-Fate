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
    public GameObject parentGround;
    private States.CharacterState _states;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    void Spawn()
    {
        var spawnPoint = Random.Range(0, spawnPoints.Length);
        var spawn = Instantiate(objects[Random.Range(0, objects.Length)],spawnPoints[spawnPoint].transform.position ,
            Quaternion.Euler(90, 0, 0));
        spawn.transform.parent = parentGround.transform;
    }

   public void SpawnObjects()
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
