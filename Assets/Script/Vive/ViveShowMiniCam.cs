using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveShowMiniCam : MonoBehaviour {

    private float showTime = 0;
    private MeshRenderer _meshRenderer;

    void Start () {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
    }
	
	
	void Update () {
        showTime -= Time.deltaTime;

        if (showTime <= 0)
        {
            _meshRenderer.enabled = false;
        }
	}

    public void ShowMiniCam(float _showTime)
    {
        showTime = _showTime;
        _meshRenderer.enabled = true;
    }
}
