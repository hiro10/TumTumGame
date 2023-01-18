using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class Title : MonoBehaviour
{
    // 点滅スピード
    public float speed = 1.0f;

    // 点滅させたいUI格納
    [SerializeField] private TextMeshProUGUI tapText;

    // 点滅頻度
    private float time;

    // 点滅解除後に表示させるUI
    [SerializeField] GameObject StartMenu;

    [SerializeField] Button[] MenmuButton = new Button[3];

    // オプション画面用(DoTween)
    public GameObject optionPanel;
    private bool isDefaultScaleoptionPanel;

    // タイトル用(DoTween)
    public TextMeshProUGUI title;

    // 背景用
    [SerializeField] GameObject[] backGround;

    [SerializeField] Fade fade;


    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        fade.FadeOut(1f);

        ChangeBackGround();

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

    /// <summary>
    /// スタートボタンを押したときの処理
    /// </summary>
    public void OnStartButton()
    {
        // 決定音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);

        // Mainシーンに遷移
        fade.FadeIn(1f, () => SceneManager.LoadScene("Main"));
    }

    /// <summary>
    /// タイトルをタップしたときの処理
    /// </summary>
    public void OnTestButton()
    {
        //DOTweenTMPAnimatorを作成
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(title);

        //1文字ずつアニメーションを設定(iが何番目の文字かのインデックス)
        //Sequenceで全文字のアニメーションをまとめる
        var sequence = DOTween.Sequence();


        //一文字ずつにアニメーション設定
        var duration = 0.2f;//1回辺りのTween時間
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            sequence.Join(DOTween.Sequence()
              //上に移動して戻る
              .Append(animator.DOOffsetChar(i, animator.GetCharOffset(i) + new Vector3(0, 30, 0), duration).SetEase(Ease.OutFlash, 2))
              //同時に1.2倍に拡大して戻る
              .Join(animator.DOScaleChar(i, 1.2f, duration).SetEase(Ease.OutFlash, 2))
              //同時に360度回転
              .Join(animator.DORotateChar(i, Vector3.forward * -360, duration, RotateMode.FastBeyond360).SetEase(Ease.OutFlash))
              //同時に色を黄色にして戻す
              .Join(animator.DOColorChar(i, Color.yellow, duration * 0.5f).SetLoops(2, LoopType.Yoyo))
              //アニメーション後、1秒のインターバル設定
              .AppendInterval(1f)
              //開始は0.15秒ずつずらす
              .SetDelay(0.15f * i)
            );
        }
    }

    /// <summary>
    /// オプションボタンの処理
    /// </summary>
    public void OnOppTionButton()
    {
        // 決定音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);

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
        // 閉じる音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Close);

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

    /// <summary>
    /// オプション画面のスコアボタンを押したとき
    /// </summary>
    public void OnScoreButton()
    {
        // 決定音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(100);
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

    /// <summary>
    /// 時間帯によって背景を変える処理
    /// </summary>
    private void ChangeBackGround()
    {
        // 夜
        if (DateTime.Now.Hour >= 19 || DateTime.Now.Hour <= 6)
        {
            backGround[0].SetActive(true);
        }
        // 夕方,早朝
        else if (DateTime.Now.Hour == 7 || DateTime.Now.Hour == 18)
        {
            backGround[1].SetActive(true);
        }
        // 朝、昼
        else
        {
            backGround[2].SetActive(true);
        }
    }

    void OnActiveSceneChanged()
    {
        
    }
}
