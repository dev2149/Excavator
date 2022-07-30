using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NowTime : MonoBehaviour {
    public TextMesh mTime;
    public TextMesh nowTime;

    public Text mTime_VR;
    public Text nowTime_VR;

   // System.DateTime date = new System.DateTime("");
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (System.DateTime.Now.Second % 2 == 0)
            nowTime.text = System.DateTime.Now.Hour.ToString(("00")) + ":" + System.DateTime.Now.Minute.ToString(("00"));
        else
            nowTime.text = System.DateTime.Now.Hour.ToString(("00")) + "." + System.DateTime.Now.Minute.ToString(("00"));

        /*
        if (System.DateTime.Now.Hour > 12)
        {
            mTime.text = "PM";
            if (System.DateTime.Now.Second % 2 == 0)
                nowTime.text = (System.DateTime.Now.Hour - 12).ToString(("00")) +":" + System.DateTime.Now.Minute.ToString(("00"));
            else
                nowTime.text = (System.DateTime.Now.Hour - 12).ToString(("00")) +"." + System.DateTime.Now.Minute.ToString(("00"));           
        }
        else
        {
            mTime.text = "AM";
            if (System.DateTime.Now.Second % 2 == 0)
                nowTime.text = System.DateTime.Now.Hour.ToString(("00")) + ":"+ System.DateTime.Now.Minute.ToString(("00"));
            else
                nowTime.text = System.DateTime.Now.Hour.ToString(("00")) +"." + System.DateTime.Now.Minute.ToString(("00"));
        }
        */
        if (mTime_VR != null && nowTime_VR != null)
        {
            mTime_VR.text = mTime.text;
            nowTime_VR.text = nowTime.text;
        }
    }
}
