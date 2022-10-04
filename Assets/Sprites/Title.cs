﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    // 点滅スピード
    public float speed = 1.0f;

    // 点滅させたいUI格納
    [SerializeField] private Text tapText;

    // 点滅頻度
    private float time;

    // 点滅解除後に表示させるUI
    [SerializeField] GameObject StartMenu;

    // オプションメニュー
    [SerializeField] GameObject OptionMenu;

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        OptionMenu.SetActive(false);

        StartMenu.SetActive(false);

        SoundManager.instance.PlayBGM(SoundManager.BGM.Title);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        tapText.color = GetAlphaColor(tapText.color);

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(TapText());
        }

    }

    public void OnStartButton()
    {
        // Mainシーンに遷移
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// オプションボタンの処理
    /// </summary>
    public void OnOppTionButton()
    {
        OptionMenu.SetActive(true);

        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
    }

    public void OnCloseButton()
    {
        OptionMenu.SetActive(false);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }


    //Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }

    IEnumerator TapText()
    {
        speed = 4f;
        yield return new WaitForSeconds(1f);
        tapText.gameObject.SetActive(false);
        StartMenu.SetActive(true);

        //FadeManager.Instance.LoadScene("Main", 1f);
    }

}
