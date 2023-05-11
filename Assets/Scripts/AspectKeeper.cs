using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectKeeper : MonoBehaviour
{
    //対象とするカメラ
    [SerializeField] private Camera targetCamera;
    
    //目的解像度
    [SerializeField] private Vector2 aspectVec; 

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //画面のアスペクト比
        var screenAspect = Screen.width / (float)Screen.height;
        
        //目的のアスペクト比
        var targetAspect = aspectVec.x / aspectVec.y;

        //目的アスペクト比にするための倍率
        var magRate = targetAspect / screenAspect;

        //Viewport初期値でRectを作成
        var viewportRect = new Rect(0, 0, 1, 1); 

        if (magRate < 1)
        {
            viewportRect.width = 1; //使用する横幅を変更
           viewportRect.x = 0.5f - viewportRect.width * 0.5f;//中央寄せ
        }
        else
        {
            viewportRect.height = 1; //使用する縦幅を変更
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;//中央余生
        }

        targetCamera.rect = viewportRect; //カメラのViewportに適用
    }
}
