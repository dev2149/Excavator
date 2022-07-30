using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccidentDriveScene : MonoBehaviour {
    
    public static AccidentDriveScene instance;
    public Text message;
    public RawImage[] widgets;
    public Texture[] widgetsON;
    public Texture[] widgetsOFF;

    public Sprite[] gearSpr;
    public Texture[] gearTextures;
    public Material gearMaterial;
    public Image gearObj;
    public Steering steering { get; private set; }
    private bool _isVR = true;
	

	void Awake ()
    {
        instance = this;
        steering = Steering.Instance;
        steering.SpringForceOff();

        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            if (PlayerPrefs.GetInt("CurrentScene") == 1)
            {
                Driving.start_at_end = false; //전진
            }
            else if (PlayerPrefs.GetInt("CurrentScene") == 2)
            {
                Driving.start_at_end = true; //후진

                Animation[] ani = FindObjectsOfType<Animation>();

                for (int i = 0; i < ani.Length; i++) {

                    if (ani[i].clip.name.Equals("Take 001")
                    || ani[i].clip.name.Equals("Handle Animation")
                    || ani[i].clip.name.Equals("safe Animation"))
                    {
                        ani[i].Play();
                        BoxCollider[] boxc = ani[i].gameObject.GetComponents<BoxCollider>();
                        if (boxc != null)
                        {
                            for (int p = 0; p < boxc.Length; p++)
                            {
                                boxc[p].enabled = false;
                            }
                        }
                    }
                }
                ani = null;
            }
        }
        else
        {
            Driving.start_at_end = false; //전진
        }
	}

    public bool IsVR()
    {
        return _isVR;
    }

    public void SetColor(GEAR gear)
    {
        for (int i = 0; i < widgets.Length; i++)
        {
            GEAR _gear = (GEAR)i;

            if (i == (int)gear)
            {
                gearMaterial.mainTexture = gearTextures[i];
                widgets[i].texture = widgetsON[i];
                gearObj.sprite = gearSpr[i];
            }
            else
            {
                widgets[i].texture = widgetsOFF[i];
            }
        }        
    }

}
