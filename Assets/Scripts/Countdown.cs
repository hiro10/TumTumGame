using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class Countdown : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _textCountdown;

	[SerializeField]
	private Image _imageMask;

	private CancellationTokenSource cancellationTokenSource;

	const int COUNTDOWN_MAX = 3;

	void Start()
	{
		_textCountdown.text = "";
	}

    async public void OnClickButtonStart()
	{
		cancellationTokenSource = new CancellationTokenSource();
		if (SoundManager.instance!=null)
		{
			await CountDown(cancellationTokenSource.Token);
		}
		else
        {
			_textCountdown.gameObject.SetActive(false);
			_imageMask.gameObject.SetActive(false);
		}
	}

	private async UniTask CountDown(CancellationToken cancellationToken)
	{
		for (int i = COUNTDOWN_MAX; i >= 0; i--)
		{
			_imageMask.gameObject.SetActive(true);
			_textCountdown.gameObject.SetActive(true);
			if (i == 0)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
				SoundManager.instance.PlaySE(SoundManager.SE.CountZero);
				_textCountdown.text = "GO!";
				await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
				Destroy(this);
				cancellationToken.ThrowIfCancellationRequested();
			}
			else
			{
				await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
				SoundManager.instance.PlaySE(SoundManager.SE.CountDownSe);
				_textCountdown.text = i.ToString();
			}
		}
	}

    private void OnDestroy()
    {
		_textCountdown.gameObject.SetActive(false);
		_imageMask.gameObject.SetActive(false);
	}
}
