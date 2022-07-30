using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class ViveMenuScene : MonoBehaviour {

    private const int STEP_MENU = 0;
    private const int STEP_DRIVE = 1;
    private const int STEP_DIG = 2;
    private const int STEP_DRIVE_SELECT = 3;

    private const int IMG_OFF_DRIVE = 0;
    private const int IMG_ON_DRIVE = 1;
    private const int IMG_OFF_DRIVE_EX = 2;
    private const int IMG_ON_DRIVE_EX = 3;
    private const int IMG_OFF_DRIVE_TEST = 4;
    private const int IMG_ON_DRIVE_TEST = 5;

    private const int IMG_OFF_DIG = 0;
    private const int IMG_ON_DIG = 1;
    private const int IMG_OFF_DIG_EX = 2;
    private const int IMG_ON_DIG_EX = 3;
    private const int IMG_OFF_DIG_TEST = 4;
    private const int IMG_ON_DIG_TEST = 5;

    private const int IMG_OFF = 0;
    private const int IMG_ON = 1;

    private const int SUBMENU_DRIVE = 0;
    private const int SUBMENU_DIG = 1;
    private const int SUBMENU_DRIVE_SELECT = 2;

    public GameObject[] goSubMenu;

    public RawImage imgBtnDriveing;
    public RawImage imgBtnDigging;

    public RawImage imgBtnDrivingEx;
    public RawImage imgBtnDrivingTest;

    public RawImage imgBtnDriveMode1;
    public RawImage imgBtnDriveMode2;

    public RawImage imgBtnDiggingEx;
    public RawImage imgBtnDiggingTest;

    public Texture[] texBtnDriving;
    public Texture[] texBtnDigging;

    public Texture[] texBtnDriveSelect1; //전진, 후진 
    public Texture[] texBtnDriveSelect2; //후진

    float handleValue = 0;
    string nextSceneName = "";

    int step = 0;
    int tmpStep = 0;

    int inputAccelerateState = 0;

    void Start () {
        Option.Instance.Load();
        DeviceSound.Instance.source_bgm.clip = Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
        DeviceSound.Instance.source_bgm.Play();

        changeStep(STEP_MENU);
	}
	
	void Update () {
        
        if (VRSettings.isDeviceActive)
        {
            handleValue = Mathf.Clamp(Input.GetAxisRaw("Handle"), -1, 1);
        }
        else
        {
            handleValue = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
        }

        switch (step) {
            case STEP_MENU:
                if (handleValue < 0) //left
                {
                    imgBtnDriveing.texture = texBtnDriving[IMG_ON_DRIVE];
                    imgBtnDigging.texture = texBtnDigging[IMG_OFF_DIG];

                    tmpStep = STEP_DRIVE;
                }
                else if (handleValue > 0) //right
                {
                    imgBtnDriveing.texture = texBtnDriving[IMG_OFF_DRIVE];
                    imgBtnDigging.texture = texBtnDigging[IMG_ON_DIG];

                    tmpStep = STEP_DIG;
                }

                if (VRSettings.isDeviceActive && Input.GetAxisRaw("Accelerate") == 1f && tmpStep != STEP_MENU)
                {
                    inputAccelerateState = 1;
                    changeStep(tmpStep);
                }
                else if (VRSettings.isDeviceActive == false && Input.GetButtonDown("Fire1") && tmpStep != STEP_MENU)
                {
                    inputAccelerateState = 1;
                    changeStep(tmpStep);
                }
                break;
            case STEP_DRIVE:

                if (handleValue < 0) //주행 연습
                {
                    imgBtnDrivingEx.texture = texBtnDriving[IMG_ON_DRIVE_EX];
                    imgBtnDrivingTest.texture = texBtnDriving[IMG_OFF_DRIVE_TEST];

                    nextSceneName = "ViveDrive";

                    PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
                }
                else if (handleValue > 0) //주행 시험
                {
                    imgBtnDrivingEx.texture = texBtnDriving[IMG_OFF_DRIVE_EX];
                    imgBtnDrivingTest.texture = texBtnDriving[IMG_ON_DRIVE_TEST];

                    nextSceneName = "ViveDrive";

                    PlayerPrefs.SetInt("Tutorial", 0); //0-시험, 1-연습모드
                }

                if (inputAccelerateState == 1)
                {
                    if (Input.GetAxisRaw("Accelerate") < 0.5f)
                        inputAccelerateState = 0;
                    return;
                }


                if (VRSettings.isDeviceActive && Input.GetAxisRaw("Accelerate") == 1f && !nextSceneName.Equals(""))
                {
                    changeScene();
                }
                else if (VRSettings.isDeviceActive == false && Input.GetButtonDown("Fire1") && !nextSceneName.Equals(""))
                {
                    changeScene();
                }
                break;
            case STEP_DRIVE_SELECT:
                if (handleValue < 0) //전진 후진
                {
                    imgBtnDriveMode1.texture = texBtnDriveSelect1[IMG_ON];
                    imgBtnDriveMode2.texture = texBtnDriveSelect2[IMG_OFF];

                    PlayerPrefs.SetInt("CurrentScene", 1);

                    //nextSceneName = "ViveAccident";
                    nextSceneName = "ViveDrive";
                }
                else if (handleValue > 0) //후진
                {
                    imgBtnDriveMode1.texture = texBtnDriveSelect1[IMG_OFF];
                    imgBtnDriveMode2.texture = texBtnDriveSelect2[IMG_ON];

                    PlayerPrefs.SetInt("CurrentScene", 2);

                    //nextSceneName = "ViveAccident";
                    nextSceneName = "ViveDrive";
                }

                if (inputAccelerateState == 1)
                {
                    if (Input.GetAxisRaw("Accelerate") < 0.5f)
                        inputAccelerateState = 0;
                    return;
                }


                if (VRSettings.isDeviceActive && Input.GetAxisRaw("Accelerate") == 1f && !nextSceneName.Equals(""))
                {
                    changeScene();
                }
                else if (VRSettings.isDeviceActive == false && Input.GetButtonDown("Fire1") && !nextSceneName.Equals(""))
                {
                    changeScene();
                }                
                break;
            case STEP_DIG:
                if (handleValue < 0) //작업 연습
                {
                    imgBtnDiggingEx.texture = texBtnDigging[IMG_ON_DIG_EX]; 
                    imgBtnDiggingTest.texture = texBtnDigging[IMG_OFF_DIG_TEST];

                    //nextSceneName = "ViveAccident";
                    nextSceneName = "ViveDig";

                    PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
                    PlayerPrefs.SetInt("CurrentScene", 0);
                }
                else if (handleValue > 0) //작업 시헙
                {
                    imgBtnDiggingEx.texture = texBtnDigging[IMG_OFF_DIG_EX];
                    imgBtnDiggingTest.texture = texBtnDigging[IMG_ON_DIG_TEST];

                    nextSceneName = "ViveDig";

                    PlayerPrefs.SetInt("Tutorial", 0); //0-시험, 1-연습모드
                }

                if (inputAccelerateState == 1)
                {
                    if (Input.GetAxisRaw("Accelerate") < 0.5f)
                        inputAccelerateState = 0;
                    return;
                }

                if (VRSettings.isDeviceActive && Input.GetAxisRaw("Accelerate") == 1f && !nextSceneName.Equals(""))
                {
                    changeScene();
                }
                else if (VRSettings.isDeviceActive == false && Input.GetButtonDown("Fire1") && !nextSceneName.Equals(""))
                {
                    changeScene();
                }
                break;
        }        
    }

    void changeScene()
    {
        if (nextSceneName.Equals("ViveDrive") && step == STEP_DRIVE && PlayerPrefs.GetInt("Tutorial") == 1)
        {
            changeStep(STEP_DRIVE_SELECT);
            return;
        }

        Debug.Log("Next Scene=" + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

    void changeStep(int nextStep)
    {
        handleValue = 0;

        step = nextStep;
        tmpStep = 0;
        nextSceneName = "";

        Debug.Log("Next Step=" + step);

        switch (step)
        {
            case STEP_MENU:

                imgBtnDriveing.transform.parent.gameObject.SetActive(true);
                imgBtnDigging.transform.parent.gameObject.SetActive(true);

                for (int i = 0; i < goSubMenu.Length; i++) {
                    goSubMenu[i].SetActive(false);
                }

                //주행
                imgBtnDrivingEx.texture = texBtnDriving[IMG_OFF_DRIVE_EX];
                imgBtnDrivingTest.texture = texBtnDriving[IMG_OFF_DRIVE_TEST];

                //굴삭
                imgBtnDiggingEx.texture = texBtnDigging[IMG_OFF_DIG_EX];
                imgBtnDiggingTest.texture = texBtnDigging[IMG_OFF_DRIVE_TEST];

                break;
            case STEP_DRIVE:
                imgBtnDriveing.transform.parent.gameObject.SetActive(true);
                imgBtnDigging.transform.parent.gameObject.SetActive(false);

                //주행
                imgBtnDriveing.texture = texBtnDriving[IMG_OFF_DRIVE];
                imgBtnDrivingEx.texture = texBtnDriving[IMG_OFF_DRIVE_EX];
                imgBtnDrivingTest.texture = texBtnDriving[IMG_OFF_DRIVE_TEST];

                goSubMenu[SUBMENU_DRIVE].SetActive(true);
                goSubMenu[SUBMENU_DIG].SetActive(false);
                goSubMenu[SUBMENU_DRIVE_SELECT].SetActive(false);
                break;
            case STEP_DRIVE_SELECT:
                goSubMenu[SUBMENU_DRIVE_SELECT].SetActive(true);

                imgBtnDriveMode1.texture = texBtnDriveSelect1[IMG_OFF];
                imgBtnDriveMode2.texture = texBtnDriveSelect2[IMG_OFF];

                break;
            case STEP_DIG:
                imgBtnDriveing.transform.parent.gameObject.SetActive(false);
                imgBtnDigging.transform.parent.gameObject.SetActive(true);

                //굴삭
                imgBtnDigging.texture = texBtnDigging[IMG_OFF_DRIVE];
                imgBtnDiggingEx.texture = texBtnDigging[IMG_OFF_DIG_EX];
                imgBtnDiggingTest.texture = texBtnDigging[IMG_OFF_DRIVE_TEST];

                goSubMenu[SUBMENU_DRIVE].SetActive(false);
                goSubMenu[SUBMENU_DIG].SetActive(true);
                goSubMenu[SUBMENU_DRIVE_SELECT].SetActive(false);
                break;
        }
    }

}
