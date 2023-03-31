using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapParticle : MonoBehaviour
{
    // タップエフェクト
    public GameObject prefab;

    // 削除までの時間
    public float deleteTime = 1.0f;

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //マウスカーソルの位置を取得。
            var mousePosition = Input.mousePosition;

            mousePosition.z = 30f;

            GameObject clone = Instantiate(prefab, Camera.main.ScreenToWorldPoint(mousePosition),
                Quaternion.identity);

            // 1秒後に削除
            Destroy(clone, deleteTime);
        }
    }
}
