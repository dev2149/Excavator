using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextureScale : MonoBehaviour
{

    private bool scale = false;
    public GameObject Camera_;
    public GameObject result;
    public GameObject RenderTexture_;
    public GameObject cameraButton;

    void Start()
    {
        //  RenderTexture_.GetComponent<BoxCollider>().enabled = false;
    }

    public void SetCameraScale()
    {
        if (RenderTexture_.activeSelf == true)
        {
            scale = !scale;

            if (scale)
            {
                Camera_.SetActive(true);
                RenderTexture_.SetActive(false);
                cameraButton.SetActive(false);
            }
            else
            {
                Camera_.SetActive(false);

                RenderTexture_.SetActive(true);

                cameraButton.SetActive(true);
            }
        }
    }

    public void OnScale()
    {
        scale = !scale;

        if (scale)
        {
            Camera_.SetActive(true);
            RenderTexture_.SetActive(false);
            cameraButton.SetActive(false);
        }
        else
        {
            Camera_.SetActive(false);

            RenderTexture_.SetActive(true);

            cameraButton.SetActive(true);
        }

    }

    void Update()
    {
        if (result.activeSelf)
        {
            Camera_.SetActive(false);
        }

        if (RenderTexture_.activeSelf == false)
        {
            RenderTexture_.SetActive(false);
            cameraButton.SetActive(false);
        }

    }

}
