using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDriveResult : MonoBehaviour
{
    public GameObject success;
    public GameObject fail;
    public UIKeyBinding keybinding;
    public AudioClip sound_success;
    public AudioClip sound_fail;
    public AudioSource diesel;
    public AudioSource audio_;
    public UILabel mistake_label;

    private bool result = false;
    public DriveMap map;

    //???愿??
    private SaveData data;
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();

    void OnEnable() //?쇱떆?뺤?.
    {
        //DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
        DeviceSound.Instance.TimePause = true;
        //  Time.timeScale = 0.0f;

        // data = SaveData.Load(Application.streamingAssetsPath + "\\" + PlayerPrefs.GetString("UserID") + ".xml");

        // lLog = data.GetValue<List<string>>(user.Log);
        // lContent = data.GetValue<List<string>>(user.Content);
    }

    void OnDisable()
    {
        DeviceSound.Instance.TimePause = false;
        //DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
        Time.timeScale = 1.0f;
    }


    IEnumerator HMD_END()
    {
        Debug.Log(PlayerPrefs.GetInt("selected_Mode"));
        yield return new WaitForSeconds(5);
        Debug.Log(PlayerPrefs.GetInt("aaaaaaaaaaaa"));
        if (PlayerPrefs.GetInt("selected_Mode") == 0)
        {
            if (Application.loadedLevelName == "dig")
            {
                Application.LoadLevel("dig_mode");
            }
            else if (Application.loadedLevelName == "drive")
            {
                Application.LoadLevel("drive_mode");
                Debug.Log(PlayerPrefs.GetInt("aaaaaaaaaaaa"));
            }
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
            success.SetActive(true);
            fail.SetActive(false);

            NGUITools.PlaySound(sound_success, DeviceSound.Instance.Effect - 0.5f); //寃곌낵 ?④낵??
            GuideSound("DriveTest/success", true);// 寃곌낵???곕Ⅸ ?섎젅?댁뀡

            if (map.TestResultIndex == 3) { mistake_label.text = "[ff0000]미정지 -5점[-]"; } 
            else { mistake_label.text = "[0000ff]Perfect!![-]"; }

           // Save();
        }
        else
        {

            success.SetActive(false);
            fail.SetActive(true);
            NGUITools.PlaySound(sound_fail, DeviceSound.Instance.Effect - 0.5f);
            GuideSound("DriveTest/fail", true);

            //?ㅽ뙣?먯씤???쒗뿕 寃곌낵李쎌뿉 蹂댁뿬以??
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

        StartCoroutine(HMD_END());      //HMD 버젼일때만 자동 종료

        keybinding.enabled = false;
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
        Application.LoadLevel("drive_mode");

        DeviceSound.Instance.source_bgm.clip = Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
        DeviceSound.Instance.source_bgm.Play();
    }

    public void OnReset()
    {
       // DeviceVibrate.Play();
        DeviceSound.Instance.source_bgm.Stop();
        Application.LoadLevel("drive");
    }

    void GuideSound(string str, bool on)
    {
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
