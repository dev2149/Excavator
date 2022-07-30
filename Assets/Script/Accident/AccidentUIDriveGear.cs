using UnityEngine;
using System.Collections;

public class AccidentUIDriveGear : MonoBehaviour
{
    public AccidentDriveing driving;

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
        AccidentDriveScene.instance.SetColor(gear);
    }
}
