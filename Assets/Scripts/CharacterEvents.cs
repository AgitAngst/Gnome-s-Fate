using UnityEngine;

public class CharacterEvents : MonoBehaviour
{
    private Character character;
    private AudioManager audioManager;
    
    void Start()
    {
        character = GetComponentInParent<Character>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
    }
    public void Step()
    {
        StepCount();
        audioManager.PlaySound(4);
    }

    public void Damage()
    {
        character.Attack();
    }

    void StepCount()
    {
        ScoreManager.instance.SetStep(1);
    }

}