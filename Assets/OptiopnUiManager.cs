﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OptiopnUiManager : MonoBehaviour
{
    [SerializeField] GameObject optionBackPanel;
    public void OnOptionButton()
    {
        SoundManager.instance.PlaySE(SoundManager.SE.Decision);
        this.gameObject.SetActive(true);


        // オプションウィンドウをだんだん拡大
        this.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetLink(gameObject);
        optionBackPanel.SetActive(true);

    }

    public void OnOptionCloseButton()
    {
        optionBackPanel.SetActive(false);
        SoundManager.instance.PlaySE(SoundManager.SE.Close);
        // オプションウィンドウをだんだん拡大
        this.gameObject.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).SetLink(gameObject).OnComplete(NonActive);


    }

    private void NonActive()
    {
        this.gameObject.SetActive(false);
    }
}
