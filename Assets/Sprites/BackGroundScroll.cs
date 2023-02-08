using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackGroundScroll : MonoBehaviour
{
    // 背景用
    [SerializeField] Texture[] backGround;

    [SerializeField] Material back;
    // Start is called before the first frame update
    void Start()
    {
        ChangeBackGround();
    }

    /// <summary>
    /// 時間帯によって背景を変える処理
    /// </summary>
    private void ChangeBackGround()
    {
        // 夜
        if (DateTime.Now.Hour >= 19 || DateTime.Now.Hour <= 6)
        {
            back.SetTexture("_Scroll_title", backGround[0]);
        }
        // 夕方,早朝
        else if (DateTime.Now.Hour == 7 || DateTime.Now.Hour == 18)
        {
            back.SetTexture("_Scroll_title", backGround[1]);
        }
        // 朝、昼
        else
        {
            back.SetTexture("_Scroll_title", backGround[2]);
        }
    }
}
