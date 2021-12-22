using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class Character : MonoBehaviour
{
    private float xPosition;
    public float waitBeforeChangeSide = 0.5f;
    public float characterSpeed = 20f;
    private bool keyPressed = false;
    private Animator animator;
    private States state;
    public States.CharacterState _states;
    private Ground _ground;
    public GameObject groundObject;
    private float tmpSpeed;
    public float keyPressTimer = 0.5f;
    private ObjectsSpawner _objectsSpawner;
    public GameObject SpawnPoints;
    private float positionSum;
    void Start()
    {
        _states = States.CharacterState.Idle;
        animator = GetComponentInChildren<Animator>();
        tmpSpeed = characterSpeed;
        _ground = groundObject.GetComponent<Ground>();
        _objectsSpawner = SpawnPoints.GetComponent<ObjectsSpawner>();
        positionSum = 0f;
    }

    void Update()
    {
        CharacterControl();
        switch (_states)
        {
            case States.CharacterState.Idle:
                Idle();
                _ground.degreesPerSecond = 0f;
                break;
            case States.CharacterState.Running:
                Run();
                _objectsSpawner.SpawnObjects();
                break;
            case States.CharacterState.Dead:
                Death();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    private void CharacterControl()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            positionSum -= 3f;
            if (positionSum <= -3f)
            {
                positionSum = -3f;
            }
            transform.DOMove(new Vector3(positionSum ,0 , 0) , 1f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            positionSum += 3f;
            if (positionSum >= 3f)
            {
                positionSum = 3f;
            }
            transform.DOMove(new Vector3(positionSum ,0 , 0) , 1f);
        }
        // if (Input.GetKey(KeyCode.D) && transform.position.x < 3)
        // {
        //     xPosition = transform.position.x;
        //     xPosition = xPosition += 3;
        //     transform.position = new Vector3(xPosition, 0, 0);
        // }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
            Attack();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _states = States.CharacterState.Running;
        }
    }
    

    public void Attack()
    {
        
    }

    public void Idle()
    {
        characterSpeed = 0f;
        _ground.degreesPerSecond = characterSpeed;

    }
    public void Death()
    {
        animator.SetTrigger("Die");
    }

    public void Run()
    {
        animator.SetBool("isRunning",true);
        characterSpeed = tmpSpeed;

        _ground.degreesPerSecond = characterSpeed;
    }

    void KeyPressTimer()
    {
        if (keyPressed)
        {
            keyPressTimer -= Time.deltaTime;
            if (keyPressTimer <= 0f)
            {
                
                keyPressed = false;
            } 
        }
       
        
    }
}
