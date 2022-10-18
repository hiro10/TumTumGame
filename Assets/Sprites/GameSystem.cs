using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] Text scoreText = default;
    [SerializeField] Text higtscoreText = default;
    [SerializeField] Text resultscoreText = default;

    // ポイント生成用プレハブ
    [SerializeField] GameObject pointEffectPrehab = default;

    // 時間
    [SerializeField] Text timerText;
    int timeCount;

    // リザルト画面格納
    [SerializeField] GameObject resultPanel;

    [SerializeField] CameraShake cameraShake;

    bool gameOver;

    [SerializeField] Image coundDownicon;

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        score = 0;
        highScore = 0; //PlayerPrefs.GetInt("SCORE", 0);
        AddScore(0);
        scoreText.text = score.ToString();
        higtscoreText.text = highScore.ToString();

        timeCount = ParamsSO.Entity.timeCount;

        StartCoroutine(ballGenerater.Spown(ParamsSO.Entity.initBallCount));
        StartCoroutine(CountDown());

        resultPanel.SetActive(false);
        SoundManager.instance.PlayBGM(SoundManager.BGM.Main);
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        coundDownicon = GameObject.Find("Image").GetComponent<Image>();
    }

    /// <summary>
    /// カウントダウン
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

        // ゲーム内のスコアの更新
        ChangeHightScore();

        // リザルト画面を表示
        resultPanel.SetActive(true);
    }

    /// <summary>
    /// スコアを加える処理trititttttt
    /// </summary>
    /// <param name="point"></param>
    void AddScore(int point)
    {
        score += point;
        scoreText.text = score.ToString();
        timerText.text = timeCount.ToString();
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
            if (ball.id == currentDraggingBall.id)
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
        // すべての removeballを元に戻す
        for (int i = 0; i < removeCount; i++)
        {
            // 大きさを戻す
            removeBalls[i].transform.localScale = new Vector3(1.2f,1.2f,1);
            // 色を戻す
            removeBalls[i].GetComponent<SpriteRenderer>().color = Color.white;
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
            // リストに加えた時にボールを大きくする
            // TODO スクリプタブルオブジェクトにしよう大きさと色
            ball.transform.localScale = Vector3.one * 1.4f;
            // 色を変える
            ball.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.4f);

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
        // 同じシーンを再読み込み
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// タイトルボタンの処理
    /// </summary>
    public void OnTitleButton()
    {
        // 同じシーンを再読み込み
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// オプションボタンの処理
    /// </summary>
    public void OnRankingButton()
    {
        // 現在スコアがハイスコアを超えていたら
        //if (highScore < score)
        {
            // オンラインランキングに登録する
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
        }
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    /// <returns></returns>
    public IEnumerator CameraShake()
    {
        // カメラを揺らす
        cameraShake.Shake(0.25f, 0.1f);

        yield return new WaitForSeconds(2.0f);
    }

    /// <summary>
    /// スコアがハイスコアを上回っていた時のスコア更新処理
    /// </summary>
    public void ChangeHightScore()
    {
        resultscoreText.text = score.ToString();
        if(highScore<score)
        {
            highScore = score;
            PlayerPrefs.SetInt("SCORE", highScore);
            PlayerPrefs.Save();
        }
    }
}
