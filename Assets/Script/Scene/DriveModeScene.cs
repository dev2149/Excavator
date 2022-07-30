using UnityEngine;
using System.Collections;

public class DriveModeScene : MonoBehaviour
{
    public GameObject pratice;
    public GameObject buttonN;

    public bool isCaterpiller = false;

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
        {
            Application.LoadLevel("menu");
        }
	}

    public void OnCaterpiller()
    {
        Driving.tutorial = true;
        Driving.start_at_end = false;

        if (buttonN.active == false)
        {
            if (PlayerPrefs.GetInt("Gear") == 0)
            {
                PlayerPrefs.SetInt("CurrentScene", 3);
                Application.LoadLevel("accident");
            }
            else
            {
                pratice.SetActive(false);
                buttonN.SetActive(true);

                isCaterpiller = true;
            }
        }
	
    }

    public void OnTest()
    {
        Driving.tutorial = false;
        Driving.start_at_end = false;
        //   DeviceVibrate.Play();
        if (buttonN.active == false)
        {
            if (PlayerPrefs.GetInt("Gear") == 0)
            {
                Application.LoadLevel("drive");
            }
            else
            {
                buttonN.SetActive(true);
                isCaterpiller = false;
            }

        }

    }

    public void OnPratice()
    {
        pratice.SetActive(true);
	//	DeviceVibrate.Play();
    }

    public void OnTutorial()
    {
        Driving.tutorial = true;
        Driving.start_at_end = false;
    //    DeviceVibrate.Play();
        Application.LoadLevel("drive_tutorial");
		
    }

    public void OnBack()
    {
      //  DeviceVibrate.Play();
        Application.LoadLevel("menu");
		
    }

    public void OnPraticeStartAtStart()
    {
//		Vibration.Vibrate(100);
        Driving.tutorial = true;
        Driving.start_at_end = false;

        if(buttonN.active == false)
        {
            if (PlayerPrefs.GetInt("Gear") == 0)
            {
                PlayerPrefs.SetInt("CurrentScene", 1);
                Application.LoadLevel("accident");
            }
            else
            {
                pratice.SetActive(false);
                buttonN.SetActive(true);
            }
        }
       // Application.LoadLevel("drive");
		
    }

    public void GoDrive()
    {
        if (PlayerPrefs.GetInt("Gear") == 0)
        {
            if (isCaterpiller == false)
            {
                Application.LoadLevel("drive");
            }
            else
            {
                Application.LoadLevel("accident");
            }
            
        }
    }

    public void OnPraticeStartAtEnd()
    {
	//	Vibration.Vibrate(100);
        Driving.tutorial = true;
        Driving.start_at_end = true;
        if (buttonN.active == false)
        {
            if (PlayerPrefs.GetInt("Gear") == 0)
            {
                PlayerPrefs.SetInt("CurrentScene", 2);
                Application.LoadLevel("accident");
            }
            else
            {
                pratice.SetActive(false);
                buttonN.SetActive(true);
            }
        }
       // Application.LoadLevel("drive");
		
    }
	public void OnVibration()
	{
		if (!Option.Instance.Vibration)
			return;

	//	Vibration.Vibrate(100);
	}
}
