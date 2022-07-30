using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum GearType
{
    reverse = -1,
    neutral = 0,
    forward = 1,
}

public enum SpringState
{
    None,
    SpringOn,
    SpringOff
}

public class Steering : MonoBehaviour
{

    #region 싱글톤.

    private static Steering _instance = null;

    public static Steering Instance
    {
        get
        {

            if (_instance == null)
            {
                _instance = FindObjectOfType<Steering>();

                GameObject obj = new GameObject("SteeringManager");
                _instance = obj.AddComponent<Steering>();
                obj.transform.parent = GameObject.Find("Singleton").transform;
                _instance.Init();
                DontDestroyOnLoad(GameObject.Find("Singleton"));
                //DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    #endregion

    [SerializeField] GearType gearType;

    [SerializeField] int index = 0;

    [SerializeField] float wheelAngle = 0f;

    [SerializeField] float acceleraterPedal = 0f;
    [SerializeField] float brakePedal = 0f;
    [SerializeField] float cluchPedal = 0f;

    [SerializeField] bool forwardGear;
    [SerializeField] bool reverseGear;
    [SerializeField] bool springForce;

    public DataInfo dataInfo;

    public bool IsChange { get; private set; }
    public bool IsFirstRun { get; private set; }
    public bool IsNotStop = true;
    public SpringState ChangeState { get; private set; }

    private void Init()
    {
        string s_JsonData = File.ReadAllText(Application.persistentDataPath + "/dataInfo.json");
        dataInfo = JsonUtility.FromJson<DataInfo>(s_JsonData);
        LogitechGSDK.LogiSteeringInitialize(false);
        LogitechDeviceSearch();
        IsFirstRun = true;
        StartCoroutine(InitSteering());
    }

    IEnumerator InitSteering()
    {
        yield return new WaitForSeconds(2.0f);

        IsFirstRun = false;
        SpringForceOn();

        yield return new WaitForSeconds(1.0f);

        PlayFrontalCollisionForce(60);
    }

    void LogitechDeviceSearch()
    {
        string[] names = Input.GetJoystickNames();

        for (int i = 0; i < names.Length; i++)
        {
            StringBuilder devicename = new StringBuilder(256);
            LogitechGSDK.LogiGetFriendlyProductName(i, devicename, 256);

            if (!devicename.ToString().IndexOf("G29").Equals(-1) || LogitechGSDK.LogiIsDeviceConnected(i, LogitechGSDK.LOGI_DEVICE_TYPE_WHEEL))
            {
                index = i;
                break;
            }
        }
    }

    void LogitechwheelUpdate()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(index))
        {
            if (IsChange)
            {
                if (ChangeState.Equals(SpringState.SpringOff))
                {
                    LogitechGSDK.LogiStopSpringForce(index);
                }
                else if (ChangeState.Equals(SpringState.SpringOn))
                {
                    LogitechGSDK.LogiIsPlaying(index, LogitechGSDK.LOGI_FORCE_SPRING);
                    LogitechGSDK.LogiPlaySpringForce(index, 0, 50, dataInfo.SteeringPower); // 핸들 파워
                }
                ChangeState = SpringState.None;
                IsChange = false;
            }

            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateCSharp(index);

            wheelAngle = rec.lX;

            acceleraterPedal = rec.lY;

            brakePedal = rec.lRz;

            cluchPedal = rec.rglSlider[0];

            //전진기어.
            if (GetState(rec.rgbButtons[12]) || GetState(rec.rgbButtons[14]) || GetState(rec.rgbButtons[16]))
                forwardGear = true;
            else
                forwardGear = false;

            //후진기어.
            if (GetState(rec.rgbButtons[13]) || GetState(rec.rgbButtons[15]) ||
                GetState(rec.rgbButtons[17]) || GetState(rec.rgbButtons[18]))
                reverseGear = true;
            else
                reverseGear = false;
        }
    }

    public GearType GetGearType()
    {
        if (forwardGear && !reverseGear) return GearType.forward;

        else if (!forwardGear && !reverseGear) return GearType.neutral;

        else return GearType.reverse;
    }

    public float GetWheelAngle()
    {
        float _wheelAngle = -1f * wheelAngle / 32767f;

        return _wheelAngle;
    }

    public float GetAcceleraterPedal()
    {
        float accele = -1f * acceleraterPedal / 32767f;

        if (accele < 0)
        {
            return 0;
        }
        else
        {
            return accele;
        }

    }

    public float GetBrakePedal()
    {
        float brake = -1f * brakePedal / 32767f;

        if (brake < 0)
        {
            return 0;
        }
        else
        {
            return brake;
        }
    }

    public float GetCluchPedal()
    {
        float cluch = 32768 + cluchPedal;

        if (cluch == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public float GetCluchPedalPos()
    {
        float cluch = -1f * cluchPedal / 32767f;

        if (cluch < 0)
        {
            return 0;
        }
        else
        {
            return cluch;
        }
    }


    public void PlayFrontalCollisionForce(int a)
    {
        LogitechGSDK.LogiPlayFrontalCollisionForce(index, a);
    }

    public void SpringForceOn()
    {
        ChangeState = SpringState.SpringOn;
        IsChange = true;
    }

    public void SpringForceOff()
    {
        ChangeState = SpringState.SpringOff;
        IsChange = true;
    }

    private bool GetState(int _button)
    {
        bool ret = _button.Equals(128) ? true : false;

        return ret;
    }

    private void Update()
    {
        if (IsNotStop)
        {
            LogitechDeviceSearch();
            LogitechwheelUpdate();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        IsNotStop = focus;

        if (focus.Equals(false))
        {
            LogitechGSDK.LogiSteeringShutdown();
            SpringForceOff();
        }
        else if (focus.Equals(true))
        {
            LogitechGSDK.LogiSteeringInitialize(false);
            SpringForceOn();
        }
    }

    private void OnApplicationQuit()
    {
        LogitechGSDK.LogiSteeringShutdown();
    }
}
