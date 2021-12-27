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
    private float positionSum;
    private float animatorBlendValue;
    private bool isDead = false;

    private float test1;
    private float lerpTimeElapsed;
    public float lerpDuration = 0.5f;

    public UIUpdater uiUpdater;
    void Start()
    {
        _states = States.CharacterState.Idle;
        animator = GetComponentInChildren<Animator>();
        tmpSpeed = characterSpeed;
        _ground = groundObject.GetComponent<Ground>();
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
                break;
            case States.CharacterState.Dead:
                if (!isDead)
                {
                    Death();
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _states = States.CharacterState.Dead;

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

            transform.DOMove(new Vector3(positionSum, 0, 0), 1f);

            // if (lerpTimeElapsed < lerpDuration)
            // {
            //     animator.SetFloat("RunDirection", Mathf.Lerp(0f, -1f, lerpTimeElapsed / lerpDuration));
            //     lerpTimeElapsed += Time.deltaTime;
            // }

            StartCoroutine("AnimatorLerp");

            //animator.SetFloat("RunDirection", -1f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            positionSum += 3f;
            if (positionSum >= 3f)
            {
                positionSum = 3f;
            }
            transform.DOMove(new Vector3(positionSum ,0 , 0) , 1f);
            animator.SetFloat("RunDirection", 1f);

        }
        // if (Input.GetKey(KeyCode.D) && transform.position.x < 3)
        // {
        //     xPosition = transform.position.x;
        //     xPosition = xPosition += 3;
        //     transform.position = new Vector3(xPosition, 0, 0);
        // }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
            Attack();
            uiUpdater.CashUpdate(100);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _states = States.CharacterState.Running;
            uiUpdater.StepTextUpdate(1000);
        }
    }

   private IEnumerator AnimatorLerp()
    {
        test1 = animator.GetFloat("RunDirection");
      //  DOTween.To(() => test1, x => test1 = x, -1f, .5f);
      DOTween.To(() => test1, x => test1 = x, -1f, .5f);
  
      animator.SetFloat("RunDirection",test1);
        Debug.Log("spam");
        yield return null;
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
        characterSpeed = 0f;
        _ground.degreesPerSecond = characterSpeed;
        isDead = true;
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

    void ResetBlend()
    {
        animatorBlendValue = 0.5f;
    }
}
