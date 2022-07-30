using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDriveHandle : MonoBehaviour 
{
	public Driving driving;
	public DigingWarning dieselsteam;

	private bool play = false;
	private bool mouse = false;
	private int touch = -1;
	private Vector3 last;
	private float angle = 0.0f;

	private float Maxangle = 360.0f;
	private float Limitangle = -360.0f;
	private bool over;
	
	// Use this for initialization
	void Start () 
	{                                                                                                                                                                                                 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (false == play)
			return;

		Vector3 center = this.transform.localPosition;
		Vector3 position = GetPosition ();
		Vector3 v1 = last - center;
		Vector3 v2 = position - center;

		v1.Normalize ();
		v2.Normalize ();

		float delta = AngleSigned (v1, v2, Vector3.forward);
		angle += delta;
		last = position;

		angle = Mathf.Clamp (angle, Limitangle, Maxangle);
		this.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, angle);
		driving.steer = (0.0f != angle) ? -angle / 360.0f : 0.0f;


		if (angle <= -360) 
		{

			over = true;
			Limitangle -= 0.05f;

			if(angle <= -370)
				over = false;
		}
		else
			Limitangle = -360.0f;

		if (angle >= 360) 
		{

			over = true;
			Maxangle += 0.05f;

			if(angle >= 370)
				over = false;
		}
		else
			Maxangle = 360.0f;
			
		if (angle >= 360 || angle <= -360)
			if (over) 
				dieselsteam.On();

	}


	
	public void OnPressButton(GameObject btn)
	{
		play = true;
		
		mouse = false;
		touch = -1;

        if (UICamera.GetMouse(0).pressed == btn)
        {
            mouse = true;
        }
        else
        {
            for (int i = 0; i < UICamera.activeTouches.Count; i++)
            {
                if (UICamera.activeTouches[i].pressed == btn)
                {
                    touch = i;
                    break;
                }
            }
        }

		last = GetPosition();

		GameEvent.notify (GameEvent.TAG.DRIVE_HANDLE);
	}
	
	public void OnReleaseButton(GameObject btn)
	{
		play = false;
	}

	Vector3 GetPosition()
	{
		Vector2 position = Vector2.zero;
		if (true == mouse) {
			position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		} 
		else if (-1 != touch) {
			position = Input.touches[touch].position;
		}

		return new Vector3(UITools.GetWidth (position.x), UITools.GetHeight (position.y), 0.0f);
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(
			Vector3.Dot(n, Vector3.Cross(v1, v2)),
			Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}
