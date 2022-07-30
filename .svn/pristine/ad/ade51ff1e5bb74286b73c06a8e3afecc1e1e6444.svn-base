using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour
{
    public GameObject Login;
    IEnumerator Start()
    {
		Option.Instance.Load ();
		DeviceSound.Instance.source_bgm.clip =  Resources.Load("M_SS_ Tr01 Good Choice") as AudioClip;
		DeviceSound.Instance.source_bgm.Play ();

        yield return new WaitForSeconds(1.0f);

        if (PlayerPrefs.GetString("copy_ok").Equals("ok"))
        {
            if (Login.active == false)
            {
                Login.SetActive(true);
            }
        }
       // PlayerPrefs.
       //if(PlayerPrefs.GetString("copy_ok").Equals("ok"))
       // {
       //      Application.LoadLevel("warning");
       // }

      //  Application.LoadLevel("warning");

    }
}
