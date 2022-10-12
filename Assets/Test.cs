using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Test : MonoBehaviour
{
    // === 外部パラメーター（インスペクター表示）=============================

    public Transform moneyTextPos;      //「5000兆円」テキストのtransform
    public Transform hoshiiTextPos;     //「欲しい！」テキストのtransform

    public float shakePower;            // 揺らす強さ


    // === 内部パラメーター ==================================================

    Vector3 moneyTextInitPos;           // 開始時の位置
    Vector3 hoshiiTextInitPos;


    // === コード（MonoBehaviour基本機能の実装）==============================

    private void Start()
    {
        // 開始時の位置を取得
        moneyTextInitPos = moneyTextPos.position;
        hoshiiTextInitPos = hoshiiTextPos.position;
    }


    private void Update()
    {
        // ランダムに揺らす
        moneyTextPos.position = moneyTextInitPos + Random.insideUnitSphere * shakePower;
        hoshiiTextPos.position = hoshiiTextInitPos + Random.insideUnitSphere * shakePower;
    }
}
