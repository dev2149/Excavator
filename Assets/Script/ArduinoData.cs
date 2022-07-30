using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;

public class ArduinoData : MonoBehaviour
{
    /// <summary>
    /// 안전벨트,
    /// 사용하지않음,
    /// 사용하지않음,
    /// 사이드브레이크,
    /// 기어봉
    /// </summary>
    public enum ArudinoIndex
    {
        SafeBalt,
        SafeBar,
        RPM,
        up,
        down
    }

    #region 싱글톤.

    private static ArduinoData _instance = null;
    public static ArduinoData Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
    
    public bool Initialization { get; private set; }
    public bool rpmOk;
    public bool arduinoOk;
    bool testAD;
    #region Arduino

    private SerialPort sp;
    private int portnum;
    private int CurrentSerialportIndex = 0;

    private string RecvText;
    private List<int> rpmL;

    public bool RunReadThread = false;

    public bool safebelt;
    public bool safebar;
    public bool safepin;
    [Range(0,255)]
    public int rpm;

    public int gearUp;
    public int gearDown;

    #endregion
    // Use this for initialization
    public DataInfo dataInfo;

    public void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        rpmL = new List<int>();
        for (int i = 0; i <= 255; i++)
        {
            rpmL.Add(i);
        }

        try
        {
            SimgData simgData = new SimgData();
            var com = simgData.GetData();
            portnum = int.Parse(com.Substring(3));
            // "COM5"는 컴퓨터,포트 위치 등등 다른이유로 바뀔수도 있음 확인 필요 -> SerialPort.GetPortNames() 자동으로 가져옴
            // 확인 방법은 컴퓨터의 장치관리자 -> 포트(com & lpt) 에서 아두이노 드라이버를 설치했을 경우 아두이노라고 보인다.

            if (portnum >= 10)
            {
                sp = new SerialPort("\\\\.\\COM" + portnum, 9600);
            }
            else
            {
                sp = new SerialPort(com, 9600);
            }
            //sp = new SerialPort(com, 9600);
            sp.ReadTimeout = 500;       //타임아웃 시간
            print("Opening Port");
            sp.Open();
            print("Port Open Successful");

            Initialization = true;
            // recvData = new int[3];

            sp.Write("O");
        }
        catch (System.Exception e)
        {
            Initialization = true;
            testAD = true;
            Debug.Log("Port connection failed : " + e);

            return;
        }

        RunReadThread = true;
        new Thread(ReadThread).Start();

        StartCoroutine(arduinoAcive());
    }
    private void Update()
    {
        string s_JsonData = File.ReadAllText(Application.persistentDataPath + "/dataInfo.json");
        dataInfo = JsonUtility.FromJson<DataInfo>(s_JsonData);
        if (testAD)
        {
            arduinoOk = true;
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (gearUp == 0)
                {
                    gearDown = 1;
                    gearUp = 1;
                }
                else
                {
                    gearDown = 1;
                    gearUp = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (gearDown == 0)
                {
                    gearDown = 1;
                    gearUp = 1;
                }
                else
                {
                    gearUp = 1;
                    gearDown = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (safebelt)
                {
                    safebelt = false;
                }
                else
                {
                    safebelt = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (safebar)
                {
                    safebar = false;
                }
                else
                {
                    safebar = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (safepin)
                {
                    safepin = false;
                }
                else
                {
                    safepin = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                rpm = 0;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                rpm = 100;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                rpm = 255;
            }
        }
    }
    void ReadThread()
    {
        Debug.Log("Start ReadThread");
        while (RunReadThread)
        {
            try
            {
                if (sp != null)
                {
                    if (sp.IsOpen)
                    {
                        sp.Write("2");
                        if (dataInfo.Rpm == false && rpmL[0] == 255)
                        {
                            rpmL.Reverse();
                        }
                        else if (dataInfo.Rpm == true && rpmL[0] == 0)
                        {
                            rpmL.Reverse();
                        }
                        RecvText = sp.ReadLine();       //아두이노가 보내는 신호 1줄을 읽는다.

                        if (RecvText.Length > 2)
                        {
                            if (RecvText[0].Equals('S') && RecvText[RecvText.Length - 1].Equals('E'))
                            {
                                RecvText = RecvText.TrimStart('S').TrimEnd('E');
                                string[] Recvdata = RecvText.Split(',');
                                if (Recvdata.Length.Equals(9))
                                {
                                    rpm = int.Parse(Recvdata[7]);
                                    rpm = rpmL[rpm];
                                    gearDown = dataInfo.GearDown ? int.Parse(Recvdata[5]) : 1 - int.Parse(Recvdata[5]);
                                    gearUp = dataInfo.GearUp ? int.Parse(Recvdata[6]) : 1 - int.Parse(Recvdata[6]);
                                    safebelt = dataInfo.Belt ? ((int.Parse(Recvdata[1]) == 1) ? true : false) : ((int.Parse(Recvdata[1]) == 1) ? false : true);
                                    safebar = dataInfo.SafeBar ? ((int.Parse(Recvdata[2]) == 1) ? true : false) : ((int.Parse(Recvdata[2]) == 1) ? false : true);
                                    safepin = dataInfo.SafeLever ? ((int.Parse(Recvdata[8]) == 1) ? true : false) : (safepin = (int.Parse(Recvdata[8]) == 1) ? false : true);                                   
                                    arduinoOk = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Thread recv data is error : " + e);
                arduinoOk = false;
                //if (CurrentSerialportIndex < SerialPort.GetPortNames().Length - 1)
                //    ReLoadSerialPort();
            }
        }
    }

    private bool GetStatekey(int value)
    {
        return (value.Equals(1)) ? true : false;
    }

    private void ReLoadSerialPort()
    {
        sp = null;
        CurrentSerialportIndex++;

        sp = new SerialPort(SerialPort.GetPortNames()[CurrentSerialportIndex], 9600);
        sp.ReadTimeout = 500;       //타임아웃 시간
        print("Opening Port");
        sp.Open();
        print("Port Open Successful");
        sp.Write("O");
    }

    void OnApplicationQuit()
    {
        RunReadThread = false;
        if (sp != null)
        {
            OffButtons();
            sp.Close();
        }
    }

    public void OffButtons()
    {
        if (sp != null)
            sp.Write("X");
    }

    public void OnActiveCheck()
    {
        if (sp != null)
            sp.Write("A");
    }

    /*// <summary>
    /// 다른버튼 켜줌(파랑)
    /// </summary>
    public void Bar()
    {
        if (sp != null)
            sp.Write("L");
    }
    public void Balt()
    {
        if (sp != null)
            sp.Write("M");
    }*/

    IEnumerator arduinoAcive()
    {
        while (true)
        {
            if (sp != null)
                sp.Write("A");
            yield return new WaitForSeconds(3f);
        }
    }
    
}