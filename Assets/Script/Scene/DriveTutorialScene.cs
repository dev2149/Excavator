using UnityEngine;
using System.Collections;

public class DriveTutorialScene : MonoBehaviour 
{
	void OnFinish()
    {
		DeviceVibrate.Play ();
        Application.LoadLevel("drive_mode");
	}
}
