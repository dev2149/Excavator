using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBase : UI
{
    public System.Action OnClick;

    [SerializeField] protected Image TargetGraphic;
    [SerializeField] protected Sprite PressedSprite;

    virtual protected void PressButton()
    {
        TargetGraphic.overrideSprite = null;
        GetComponent<Button>().onClick.Invoke();
        transform.root.GetComponent<AudioSource>().Play();
        //OnClick();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(TargetGraphic == null)
            TargetGraphic = GetComponent<Image>();
    }

    public void SetSprite(Sprite SourceImage, Sprite OverrideSprite)
    {
        TargetGraphic.sprite = SourceImage;
        PressedSprite = OverrideSprite;
    }
}
