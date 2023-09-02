using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;
 
public class ButtonUiAnim :UIBehaviour,IPointerDownHandler, IPointerUpHandler
{
    private Vector3 BaseScale;

    protected override void Start()
    {
        BaseScale = transform.localScale;
    }
   

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(BaseScale * 0.8f, 0.25f)
       .Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(BaseScale, 0.25f)
       .Play();
    }


}
