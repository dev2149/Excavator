using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    #region singleton
    private static Director _Instance;
    public static Director Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<Director>();
                if (_Instance == null)
                {
                    GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Dir/Directory"));
                    obj.name = "Directory";
                    _Instance = obj.GetComponent<Director>();
                    obj.transform.parent = GameObject.Find("Singleton").transform;
                }
                _Instance.Init();
                DontDestroyOnLoad(_Instance);
            }
            return _Instance;
        }
    }
    #endregion

    public ObjectPoolingManager poolingManager { get; private set; }

    public bool Initialize { get; private set; }

    private void Init()
    {
        poolingManager = GetComponentInChildren<ObjectPoolingManager>();

        poolingManager.Init();

        StartCoroutine(Checking());
    }

    IEnumerator Checking()
    {
        while (true)
        {
            if (poolingManager.Initialization)
            {
                Initialize = true;
                yield break;
            }
            yield return null;
        }
    }
}