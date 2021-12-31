using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleHp : MonoBehaviour
{
    [FormerlySerializedAs("maxHP")] public int maxHp = 1;
    public bool getDistance = false;
    private int currentHp;
    public float scorePerObject;
    private Character character;
    private GameManager gameManager;
    private Animator animator;
    private float distance;
    private AudioManager audioManager;

    private void Update()
    {
        GetDistanceToPlayer();
    }

    private void Start()
    {
        currentHp = maxHp;
        audioManager = FindObjectOfType<AudioManager>();
        character = FindObjectOfType<Character>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameObject.GetComponent<Animator>())
        {
            animator = gameObject.GetComponent<Animator>();
        }
    }

    public void Damage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            gameManager.ChangeSpeed(0.12f);
            Die();
        }
    }

    private void Die()
    {
        var score = scorePerObject * gameManager.scoreMultiplyer;
        character.characterEvents.CashUpdate(Mathf.RoundToInt(score));
        Destroy(gameObject);
        audioManager.PlaySound(6);
    }

    private void GetDistanceToPlayer()
    {
        if (getDistance)
        {
            distance = Vector3.Distance(character.transform.position, gameObject.transform.position);

            if (distance <= 3f)
            {
                animator.SetTrigger("Attack");
            }
        }
       
    }
}