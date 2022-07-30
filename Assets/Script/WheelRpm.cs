using UnityEngine;
using System.Collections;

public class WheelRpm : MonoBehaviour
{
    public WheelCollider wheel;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(wheel.rpm * 6 * Time.deltaTime, 0.0f, 0.0f);
    }
}
