using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RankingUiAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        transform.DOScale(Vector3.one, 0.25f)
       .Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClose()
    {
        transform.DOScale(Vector3.zero, 0.25f)
      .Play();
    }
}
