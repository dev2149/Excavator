using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;

public class ViveResultVR2 : MonoBehaviour {

    private const int BTN_OFF = 0;
    private const int BTN_ON = 1;

    public GameObject success;
    public GameObject fail;

    public AudioClip sound_success;
    public AudioClip sound_fail;
    public AudioSource diesel;
    public AudioSource audio_;
    public Text mistake_label;

    private bool result = false;
    public DriveMap map;

    private SaveData data;
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();

    public RawImage imgBtn_Reset;
    public RawImage imgBtn_End;

    public Texture[] texBtnReset;
    public Texture[] texBtnEnd;


    float handleValue = 0;
    string nextSceneName = "";

    void OnEnable()
    {
        DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
    }

    void OnDisable()
    {
        DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
        Time.timeScale = 1.0f;
    }


    IEnumerator HMD_END()
    {
        Debug.Log(PlayerPrefs.GetInt("selected_Mode"));
        yield return new WaitForSeconds(5);

        if (PlayerPrefs.GetInt("selected_Mode") == 0)
        {
            SceneManager.LoadScene("ViveMenu");
        }
    }

    void Start()
    {
        //imgBtn_Reset.texture = texBtnReset[BTN_OFF];
        //imgBtn_End.texture = texBtnEnd[BTN_OFF];
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
            imgBtn_Reset.texture = texBtnReset[BTN_ON];
            imgBtn_End.texture = texBtnEnd[BTN_OFF];

            nextSceneName = "ViveDrive";
        }
        else if (handleValue > 0) //right
        {
            imgBtn_Reset.texture = texBtnReset[BTN_OFF];
            imgBtn_End.texture = texBtnEnd[BTN_ON];

            nextSceneName = "ViveMenu";
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

        if (nextSceneName.Equals("ViveDrive"))
        {
            OnReset();
        }
        else if (nextSceneName.Equals("ViveMenu"))
        {
            OnEnd();
        }
    }

    void Save()
    {
        //lLog.Add("- 주행시험 -  " + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
        //lContent.Add(mistake_label.text);
        //data[user.Log] = lLog;
        //data[user.Content] = lContent;
        //data.Save();
    }


    public void OnOpen(bool _success)
    {
        diesel.GetComponent<AudioSource>().loop = false;
        diesel.GetComponent<AudioSource>().Stop();
        DeviceSound.Instance.AudioPuase();
        result = _success;

        if (result)
        {
            if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
            {
                SimgLauncherUnity.Instance.ToJson(2, 1, "합격"); //주행 시험
            }
            success.SetActive(true);
            fail.SetActive(false);

            NGUITools.PlaySound(sound_success, DeviceSound.Instance.Effect - 0.5f);
            GuideSound("DriveTest/success", true);

            if (map.TestResultIndex == 3) { mistake_label.text = "<color=#ff0000>미정지 -5점</color>"; }
            else { mistake_label.text = "<color=#0000ff>Perfect!!</color>"; }

            // Save();
        }
        else
        {

            success.SetActive(false);
            fail.SetActive(true);
            NGUITools.PlaySound(sound_fail, DeviceSound.Instance.Effect - 0.5f);
            GuideSound("DriveTest/fail", true);

            switch (map.TestResultIndex)
            {
                case 17:
                    mistake_label.text = "주차브레이크를 해제하지 않았습니다.";
                    Save();
                    break;
                case 16:
                    mistake_label.text = "안전레버를 작동하지 않았습니다.";
                    Save();
                    break;
                case 11:
                    mistake_label.text = "오른쪽 앞바퀴가 주행선을 밟았습니다.";
                    Save();
                    break;
                case 12:
                    mistake_label.text = "왼쪽 앞바퀴가 주행선을 밟았습니다.";
                    Save();
                    break;
                case 13:
                    mistake_label.text = "오른쪽 뒷바퀴가 주행선을 밟았습니다.";
                    Save();
                    break;
                case 14:
                    mistake_label.text = "왼쪽 뒷바퀴가 주행선을 밟았습니다.";
                    Save();
                    break;
                case 2:
                    mistake_label.text = "시험 시간이 종료되었습니다.";
                    Save();
                    break;
                case 3:
                    mistake_label.text = "정지선을 넘었습니다.";
                    Save();
                    break;
                case 4:
                    mistake_label.text = "뒷바퀴가 라인을 넘지 못했습니다.";
                    Save();
                    break;
                case 5:
                    mistake_label.text = "시험장을 벗어났습니다.";
                    Save();
                    break;
                case 6:
                    mistake_label.text = "출발선에 들어서지 않았습니다.";
                    Save();
                    break;
            }

        }

        //StartCoroutine(HMD_END());      //HMD 버젼일때만 자동 종료
    }

    void SetTime(UILabel label, float time)
    {
        int minute = Mathf.FloorToInt(time / 60.0f);
        int second = Mathf.FloorToInt(time) % 60;
        int msecond = Mathf.FloorToInt(time * 100) % 100;
        label.text = string.Format("{0:00}:{1:00}:{2:00}", minute, second, msecond);
    }


    public void OnEnd()
    {
        // DeviceVibrate.Play();
        DeviceSound.Instance.source_bgm.Stop();
        SceneManager.LoadScene("ViveMenu");

        DeviceSound.Instance.source_bgm.clip = Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
        DeviceSound.Instance.source_bgm.Play();
    }

    public void OnReset()
    {
        // DeviceVibrate.Play();
        DeviceSound.Instance.source_bgm.Stop();
        SceneManager.LoadScene("ViveDrive");
    }

    void GuideSound(string str, bool on)
    {
        if (ViveDriveScene.instance.IsVR())
        {
            DeviceSound.Instance.Play(Resources.Load(str.ToString()) as AudioClip, DeviceSound.Instance.Effect);
            return;
        }

        if (on)
        {
            if (!audio_.isPlaying)
            {
                audio_.clip = Resources.Load(str.ToString()) as AudioClip;
                audio_.Play();
            }
            on = false;
        }
    }

}

