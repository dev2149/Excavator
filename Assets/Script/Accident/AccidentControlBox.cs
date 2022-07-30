using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AccidentControlBox : MonoBehaviour
{
    public AccidentDiging.AXIS mHandle;
    public AccidentDiging.AXIS mRightBar;
    public AccidentDiging.AXIS mLeftBar;
    public AccidentDiging.AXIS mAccelerate;
    public AccidentDiging.AXIS mBreak;
    public AccidentDiging.AXIS mCluch;
    public AccidentDiging.AXIS mGear;

    public DigingWarning dieselsteam;

    public float HandleValue;

    Steering steering;

    void Start()
    {
        mHandle.angle = 0.0f;
        mRightBar.angleR = 0.0f;
        mLeftBar.angleR = 0.0f;
        mRightBar.angleL = 0.0f;
        mLeftBar.angleL = 0.0f;

        mAccelerate.angle = 0.0f;
        mBreak.angle = 0.0f;
        mCluch.angle = 0.0f;
        mGear.angle = 0.0f;

        steering = AccidentDriveScene.instance.steering;
    }

    // Update is called once per frame
    void Update()
    {
        InputControlAnim();
    }

    void InputControlAnim()
    {
        //핸들
        if (mHandle.node != null)
        {
            mHandle.angle = Mathf.Clamp(steering.GetWheelAngle(), -1, 1) * -440;

            mHandle.node.localRotation = Quaternion.Euler(mHandle.node.transform.rotation.x, mHandle.node.transform.rotation.y, mHandle.angle + 180f);

            if (mHandle.angle >= 550 || mHandle.angle <= -550)      // 핸들 스팀소리
            {
                dieselsteam.On();
            }
        }

        //왼쪽w
        if (mLeftBar.node != null)
        {
           
            int joy2 = GetJoysticks.Instance.GetJoystickNumber(JoystickType.Left);

            if (joy2 >= 0)
            {
                mLeftBar.angleR = Mathf.Clamp(Input.GetAxisRaw(joy2 + "_Joystick_Horizontal"), -1, 1) * 30;
                mLeftBar.angleL = -1f * Mathf.Clamp(Input.GetAxis(joy2 + "_Joystick_Vertical"), -1, 1) * 30;

                mLeftBar.node.localRotation = Quaternion.Euler(mLeftBar.angleL, 0.0f + mLeftBar.angleR , 0.0f);
            }

            
        }

        //////오른쪽
        if (mRightBar.node != null)
        {
            int joy1 = GetJoysticks.Instance.GetJoystickNumber(JoystickType.Right);

            if (joy1 >= 0)
            {
                mRightBar.angleR = Mathf.Clamp(Input.GetAxisRaw(joy1 + "_Joystick_Horizontal"), -1, 1) * 30;
                mRightBar.angleL = -1f * Mathf.Clamp(Input.GetAxis(joy1 + "_Joystick_Vertical"), -1, 1) * 30;
                
                mRightBar.node.localRotation = Quaternion.Euler(mRightBar.angleL, 0.0f + mRightBar.angleR, 0.0f);
            }
        }

        ////엑셀
        if (mAccelerate.node != null)
        {
            mAccelerate.angle = Mathf.Clamp(steering.GetAcceleraterPedal(), 0, 1) * 42f;
            mAccelerate.node.localRotation = Quaternion.Euler(mAccelerate.node.transform.rotation.x, mAccelerate.node.transform.rotation.y, 42f + mAccelerate.angle);
        }

        ////브레이크
        if (mBreak.node != null)
        {
            mBreak.angle = Mathf.Clamp(steering.GetBrakePedal(), 0, 1) * 42f;
            mBreak.node.localRotation = Quaternion.Euler(mBreak.node.transform.rotation.x, mBreak.node.transform.rotation.y, 42.0f + mBreak.angle);
        }

        ////클러치
        if (mCluch.node != null)
        {
            mCluch.angle = Mathf.Clamp(steering.GetCluchPedalPos(), 0, 1) * 31.5f;
            mCluch.node.localRotation = Quaternion.Euler(mCluch.node.transform.rotation.x, -94.0f - mCluch.angle, mCluch.node.transform.rotation.z);
        }

        // 기어w

        if (mGear.node != null)
        {
            if (steering.GetGearType() == GearType.reverse)
                mGear.angle = 0.0f;
            else if (steering.GetGearType() == GearType.forward)
                mGear.angle = -54.0f;
            else
                mGear.angle = -27.0f;

            mGear.node.localRotation = Quaternion.Euler(82.61371f, 61.92733f, mGear.angle);
        }
    }
}
