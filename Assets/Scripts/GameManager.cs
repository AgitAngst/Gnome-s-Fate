using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float minGameSpeed = 1f;
    public float maxGameSpeed = 3f;
    public float currentGameSpeed = 1f;
    public float speedMultiplyer;
    public Character character;

    private void Awake()
    {
        InitializeManager();
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void InitializeManager()
    {
        currentGameSpeed = minGameSpeed;
    }

    void Update()
    {
    }

    public void ChangeSpeed(float speedMultiply)
    {
        if (currentGameSpeed < maxGameSpeed || speedMultiplyer < maxGameSpeed)
        {
            speedMultiplyer += speedMultiply;
            currentGameSpeed += speedMultiply;
            character.characterSpeed += currentGameSpeed;
        }
        else
        {
            currentGameSpeed = maxGameSpeed;
        }
    }
}