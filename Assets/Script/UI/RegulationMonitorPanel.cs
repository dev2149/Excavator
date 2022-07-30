using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegulationMonitorPanel : MonoBehaviour
{
    [SerializeField] private Image[] Icons;
    [SerializeField] private Sprite CurrentPageIcon;
    [SerializeField] private RectTransform Content;

    private Vector2 TargetPos;
    private Vector2 a;

    private bool IsMove = false;

    private float DeltaTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Icons[0].overrideSprite = CurrentPageIcon;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMove)
        {
            DeltaTime += Time.deltaTime * 5.0f;

            Content.anchoredPosition = Vector2.Lerp(a, TargetPos, DeltaTime);

            if(DeltaTime >= 1.0f)
            {
                DeltaTime = 0.0f;
                IsMove = false;
                a = TargetPos;
            }
        }
    }

    public void SetIcon(int CurrentPage)
    {
        IsMove = true;
        a = Content.anchoredPosition;
        TargetPos = new Vector2(CurrentPage * -1159.0f, 0.0f);

        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].overrideSprite = (i.Equals(CurrentPage)) ? CurrentPageIcon : null;
        }
    }
}
