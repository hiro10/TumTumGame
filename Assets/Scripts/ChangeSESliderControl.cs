using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSESliderControl : MonoBehaviour
{
    // SE用のスライダー格納
    [SerializeField] Slider slider;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("SE_VOLUME");
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
         SoundManager.instance.ChangeVolumeSE(slider.value);
    }
}
