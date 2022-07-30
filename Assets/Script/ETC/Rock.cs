using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Rock : MonoBehaviour
{
    #region 싱글톤.

    private static Rock _instance = null;

    public static Rock Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("RockeyManager");
                _instance = obj.AddComponent<Rock>();
                obj.transform.parent = GameObject.Find("Singleton").transform;
                DontDestroyOnLoad(GameObject.Find("Singleton"));
            }
            return _instance;
        }
    }

    #endregion

    [DllImport("Rockey4ND_X64")]//, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    static extern ushort Rockey(ushort function, out ushort handle, out uint lp1, out uint lp2, out ushort p1, out ushort p2, out ushort p3, out ushort p4, ref byte buffer);

    private void Start()
    {
        StartCoroutine("StartCheckRockey");
    }

    bool isRockeyFail = false;

    IEnumerator StartCheckRockey()
    {
        if (GetRockey() == false)
        {
            yield return new WaitForSeconds(3f);

            Application.Quit();
        }

        while (true)
        {
            yield return new WaitForSeconds(30f);

            if (GetRockey() == false)
            {
                yield return new WaitForSeconds(3f);

                Application.Quit();
            }
        }
    }

    private void OnGUI()
    {
        if (isRockeyFail)
        {
            GUI.Box(new Rect(Screen.width * 0.3f, Screen.height / 2, 800, 50), " 시뮬레이터 인증 USB가 인식되지 않습니다.");
        }

    }

    public bool GetRockey()
    {
        const ushort RY_FIND = 1;
        const ushort RY_FIND_NEXT = 2;
        const ushort RY_OPEN = 3;
        const ushort RY_CLOSE = 4;
        const ushort RY_READ = 5;
        const ushort RY_WRITE = 6;
        const ushort RY_RANDOM = 7;
        const ushort RY_SEED = 8;
        const ushort RY_WRITE_USERID = 9;
        const ushort RY_READ_USERID = 10;
        const ushort RY_SET_MOUDLE = 11;
        const ushort RY_CHECK_MOUDLE = 12;
        const ushort RY_WRITE_ARITHMETIC = 13;
        const ushort RY_CALCULATE1 = 14;
        const ushort RY_CALCULATE2 = 15;
        const ushort RY_CALCULATE3 = 16;
        const ushort RY_DECREASE = 17;

        ushort[] m_Handle = new ushort[32];
        int m_HandleNum = 0;

        ushort ret;
        ushort p1, p2, p3, p4;
        uint lp1, lp2;
        byte[] buffer = new byte[1024];

        p1 = 0x19F8;
        p2 = 0x19EA;
        p3 = 0x0;
        p4 = 0x0;


        ret = Rockey(RY_FIND, out m_Handle[0], out lp1, out lp2, out p1, out p2, out p3, out p4, ref buffer[0]);

        if (ret != 0)
        {
            Debug.Log("RY_FIND error");
            isRockeyFail = true;
            return false;
        }
        else
        {
            isRockeyFail = false;
            Debug.Log("HID: " + lp1.ToString("X") + "\r\n");

            // 마스터키 감지
            if (lp1.ToString("X") == "39535D91")
            {
                return true;
            }
        }

        ret = Rockey(RY_OPEN, out m_Handle[0], out lp1, out lp2, out p1, out p2, out p3, out p4, ref buffer[0]);

        if (ret != 0)
        {
            Debug.Log("RY_OPEN error");
            isRockeyFail = true;
            return false;
        }

        m_HandleNum = 1;


        ret = Rockey(RY_READ_USERID, out m_Handle[0], out lp1, out lp2, out p1, out p2, out p3, out p4, ref buffer[0]);

        if (ret != 0)
        {
            isRockeyFail = true;
            return false;
        }
        else
        {
            if (lp1.ToString("X") == "3000")
            {
                return true;
            }
            else if (lp1.ToString("X") == "3001")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
