using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;
    private RewardedAd rewardedAd;

#if UNITY_ANDROID
    private string adUnitId = "ca-app-pub-3940256099942544/5224354917"; 

#elif UNITY_IPHONE
    private string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string adUnitId = "unused";
#endif

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob initialized!");
            LoadRewardedAd();
        });
    }

    // ===== Tải quảng cáo =====
    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading Rewarded Ad...");
        AdRequest adRequest = new AdRequest();

        RewardedAd.Load(adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError(" Failed to load rewarded ad: " + error);
                return;
            }

            Debug.Log(" Rewarded ad loaded!");
            rewardedAd = ad;

            RegisterEventHandlers(ad);
        });
    }

    // ===== Hiển thị quảng cáo =====
    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($" User earned reward: {reward.Type}, amount: {reward.Amount}");
                GameManager.instance.StartCoroutine(ReviveAfterAd());
            });
        }
        else
        {
            Debug.LogWarning(" Rewarded ad not ready yet.");
            LoadRewardedAd();
        }
    }

    // ===== Đăng ký sự kiện quảng cáo =====
    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log(" Ad closed, reloading...");
            LoadRewardedAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError(" Ad failed to show: " + error);
            LoadRewardedAd();
        };
    }

    // ===== Hồi sinh sau khi xem quảng cáo =====
    private IEnumerator ReviveAfterAd()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.RevivePlayer();
    }
}
