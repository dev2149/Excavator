using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuketIsObjController : MonoBehaviour
{
    public static bool shoveling = false;

    private MissionManager myMission;
    public bool stall = false;

    private Transform _transform;

    public Flatnessing flatnessing;
    public DigingWarning diesel;

    private float Angle
    {
        get
        {
            return Vector3.Angle(Vector3.up, _transform.forward);
        }
    }

    void Start()
    {
        shoveling = false;
        _transform = transform;
        myMission = GameObject.Find("Environment").GetComponent<MissionManager>();
        originPos = MainCam.transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Gold":
                myMission.i_GoldCount++;
                other.gameObject.SetActive(false);
                break;
            case "Gasoline":
                myMission.i_GasolineCount++;
                myMission.f_FirstTime += 0.1f;
                other.gameObject.SetActive(false);
                break;
            case "Stone0":
                myMission.i_StoneCount++;
                myMission.f_FirstTime -= 0.05f;
                other.gameObject.SetActive(false);
                StartCoroutine(Stone(0.25f,0.5f));
                break;
        }
      
        if (other.tag == "DIG_TERRAIN")
        {
            stall = true;
            shoveling = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "DIG_TERRAIN")
        {
            stall = false;
            shoveling = false;
            flatnessing.bucket.speed = 60.0f;
            flatnessing.fore.speed = 50.0f;
            flatnessing.upper.speed = 50.0f;
            
        }
    }

    Vector3 originPos;
    [SerializeField]
    GameObject MainCam;

    IEnumerator Stone(float _amount, float _duration)
    {
        GameObject.Find("CanvasUI").transform.Find("RedFrame").gameObject.SetActive(true);
        float timer = 0;
        while (timer <= _duration)
        {
            MainCam.transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        GameObject.Find("CanvasUI").transform.Find("RedFrame").gameObject.SetActive(false);
        MainCam.transform.localPosition = originPos;
    }
}