using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterEvents : MonoBehaviour
{
    public TextMeshProUGUI textSteps;
    public TextMeshProUGUI textCash;
    private int stepsCount = 0;
    private int cashCount = 0;

    void Start()
    {
        StepUpdate(0);
        CashUpdate(0);
    }

    void Update()
    {
        
    }

    public void Step()
    {
        StepUpdate(1);
    }
    public int StepUpdate(int count)
    {
        stepsCount += count;
        textSteps.text = stepsCount.ToString();
        return count;
    }

    public int CashUpdate(int cash)
    {
        cashCount += cash;
        textCash.text = cashCount.ToString();
        return cash;
    }
}