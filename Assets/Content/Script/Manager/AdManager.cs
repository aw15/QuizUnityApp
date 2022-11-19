using System;
using UnityEngine;
using UnityEngine.Advertisements;
 
public class AdManager : MonoBehaviour
{
    //¹è³Ê
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

#if UNITY_IOS
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
#elif UNITY_ANDROID
    [SerializeField] private string _androidAdUnitId = "Banner_Android";
#endif

    private string _adUnitId = null; // This will remain null for unsupported platforms.
 
    void Awake()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#else
        _adUnitId = _androidAdUnitId;
#endif
    }

    private void Start()
    {
        // Set the banner position:
        Advertisement.Banner.SetPosition(_bannerPosition);
        LoadBanner();
    }

    private void LoadBanner()
    {
        try
        {
            // Set up options to notify the SDK of load events:
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            // Load the Ad Unit with banner content:
            Advertisement.Banner.Load(_adUnitId, options);
        }
        catch (Exception e)
        {
            Debug.Log($"LoadBanner Error {e.Message}");
        }
    }
 
    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
    }
 
    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }
 
    public void ShowBannerAd()
    {
        try
        {
            // Set up options to notify the SDK of show events:
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(_adUnitId, options);
        }
        catch (Exception e)
        {
            Debug.Log($"ShowBannerAd Error: {e.Message}");
        }
    }
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }
    
    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
}