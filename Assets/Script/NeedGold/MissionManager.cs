using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour {

    #region MissionObjCount
    public int i_FirstGoldCount = 30, i_CurrentGold = 0;
    public int i_FirstStoneCount = 20;
    public int i_FistGasolineCount = 10;
    public float f_FirstTime = 1, f_CurrentTime = 0;
    #endregion

    public int i_GoldCount,i_StoneCount,i_GasolineCount = 0;
    public bool b_MissionStart = false;
    [SerializeField]
    GameObject end;
    private RespawnObject respawnObject;

    private void Start()
    {
        ChildLoad();
    }

    public void ChildLoad()
    {
        f_CurrentTime = f_FirstTime;
        i_CurrentGold = i_FirstGoldCount;
        respawnObject = GameObject.Find("Environment").GetComponent<RespawnObject>();
        StartCoroutine(respawnObject.CreateGold());
    }

    private void Update()
    {
        if (f_FirstTime > 0)
        {
            f_FirstTime -= Time.deltaTime / 30;
            GameObject.Find("FlatnessManger").GetComponent<FlatnessMessage>().UISlider.GetComponent<Slider>().value = f_FirstTime;
        }
        else if (f_FirstTime <= 0 && GameObject.Find("FlatnessManger").GetComponent<Flatnessing>().enabled)
        {
            end.SetActive(true);
            GameObject.Find("FlatnessManger").GetComponent<Flatnessing>().enabled = false;
            end.transform.Find("EndUI").transform.Find("Text_Gold").GetComponent<Text>().text = "" + i_GoldCount;
            end.transform.Find("EndUI").transform.Find("Text_Ston").GetComponent<Text>().text = "" + i_StoneCount;
            end.transform.Find("EndUI").transform.Find("Text_Soil").GetComponent<Text>().text = "" + i_GasolineCount;
            GameObject.Find("DeviceSound").transform.Find("effect").GetComponent<AudioSource>().clip = Resources.Load("Effect/correct") as AudioClip;
            GameObject.Find("DeviceSound").transform.Find("effect").GetComponent<AudioSource>().Play();
            for (int i = 0; i < Director.Instance.poolingManager.OnObjectList.Count; i++)
            {
                Director.Instance.poolingManager.OnObjectList[i].gameObject.SetActive(false);
            }
        }
    }
}
