using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;

public class AdManager : MonoBehaviour
{
    public UnityEvent OnAdLoaded = new UnityEvent();
    private BannerView bannerView;
    // Start is called before the first frame update
    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        RequestBanner();
    }

    private void OnDestroy()
    {
        bannerView.Destroy();
    }

    public void Hide()
    {
        if (bannerView != null)
            bannerView.Hide();
    }
    public void Show()
    {
        if (bannerView != null)
            bannerView.Show();
    }
    private void RequestBanner()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Clean up banner ad before creating a new one.
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

        //Register for ad events.
        bannerView.OnAdLoaded += HandleAdLoaded;
        bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
        bannerView.OnAdOpening += HandleAdOpened;
        bannerView.OnAdClosed += HandleAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        // Load a banner ad.
        bannerView.LoadAd(request);
    }

    #region Banner callback handlers

    private void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        MonoBehaviour.print(String.Format("Ad Height: {0}, width: {1}",
            bannerView.GetHeightInPixels(),
            bannerView.GetWidthInPixels()));
        Hide();
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
                "HandleFailedToReceiveAd event received with message: " + args.LoadAdError.GetMessage());
    }

    private void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    private void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    private void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion
}
