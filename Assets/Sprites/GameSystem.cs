using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲームを管理
public class GameSystem : MonoBehaviour
{
    // ツム生成用
    [SerializeField] BallGenerater ballGenerater = default;

    // ドラッグ中かの判定
    bool isDragging;

    //
    [SerializeField] List<Ball> removeBalls = new List<Ball>();

    // 今選択しているボールを格納
    Ball currentDraggingBall;

    // スコア:TODO別クラスにしよう
    int score;
    [SerializeField] Text scoreText = default;

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        score = 0;
        AddScore(0);
        scoreText.text = score.ToString();

        StartCoroutine(ballGenerater.Spown(ParamsSO.Entity.initBallCount));
    }

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
        else if(isDragging)
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
        RaycastHit2D hit = Physics2D.Raycast(mousePosition,Vector2.zero); 

        // ボールにヒットしたか
        if(hit&& hit.collider.GetComponent<Ball>())
        {
            Ball ball = hit.collider.GetComponent<Ball>();
            AddRemoveBall(ball);

            isDragging = true;
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
                Destroy(removeBalls[i].gameObject);
            }
            // 消えた数だけツムを追加する
            StartCoroutine(ballGenerater.Spown(removeCount));
            AddScore(removeCount *ParamsSO.Entity.ScorePint);
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
        if(removeBalls.Contains(ball)==false)
        {
            // ballをリストに追加する
            removeBalls.Add(ball);
        } 
    }
}
