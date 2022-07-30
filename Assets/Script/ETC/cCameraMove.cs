using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class cCameraMove : MonoBehaviour
{
    public Transform mCamera;
	// Use this for initialization
	void Start () {
       // mCamera = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (PlayerPrefs.GetInt("selected_Mode") == 0)
        {
            if (Application.loadedLevelName == "drive")
            {
                if (mCamera.transform.localPosition.x < 0.25f )
                {

                    transform.localPosition = new Vector3(InputTracking.GetLocalPosition(VRNode.Head).x * 14, InputTracking.GetLocalPosition(VRNode.Head).y * -2, 2 + InputTracking.GetLocalPosition(VRNode.Head).z * -2);
                    //  Debug.Log(transform.localPosition.x * (InputTracking.GetLocalPosition(VRNode.Head).x));
                }
            }
        }
      //  Debug.Log(mCamera.transform.localPosition.ToString());
	}
}
