using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class ModeScene : MonoBehaviour
{
    void Start()
    {
        VRSettings.enabled = false;
    }

    public void OnHMD()
    {
        VRSettings.loadedDevice = VRDeviceType.Oculus;
        VRSettings.enabled = true;          //VR모드 
        InputTracking.Recenter();           //트레킹 센터
     //   DeviceVibrate.Play();
        //    if (false == PlayerPrefs.HasKey("selected_menu_drive"))
        //    {
        PlayerPrefs.SetInt("selected_Mode", 0);
        Application.LoadLevel("Log");
        //}
        //else
        //{
        //    Application.LoadLevel("drive_mode");
        //}
    }

    public void OnMonitor()
    {
        VRSettings.enabled = false;
        DeviceVibrate.Play();
        //if (false == PlayerPrefs.HasKey("selected_menu_dig"))
        //{
        PlayerPrefs.SetInt("selected_Mode", 1);
        Application.LoadLevel("Log");
        //}
        //else
        //{
        //    Application.LoadLevel("dig_mode");
        //}
    }

    public void Quit()
    {
        Application.Quit();
    }
}
