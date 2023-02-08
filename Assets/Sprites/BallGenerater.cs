using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGenerater : MonoBehaviour
{
    // ball生成
    // ballのプレハブの用意
    // instantiateする
    [SerializeField] GameObject ballPrehab = default;

    // 画像の設定
    // 設定する画像の用意
    [SerializeField] Sprite[] ballSprites = default;

    // ボム
    [SerializeField] Sprite bombSprite = default;

    public Material materials;

    public int ballID;
    // マテリアル格納用（5つ）
    public Material ColorSet = default;


    /// <summary>
    /// ツムの生成
    /// </summary>
    public IEnumerator Spown(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 生成位置をランダムに
            Vector2 pos = new Vector2(Random.Range(-0.1f, 0.1f), 2f);
            // 生成 Quaternion.identity= 回転の初期値
            GameObject ball = Instantiate(ballPrehab, pos, Quaternion.identity);

            // 画像の設定
            // ボールの種類だけランダムに(ボムの時はidをー１)
            ballID = Random.Range(0, ballSprites.Length);
            // もしボムなら、idを-1それ以外なら今ままで道理
            if (Random.Range(0, 100) < ParamsSO.Entity.bomSpownRange)
            {
                ballID = -1;
                ball.GetComponent<Renderer>().material = materials;
                ball.GetComponent<SpriteRenderer>().sprite = bombSprite;
            }
            else
            {
                ball.GetComponent<SpriteRenderer>().sprite = ballSprites[ballID];
            }
            ball.GetComponent<Ball>().id = ballID;
            yield return new WaitForSeconds(0.04f);
        }
    }
}
