using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViveAccident : MonoBehaviour
{
    private class AXIS
    {
        public AccidentDiging.AXIS @ref;
        public float target;
        public string targetName;
        public bool complete;

        public static AXIS Create(AccidentDiging.AXIS _ref, float _target, string _targetName)
        {
            AXIS axis = new AXIS();
            axis.@ref = _ref;
            axis.target = _target;
            axis.targetName = _targetName;
            axis.complete = false;
            return axis;
        }
    }
    public AccidentDiging diging;
    private List<AXIS> axis;
    public AccidentUIAxisRange[] ranges_vr;
    private AudioClip alram;
    bool isSpinFail = false;
    public bool brake;
    AudioManager AManager;
    public GameObject[] AccidentOBJ;

    // Start is called before the first frame update
    void Start()
    {
        alram = Resources.Load("띠롱") as AudioClip;
        StartCoroutine(MainSquencCoru());
        AManager = GetComponent<AudioManager>();
        brake = false;
    }

    void Update()
    {
        if (null == axis)
        {
            return;
        }
        int cout = 0;
        int i;
        for (i = 0; i < axis.Count; ++i)
        {
            float angle = axis[i].@ref.angle; //움직이는 화살표
            float d = axis[i].target - angle;

            if (1.0f >= Mathf.Abs(d))
            {
                ranges_vr[i].gameObject.GetComponent<RawImage>().color = Color.green;
                ranges_vr[i].transform.Find("angle").GetComponent<RawImage>().color = Color.green;
            }
            else
            {
                ranges_vr[i].gameObject.GetComponent<RawImage>().color = Color.white;
                ranges_vr[i].transform.Find("angle").GetComponent<RawImage>().color = Color.white;
            }

            if (true == axis[i].complete)
            {
                ++cout;
                continue;
            }

            if (1.0f >= Mathf.Abs(d))
            {
                axis[i].complete = true;
                NGUITools.PlaySound(alram, DeviceSound.Instance.Effect);
            }

            if (Mathf.Abs(d) >= 190)
            {
                isSpinFail = true;
            }
            else
            {

                isSpinFail = false;
            }
        }
    }

    void EnableRage()
    {
        int count = 0;

        if (null != axis)
        {
            count = axis.Count;
        }
        for (int i = 0; i < count; ++i)
        {
            ranges_vr[i].gameObject.SetActive(true);
            ranges_vr[i].axis = axis[i].@ref;
            ranges_vr[i].transform.GetComponentInChildren<Text>().text = axis[i].targetName;
            ranges_vr[i].targetAngle = axis[i].target;
        }

        for (int i = count; i < ranges_vr.Length; ++i)
        {
            ranges_vr[i].gameObject.SetActive(false);
        }
    }

    void OnStartPointEnter(GameObject obj)
    {
        AManager.PlaySound(2);
    }
    void OnStartPointExit(GameObject obj)
    {
        brake = true;
        //GetComponent<AccidentDriveing>().enabled = false;
    }

    IEnumerator MainSquencCoru()
    {
        DeviceSound.Instance.source_bgm.Stop();
        yield return new WaitForSeconds(1.0f);
        while (true)
        {
            if (ArduinoData.Instance.arduinoOk && ArduinoData.Instance.safebar && ArduinoData.Instance.safebelt && ArduinoData.Instance.safepin)
                break;
           
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.2f);
        AccidentOBJ[0].SetActive(true);
        GetComponent<AccidentDriveing>().enabled = true;
        while (true)
        {
            if (GetComponent<AccidentDriveing>().isLever_Press_btn)
            {
                AccidentOBJ[0].SetActive(false);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        AManager.PlaySound(0);
        yield return new WaitForSeconds(3.2f);
        while (true)
        {
            if(ArduinoData.Instance.arduinoOk && ArduinoData.Instance.gearUp == 0 && ArduinoData.Instance.gearDown == 1)
            {
                AManager.PlaySound(1);
                AccidentOBJ[1].SetActive(true);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.0f);
        while (true)
        {
            if (brake)
            {
                AccidentOBJ[1].SetActive(false);
                AManager.PlaySound(3);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.7f);
        while (true)
        {
            if (ArduinoData.Instance.arduinoOk && ArduinoData.Instance.gearUp == 1 && ArduinoData.Instance.gearDown == 1)
            {
                AManager.PlaySound(4);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.3f);
        AccidentOBJ[4].SetActive(true);
        AccidentOBJ[5].SetActive(true);
        AccidentOBJ[6].SetActive(true);
        while (true)
        {
            if (true)
            {
                AccidentOBJ[2].SetActive(true);
                AccidentOBJ[3].SetActive(true);
                AccidentOBJ[3].GetComponent<AccidentDiging>().enabled = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        axis = new List<AXIS>();
        axis.Add(AXIS.Create(diging.upper, 0.0f, "붐"));
        EnableRage();
        yield return new WaitForSeconds(2f);
    //    SceneManager.LoadScene(0);
    }
}