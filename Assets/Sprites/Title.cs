using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Title);
    }

    public void OnStartButton()
    {
        // Mainシーンに遷移
        SceneManager.LoadScene("Main");
    }
}
