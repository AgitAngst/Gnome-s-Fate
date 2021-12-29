using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObstacleHp : MonoBehaviour
{
    [FormerlySerializedAs("maxHP")] public int maxHp = 1;

    private int currentHp;
    public float scorePerObject;
    public Character character;
    private GameManager gameManager;

    private void Start()
    {
        currentHp = maxHp;
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
        gameManager = FindObjectOfType<GameManager>();
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

    void Die()
    {
        var score = scorePerObject * gameManager.scoreMultiplyer;
        character.characterEvents.CashUpdate(Mathf.RoundToInt(score));
        Destroy(gameObject);
    }
}