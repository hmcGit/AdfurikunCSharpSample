using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdfuri : MonoBehaviour {

    private void Start() {
        //MovieNativeADSelectStageManager.Instance.startNativeMovieAD(0);
    }
    public void onPushTestInterAd() {
        MovieInterAdManager movieAd = GameObject.Find("MovieInterAdManager").GetComponent<MovieInterAdManager>();
        StartCoroutine(movieAd.showInterAdMovie());
    }
    public void onPushTestNativeMovidAd() {
        MovieNativeADSelectStageManager.Instance.startNativeMovieAD(0);
    }
    public void onPushTestRewardMovieAd() {
        MovieRewardADManager rewardAd = GameObject.Find("MovieRewardAdManager").GetComponent<MovieRewardADManager>();
        rewardAd.playRewardMovie();
    }
}
