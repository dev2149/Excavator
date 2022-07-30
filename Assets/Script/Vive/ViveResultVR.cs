using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;

public class ViveResultVR : MonoBehaviour {

    private const int BTN_OFF = 0;
    private const int BTN_ON = 1;

    public GameObject success;
    public GameObject fail;
    public GameObject mistake;
    public Text FoulLabel;
    //public UIKeyBinding keybinding;
    public AudioClip sound_success;
    public AudioClip sound_fail;
    public AudioSource diesel;
    public AudioSource audio_;
    public Text label;

    public digMap map;
    private bool result = false;

    public RawImage imgBtn_Reset;
    public RawImage imgBtn_End;

    public Texture[] texBtnReset;
    public Texture[] texBtnEnd;


    float handleValue = 0;
    string nextSceneName = "";

    //저장 관련
    private SaveData data;
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();

    void Start ()
    {
        // imgBtn_Reset.texture = texBtnReset[BTN_OFF];
        //imgBtn_End.texture = texBtnEnd[BTN_OFF];
    }
	
	void Update () {

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

            nextSceneName = "ViveDig";
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
        //SceneManager.LoadScene(nextSceneName);

        if (nextSceneName.Equals("ViveDig"))
        {
            OnReset(); 
        }
        else if (nextSceneName.Equals("ViveMenu"))
        {
            OnEnd();
        }
    }

    void OnEnable()
    {
     //   Time.timeScale = 0.0f;

        data = SaveData.Load(Application.streamingAssetsPath + "\\" + PlayerPrefs.GetString("UserID") + ".xml");

        lLog = data.GetValue<List<string>>(user.Log);
        lContent = data.GetValue<List<string>>(user.Content);
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }


    IEnumerator HMD_END()
    {
        yield return new WaitForSeconds(5);

        if (PlayerPrefs.GetInt("selected_Mode") == 0)
        {
            SceneManager.LoadScene("ViveMenu");
        }
    }


    void Save()
    {
        lLog.Add("- 굴삭시험 -  " + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
        lContent.Add(label.text);
        data[user.Log] = lLog;
        data[user.Content] = lContent;
        data.Save();
    }

    public void OnOpen(bool success_)
    {
        diesel.loop = false;
        diesel.Stop();

        result = success_;

        DeviceSound.Instance.AudioPuase();

        if (result) // 감점은 있을 수 있다.
        {
            success.SetActive(true);
            fail.SetActive(false);
            mistake.SetActive(false);
            NGUITools.PlaySound(sound_success, DeviceSound.Instance.Effect);
            GuideSound("DriveTest/success", true);

            if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
            {
                SimgLauncherUnity.Instance.ToJson(2, 3, "합격"); // 굴착 시험
            }
            if (ViveDigScene.instance.IsVR() == false)
            {
                if (map.mistake < 1)
                    label.text = "[0000ff]Perfect!![-]";
                else
                    label.text = "[ff0000]감점 " + map.mistake + "번[-]";
            }
            else
            {
                if (map.mistake < 1)
                    label.text = "<color=#0000ff>Perfect!!</color>";
                else
                    label.text = "<color=#ff0000>감점 " + map.mistake + "번</color>";
            }
            //Save();
        }
        else // 감점또는 실격 감점과다.
        {
            success.SetActive(false);
            NGUITools.PlaySound(sound_fail, DeviceSound.Instance.Effect);
            GuideSound("DriveTest/fail", true);

            if (ViveDigScene.instance.IsVR() == false)
            {
                if (map.mistake > 4)
                {
                    label.text = "[ff0000]감점과다[-]";
                }
                else
                {
                    //label.text += "[ff0000]실격[-]";

                    label.text = Regex.Replace(label.text, "\n", String.Empty);

                    label.text = label.text.Substring(17);
                }
            }
            else
            {
                if (map.mistake > 4)
                {
                    //label.text = "<color=#ff0000>감점과다</color>";
                    //FoulLabel.text = "▶케빈 흔들림: " + map.Foul[0] + "번 ▶작업영역 이탈: " + map.Foul[1] + "번 \n▶흙덩이 낙하: " + map.Foul[2] + "번 ▶과부하: " + map.Foul[3] + "번\n▶RPM 수치 비정상 : " + map.Foul[4] + "번";
                    mistake.SetActive(true);
                }
                else
                {
                    fail.SetActive(true);
                }
            }

            //Save();
        }

        //StartCoroutine(HMD_END());      //HMD 버젼일때만 자동 종료

        //keybinding.enabled = false;
    }


    public void OnEnd()
    {
        DeviceVibrate.Play();
        DeviceSound.Instance.source_bgm.Stop();

        SceneManager.LoadScene("ViveMenu");


        DeviceSound.Instance.source_bgm.clip = Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
        DeviceSound.Instance.source_bgm.Play();

    }


    public void OnReset()
    {
        DeviceVibrate.Play();
        DeviceSound.Instance.source_bgm.Stop();

        SceneManager.LoadScene("ViveDig");
    }


    void GuideSound(string str, bool on)
    {
        if (ViveDigScene.instance.IsVR())
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
