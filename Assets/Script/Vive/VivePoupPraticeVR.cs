using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;

public class VivePoupPraticeVR : MonoBehaviour {

    private const int BTN_OFF = 0;
    private const int BTN_ON = 1;
    public RawImage imgBtn_Start;
    public RawImage imgBtn_BackStart;

    public Texture[] texBtnStart;
    public Texture[] texBtnBackStart;


    float handleValue = 0;
    string nextSceneName = "";


    void OnEnable()
    {
        DeviceSound.Instance.TimePause = true;

       // Time.timeScale = 0.0f;
    }

    //void OnDisable()
    //{
    //    DeviceSound.Instance.TimePause = false;

    //    Time.timeScale = 1.0f;
    //}

    void Start()
    {
       // imgBtn_Start.texture = texBtnStart[BTN_OFF];
        //imgBtn_BackStart.texture = texBtnBackStart[BTN_OFF];
    }

    void Update()
    {
        return;

        if (VRSettings.isDeviceActive)
        {
            handleValue = Mathf.Clamp(Input.GetAxisRaw("Handle"), -1, 1);
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                handleValue = -1;
            else if (Input.GetKey(KeyCode.RightArrow))
                handleValue = 1;
        }


        if (handleValue < 0) //left
        {
            imgBtn_Start.texture = texBtnStart[BTN_ON];
            imgBtn_BackStart.texture = texBtnBackStart[BTN_OFF];

            nextSceneName = "ViveDriveStart";
        }
        else if (handleValue > 0) //right
        {
            imgBtn_Start.texture = texBtnStart[BTN_OFF];
            imgBtn_BackStart.texture = texBtnBackStart[BTN_ON];

            nextSceneName = "ViveDriveBackStart";
        }

        if (VRSettings.isDeviceActive && Input.GetAxisRaw("Accelerate") == 1f && !nextSceneName.Equals(""))
        {
            changeScene();
        }
        else if (VRSettings.isDeviceActive == false && Input.GetButtonDown("Fire1") && !nextSceneName.Equals(""))
        {
            changeScene();
        }
    }

    void changeScene()
    {
        Debug.Log("Next Scene=" + nextSceneName);

        if (nextSceneName.Equals("ViveDriveStart"))
        {
            OnPraticeStartAtStart();
        }
        else if (nextSceneName.Equals("ViveDriveBackStart"))
        {
            OnPraticeStartAtEnd();
        }
    }

    public void OnPraticeStartAtStart()
    {
        Driving.tutorial = true;
        Driving.start_at_end = false;

        DeviceSound.Instance.TimePause = false;
        Time.timeScale = 1.0f;

        PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
        PlayerPrefs.SetInt("CurrentScene", 1);

        SceneManager.LoadScene("ViveDrive");
    }

    public void OnPraticeStartAtEnd()
    {
        Driving.tutorial = true;
        Driving.start_at_end = true;

        DeviceSound.Instance.TimePause = false;
        Time.timeScale = 1.0f;

        PlayerPrefs.SetInt("Tutorial", 1); //0-시험, 1-연습모드
        PlayerPrefs.SetInt("CurrentScene", 2);

        SceneManager.LoadScene("ViveDrive");

    }

    public void GoDrive()
    {
        if (PlayerPrefs.GetInt("Gear") == 0)
        {
            DeviceSound.Instance.TimePause = false;
            Time.timeScale = 1.0f;

            SceneManager.LoadScene("ViveDrive");

        }
    }

    public void OnEnd()
    {
        DeviceVibrate.Play();
        SceneManager.LoadScene("ViveMenu");

        DeviceSound.Instance.source_bgm.clip = Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
        DeviceSound.Instance.source_bgm.Play();

    }
}
