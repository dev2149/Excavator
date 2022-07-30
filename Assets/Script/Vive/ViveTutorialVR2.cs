using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;


public class ViveTutorialVR2 : MonoBehaviour {

    private const int BTN_OFF = 0;
    private const int BTN_ON = 1;

    public AudioClip sound_success;
    public AudioClip sound_fail;

    public Text FoulLabel;
    public Text Encourage;

    public AudioSource audio_;
    public AudioSource diesel;

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

    void OnEnable() //일시정지.
    {
        DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
       // Time.timeScale = 0.0f;

        //활성화시 데이터 로드
       // data = SaveData.Load(Application.streamingAssetsPath + "\\" + PlayerPrefs.GetString("UserID") + ".xml");
       // lLog = data.GetValue<List<string>>(user.Log);
       // lContent = data.GetValue<List<string>>(user.Content);

        //FoulLabel.text = "";
        Encourage.text = "";
    }

    void OnDisable()
    {
        DeviceSound.Instance.TimePause = !DeviceSound.Instance.TimePause;
        Time.timeScale = 1.0f;
    }

    void Start()
    {
       // imgBtn_Reset.texture = texBtnReset[BTN_OFF];
      //  imgBtn_End.texture = texBtnEnd[BTN_OFF];
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
            OnEnd_drive();
        }
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

        if (int_collision >= 3.0f || GameObject.Find("GameObject").transform.Find("Map").GetComponent<DriveMap>().ResultFail)
        {
            //success
            NGUITools.PlaySound(sound_success, DeviceSound.Instance.Effect);
            FoulLabel.text += "콘에 부딪힌 횟수: " + int_collision + "번";
            Encourage.text = "포기하지 마시고 끝까지 노력해주세요.";
            GuideSound("DigResult/Result_002", true);

           // Save();
        }
        else
        {

            if (int_collision == 0 && !GameObject.Find("GameObject").transform.Find("Map").GetComponent<DriveMap>().ResultFail)
            {
                NGUITools.PlaySound(sound_success, DeviceSound.Instance.Effect);
                FoulLabel.text = "콘에 부딪힌 횟수: " + int_collision + "번";
                Encourage.text = "시험모드에 도전 하셔도 되겠어요!";
                GuideSound("DigResult/Result_003", true);
                if (GameObject.Find("SIMG").GetComponent<SimgLauncherUnity>().enabled)
                {
                    SimgLauncherUnity.Instance.ToJson(2, 0, "합격"); // 주행 연습
                }
                // Save();
            }
            else
            {
                NGUITools.PlaySound(sound_fail, DeviceSound.Instance.Effect);
                FoulLabel.text = "콘에 부딪힌 횟수: " + int_collision + "번";
                Encourage.text = "거의 합격인데… 조금 더 노력해주세요.";
                GuideSound("DigResult/Result_004", true);

               // Save();
            }
        }
    }

    public void OnEnd_drive()
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
