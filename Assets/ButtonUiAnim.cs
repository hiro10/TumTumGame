using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;
 
public class ButtonUiAnim :UIBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    [SerializeField] private float Rate;
    private Vector3 BaseScale;

    protected override void Start()
    {
        BaseScale = transform.localScale;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(BaseScale * Rate, 0.25f)
        .Play();
    }


    public void OnPointerExit(PointerEventData eventData)
    {

        transform.DOScale(BaseScale, 0.25f)
        .Play();

    }

    public void OnPointerDown
        (PointerEventData eventData)
    {
        transform.DOScale(BaseScale * 0.8f, 0.25f)
       .Play();
    }


}
