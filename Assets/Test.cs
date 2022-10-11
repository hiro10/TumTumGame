using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Test : MonoBehaviour
{
    TextMeshProUGUI tmpro;

    DOTweenTMPAnimator tmproAnimator; 
    // Start is called before the first frame update
    void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
        tmproAnimator = new DOTweenTMPAnimator(tmpro);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tmproAnimator.textInfo.characterCount; ++i)
        {
            tmproAnimator.DOShakeCharOffset(i, 3f, 50f, 3, fadeOut: false).SetLoops(-1);
        }
    }
}
