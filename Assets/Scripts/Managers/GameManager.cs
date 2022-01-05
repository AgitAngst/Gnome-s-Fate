using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public bool isGameWon = false;
    public static GameManager instance = null;
    public float minGameSpeed = 1f;
    public float maxGameSpeed = 3f;
    public float currentGameSpeed = 1f;
    [FormerlySerializedAs("speedAndScoreMultiplyer")] [FormerlySerializedAs("speedMultiplyer")] public float scoreMultiplyer;
    public Character character;
    public KeyCode restartKey = KeyCode.R;
    public KeyCode strafeLeftKey = KeyCode.A;
    public KeyCode strafeRightKey = KeyCode.D;
    public KeyCode startRunKey = KeyCode.Space;
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode cameraChangeKey = KeyCode.V;

    public Camera cameraBack;
    public Camera cameraFront;

    [HideInInspector]public bool isCameraChanged = false;
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


    }
    
    public void ChangeCamera()
    {
        if (Input.GetKeyDown(cameraChangeKey))
        {
            isCameraChanged =! isCameraChanged;
            if (!isCameraChanged)
            {
                cameraBack.enabled = true;
                cameraFront.enabled = false;
            }
            else
            {
                cameraBack.enabled = false;
                cameraFront.enabled = true;
            }
        }
    }
    private void InitializeManager()
    {
        currentGameSpeed = minGameSpeed;
    }

    void Update()
    {
        if (character.isDead && Input.GetKeyDown(restartKey))
        {
#pragma warning disable CS0618
            Application.LoadLevel(Application.loadedLevel);
#pragma warning restore CS0618
        }
        GameWin();
        ChangeCamera();
    }

    public void ChangeSpeed(float speedAdd)
    {
        if (currentGameSpeed < maxGameSpeed || scoreMultiplyer < maxGameSpeed)
        {
            scoreMultiplyer += speedAdd;
            currentGameSpeed += speedAdd;
            character.characterSpeed += currentGameSpeed;
        }
        else
        {
            currentGameSpeed = maxGameSpeed;
        }
    }
    
    public void GameWin()
    {
        if (ScoreManager.instance.GetScore() >= ScoreManager.instance.scoreToWin)
        {
            isGameWon = true;
        }
    }
}