using UnityEngine;
using System.Collections;

public class MenuScene : MonoBehaviour
{
    public GameObject quitUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitUI.SetActive(true);
        }
    }

  
   

    public void OnDrive()
    {
      //  DeviceVibrate.Play();
        if (false == PlayerPrefs.HasKey("selected_menu_drive"))
        {
            PlayerPrefs.SetInt("selected_menu_drive", 1);
            Application.LoadLevel("drive_tutorial");
        }
        else
        {
            Application.LoadLevel("drive_mode");
        }
    }

    public void OnDig()
    {
     //   DeviceVibrate.Play();
        if (false == PlayerPrefs.HasKey("selected_menu_dig"))
        {
            PlayerPrefs.SetInt("selected_menu_dig", 1);
            Application.LoadLevel("dig_tutorial");
        }
        else
        {
            Application.LoadLevel("dig_mode");
        }
    }

    public void OnVibration()
    {
        if (!Option.Instance.Vibration)
        {
            return;
        }

      //  DeviceVibrate.Play();
    }

    public void HomePage()
    {
        Application.OpenURL("http://www.simglab.com");
    }

    public void Quit()
    {
        StartCoroutine(QuitStart());
    }

    IEnumerator QuitStart()
    {
        yield return new WaitForSeconds(0.3f);

        Application.Quit();
    }
}
