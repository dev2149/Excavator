using UnityEngine;
using System.Collections;

public class UIAxisRange : MonoBehaviour 
{
	[HideInInspector]
	public Diging.AXIS axis;
	public float targetAngle;
	public Transform angle;
	public Transform target;

	private Vector3 v1 = Vector3.zero;

	void Update () 
	{
		if (null == axis)
			return;

		float range = axis.max - axis.min;
		float value = axis.angle - axis.min;
		SetPosition (angle, value * (1.0f / range));

		value = targetAngle - axis.min;
		SetPosition (target, value * (1.0f / range));
	}

	void SetPosition(Transform t, float value)
	{
		v1.x = (340.0f * value) - 170.0f;
        v1.y = -12.0f;
		t.localPosition = v1;
	}
}
