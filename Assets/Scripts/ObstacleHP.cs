using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHP : MonoBehaviour
{
    public int maxHP = 1;

    private int currentHP ;
    public Character character;


    private void Start()
    {
        currentHP = maxHP;
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    public void Damage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        character.characterEvents.CashUpdate(10);
        Destroy(gameObject);
    }
}
