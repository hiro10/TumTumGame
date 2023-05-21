using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUiChange : MonoBehaviour
{
    [SerializeField] Sprite[] onPauseButtonImage;

    bool onPauseButton;

    private void Awake()
    {
        onPauseButton = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPauseButton()
    {
        if (onPauseButton == false)
        {
            this.gameObject.GetComponent<Image>().sprite = onPauseButtonImage[1];
            onPauseButton = true;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = onPauseButtonImage[0];
            onPauseButton = false;
        }
    }
}
