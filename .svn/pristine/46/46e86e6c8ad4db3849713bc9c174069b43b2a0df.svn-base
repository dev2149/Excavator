using UnityEngine;
using System.Collections;

public class UIPause : MonoBehaviour 
{
	public string scene;

	void OnEnable() //일시정지.
	{
        DeviceSound.Instance.TimePause = true;
		Time.timeScale = 0.0f;
	}
	
	void OnDisable()
	{
        DeviceSound.Instance.TimePause = false;
		Time.timeScale = 1.0f;
	}

	public void OnReset()
	{
		//DeviceVibrate.Play ();
		Application.LoadLevel (scene);
	}

	public void OnMenu()
	{
	//	DeviceVibrate.Play ();
		Application.LoadLevel ("menu");
	}
}
