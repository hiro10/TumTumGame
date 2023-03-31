using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSoundSliderControl : MonoBehaviour
{
    // サウンド用のスライダー格納
    [SerializeField] Slider slider;
    
    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // スライダーを保存したBGM数値に
        slider.value = PlayerPrefs.GetFloat("BGM_VOLUME");
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // サウンドマネージャーの音量を更新
         SoundManager.instance.ChangeVolumeBGM(slider.value);
    }
}
