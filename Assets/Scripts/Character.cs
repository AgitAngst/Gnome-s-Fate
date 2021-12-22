using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float xPosition;
    public float waitBeforeChangeSide = 0.5f;
    private bool keyPressed = false;
    private Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) && transform.position.x > -3)
        {
            xPosition = transform.position.x;
            xPosition = xPosition -= 3;
            transform.position = new Vector3(xPosition, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < 3)
        {
            xPosition = transform.position.x;
            xPosition = xPosition += 3;
            transform.position = new Vector3(xPosition, 0, 0);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetTrigger("Attack");
        }
    }

    void TimerStart()
    {
        
    }
}
