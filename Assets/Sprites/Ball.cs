using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int id;

    // 選択されているか
    public bool select = false;

    // 爆破エフェクト
    [SerializeField] GameObject explosonPrehab = default;

    public Material material;
    [SerializeField] Texture texture;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // マテリアルのシェーダーエフェクトを0に
        material.SetFloat("_Effect", 0);
    }

    /// <summary>
    /// 爆発の発生
    /// </summary>
    public void Explosion()
    {
        // 自分自身の破壊
        Destroy(gameObject);

        // 爆破エフェクトの生成
        GameObject explosion = Instantiate(explosonPrehab, transform.position, transform.rotation);

        Destroy(explosion, 0.3f);

    }

    /// <summary>
    /// ボムかどうかの判定
    /// </summary>
    /// <returns></returns>
    public bool isBomb()
    {
        return id == -1;
    }
}
