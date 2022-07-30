using UnityEngine;
using System.Collections;

public class UIDriveCamera : MonoBehaviour 
{
	public DrivingCamera drivingCamera;
	public UISprite[] widgets;

	void Start()
	{
		SetColor (0);
	}

	public void OnSwitchCenter()
	{
		drivingCamera.target = drivingCamera.center;
		SetColor (0);

		GameEvent.notify (GameEvent.TAG.DRIVE_CAMERA_CENTER);
	}


    public void OnSwitchExpand()
    {
        GetComponent<TextureScale>().OnScale();
    //    drivingCamera.target = drivingCamera.center;
        SetColor(3);

     //   GameEvent.notify(GameEvent.TAG.DRIVE_CAMERA_EXPAND);
    }
	
	public void OnSwitchFront()
	{
		drivingCamera.target = drivingCamera.fornt;
		SetColor (1);

		GameEvent.notify (GameEvent.TAG.DRIVE_CAMERA_FRONT);
	}
	
	public void OnSwitchRear()
	{
		drivingCamera.target = drivingCamera.rear;
		SetColor (2);

		GameEvent.notify (GameEvent.TAG.DRIVE_CAMERA_REAR);
	}
	
	void SetColor(int index)
	{
		for (int i=0; i<widgets.Length; ++i) 
		{
			if (i == index)
			{
				widgets[i].spriteName = "ex_Driv_Button_View_" + widgets[i].name + "_Push";
			}
			else
			{
				widgets[i].spriteName = "ex_Driv_Button_View_" + widgets[i].name + "_Normal";
			}
		}
	}
}
