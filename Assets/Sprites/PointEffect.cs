using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointEffect : MonoBehaviour
{
    // スコアに応じて表示を変化
    // 上にあげる

    [SerializeField] Text text;
    
    public void Show(int score)
    {
        // 受け取ったスコアを表示する
        text.text = score.ToString();
        StartCoroutine(MoveUp());
    }
    
    /// <summary>
    /// スコアの位置を上にあげる
    /// (少しづつ挙げるのでコルーチンを使う) 
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveUp()
    { 
        for(int i =0; i<20; i++)
        {
            // 0.1秒おきに
            //yield return new WaitForSeconds(0.01f);

            yield return null;
            // 0.1f上にあげる
            transform.Translate(0, 0.1f, 0);
        }
        // 上がり切ったら破棄
        Destroy(gameObject, 0.2f);

    }
}
