using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{
    public bool isShakeCamera = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isShakeCamera)
            {
                ShakeCameraLight();
            }
        }
    }

    public void ShakeCameraLight()
    {
        Camera.main.DOShakePosition(1f, .25f, 20, 70f, true);
    }

    public void ShakeCameraHard()
    {
        Camera.main.DOShakePosition(.5f, .25f, 10, 90f, true);
    }

    public void ObstacleReaction(Transform obstacle)
    {
        obstacle.DOLocalJump(new Vector3(obstacle.transform.position.x, 2f, transform.position.z), 2f, 3, 2, false);
    }
}