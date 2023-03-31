using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Countdown : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _textCountdown;

	[SerializeField]
	private Image _imageMask;

	void Start()
	{
		_textCountdown.text = "";
	}

	public void OnClickButtonStart()
	{
		StartCoroutine(CountdownCoroutine());
	}

	IEnumerator CountdownCoroutine()
	{
		_imageMask.gameObject.SetActive(true);
		_textCountdown.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySE(SoundManager.SE.CountDownSe);
		_textCountdown.text = "3";
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySE(SoundManager.SE.CountDownSe);
		_textCountdown.text = "2";
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySE(SoundManager.SE.CountDownSe);
		_textCountdown.text = "1";
		yield return new WaitForSeconds(1.0f);
		SoundManager.instance.PlaySE(SoundManager.SE.CountZero);
		_textCountdown.text = "GO!";
		yield return new WaitForSeconds(1.0f);

		_textCountdown.text = "";
		_textCountdown.gameObject.SetActive(false);
		_imageMask.gameObject.SetActive(false);
	}
}
