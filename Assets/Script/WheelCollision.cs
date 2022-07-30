using UnityEngine;
using System.Collections;

public class WheelCollision : MonoBehaviour {

	public AudioClip clip;
	public Sender sender;

	public string wheel;
	
	void OnCollisionEnter(Collision collision)
	{
		if (true == Driving.finish)
			return;
		
		if (collision.transform.tag != "Con")
			return;

//		print (wheel);
		int _index = 0;

		if (wheel == "Wheel_F_R")
			_index = 11; 
		if (wheel == "Wheel_F_L")
			_index = 12; 
		if (wheel == "Wheel_R_R")
			_index = 13; 
		if (wheel == "Wheel_R_L")
			_index = 14; 


		DeviceVibrate.Play ();
		sender.SendMessage("OnCollisionCon", _index);
		Destroy (this.gameObject);
		
		if (null != clip)
			DeviceSound.Instance.Play (clip,DeviceSound.Instance.Effect);	
		
	}
}
