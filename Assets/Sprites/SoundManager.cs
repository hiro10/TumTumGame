using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 音を鳴らす
    // AudioSource:: スピーカー
    // AudioClip:: CD（素材）

    //BGM
    [SerializeField] AudioSource audioSourceBGM;
    [SerializeField] AudioClip[] audioClipsBGM;

    //SE
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] AudioClip[] audioClipsSE;

    /// <summary>
    /// BGMの列挙型
    /// </summary>
    public enum BGM
    {
        Title, // タイトル画面用の曲
        Main   // ゲーム画面用の曲
    }

    /// <summary>
    /// BGMの列挙型
    /// </summary>
    public enum SE
    {
        Destoy, // ボールが破裂するとき
        Touch,   // ボールに触れた時
        Explosion // 爆弾が爆発するとき

    }

    // シングルトンにする
    // ゲーム内にただ一つのだけのもの（シーンが変わっても破壊されない）
    // どのコードからもアクセスしやすい
    public static SoundManager instance;
    /// <summary>
    ///  シングルトンのお約束
    /// </summary>
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    public void PlayBGM(BGM bgm)
    {
        // 列挙型から流したいBGMを選ぶ（intでキャスト）
        audioSourceBGM.clip = audioClipsBGM[(int)bgm];
        audioSourceBGM.Play();
    }

    /// <summary>
    /// SEの再生
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(SE se)
    {
        audioSourceSE.clip = audioClipsSE[(int)se];
        audioSourceSE.Play();
    }

}
