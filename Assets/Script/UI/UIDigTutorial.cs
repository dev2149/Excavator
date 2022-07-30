using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDigTutorial : MonoBehaviour {

	public UIKeyBinding keybinding;
	public AudioClip sound_success;
	public AudioClip sound_fail;
	public UILabel CloseLabel;
	public UILabel FoulLabel;
	public UILabel Encourage;
	public AudioSource audio_;

	public AudioSource diesel;

    public GameObject rmpeui;

    //저장 관련
    private SaveData data;
    private Users user = new Users();
    private List<string> lLog = new List<string>();
    private List<string> lContent = new List<string>();

	void OnEnable()
	{
		Time.timeScale = 0.0f;
        //활성화시 데이터 로드
        data = SaveData.Load(Application.streamingAssetsPath + "\\" + PlayerPrefs.GetString("UserID") + ".xml");

        //foreach(string s in data.GetValue<List<string>>(user.Log))
        //{
        //    lLog.Add(s);
        //}
        lLog = data.GetValue<List<string>>(user.Log);
        lContent = data.GetValue<List<string>>(user.Content);

        //foreach (string s in data.GetValue<List<string>>(user.Log))
        //{
        //    Debug.Log(s);
        //}
	}
	
	void OnDisable()
	{
		Time.timeScale = 1.0f;

     //   Save();      //비활성화시 데이터 저장
	}


    void Save()
    {
        lLog.Add("- 굴삭연습 -  " + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
        lContent.Add(FoulLabel.text);
        data[user.Log] = lLog;
        data[user.Content] = lContent;
        data.Save();  
    }

	public void DigTutorial(int[] Foul) //dig
	{		

		DeviceSound.Instance.AudioPuase();
		diesel.Stop ();
        rmpeui.SetActive(true);
		int int_foul = Foul[0] + Foul[1] + Foul[2] + Foul[3] + Foul[4];
		
//		if (int_foul == 0) {	// 시작하자 마자 종료하여 평가 받을 경우.
//			NGUITools.PlaySound (sound_fail,DeviceSound.Instance.BGM);
//			CloseLabel.text = "끝까지 도전하여 평가 받아보세요.";
//		}
		if (int_foul >= 3)
        {		//연습모드 중간에 결과를 평가 받을 경우.
			NGUITools.PlaySound (sound_fail,DeviceSound.Instance.Effect);

            FoulLabel.text = "▶케빈 흔들림 : " + Foul[0] + "번 ▶작업영역 이탈 : " + Foul[1] + "번 \n▶흙덩이 낙하 : " + Foul[2] + "번 ▶과부하 : " + Foul[3] + "번\n▶RPM 수치 비정상 : " + Foul[4] + "번";
			Encourage.text = "끝까지 도전하여 평가 받아보세요.";

          
            Save(); 
		}
		else
        {
			NGUITools.PlaySound (sound_fail,DeviceSound.Instance.Effect);

            FoulLabel.text = "▶케빈 흔들림 : " + Foul[0] + "번 ▶작업영역 이탈 : " + Foul[1] + "번 \n▶흙덩이 낙하 : " + Foul[2] + "번 ▶과부하 : " + Foul[3] + "번\n▶RPM 수치 비정상 : " + Foul[4] + "번";
            Encourage.text = "포기하지 마시고 끝까지 노력해주세요.";

       
            Save(); 
		}
		
		keybinding.enabled = false;
	}

	public void DigOnOpen(int[] Foul)
	{
		DeviceSound.Instance.AudioPuase ();
		diesel.Stop ();
        rmpeui.SetActive(true);

        int int_foul = Foul[0] + Foul[1] + Foul[2] + Foul[3] + Foul[4];

		if (Foul[1] + Foul[3] > 0)
        {		//연습모드를 끝마쳤을 경우.
			GuideSound("DigResult/Result_002",true);
			NGUITools.PlaySound (sound_success,DeviceSound.Instance.Effect);
            FoulLabel.text = "▶케빈 흔들림: " + Foul[0] + "번 ▶작업영역 이탈: " + Foul[1] + "번 \n▶흙덩이 낙하: " + Foul[2] + "번 ▶과부하: " + Foul[3] + "번\n▶RPM 수치 비정상 : " + Foul[4] + "번";
            Encourage.text = "포기하지 마시고 끝까지 노력해주세요.";

            Save(); 

		}
		else {

			if(int_foul == 0 || int_foul == 1){
				GuideSound("DigResult/Result_003",true);
				NGUITools.PlaySound (sound_success,DeviceSound.Instance.Effect);
                FoulLabel.text = "▶케빈 흔들림: " + Foul[0] + "번 ▶작업영역 이탈: " + Foul[1] + "번 \n▶흙덩이 낙하: " + Foul[2] + "번 ▶과부하: " + Foul[3] + "번\n▶RPM 수치 비정상 : " + Foul[4] + "번";
                Encourage.text = "시험모드에 도전 하셔도 되겠어요!";

                Save();
            }
			else if (Foul[0] + Foul[2] >= 2)
			{
				GuideSound("DigResult/Result_004",true);
				NGUITools.PlaySound (sound_success,DeviceSound.Instance.Effect);
                FoulLabel.text = "▶케빈 흔들림: " + Foul[0] + "번 ▶작업영역 이탈: " + Foul[1] + "번 \n▶흙덩이 낙하: " + Foul[2] + "번 ▶과부하: " + Foul[3] + "번\n▶RPM 수치 비정상 : " + Foul[4] + "번";
                Encourage.text = "거의 합격인데… 조금 더 노력해주세요.";

                Save(); 

			}

		}
	}

	public void OnEnd_dig()
	{
		DeviceVibrate.Play();
		DeviceSound.Instance.source_bgm.Stop();

        Application.LoadLevel("dig_mode");

        DeviceSound.Instance.source_bgm.clip =  Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
		DeviceSound.Instance.source_bgm.Play ();
			
	}
	
	public void OnReset_Dig()
	{
		DeviceVibrate.Play();
		DeviceSound.Instance.source_bgm.Stop();
        Application.LoadLevel("dig");
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
