using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textSteps;
    [SerializeField] private TextMeshProUGUI textCash;
    [HideInInspector]public static ScoreManager instanceScore = null;
    public float scoreToWin = 2500;
    [SerializeField] private int scoreCount = 0;
    [SerializeField] private int stepsCount = 0;
    private Character character;

    private void Update()
    {
    }

    private void Start()
    {
        SetScore(0);
        SetStep(0);
        character = GetComponentInParent<Character>();
        if (instanceScore == null)
        {
            instanceScore = this;
        }
        else if (instanceScore == this)
        {
            Destroy(gameObject);
        }
    }

    public void SetScore(int score)
    {
        scoreCount += score;
        textCash.text = scoreCount.ToString();
    }

    public int GetScore()
    {
        return scoreCount;
    }

    public int SetStep(int count)
    {
        stepsCount += count;
        textSteps.text = stepsCount.ToString();
        return count;
    }
}