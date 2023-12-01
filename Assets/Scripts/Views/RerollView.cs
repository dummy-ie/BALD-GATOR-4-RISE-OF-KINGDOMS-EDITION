using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class RerollView : View
{
    public static RerollView Instance;
    UIDocument _document;
    VisualElement _root;
    Button _watchAd;
    Button _close;

    private bool _clickedAds = false;
    public bool ClickedAds
    {
        get { return _clickedAds; }
        set { _clickedAds = value; }
    }
    public override void Initialize()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;
        _watchAd = _root.Q<Button>("WatchAdButton");
        _close = _root.Q<Button>("CloseButton");
        _watchAd.clicked += () => {
            AdsManager.Instance.ShowRewardedAd();
            Hide();
        };
        _close.clicked += () => {
            Hide();
        };
    }
}
