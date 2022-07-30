using UnityEngine;
using System.Collections;

public class WheelSteer : MonoBehaviour
{
    public WheelCollider wheel;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0.0f, wheel.steerAngle, 0.0f);
    }
}
