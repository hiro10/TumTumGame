using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームを管理
public class GameSystem : MonoBehaviour
{
    // ツム生成用
    [SerializeField] BallGenerater ballGenerater = default;

    // ドラッグ中かの判定
    bool isDragging;

    //
    [SerializeField] List<Ball> removeBalls = new List<Ball>();

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        StartCoroutine(ballGenerater.Spown(200));
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
            Debug.Log("オブジェクトにHitした");
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
            Debug.Log("オブジェクトにHitした");
            Ball ball = hit.collider.GetComponent<Ball>();
            
            AddRemoveBall(ball);
        }
    }
    /// <summary>
    /// ドラッグ終了時
    /// </summary>
    private void OnDragEnd()
    {
        int removeCount = removeBalls.Count;
        for(int i = 0; i <removeCount;i++)
        {
            Destroy(removeBalls[i].gameObject);
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
        // リストがBallを持っていなかったら
        if(removeBalls.Contains(ball)==false)
        {
            // ballをリストに追加する
            removeBalls.Add(ball);
        }
        
    }
}
