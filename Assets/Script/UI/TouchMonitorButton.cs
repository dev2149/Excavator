using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMonitorButton : ButtonBase, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (PressedSprite != null)
            TargetGraphic.overrideSprite = PressedSprite;

        //Director.Instance.SoundManager.PlayEffect("E_5");

        Invoke("PressButton", 0.15f);
    }

    protected override void PressButton()
    {
        base.PressButton();
        
    }
}
