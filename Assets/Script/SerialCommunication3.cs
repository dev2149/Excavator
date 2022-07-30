using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;

public class SerialCommunication3 : MonoBehaviour
{
    #region 싱글톤.

    private static SerialCommunication3 _instance = null;

    public static SerialCommunication3 Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "DataManager";
                _instance = obj.AddComponent<SerialCommunication3>();
                DontDestroyOnLoad(obj);
            }

            return _instance;
        }
    }

    #endregion

    public float[] analogValues = new float[9];
    public float[] digitalValues = new float[9];

    public int Gear;

    bool m_bRunThread = true;

    long m_lLastSensorInputTime = 0;
    long m_lInputDuration = 0;

    string message;

    byte iByte;

    string[] ports;

    int portCount;

    //시리얼 포트 번호. 
    SerialPort stream;

    //진동 (컨트롤러 번호, 파워세기 , 시간 ) 
    public void Vibration(int num, string pow, string time)
    {
        stream.Write("MTON0:1:" + pow + ":" + time + ":FIN");
    }

    //진동강제 종료 (컨트롤러 번호)
    public void VibrationOFF(int num)
    {
        stream.Write("MTOF" + num + ":FIN");
    }

    //영점 잡기 시작.
    public void ZeroPoint(int num)
    {
        stream.Write("STAD" + num + ":FIN");
    }

    //시리얼 통신 속도 (속도 수치)
    public void SetPeriodSpeed(string speed)
    {
        stream.Write("SETPR:" + speed + ":FIN");
    }

    void StartThread()
    {
        m_bRunThread = true;

        //이미 시리얼 통신이 진행중일 경우.
        if (stream.IsOpen)
        {
            stream.Close();

            Debug.Log("Close Sream Reset Unity");
        }

        //시리얼 통신 OPEN.
        try
        {
            Debug.Log("Open Sream");
            stream.Open();

            stream.ReadTimeout = 50;
        }

        //에러발생시.
        catch (Exception e)
        {
            Debug.Log("Error opening port " + e.ToString()); // I never see this message
        }

        //시리얼 통신 시작.
        new Thread(ReadThread).Start();
    }

    void Start()
    {
        ports =  SerialPort.GetPortNames();

        if (ports.Length > 0)
        {
            Debug.Log(ports[ports.Length - 1]);

            stream = new SerialPort(ports[ports.Length - 1], 115200, Parity.None, 8, StopBits.One);

           // StartThread();
        }
    }

    //어플이 종료시 스레드 닫아주기.
    void OnApplicationQuit()
    {
        m_bRunThread = false;

        if (stream != null)
            stream.Close();
    }

    void ReadThread()
    {
        Debug.Log("Start ReadThread");

        while (m_bRunThread)
        {
            message = "";

            try
            {
                // 받은 바이트 값. 
                iByte = (byte)stream.ReadByte();

                while (iByte != 255)
                {
                    message += ((char)stream.ReadByte());

                    //FIN이 들어가면 종료.
                    if (message.Contains("FIN")) { break; }
                }
            }

            //오류 발생시.
            catch (Exception e)
            {
                Debug.Log("Error " + e.ToString()); // I never see this message
            }

            if (message != "")
            {
                //파싱을 시작.
                Parsing(message);
            }
        }
    }

    void Parsing(string message)
    {
        // PAT (number) : 아날로그 포트 
        // DAT (number) : 디지털 포트

        // : 기준으로 분리.
        string[] mes = message.Split(':');

        //디지털 인가 아날로그 인가 구분 
        bool isAnalog = mes[0].Substring(1, 1) == "A" ? true : false;

        int PortNumber = int.Parse(mes[0].Substring(3, 1));

        //아날로그 일 경우.
        if (isAnalog)
        {
            //데이터가 다 넘어오지 못함 경우 reture.
            if (mes.Length < 2) { return; }

            //부호.
            int mark = mes[1].Substring(0, 1) == "-" ? -1 : 1;

            // 데이터 값.
            float value = float.Parse(mes[1].Substring(1, 4));

            //NULL 값이 아닌지 체크.
            if (value < 1000)
            {
                analogValues[PortNumber] = mark * value / 350f;
            }
            else
            {
                analogValues[PortNumber] = 9999;
            }

        }
        //디지털일 경우.
        else
        {
            digitalValues[PortNumber] = int.Parse(mes[1]);
        }

        //기어 상태를 구함.
        GetGearValue();
    }

    void GetGearValue()
    {
        if (digitalValues[1] == 0 && digitalValues[2] == 1)
        {
            Gear = 1;
        }
        else if (digitalValues[1] == 1 && digitalValues[2] == 1)
        {
            Gear = 0;
        }
        else
        {
            Gear = -1;
        }
    }
}