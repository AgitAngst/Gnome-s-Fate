using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    private Animator animator;
    public int scoreToReact;
    public string animatorBoolName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameWon)
        {
            
        }

        if (ScoreManager.instanceScore.GetScore() >= scoreToReact)
        {
            animator = GetComponent<Animator>();
            animator.SetBool(animatorBoolName, true);
        }
    }
}
