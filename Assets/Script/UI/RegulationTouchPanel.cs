using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RegulationTouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RegulationMonitorPanel regulationMonitorPanel;

    private Vector3 FirstPos;
    private Vector3 LastPos;

    public int CurrentPage;
    public void OnPointerDown(PointerEventData eventData)
    {
        FirstPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LastPos = eventData.position;
        float distance = Vector3.Distance(FirstPos, LastPos);
        float dir = (FirstPos - LastPos).normalized.x;

        if (distance > 30.0f)
        {
            if (dir >= 0.0f)
            {
                if (CurrentPage == 7)
                {
                    StartCoroutine(Book());
                }
                else
                {
                    GetComponent<AudioSource>().Play();
                }
                CurrentPage++;
                CurrentPage = Mathf.Clamp(CurrentPage, 0, 7);
            }
            else if (dir < 0.0f)
            {
                if (CurrentPage == 0)
                {
                    StartCoroutine(Book());
                }
                else
                {
                    GetComponent<AudioSource>().Play();
                }
                CurrentPage--;
                CurrentPage = Mathf.Clamp(CurrentPage, 0, 7);
            }
        }

        SetIcon();

        FirstPos = LastPos = Vector3.zero;
    }

    // Start is called before the first frame update

    public void PageP()
    {
        if (CurrentPage == 7)
        {
            StartCoroutine(Book());
        }
        else
        {
            GetComponent<AudioSource>().Play();
        }
        CurrentPage++;
        CurrentPage = Mathf.Clamp(CurrentPage, 0, 7);
        SetIcon();
    }
    public void PageM()
    {
        if (CurrentPage == 0)
        {
            StartCoroutine(Book());
        }
        else
        {
            GetComponent<AudioSource>().Play();
        }
        CurrentPage--;
        CurrentPage = Mathf.Clamp(CurrentPage, 0, 7);
        SetIcon();
    }

    private void SetIcon()
    {
        regulationMonitorPanel.SetIcon(CurrentPage);
    }

    IEnumerator Book()
    {
        //Vector3 originPos = transform.parent.localPosition;
        Vector3 originPos2 = regulationMonitorPanel.transform.localPosition;
        int st = 0;
        while (true)
        {
            regulationMonitorPanel.transform.localPosition = Vector2.left * 3;
            //transform.parent.localPosition = Vector2.left;
            yield return new WaitForSeconds(0.1f);
            regulationMonitorPanel.transform.localPosition = Vector2.right * 3;
            //transform.parent.localPosition = Vector2.right;
            yield return new WaitForSeconds(0.1f);
            st++;
            if (st > 2)
            {
                st = 0;
                //transform.parent.localPosition = originPos;
                regulationMonitorPanel.transform.localPosition = originPos2;
                break;
            }
        }
    }
}