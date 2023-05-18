using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour
{
    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;
    // スクロールバー
    [SerializeField] GameObject scrollbar;
    public float scroll_pos = 0;

    public float[] pos;

    private void Awake()
    {
        scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        LeftButton.SetActive(false);
        RightButton.SetActive(true);
        scroll_pos = 0;
    }
    private void Update()
    {
        pos = new float[transform.childCount];
        float distabce = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distabce * i;
        }
       
        
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distabce / 2) && scroll_pos > pos[i] - (distabce / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distabce / 2) && scroll_pos > pos[i] - (distabce / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }

        }
    }

    public void OnClickLeftButton()
    {
        if (scroll_pos <= 0.5f)
        {
            LeftButton.SetActive(false);
            scroll_pos = 0;
        }
        else
        {
            RightButton.SetActive(true);
            LeftButton.SetActive(true);
            scroll_pos -= 0.5f;
        }
    }

    public void OnClickRightButton()
    {
        if (scroll_pos >= 0.5f)
        {
            RightButton.SetActive(false);
            scroll_pos = 1;
        }
        else
        {
            //if (RightButton.activeSelf == false)
            {
                LeftButton.SetActive(true);
                RightButton.SetActive(true);
            }
            scroll_pos += 0.5f;
        }
    }
}
