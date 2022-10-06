using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField] Button[] MenmuButton = new Button[3];

    // オプション画面用(DoTween)
    public GameObject optionPanel;
    private bool isDefaultScaleoptionPanel;

    // タイトル用(DoTween)
    public GameObject title;
    private bool isDefaultScaleTitle;

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    { 
        StartMenu.SetActive(false);

        // dotweenの判定トリガーをfalseに
        isDefaultScaleoptionPanel = false;

        // タイトルBGMの再生
        SoundManager.instance.PlayBGM(SoundManager.BGM.Title);

        // オプションウィンドウのnullチェック
        if (optionPanel == null)
        {
            optionPanel = GameObject.Find("OptionWindow");
        }
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

        if (!isDefaultScaleoptionPanel)
        {
            // オプションウィンドウをだんだん拡大
            optionPanel.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
            
            isDefaultScaleoptionPanel = true;
        }

        // それ以外のボタンを押せないように
        for (int i = 0; i < MenmuButton.Length; i++)
        {
            MenmuButton[i].interactable = false;
        }
    }

    /// <summary>
    /// オプション画面の閉じるボタンを押したとき
    /// </summary>
    public void OnCloseButton()
    {
        if (isDefaultScaleoptionPanel)
        {
            // オプションウィンドウをだんだん縮小
            optionPanel.transform.DOScale(new Vector3(0, 0, 0), 0.2f);
            isDefaultScaleoptionPanel = false;
        }

        // 押せなかったボタンを押せるように
        for (int i = 0; i < MenmuButton.Length; i++)
        {
            MenmuButton[i].interactable = true;
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
