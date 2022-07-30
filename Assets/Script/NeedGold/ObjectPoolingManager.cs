using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour {

    public List<GameObject> OnObjectList = new List<GameObject>();
    public List<GameObject> OffObjectList = new List<GameObject>();

    public bool Initialization { get; private set; }

    private void FixedUpdate()
    {
        OffObjectListChange();
    }

    public void Init()
    {
        Initialization = true;
        ObjectPooling();
    }

    private void ObjectPooling()
    {
        string PrefabsPath = "NeedGold";
        ComBineFuntion(PrefabsPath,"Object");
    }

    private void ComBineFuntion(string _resoutcePath , string _tagName)
    {

        GameObject[] g_Prefabs = Resources.LoadAll<GameObject>(_resoutcePath);

        GameObject g_tagObj = new GameObject(_tagName);
        g_tagObj.transform.SetParent(transform);

        for (int i = 0; i < g_Prefabs.Length; i++)
        {
            string s_ObjName = g_Prefabs[i].name.Split('_')[0] + g_Prefabs[i].name.Split('_')[2];
            int i_objCreateCount = int.Parse(g_Prefabs[i].name.Split('_')[1]);

            for (int j = 0; j < i_objCreateCount; j++)
            {
                GameObject g_tempObj = Instantiate(g_Prefabs[i]);
                g_tempObj.SetActive(false);
                g_tempObj.transform.SetParent(g_tagObj.transform);
                g_tempObj.name = s_ObjName;
                OffObjectList.Add(g_tempObj);
            }
        }
    }

    public GameObject GetObject(string _objName)
    {
        GameObject returnObj = null;

        for (int i = 0; i < OffObjectList.Count; i++)
        {
            if (OffObjectList[i].name.Equals(_objName) && OffObjectList[i].activeSelf.Equals(false))
            {
                returnObj = OffObjectList[i];
            }
        }

        return returnObj;
    }

    public void ObjActiveOn(GameObject _Obj)
    {
        OnObjectList.Add(_Obj);
        OffObjectList.Remove(_Obj);
        _Obj.SetActive(true);
    }

    public void OffObjectListChange()
    {
        for (int i = 0; i < OnObjectList.Count; i++)
        {
            if (OnObjectList[i].activeSelf.Equals(false))
            {
                OffObjectList.Add(OnObjectList[i]);
                OnObjectList.Remove(OnObjectList[i]);
            }
        }
    }
}

