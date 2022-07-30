using UnityEngine;
using System.Collections;

public class UIDriveGear : MonoBehaviour
{
    public Driving driving;
    public UISprite[] widgets;

    void Start()
    {
        SetColor(GearManager.gear);
    }

    public void OnSwitchN()
    {
        if(driving != null)
        driving.gear = GEAR.N;

        SetColor(GEAR.N);

        if (driving != null)
            GameEvent.notify(GameEvent.TAG.DRIVE_GEAR_N);
    }

    public void OnSwitchD()
    {
        if (driving != null)
            driving.gear = GEAR.D;

        SetColor(GEAR.D);

        if (driving != null)
            GameEvent.notify(GameEvent.TAG.DRIVE_GEAR_D);
    }

    public void OnSwitchR()
    {
        if (driving != null)
            driving.gear = GEAR.R;

        SetColor(GEAR.R);

        if (driving != null)
            GameEvent.notify(GameEvent.TAG.DRIVE_GEAR_R);
    }

    void Update()
    {
        SetColor(GearManager.gear);
    }

    void SetColor(GEAR gear)
    {
        //for (int i = 0; i < widgets.Length; i++)
        //{
        //    GEAR _gear = (GEAR)i;

        //    if (i == (int)gear)
        //    {
        //        widgets[i].spriteName = "ex_Driv_Button_" + _gear.ToString() + "_Push";
        //    }
        //    else
        //    {
        //        widgets[i].spriteName = "ex_Driv_Button_" + _gear.ToString() + "_Normal";
        //    }
        //}

        if (ViveDriveScene.instance != null && ViveDriveScene.instance.IsVR())
        {
            ViveDriveScene.instance.SetColor(gear);
        }
    }
}
