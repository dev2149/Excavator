using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class OptionScene : MonoBehaviour
{
	public UISprite checkVibration;
	public UISprite checkSound;
	public UISprite bgmcheck;
	public UISprite effectcheck;
	public UIKeyBinding keybiding;
	
	public UISlider _slider;
	public UISlider _slider2;

    ProcessStartInfo process;

    void OnEnable()
	{
		SetSprite (checkVibration, Option.Instance.Vibration);
		SetSprite (checkSound, Option.Instance.Sound);

		if (null != keybiding) keybiding.enabled = false;
	}

	void OnDisable()
	{
		if (null != keybiding) keybiding.enabled = true;
	}


    public void OnLog()
    {
        Application.LoadLevel("log");
    }

    public void OnSafeQuiz()
    {
        Application.LoadLevel("Quiz");
    }

    public void OnManual()
    {
        process = new ProcessStartInfo("AcroRd32.exe", Application.dataPath + "/Resources/manual.pdf");
        Process.Start(process);

      //  UnityEngine.Debug.Log(proc.Id);
        //Application.OpenURL("http://simglab.com/ARBookImage.jpg");
    }

    public void OnVibration ()
	{
        //Option.Instance.Vibration = !Option.Instance.Vibration;
        //SetSprite (checkVibration, Option.Instance.Vibration);

        //DeviceVibrate.Play ();
	}

	public void OnSound ()
	{
        Option.Instance.Sound = !Option.Instance.Sound;

		SetSprite (checkSound, Option.Instance.Sound);		
		SetSprite2 (bgmcheck, Option.Instance.Sound);
		SetSprite2 (effectcheck, Option.Instance.Sound);

		//DeviceVibrate.Play ();
     //   UnityEngine.Debug.Log("0000");
		DeviceSound.Instance.Enable = Option.Instance.Sound;
		
		if (Option.Instance.Sound == false)
        {
            //UnityEngine.Debug.Log("1111");
            //PlayerPrefs.SetFloat("BGM_Temp", DeviceSound.Instance.BGM);         //임시로 이렇게 대처 사용자 설정값 저장 꼬여있는데 풀 시간이 없음
            //PlayerPrefs.SetFloat("Effect_Temp", DeviceSound.Instance.Effect);

            _slider.value = 0.0f;	
			_slider2.value = 0.0f;
         

		}
        else
        {
     //       UnityEngine.Debug.Log("2222");
            Option.Instance.Sound = true;
            SetSprite(checkSound, Option.Instance.Sound);
            DeviceSound.Instance.Enable = Option.Instance.Sound;

            DeviceSound.Instance.BGM = 0.5f;
            DeviceSound.Instance.Effect = 0.8f;

            _slider.value = DeviceSound.Instance.BGM;
            _slider2.value = DeviceSound.Instance.Effect;

            if (!DeviceSound.Instance.source_bgm.isPlaying && DeviceSound.Instance.Enable)
                DeviceSound.Instance.source_bgm.Play();

            if (!DeviceSound.Instance.source_effect.isPlaying && DeviceSound.Instance.Enable)
                DeviceSound.Instance.source_effect.Play();
        }
	}

	void Update()
	{
     //   UnityEngine.Debug.Log(DeviceSound.Instance.Enable);
		if (!Option.Instance.Sound)
        {
			_slider.value = 0.0f;
			_slider2.value = 0.0f;			
		}
        //else
        //{
        //    DeviceSound.Instance.BGM = 0.5f;
        //    DeviceSound.Instance.Effect = 0.8f;

        //   // DeviceSound.Instance.BGM = save_bgm;
        //    //DeviceSound.Instance.Effect = save_effect;
        //}
    }
	
	public void OnVolume() //BGMSound
	{
		DeviceSound.Instance.BGM = _slider.value ;
        //save_bgm = _slider.value;

        Enable ();

		if (_slider.value == 0.0f)
			SetSprite2 (bgmcheck, false);
		else
			SetSprite2 (bgmcheck, true);
	}

	public void Effect()   //EffectSound
	{
		DeviceSound.Instance.Effect = _slider2.value;
       // save_effect = _slider2.value;

        Enable ();

		if (_slider2.value == 0.0f)
			SetSprite2 (effectcheck, false);
		else
			SetSprite2 (effectcheck, true);
	}
	

	public void OnCredit()
	{
	//	DeviceVibrate.Play ();
	}

	public void OnBack()
	{
	//	DeviceVibrate.Play ();
		Application.LoadLevel("menu");
	}

	void SetSprite(UISprite sprite, bool on)
	{
		sprite.spriteName = (on) ? "popup_switch_bg_on" : "popup_switch_bg_off";
	}

	void SetSprite2(UISprite sprite, bool on)
	{
		sprite.spriteName = (on) ? "popup_switch_on" : "popup_switch_off";
	}

	void Enable()
	{

		if (_slider.value == 0.0f && _slider2.value == 0.0f)
        {
			Option.Instance.Sound = false;
			SetSprite (checkSound, Option.Instance.Sound);
			DeviceSound.Instance.Enable = Option.Instance.Sound;
		}
        else
        {
			Option.Instance.Sound = true;
			SetSprite (checkSound, Option.Instance.Sound);
			DeviceSound.Instance.Enable = Option.Instance.Sound;
		}
	}


	public void Flug()
	{
		//DeviceVibrate.Play ();

		if(this.gameObject.activeSelf == false)
			this.gameObject.SetActive (true);
	}
	
	public void Load()
	{
		Option.Instance.Vibration = (1 == PlayerPrefs.GetInt ("option_vibration", 1));
		Option.Instance.Sound = (1 == PlayerPrefs.GetInt ("option_sound", 1));
		
		if (Option.Instance.Sound)
        {
           // _slider.value = save_bgm;
           // _slider2.value = save_effect; 

            _slider.value = DeviceSound.Instance.BGM;
            _slider2.value = DeviceSound.Instance.Effect;
        }
        else
        {
			_slider.value = 0.0f;
			_slider2.value = 0.0f;			
		}
		
	}
}
