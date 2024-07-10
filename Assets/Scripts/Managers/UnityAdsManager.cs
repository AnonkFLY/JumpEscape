using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _placementID;
    [SerializeField] private bool _testMode = true;
    [SerializeField] private bool _enabled = false;

    private string _gameId;

    void Awake()
    {
        if (!_enabled) return;
        _gameId = _androidGameId;
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Debug.Log("Ads Initialing");
            Advertisement.Initialize(_gameId, _testMode, this);
            Debug.Log("Ads Initialing over?");
        }
        else
        {
            Debug.LogError("Advertisement is not Supported");
        }

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Show Ads Start:" + placementId);
    }
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Loaded Ads complete:" + placementId);
        //Advertisement.Show(_placementID, this);
    }
    public void ShowAds()
    {
        Advertisement.Show(_placementID, this);

    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Click Ads:" + placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Show Ads complete:" + placementId + ",State:" + showCompletionState);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        //
        Advertisement.Load(_placementID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }



    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Load Ads Failed[{error}]:{placementId},{message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Show Ads Failed[{error}]:{placementId},{message}");
    }
}
