using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDriveTutorial : MonoBehaviour
{
    public UIKeyBinding keybinding;
    public AudioClip sound_success;
    public AudioClip sound_fail;

    public UILabel FoulLabel;
    public UILabel Encourage;

    public AudioSource audio_;
    public AudioSource diesel;

    //저장 관련
    private SaveData data;
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();

    void OnEnable() //일시정지.
    {
        DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
        Time.timeScale = 0.0f;

        //활성화시 데이터 로드
        data = SaveData.Load(Application.streamingAssetsPath + "\\" + PlayerPrefs.GetString("UserID") + ".xml");
        lLog = data.GetValue<List<string>>(user.Log);
        lContent = data.GetValue<List<string>>(user.Content);
    }

    void OnDisable()
    {
        DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
        Time.timeScale = 1.0f;
    }

    void Save()
    {
        lLog.Add("- 주행연습 -  " + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
        lContent.Add(FoulLabel.text);
        data[user.Log] = lLog;
        data[user.Content] = lContent;
        data.Save();
    }

    public void OnOpen(int int_collision) //drive
    {
        DeviceSound.Instance.AudioPuase();
        diesel.Stop();
        if (int_collision >= 3.0f)
        { 
            //success
            NGUITools.PlaySound(sound_success, DeviceSound.Instance.Effect);
            FoulLabel.text += "콘에 부딪힌 횟수: " + int_collision + "번";
            Encourage.text += "포기하지 마시고 끝까지 노력해주세요.";
            GuideSound("DigResult/Result_002", true);

            Save();
        }
        else
        {

            if (int_collision == 0)
            {
                NGUITools.PlaySound(sound_success, DeviceSound.Instance.Effect);
                FoulLabel.text += "콘에 부딪힌 횟수: " + int_collision + "번";
                Encourage.text += "시험모드에 도전 하셔도 되겠어요!";
                GuideSound("DigResult/Result_003", true);

                Save();
            }
            else
            {
                NGUITools.PlaySound(sound_fail, DeviceSound.Instance.Effect);
                FoulLabel.text += "콘에 부딪힌 횟수: " + int_collision + "번";
                Encourage.text += "거의 합격인데… 조금 더 노력해주세요.";
                GuideSound("DigResult/Result_004", true);

                Save();
            }
        }

        keybinding.enabled = false;
    }

    public void OnEnd_drive()
    {
        DeviceVibrate.Play();
        DeviceSound.Instance.source_bgm.Stop();

        if (ViveDriveScene.instance.IsVR() == false)
        {
            Application.LoadLevel("drive_mode");
        }
        else
        {
            Application.LoadLevel("ViveMenu");
        }


        DeviceSound.Instance.source_bgm.clip = Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
        DeviceSound.Instance.source_bgm.Play();
    }

    public void OnReset()
    {
        DeviceVibrate.Play();
        DeviceSound.Instance.source_bgm.Stop();
        if (ViveDriveScene.instance.IsVR() == false)
        {
            Application.LoadLevel("drive");
        }
        else
        {
            Application.LoadLevel("ViveDrive");
        }
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
