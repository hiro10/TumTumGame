using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int id;

    // 爆破エフェクト
    [SerializeField] GameObject explosonPrehab = default;

    /// <summary>
    /// 爆発の発生
    /// </summary>
    public void Explosion()
    {
        // 自分自身の破壊
        Destroy(gameObject);

        // 爆破エフェクトの生成
        GameObject explosion = Instantiate(explosonPrehab, transform.position, transform.rotation);

        Destroy(explosion, 0.2f);

    }

    public bool isBomb()
    {
        return id == -1;
    }
}
