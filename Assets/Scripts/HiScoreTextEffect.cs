using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using System.Linq;

public class HiScoreTextEffect : MonoBehaviour
{
    public TextMeshProUGUI hiscoreText = default;
    DOTweenTMPAnimator animator;
  
    public void HiScoreUiEffect()
    {
        animator = new DOTweenTMPAnimator(hiscoreText);
        //1文字ずつアニメーションを設定(iが何番目の文字かのインデックス)
        //Sequenceで全文字のアニメーションをまとめる
        var sequence = DOTween.Sequence();

        sequence.SetLoops(-1);//無限ループ設定
       
        //一文字ずつにアニメーション設定
        var duration = 0.2f;//1回辺りのTween時間
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {

            sequence.Join(DOTween.Sequence()

               //同時に色を黄色にして戻す
               .Join(animator.DOColorChar(i, Color.white, duration * 0.5f).SetLoops(2, LoopType.Yoyo))
               //アニメーション後、1秒のインターバル設定
               .AppendInterval(1f)
               //開始は0.15秒ずつずらす
               .SetDelay(0.15f * i)
             );
        }
    }
}
