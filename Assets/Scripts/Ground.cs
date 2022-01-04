using UnityEngine;

public class Ground : MonoBehaviour

{
    [HideInInspector] public float degreesPerSecond;
    void Update()
    {
        transform.Rotate(new Vector3(degreesPerSecond, 0, 0) * Time.deltaTime);
    }

    public void SetGroundSpeed(float speed)
    {
        degreesPerSecond = speed;
    }
}