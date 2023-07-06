using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject screenExplanation;
    [SerializeField] GameObject bomTutorial;
    [SerializeField] GameObject gameRule;
    //[SerializeField] GameObject Panel;

    private void Awake()
    {
        InitTutorial();
    }
    /// <summary>
    /// チュートリアルウインドウ処理
    /// </summary>
    public void OnClickTutorialButton()
    {
        //Panel.SetActive(true);
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        tutorial.SetActive(true);
        // オプションウィンドウをだんだん拡大
        tutorial.gameObject.transform.DOScale(new Vector3(8f, 12f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickCloseButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Close);
        //Panel.SetActive(false);
        // オプションウィンドウをだんだん拡大
        tutorial.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(NonActiveTutorial);
        
    }

    private void NonActiveTutorial()
    {
        
        tutorial.SetActive(false);
    }
    /// <summary>
    /// 画面説明処理
    /// </summary>
    public void OnClickScreenExplanationButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        NonActiveTutorial();
        screenExplanation.SetActive(true);
        // オプションウィンドウをだんだん拡大
        screenExplanation.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickScreenExplanationCloseButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Close);
        // オプションウィンドウをだんだん拡大
        screenExplanation.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(NonActiveScreenExplanation);

    }

    private void NonActiveScreenExplanation()
    {
        screenExplanation.SetActive(false);
        tutorial.SetActive(true);
    }

    /// <summary>
    /// ボム説明
    /// </summary>
    /// 
    public void OnClickBomTutrialButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        NonActiveTutorial();
        bomTutorial.SetActive(true);
        // オプションウィンドウをだんだん拡大
        bomTutorial.gameObject.transform.DOScale(new Vector3(8f, 12f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickBomTutrialCloseButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Close);

        // オプションウィンドウをだんだん拡大
        bomTutorial.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(NonActiveBomTutorial);

    }
    private void NonActiveBomTutorial()
    {
        bomTutorial.SetActive(false);
        tutorial.SetActive(true);
    }

    /// <summary>
    /// ゲームルール説明
    /// </summary>
    /// 
    public void OnClickGameRuleButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        NonActiveTutorial();
       
        gameRule.SetActive(true);
        // オプションウィンドウをだんだん拡大
        gameRule.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
    }

    public void OnClickGameRuleCloseButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Close);
        // オプションウィンドウをだんだん拡大
        gameRule.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(NonActiveGameRuleTutorial);

    }
    private void NonActiveGameRuleTutorial()
    {
        
        gameRule.SetActive(false);
        tutorial.SetActive(true);
    }


    private void InitTutorial()
    {
        tutorial.transform.localScale = Vector3.zero;
        tutorial.SetActive(false);
        screenExplanation.transform.localScale = Vector3.zero;
        screenExplanation.SetActive(false);
        bomTutorial.transform.localScale = Vector3.zero;
        bomTutorial.SetActive(false);
        gameRule.transform.localScale = Vector3.zero;
        gameRule.SetActive(false);
    }
}
