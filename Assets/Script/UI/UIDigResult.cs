using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public class UIDigResult : MonoBehaviour 
{
	public GameObject success;
	public GameObject fail;
	public UIKeyBinding keybinding;
    public AudioClip sound_success;
    public AudioClip sound_fail;
	public AudioSource diesel;
	public AudioSource audio_;
	public UILabel label;

	public digMap map;
	private bool result = false;

    //저장 관련
    private SaveData data;
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();

	void OnEnable()
	{
		Time.timeScale = 0.0f;

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

        if (ViveDigScene.instance.IsVR() == false)
        {
            if (PlayerPrefs.GetInt("selected_Mode") == 0)
            {
                if (Application.loadedLevelName == "dig")
                {
                    Application.LoadLevel("dig_mode");
                }
                else if (Application.loadedLevelName == "drive")
                {
                    Application.LoadLevel("drive_mode");
                }
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("selected_Mode") == 0)
            {
                if (Application.loadedLevelName == "dig")
                {
                    Application.LoadLevel("ViveMenu");
                }
                else if (Application.loadedLevelName == "drive")
                {
                    Application.LoadLevel("ViveMenu");
                }
            }
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
		diesel.Stop ();

		result = success_;

		DeviceSound.Instance.AudioPuase ();

		if (result) // 감점은 있을 수 있다.
		{
			success.SetActive(true);
			fail.SetActive(false);
			NGUITools.PlaySound (sound_success,DeviceSound.Instance.Effect);
			GuideSound("DriveTest/success",true);

            if (map.mistake < 1)
				label.text = "[0000ff]Perfect!![-]";
			else
				label.text = "[ff0000]감점 " + map.mistake + "번[-]";

            Save();
		}
		else // 감점또는 실격 감점과다.
		{
			success.SetActive(false);
			fail.SetActive(true);
			NGUITools.PlaySound (sound_fail,DeviceSound.Instance.Effect);		
			GuideSound("DriveTest/fail",true);

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

            Save();
        }

        //StartCoroutine(HMD_END());      //HMD 버젼일때만 자동 종료

        keybinding.enabled = false;
	}


	public void OnEnd()
	{	
		DeviceVibrate.Play();
		DeviceSound.Instance.source_bgm.Stop();
        if (ViveDigScene.instance.IsVR() == false)
        {
            Application.LoadLevel("dig_mode");
        }
        else
        {
            Application.LoadLevel("ViveMenu");
        }
        
		
		DeviceSound.Instance.source_bgm.clip =  Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
		DeviceSound.Instance.source_bgm.Play ();

	}


	public void OnReset()
	{
		DeviceVibrate.Play();
		DeviceSound.Instance.source_bgm.Stop();
        if (ViveDigScene.instance.IsVR() == false)
        {
            Application.LoadLevel("dig");
        }
        else
        {
            Application.LoadLevel("ViveDig");
        }
    }


	void GuideSound(string str, bool on)
	{
		if (on) {
			if (!audio_.isPlaying) {
				audio_.clip = Resources.Load (str.ToString()) as AudioClip;
				audio_.Play();
			}
			on = false;
		}
	}
}
