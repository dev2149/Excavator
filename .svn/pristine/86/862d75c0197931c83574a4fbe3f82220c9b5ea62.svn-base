using UnityEngine;
using System.Collections;

public class UIDrivePedal : MonoBehaviour
{
    public Driving driving;
    public UISprite imgAccel;
    public UISprite imgBreak;
    private int iGear;
    private string GearState;

    public Transform SpeedIV;
    public Transform RPMIV;

    private bool b_AccelIV = false;
    private bool b_gearIV = true;
    private float MySpeed = -100.0f;

    void Start()
    {
        imgAccel.spriteName = "ex_Driv_Button_Accel_Normal";
        iGear = 1;
        GearState = "저속 모드";
    }


    public void OnGearTwo()
    {
        iGear = 20;
        GearState = "고속 모드";
    }


    public void OffGearTwo()
    {
        iGear = 1;
        GearState = "저속 모드";
    }


    public void OnPressAccel()
    {
        if (iGear == 1 && driving.motor > 3)
        {
            driving.motor -= 0.1f;
        }

        if (driving.motor < 3 * iGear)
        {
            driving.motor += 0.1f;        //패달 밟기에 따라 가속 처리
            imgAccel.spriteName = "ex_Driv_Button_Accel_Push";

            GameEvent.notify(GameEvent.TAG.DRIVE_PEDAL_ACCEL);
        }

        b_AccelIV = true;
    }


    public void OnReleaseAccel()
    {
        driving.motor = 0.0f;
        imgAccel.spriteName = "ex_Driv_Button_Accel_Normal";
        b_AccelIV = false;
    }


    public void OnPressBreak()
    {
        driving.brake = 1.0f;
        imgBreak.spriteName = "ex_Driv_Button_Break_Push";

        GameEvent.notify(GameEvent.TAG.DRIVE_PEDAL_BREAK);

        if (MySpeed > -100.0f)
        {
            MySpeed = MySpeed - 80.0f * Time.deltaTime;
            SpeedIV.localRotation = Quaternion.Euler(90, MySpeed, 0);
        }
    }


    public void OnReleaseBreak()
    {
        driving.brake = 0.0f;
        imgBreak.spriteName = "ex_Driv_Button_Break_Normal";
    }

    void SetSpeedIV()
    {
        if (b_gearIV)        //기어가 들어가면 바늘이 떨리게
        {
            SpeedIV.localRotation = Quaternion.EulerRotation(new Vector3(0, 0, 0.1f));
            if (b_AccelIV)       //악셀을 밟았을때 속도계
            {
                //알피엠
                RPMIV.localRotation = Quaternion.Euler(90.0f, Random.Range(80.0f, 75.0f), 0.0f);

                if (MySpeed < Random.Range(-20.0f, -10.0f))
                {
                    //  스피드
                    MySpeed = MySpeed + Random.Range(10.0f, 25.0f) * Time.deltaTime;
                    SpeedIV.localRotation = Quaternion.Euler(90, MySpeed, 0);
                }
                else
                {
                    MySpeed = MySpeed - 40.0f * Time.deltaTime;
                    SpeedIV.localRotation = Quaternion.Euler(90, MySpeed, 0);
                }
            }
            else if (MySpeed > -97.0f)
            {
                MySpeed = MySpeed - 40.0f * Time.deltaTime;
                SpeedIV.localRotation = Quaternion.Euler(90, MySpeed, 0);
            }
            else
            {
                MySpeed = MySpeed + 30.0f * Time.deltaTime;
                SpeedIV.localRotation = Quaternion.Euler(90, MySpeed, 0);
            }

        }
        else if (MySpeed > -100.0f)
        {
            MySpeed = MySpeed - 40.0f * Time.deltaTime;
            SpeedIV.localRotation = Quaternion.Euler(90, MySpeed, 0);
        }
        else
        {
            //MyRPM = 80.0f;
            //RPMIV.localRotation = Quaternion.Euler(90.0f, MyRPM, 0.0f);
        }
    }

    void Update()
    {
       SetSpeedIV();  //계기판		

    }
}