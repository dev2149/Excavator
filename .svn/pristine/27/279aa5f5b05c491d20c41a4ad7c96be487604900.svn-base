using UnityEngine;
using System.Collections;

public class WarningScene : MonoBehaviour
{
	private AudioClip Clip;
	
    IEnumerator Start()
    {
        yield return null;
	
        float _time = Time.time;
        //while (false == Input.GetButton("Fire1"))
        //{
        //    yield return null;

        //    if (20.0f <= (Time.time - _time))
        //        break;
        //}

    }
	
	public void OnNext()
	{
		OnVibration ();
		Application.LoadLevel("mode");	
	}

	public void OnVibration()
	{
		if (!Option.Instance.Vibration)
			return;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
        {
            Application.LoadLevel("mode");
        }
	}
	
}

