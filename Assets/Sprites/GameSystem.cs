using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System;
using System.Linq;

// ゲームを管理
public class GameSystem : MonoBehaviour
{
    // ツム生成用
    [SerializeField] BallGenerater ballGenerater = default;

    // ドラッグ中かの判定
    bool isDragging;

    // 取り除くボール
    [SerializeField] List<Ball> removeBalls = new List<Ball>();

    // 今選択しているボールを格納
    Ball currentDraggingBall;
    // スコア:TODO別クラスにしよう
    // 現在のスコア
    int score;
    // ハイスコア
    int highScore;

    [SerializeField] TextMeshProUGUI scoreText = default;
    [SerializeField] TextMeshProUGUI higtscoreText = default;
    [SerializeField] TextMeshProUGUI resultscoreText = default;
    [SerializeField] GameObject hiscore = default;

    [SerializeField] Texture[] tumTex;
   
    // ポイント生成用プレハブ
    [SerializeField] GameObject pointEffectPrehab = default;

    // 時間
    [SerializeField] TextMeshProUGUI timerText;
    int timeCount;

    // リザルト画面格納
    [SerializeField] GameObject resultPanel;

    [SerializeField] CameraShake cameraShake;

    bool gameOver;

    [SerializeField] Image coundDownicon;

    DateTime awakeDateTime = DateTime.Now;

    // フェード演出用
    [SerializeField] Fade fade;

    public Ball[] gameObjects;

    private float time;

    // 点滅
    public float speed = 0.05f;

    [SerializeField] Countdown startCountDown;
    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        
        startCountDown.GetComponent<Countdown>();
        StartCoroutine(StartGame());
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    /// <returns></returns>
    IEnumerator StartGame()
    {
        // 1秒間フェードアウト処理
        fade.FadeOut(1f);
        
        // BGM止める
        SoundManager.instance.StopBgm();

        // カウントダウン処理
        startCountDown.OnClickButtonStart();

        // スコアの初期値設定
        score = 0;

        // ハイスコアの読み込み
        highScore = PlayerPrefs.GetInt("SCORE", highScore);

        scoreText.text = score.ToString();
        higtscoreText.text = highScore.ToString();

        timeCount = ParamsSO.Entity.timeCount;

        // リザルトパネルは表示しない
        resultPanel.SetActive(false);

        hiscore.SetActive(false);

        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        coundDownicon = GameObject.Find("Image").GetComponent<Image>();

        // 4秒間待つ
        yield return new WaitForSeconds(4.0f);

        // メインBGMを流す
        SoundManager.instance.PlayBGM(SoundManager.BGM.Main);

        // ツムの生成
        StartCoroutine(ballGenerater.Spown(ParamsSO.Entity.initBallCount));

        // 制限時間
        StartCoroutine(CountDown());
    }

    /// <summary>
    /// 制限時間処理
    /// </summary>
    IEnumerator CountDown()
    {
        // 制限時間以内なら
        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            timerText.text = timeCount.ToString();
            coundDownicon.fillAmount = (float)timeCount / (float)ParamsSO.Entity.timeCount;
        }

        gameOver = true;

        // ゲーム終了時に得点になるツムをつかんでいたら消して得点にする
        OnDragEnd();

        // ゲーム内のスコアの更新
        ChangeHightScore();

        // リザルト画面を表示
        resultPanel.SetActive(true);

        // 
        ScoreUiEffect();
    }

    /// <summary>
    /// スコアを加える処理
    /// </summary>
    /// <param name="point"></param>
    void AddScore(int point)
    {
        score += point;
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        if (gameOver)
        {
            return;
        }
        // 右クリックを押し込んだ時
        if (Input.GetMouseButtonDown(0))
        {

            OnDragin();
        }
        // 右クリックを離したとき
        else if (Input.GetMouseButtonUp(0))
        {
            ReturnBallColor();
            OnDragEnd();
        }
        else if (isDragging)
        {
            OnDriging();
        }
    }

    /// <summary>
    /// ドラッグをしたとき
    /// </summary>
    void OnDragin()
    {
        // オブジェクトのヒット確認
        // Rayで判定
        // カメラからスクリーンに向かって飛ばす
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // ボールにヒットしたか
        if (hit && hit.collider.GetComponent<Ball>())
        {
            Ball ball = hit.collider.GetComponent<Ball>();

            if (ball.isBomb())
            {
                Explosion(ball);
                StartCoroutine(CameraShake());
            }
            else
            {
                AddRemoveBall(ball);

                isDragging = true;
            }
        }
    }
    /// <summary>
    /// ドラッグ中
    /// </summary>
    void OnDriging()
    {
        // ボールの状態を取得
        gameObjects = FindObjectsOfType<Ball>();

        SelctBallColorChange();

        // オブジェクトのヒット確認
        // Rayで判定
        // カメラからスクリーンに向かって飛ばす
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // ボールにヒットしたか
        if (hit && hit.collider.GetComponent<Ball>())
        {
            // 同じ種類、距離が近かったらリストに追加
            // 何と同じだったら＝＞現在ドラックしているオブジェクト
            Ball ball = hit.collider.GetComponent<Ball>();

            // 同じ種類だったら
            if (ball.id == currentDraggingBall.id&&ball.select==false)
            {
                // 距離が近かったら
                float distance = Vector2.Distance(ball.transform.position, currentDraggingBall.transform.position);
                if (distance < ParamsSO.Entity.ballDistance)
                {
                    AddRemoveBall(ball);
                }
            }
        }
    }
    /// <summary>
    /// ドラッグ終了時
    /// </summary>
    private void OnDragEnd()
    {
        int removeCount = removeBalls.Count;

        // つなげたツムが3個以上ならツムを消す
        if (removeCount >= 3)
        {
            for (int i = 0; i < removeCount; i++)
            {
                removeBalls[i].Explosion();

            }
            // 消えた数だけツムを追加する
            StartCoroutine(ballGenerater.Spown(removeCount));

            int score = removeCount * ParamsSO.Entity.ScorePint;

            AddScore(score);
            // ポイントの生成
            PointEffect(removeBalls[removeBalls.Count - 1].transform.position, score);
            // ツムの破裂SEの再生
            SoundManager.instance.PlaySE(SoundManager.SE.Touch);

        }

        removeBalls.Clear();
        isDragging = false;
    }

    /// <summary>
    /// リストがボールを持っていないかの確認
    /// </summary>
    /// <param name="ball"></param>
    void AddRemoveBall(Ball ball)
    {
        currentDraggingBall = ball;
        // リストがBallを持っていなかったら
        if (removeBalls.Contains(ball) == false)
        {
            ball.select = true;
            // リストに加えた時にボールを大きくする
            // TODO スクリプタブルオブジェクトにしよう大きさと色
            ball.transform.localScale = Vector3.one * 1.4f;
            // 色を変える
            ball.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);

            // ballをリストに追加する
            removeBalls.Add(ball);

            // タッチ音の再生
            SoundManager.instance.PlaySE(SoundManager.SE.Destoy);

        }
    }

    /// <summary>
    /// ボムによる爆破
    /// </summary>
    void Explosion(Ball bom)
    {
        List<Ball> explosionList = new List<Ball>();

        // ボムを中心に爆破するボールを集める
        Collider2D[] hitObj = Physics2D.OverlapCircleAll(bom.transform.position, ParamsSO.Entity.bomRange);
        // 爆発音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Explosion);
        for (int i = 0; i < hitObj.Length; i++)
        {
            // ボールだったら爆破
            Ball ball = hitObj[i].GetComponent<Ball>();

            if (ball)
            {
                explosionList.Add(ball);
            }

        }

        // つなげたツムが3個以上ならツムを消す
        int removeCount = explosionList.Count;

        for (int i = 0; i < removeCount; i++)
        {
            explosionList[i].Explosion();

        }
        // 消えた数だけツムを追加する
        StartCoroutine(ballGenerater.Spown(removeCount));

        int score = removeCount * ParamsSO.Entity.ScorePint;

        AddScore(score);

        // ポイントの生成
        PointEffect(bom.transform.position, score);
    }

    /// <summary>
    /// ポイントの生成
    /// </summary>
    /// <param name="position">出現する場所 </param>
    /// <param name="score">表示するスコア</param>
    void PointEffect(Vector2 position, int score)
    {
        GameObject effectobj = Instantiate(pointEffectPrehab, position, Quaternion.identity);
        PointEffect pointEffect = effectobj.GetComponent<PointEffect>();
        pointEffect.Show(score);
    }

    /// <summary>
    /// リトライボタンの処理
    /// </summary>
    public void OnRetryButton()
    {
        // 決定音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        // 同じシーンを再読み込み(1秒間フェード処理)
        fade.FadeIn(1f, () => SceneManager.LoadScene("Main"));
    }

    /// <summary>
    /// タイトルボタンの処理
    /// </summary>
    public void OnTitleButton()
    {
        // 決定音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        // 同じシーンを再読み込み
        fade.FadeIn(1f, () => SceneManager.LoadScene("Title"));
    }

    /// <summary>
    /// ランキングボタンの処理
    /// </summary>
    public void OnRankingButton()
    {
        // 決定音の再生
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        // オンラインランキングに登録する
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    /// <returns></returns>
    public IEnumerator CameraShake()
    {
        // カメラを揺らす
        cameraShake.Shake(0.25f, 0.5f);

        yield return new WaitForSeconds(2.0f);
    }

    /// <summary>
    /// スコアがハイスコアを上回っていた時のスコア更新処理
    /// </summary>
    public void ChangeHightScore()
    {
        resultscoreText.text = score.ToString();
        if (highScore < score)
        {
            hiscore.SetActive(true);
            highScore = score;
            PlayerPrefs.SetInt("SCORE", highScore);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// リザルト画面でのスコア表示のTMPアニメーション処理
    /// </summary>
    public void ScoreUiEffect()
    {
        //DOTweenTMPAnimatorを作成
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(resultscoreText);

        //1文字ずつアニメーションを設定(iが何番目の文字かのインデックス)
        //Sequenceで全文字のアニメーションをまとめる
        var sequence = DOTween.Sequence();

        sequence.SetLoops(-1);//無限ループ設定


        //一文字ずつにアニメーション設定
        var duration = 0.2f;//1回辺りのTween時間
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            sequence.Join(DOTween.Sequence()
              //上に移動して戻る
              .Append(animator.DOOffsetChar(i, animator.GetCharOffset(i) + new Vector3(0, 10, 0), duration).SetEase(Ease.OutFlash, 2))
              //同時に1.2倍に拡大して戻る
              .Join(animator.DOScaleChar(i, 1.2f, duration).SetEase(Ease.OutFlash, 2))
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
    /// 選択した種類のボールの色を変える
    /// </summary>
    private void SelctBallColorChange()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            // nullチェック
            if (gameObjects[i] == null || currentDraggingBall == null)
            {
                return;
            }
            // 選択したツムが爆弾でないとき
            if (gameObjects[i].id != -1 || currentDraggingBall.id != -1)
            {
                // 距離が近ければ
                float distance = Vector2.Distance(gameObjects[i].transform.position, currentDraggingBall.transform.position);
                if (distance < ParamsSO.Entity.ballDistance||gameObjects[i].select==true)
                {
                    // 選んだツムと同じツム
                    if (gameObjects[i].id == currentDraggingBall.id && gameObjects[i].select == true)
                    {
                        // 色を変える
                        gameObjects[i].GetComponent<SpriteRenderer>().material.SetFloat("_Effect", 1f);
                        gameObjects[i].GetComponent<SpriteRenderer>().material.SetTexture("_MainTex", tumTex[gameObjects[i].id]);
                        gameObjects[i].GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f,0.8f);
                    }
                    else if (gameObjects[i].id == currentDraggingBall.id && gameObjects[i].select == false)
                    {
                        // 色を変える
                        gameObjects[i].GetComponent<SpriteRenderer>().material.SetFloat("_Effect", 1f);

                    }
                }
                // そうでなければ変更なし
                else 
                {
                    gameObjects[i].GetComponent<SpriteRenderer>().material.SetFloat("_Effect", 0);
                }
            }
        }
    }

    /// <summary>
    /// ボールの色と形をもとに戻す
    /// </summary>
    private void ReturnBallColor()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if(gameObjects[i] == null)
            {
                return;
            }

            gameObjects[i].GetComponent<SpriteRenderer>().material.SetFloat("_Effect", 0);
            gameObjects[i].GetComponent<SpriteRenderer>().color = Color.white;
            gameObjects[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            gameObjects[i].select = false;
        }
    }
}

