using TMPro;
using UnityEngine;

public class CharacterEvents : MonoBehaviour
{
    public TextMeshProUGUI textSteps;
    public TextMeshProUGUI textCash;
    private int stepsCount = 0;
    private int cashCount = 0;
    private Character character;
    private AudioManager audioManager;

    void Start()
    {
        StepUpdate(0);
        CashUpdate(0);
        character = GetComponentInParent<Character>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
    }

    public void Step()
    {
        StepUpdate(1);
        audioManager.PlaySound(4);
    }

    public void Damage()
    {
        character.Attack();
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