using UnityEngine;
using System.Collections;

public class SetVolume : MonoBehaviour
{
    public bool b_BGM;
    AudioSource mVolume;
    // Use this for initialization
    void Start()
    {
        mVolume = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (b_BGM)
            mVolume.volume = DeviceSound.Instance.BGM;
        else
            mVolume.volume = DeviceSound.Instance.Effect;
    }
}
