using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUiManager : MonoBehaviour
{
    // 点滅スピード
    public float speed = 1.0f;

    // 点滅させたいUI格納
    [SerializeField] private Text tapText;

    // 点滅頻度
    private float time;

    // 点滅解除後に表示させるUI
    [SerializeField] GameObject StartMenu;

    // オプションメニュー
    [SerializeField] GameObject Optionmenu;

    void Start()
    {

        StartMenu.SetActive(false);
    }
        

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {

        tapText.color = GetAlphaColor(tapText.color);

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(TapText());
        }

    }

    //Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }

    IEnumerator TapText()
    {
        speed = 4f;
        yield return new WaitForSeconds(1f);
        tapText.gameObject.SetActive(false);
        StartMenu.SetActive(true);

        //FadeManager.Instance.LoadScene("Main", 1f);
    }
}
