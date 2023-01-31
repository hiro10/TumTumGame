using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UntyAdsManager : MonoBehaviour
{
    public static UntyAdsManager Instance { get; private set; }
    // 広告処理

#if UNITY_IOS
   private string gameId = "5127514";
#elif UNITY_ANDROID
    private string gameId = "5127515";
    public const string BannerId = "Banner_Android";
#endif

    void Start()
    {
        // シングルトンに
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            Advertisement.Initialize(gameId);
            StartCoroutine(ShowBannerWhenReady());
        }
    }
    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(BannerId))
        {
            yield return new WaitForSeconds(0.5f);
        }

        // バナーの位置変更
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(BannerId);
    }
}

    
