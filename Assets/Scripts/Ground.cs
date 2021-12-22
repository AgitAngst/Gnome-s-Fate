using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour

{
    [HideInInspector]public float degreesPerSecond;
    private void Start()
    {

    }

    void Update()
    {
        transform.Rotate(new Vector3(degreesPerSecond,0 , 0) * Time.deltaTime);
    }
}
