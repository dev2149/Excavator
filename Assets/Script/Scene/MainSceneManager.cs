using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public enum MenuType
{
    Main,
    DriveMode,
    DriveExerciseSelect,
    DigMode,
    Regulation,
    Page,
    Accident,
}

public class MainSceneManager : MonoBehaviour
{
    #region 싱글톤.

    static MainSceneManager instance;

    public static MainSceneManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;

        Rock.Instance.GetRockey();
    }
    #endregion

    [SerializeField] GameObject[] buttons = new GameObject[7];
    [SerializeField] RoomPlaneControl roomPlaneControl;
    [SerializeField] GameObject RPM, Belt, Gear, Safe, Pin, BeltE, SafeE, PinE;
    Steering _Steering;
    bool ViveStart;
    public GameObject rule;

    private void Start()
    {
        ViveStart = false;
        _Steering = Steering.Instance;

        if (_Steering.IsFirstRun.Equals(false))
        {
            _Steering.SpringForceOn();
        }

    }
    void Update()
    {
        if (ArduinoData.Instance.arduinoOk)
        {
            if (ArduinoData.Instance.rpm.Equals(255) && RPM.activeSelf)
            {
                RPM.SetActive(false);
            }
            if (ArduinoData.Instance.gearUp.Equals(1) && ArduinoData.Instance.gearDown.Equals(1) && Gear.activeSelf)
            {
                Gear.SetActive(false);
            }
            if ((PlayerPrefs.GetInt("CurrentScene") == 2))
            {
                if (ArduinoData.Instance.safebelt && BeltE.activeSelf)
                {
                    BeltE.SetActive(false);
                }
                if (ArduinoData.Instance.safebar && SafeE.activeSelf)
                {
                    SafeE.SetActive(false);
                }
                if (ArduinoData.Instance.safepin && PinE.activeSelf)
                {
                    PinE.SetActive(false);
                }
                if (Belt.activeSelf)
                {
                    Belt.SetActive(false);
                }
                if (Safe.activeSelf)
                {
                    Safe.SetActive(false);
                }
                if (Pin.activeSelf)
                {
                    Pin.SetActive(false);
                }
            }
            else
            {
                if (!ArduinoData.Instance.safebelt && Belt.activeSelf)
                {
                    Belt.SetActive(false);
                }
                if (!ArduinoData.Instance.safebar && Safe.activeSelf)
                {
                    Safe.SetActive(false);
                }
                if (!ArduinoData.Instance.safepin && Pin.activeSelf)
                {
                    Pin.SetActive(false);
                }
            }
        }

        /*if(Input.GetKeyDown(KeyCode.F1))
        {
            //굴삭 테스트.
            PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
            PlayerPrefs.SetInt("CurrentScene", 0);
            _Steering.SpringForceOff();

            SceneManager.LoadScene("ViveDig");
        }*/
        
    }
    public void SetMenu()
    {
        if (!ViveStart)
        {
            for (int i = 0; i < buttons.Length -1; i++)
            {
                buttons[i].SetActive(false);
            }
            buttons[0].SetActive(true);
        }
        ViveStart = true;
    }
    public void SetMenuType(MenuType type, string buttonName)
    {
        if (buttonName == "Exit")
        {
            GameObject.Find("MenuItemMaze").transform.Find("ExitMenu").gameObject.SetActive(true);
            GameObject.Find("MenuItemMaze").transform.Find("Exit").gameObject.SetActive(false);
        }
        if (buttonName == "ExitO")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        if (buttonName == "ExitX")
        {
            GameObject.Find("MenuItemMaze").transform.Find("ExitMenu").gameObject.SetActive(false);
            GameObject.Find("MenuItemMaze").transform.Find("Exit").gameObject.SetActive(true);
        }

        if (type == MenuType.DriveExerciseSelect)
        {
            RPM.SetActive(false);
            Gear.SetActive(false);
            Belt.SetActive(false);
            Safe.SetActive(false);
            Pin.SetActive(false);
            BeltE.SetActive(false);
            SafeE.SetActive(false);
            PinE.SetActive(false);
        }
        if (type != MenuType.Page)
        {
            Initialization();
        }
        if (buttonName == "Drving_forword")
        {
            PlayerPrefs.SetInt("CurrentScene", 1);
            if (ArduinoData.Instance.gearUp.Equals(1) && ArduinoData.Instance.gearDown.Equals(1) && !ArduinoData.Instance.safebelt && !ArduinoData.Instance.safebar && !ArduinoData.Instance.safepin)
            {
                //주행 전진.
                PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
                _Steering.SpringForceOff();

                if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
                {
                    SimgLauncherUnity.Instance.ToJson(2, 0, "주행연습시작");
                }
                SceneManager.LoadScene("ViveDrive");
                return;
            }
            ArdUI();
            RPM.SetActive(false);
        }
        else if (buttonName == "Driving_back")
        {
            PlayerPrefs.SetInt("CurrentScene", 2);
            if (ArduinoData.Instance.gearUp.Equals(1) && ArduinoData.Instance.gearDown.Equals(1) && ArduinoData.Instance.safebelt && ArduinoData.Instance.safebar && ArduinoData.Instance.safepin)
            {
                //주행 후진.
                PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
                _Steering.SpringForceOff();

                if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
                {
                    SimgLauncherUnity.Instance.ToJson(2, 0, "주행연습시작");
                }
                SceneManager.LoadScene("ViveDrive");

                return;
            }
            ArdUIE();
            ArdUI();
            RPM.SetActive(false);
        }
        else if (buttonName == "DrivingTest")
        {
            PlayerPrefs.SetInt("CurrentScene", 1);
            if (ArduinoData.Instance.gearUp.Equals(1) && ArduinoData.Instance.gearDown.Equals(1) && !ArduinoData.Instance.safebelt && !ArduinoData.Instance.safebar && !ArduinoData.Instance.safepin)
            {
                //주행 시험.
                PlayerPrefs.SetInt("Tutorial", 0); //0-시험, 1-연습모드

                if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
                {
                    SimgLauncherUnity.Instance.ToJson(2, 1, "주행시험시작");
                }
                SceneManager.LoadScene("ViveDrive");

                return;
            }
            ArdUI();
            RPM.SetActive(false);
        }

        else if (buttonName == "DiggingExercise")
        {
            PlayerPrefs.SetInt("CurrentScene", 0);
            if (ArduinoData.Instance.rpm.Equals(255) && ArduinoData.Instance.gearUp.Equals(1) && ArduinoData.Instance.gearDown.Equals(1) && !ArduinoData.Instance.safebelt && !ArduinoData.Instance.safebar && !ArduinoData.Instance.safepin)
            {
                //굴삭 테스트.
                PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
                _Steering.SpringForceOff();

                if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
                {
                    SimgLauncherUnity.Instance.ToJson(2, 2, "굴착연습시작");
                }
                SceneManager.LoadScene("ViveDig");
                return;
            }
            ArdUI();
        }
        else if (buttonName == "DiggingTest")
        {
            if (ArduinoData.Instance.rpm.Equals(255) && ArduinoData.Instance.gearUp.Equals(1) && ArduinoData.Instance.gearDown.Equals(1) && !ArduinoData.Instance.safebelt && !ArduinoData.Instance.safebar && !ArduinoData.Instance.safepin)
            {
                //굴삭 시험.
                PlayerPrefs.SetInt("Tutorial", 0); //0-시험, 1-연습모드
                _Steering.SpringForceOff();

                if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
                {
                    SimgLauncherUnity.Instance.ToJson(2, 3, "굴착시험시작");
                }
                SceneManager.LoadScene("ViveDig");
                return;
            }
            ArdUI();
        }
        else if (buttonName == "<")
        {
            rule.GetComponent<RegulationTouchPanel>().PageM();
        }
        else if (buttonName == ">")
        {
            rule.GetComponent<RegulationTouchPanel>().PageP();
        }
        if (type == MenuType.Accident)
        {
            if (ArduinoData.Instance.rpm.Equals(255) && ArduinoData.Instance.gearUp.Equals(1) && ArduinoData.Instance.gearDown.Equals(1) && !ArduinoData.Instance.safebelt && !ArduinoData.Instance.safebar && !ArduinoData.Instance.safepin)
            {
                SceneManager.LoadScene("ViveFlatness");
                
                return;
            }
            ArdUI();
            buttons[0].SetActive(true);
        }
        if (type == MenuType.Main)
        {
            RPM.SetActive(false);
            Gear.SetActive(false);
            Belt.SetActive(false);
            Safe.SetActive(false);
            Pin.SetActive(false);
            BeltE.SetActive(false);
            SafeE.SetActive(false);
            PinE.SetActive(false);
            for (int i = 0; i < roomPlaneControl.objs.Length; i++)
            {
                roomPlaneControl.objs[i].SetActive(false);
            }

            roomPlaneControl.objs[0].SetActive(true);
            roomPlaneControl.objs[0].GetComponent<Animation>().Play();
            roomPlaneControl.toAngle = 0;
        }
        if (type == MenuType.DriveMode)
        {
            if (buttonName == "Back")
            { 
            RPM.SetActive(false);
            Gear.SetActive(false);
            Belt.SetActive(false);
            Safe.SetActive(false);
            Pin.SetActive(false);
            BeltE.SetActive(false);
            SafeE.SetActive(false);
            PinE.SetActive(false); }
            for (int i = 0; i < roomPlaneControl.objs.Length; i++)
            {
                roomPlaneControl.objs[i].SetActive(false);
            }

            roomPlaneControl.objs[1].SetActive(true);

            roomPlaneControl.objs[1].GetComponent<Animation>().Play();
            roomPlaneControl.toAngle = 90;
        }
        else if (type == MenuType.DigMode)
        {
            for (int i = 0; i < roomPlaneControl.objs.Length; i++)
            {
                roomPlaneControl.objs[i].SetActive(false);
            }

            roomPlaneControl.objs[2].SetActive(true);
            roomPlaneControl.objs[2].GetComponent<Animation>().Play();
            roomPlaneControl.toAngle = -90;
        }
        else if (type == MenuType.Regulation)
        {
            buttons[5].SetActive(true);
            //buttons[6].SetActive(true);
        }
        if (type != MenuType.Page)
        {
            buttons[(int)type].SetActive(true);
        }
    }

    void Initialization()
    {
        for (int i = 0; i < buttons.Length - 1; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    void ArdUI()
    {
        if (!ArduinoData.Instance.rpm.Equals(255))
        {
            RPM.SetActive(true);
        }
        if (!ArduinoData.Instance.gearUp.Equals(1) || !ArduinoData.Instance.gearDown.Equals(1))
        {
            Gear.SetActive(true);
        }
        if (ArduinoData.Instance.safebelt)
        {
            Belt.SetActive(true);
        }
        if (ArduinoData.Instance.safebar)
        {
            Safe.SetActive(true);
        }
        if (ArduinoData.Instance.safepin)
        {
            Pin.SetActive(true);
        }
    }
    void ArdUIE()
    {
        if (!ArduinoData.Instance.safebelt)
        {
            BeltE.SetActive(true);
        }
        if (!ArduinoData.Instance.safebar)
        {
            SafeE.SetActive(true);
        }
        if (!ArduinoData.Instance.safepin)
        {
            PinE.SetActive(true);
        }
        if (!ArduinoData.Instance.gearUp.Equals(1) || !ArduinoData.Instance.gearDown.Equals(1))
        {
            Gear.SetActive(true);
        }
    }
}