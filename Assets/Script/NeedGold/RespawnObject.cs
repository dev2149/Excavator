using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{

    [SerializeField] private List<Transform> resPawn_GoldPos = new List<Transform>();
    [SerializeField] private Transform rt_GoldPosCount;
    [SerializeField] private int i_MissionGoldCount, i_MissionStoneCount, i_MissionGasolineCount;

    private MissionManager missionManager;

    private float XRandPos = -1, ZRandPos = 1;

    [SerializeField] private int i_RndPos;

    private void Start()
    {
        ChildLoad();
    }

    private void ChildLoad()
    {
        missionManager = GameObject.Find("Environment").GetComponent<MissionManager>();
        rt_GoldPosCount = transform.Find("PointPos");
        RespawnPosInit();
    }

    private void RespawnPosInit()
    {
        for (int i = 0; i < rt_GoldPosCount.childCount; i++)
        {
            resPawn_GoldPos.Add(rt_GoldPosCount.transform.GetChild(i));
        }
    }

    public IEnumerator CreateGold()
    {
        yield return new WaitForSeconds(1.0f);
        GoldInfo();
        StoneInfo();
        GasolineInfo();
    }

    private void GoldInfo()
    {
        i_MissionGoldCount = missionManager.i_FirstGoldCount;
        for (int i = 0; i < i_MissionGoldCount; i++)
        {
            GameObject g_Gold = Director.Instance.poolingManager.GetObject("Gold0");
            if (g_Gold != null)
            {
                i_RndPos = Random.Range(0, resPawn_GoldPos.Count);

                // 랜덤값 조절해가면서 위치 조정
                g_Gold.transform.position = resPawn_GoldPos[i_RndPos].position + new Vector3(Random.Range(XRandPos, ZRandPos), 0f, Random.Range(XRandPos, ZRandPos));
                Director.Instance.poolingManager.ObjActiveOn(g_Gold);
                resPawn_GoldPos.Remove(resPawn_GoldPos[i_RndPos]);
            }
        }
    }
    private void StoneInfo()
    {
        i_MissionStoneCount = missionManager.i_FirstStoneCount;
        for (int i = 0; i < i_MissionStoneCount; i++)
        {
            GameObject g_Gold = Director.Instance.poolingManager.GetObject("Stone0");
            if (g_Gold != null)
            {
                i_RndPos = Random.Range(0, resPawn_GoldPos.Count);

                // 랜덤값 조절해가면서 위치 조정
                g_Gold.transform.position = resPawn_GoldPos[i_RndPos].position + new Vector3(Random.Range(XRandPos, ZRandPos), 0f, Random.Range(XRandPos, ZRandPos));
                Director.Instance.poolingManager.ObjActiveOn(g_Gold);
                resPawn_GoldPos.Remove(resPawn_GoldPos[i_RndPos]);
            }
        }
    }
    private void GasolineInfo()
    {
        i_MissionGasolineCount = missionManager.i_FistGasolineCount;
        for (int i = 0; i < i_MissionGasolineCount; i++)
        {
            GameObject g_Gold = Director.Instance.poolingManager.GetObject("Gasoline0");
            if (g_Gold != null)
            {
                i_RndPos = Random.Range(0, resPawn_GoldPos.Count);

                // 랜덤값 조절해가면서 위치 조정
                g_Gold.transform.position = resPawn_GoldPos[i_RndPos].position + new Vector3(Random.Range(XRandPos, ZRandPos), 0f, Random.Range(XRandPos, ZRandPos));
                Director.Instance.poolingManager.ObjActiveOn(g_Gold);
                resPawn_GoldPos.Remove(resPawn_GoldPos[i_RndPos]);
            }
        }
    }
}