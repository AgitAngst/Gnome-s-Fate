using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    private float xPosition;
    public float waitBeforeChangeSide = 0.5f;
    public float characterSpeed = 20f;
    private bool keyPressed = false;
    private Animator animator;
    private States state;
    [FormerlySerializedAs("_states")] public States.CharacterState states;
    private Ground ground;
    public GameObject groundObject;
    private float tmpSpeed;
    public float keyPressTimer = 0.5f;
    private ObjectsSpawner objectsSpawner;
    private float positionSum;
    private float animatorBlendValue;
    private bool isDead = false;

    private float test1;
    private float lerpTimeElapsed;
    public float lerpDuration = 0.5f;
    private static readonly int RunDirection = Animator.StringToHash("RunDirection");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private TweenerCore<float, float, FloatOptions> tweener;


    public CharacterEvents characterEvents;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask layerMask;

    public int attackDamage;
    [FormerlySerializedAs("maxHP")] public int maxHp;
    [FormerlySerializedAs("playerHP")] public TextMeshProUGUI playerHp;
    private int currentHp;
    private float blendDirectionValue;
    private static readonly int Hurt = Animator.StringToHash("Hurt");

    void Start()
    {
        states = States.CharacterState.Idle;
        animator = GetComponentInChildren<Animator>();
        tmpSpeed = characterSpeed;
        ground = groundObject.GetComponent<Ground>();
        positionSum = 0f;
        currentHp = maxHp;
        playerHp.text = currentHp.ToString();
    }

    void Update()
    {
        CharacterControl();
        switch (states)
        {
            case States.CharacterState.Idle:
                Idle();
                ground.degreesPerSecond = 0f;
                break;
            case States.CharacterState.Running:
                Run();
                break;
            case States.CharacterState.Dead:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage(1);
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
            AnimatorLerp(-1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            positionSum += 3f;
            if (positionSum >= 3f)
            {
                positionSum = 3f;
            }

            transform.DOMove(new Vector3(positionSum, 0, 0), 1f);
            AnimatorLerp(1);
        }

        // if (Input.GetKey(KeyCode.D) && transform.position.x < 3)
        // {
        //     xPosition = transform.position.x;
        //     xPosition = xPosition += 3;
        //     transform.position = new Vector3(xPosition, 0, 0);
        // }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger(Attack1);
            //attack is an event in attack animation
        }

        if (Input.GetKey(KeyCode.Space))
        {
            states = States.CharacterState.Running;
            characterSpeed = tmpSpeed;
        }
    }

    private void AnimatorLerp(float direction, float duration = 0.5f)
    {
        tweener?.Kill();
        blendDirectionValue = animator.GetFloat(RunDirection);
        tweener = DOTween.To(() => blendDirectionValue, x => blendDirectionValue = x, direction, duration)
            .OnUpdate(() => { animator.SetFloat(RunDirection, blendDirectionValue); })
            .OnComplete(() =>
            {
                if (direction != 0) AnimatorLerp(0);
            });
    }

    public void Attack()
    {
        Collider[] hitenemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, layerMask);
        foreach (Collider enemy in hitenemies)
        {
            Debug.Log("enemy" + enemy.name);
            enemy.GetComponent<ObstacleHp>().Damage(attackDamage);
        }
    }

    public void Idle()
    {
        characterSpeed = 0f;
        ground.degreesPerSecond = characterSpeed;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        playerHp.text = currentHp.ToString();
        if (currentHp >= 1)
        {
            animator.SetTrigger(Hurt);
        }
        else
        {
            states = States.CharacterState.Dead;
            Death();
        }
    }

    public void Death()
    {
        characterSpeed = 0f;
        ground.degreesPerSecond = characterSpeed;
        isDead = true;
        animator.SetTrigger(Die);
    }

    public void Run()
    {
        animator.SetBool(IsRunning, true);
        //characterSpeed = tmpSpeed;
        ground.degreesPerSecond = characterSpeed;
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}