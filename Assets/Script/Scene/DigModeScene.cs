using UnityEngine;
using System.Collections;

public class DigModeScene : MonoBehaviour 
{
	public GameObject pratice;

	void Start()
	{
		DeviceSound.Instance.source_bgm.clip =  Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;	
	}
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
        {
            Application.LoadLevel("menu");
        }
	}
	public void OnTest () 
	{
		Diging.tutorial = false;
	//	DeviceVibrate.Play ();
		Application.LoadLevel("dig");
	}

	public void OnPratice () 
	{
		Diging.tutorial = true;
        // 0 -> 굴삭연습, 1 -> 주행 연습 2 -> 주행 연습 후진
        PlayerPrefs.SetInt("CurrentScene", 0);
		//DeviceVibrate.Play ();
//		pratice.SetActive(true);
        Application.LoadLevel("accident");
	}


	public void OnTutorial () 
	{
		//DeviceVibrate.Play ();
		Application.LoadLevel("dig_tutorial");
	}

	public void OnBack () 
	{
	//	DeviceVibrate.Play ();
		Application.LoadLevel("menu");
	}

//	public void OnPraticeStartAtFlat()
//	{
//		Vibration.Vibrate(100);
//		Diging.tutorial = true;
//		Diging.start_at_flat = true;
//		Application.LoadLevel("dig");
//		
//	}
//	
//	public void OnPraticeStartAtDig()
//	{
//		Vibration.Vibrate(100);
//		Diging.tutorial = true;
//		Diging.start_at_flat = false;      
//		Application.LoadLevel("dig");
//		
//	}
	public void OnVibration()
	{
		if (!Option.Instance.Vibration)
			return;
		
		//DeviceVibrate.Play ();
//		Vibration.Vibrate(100);
	}
}
