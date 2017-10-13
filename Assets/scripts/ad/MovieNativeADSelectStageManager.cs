using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieNativeADSelectStageManager : SingletonMonoBehaviour<MovieNativeADSelectStageManager>
{

    private static AdfurikunMovieNativeAdViewUtility adMovieNativeUtil;


    private enum SCENE_STATE { MAIN, QUIT_WAIT, QUIT, END };
    private static SCENE_STATE sceneState = SCENE_STATE.MAIN;

    private static bool isInit = false;
    private static int frameId = 1;
    public void Awake()
    {
        if (adMovieNativeUtil == null) adMovieNativeUtil = GameObject.Find("AdfurikunMovieNativeAdViewUtility").GetComponent<AdfurikunMovieNativeAdViewUtility>();

        if (this != Instance)
        {
            //    Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

    }
    public void startNativeMovieAD(int id)
    {
        Debug.Log("MovieNative::StartNativeMovie");
        frameId = id;
        if (!isInit) {
            Debug.Log("Native Init");
            if (adMovieNativeUtil == null) adMovieNativeUtil = GameObject.Find("AdfurikunMovieNativeAdViewUtility").GetComponent<AdfurikunMovieNativeAdViewUtility>();
            adMovieNativeUtil.initializeMovieNativeAdView();
            adMovieNativeUtil.setMovieNativeAdViewSrcObject(this.gameObject);
            isInit = true;
        }
        Debug.Log("MovieNative::before loadNativeMovie");
        adMovieNativeUtil.loadMovieNativeAdView();
    }
    public void hideMovieNative()
    {
        adMovieNativeUtil.hideMovieNativeAdView();
        sceneState = SCENE_STATE.MAIN;
    }
    private void setNativeMovieAD()
    {

        Debug.Log("setNativeAd");
        var basew = 1080.0f;
        var baseh = 1920.0f;
        GameObject target = GameObject.Find("movieAdFrame" + frameId.ToString());

        if (target == null) Debug.Log("null target");
        //Debug.Log(target.name);

        var w = target.GetComponent<RectTransform>().rect.width;
        var h = target.GetComponent<RectTransform>().rect.height;
        float wRatio = Screen.width / basew;
        float hRatio = Screen.height / baseh;
        var x = target.transform.position.x - (w * wRatio) / 2;
        var y = Screen.height - (target.transform.position.y + (h*hRatio) / 2);

#if UNITY_IOS
        adMovieNativeUtil.setMovieNativeAdView(x, y, w * wRatio, h * hRatio);
        adMovieNativeUtil.setMovieNativeAdViewFrame(x, y, w * wRatio, h * hRatio);
#else
        adMovieNativeUtil.setMovieNativeAdView(x, y, w * wRatio, h * hRatio);
        adMovieNativeUtil.setMovieNativeAdViewFrame(x, y, w * wRatio, h * hRatio);

#endif
        //Debug.Log(x+","+y+","+w*wRatio +","+ h*hRatio);
        adMovieNativeUtil.playMovieNativeAdView();
    }
    public void resetNativeMovieAD(int ipage) {

        var basew = 1080.0f;
        var baseh = 1920.0f;
        GameObject target = GameObject.Find("movieAdFrame" + ipage.ToString());

        if (target == null) Debug.Log("null target");

        var w = target.GetComponent<RectTransform>().rect.width;
        var h = target.GetComponent<RectTransform>().rect.height;
        float wRatio = Screen.width / basew;
        float hRatio = Screen.height / baseh;
        var x = target.transform.position.x - (w * wRatio) / 2;
        var y = Screen.height - (target.transform.position.y + (h * hRatio) / 2);

#if UNITY_IOS
        adMovieNativeUtil.setMovieNativeAdView(x, y, w * wRatio, h * hRatio);
        adMovieNativeUtil.setMovieNativeAdViewFrame(x, y, w * wRatio, h * hRatio);
#else
        adMovieNativeUtil.setMovieNativeAdView(x, y, w * wRatio, h * hRatio);
        adMovieNativeUtil.setMovieNativeAdViewFrame(x, y, w * wRatio, h * hRatio);

#endif
        adMovieNativeUtil.playMovieNativeAdView();
    }
    // Update is called once per frame
    void Update () {

        if (!isInit) {
            isInit = true;
            adMovieNativeUtil.setMovieNativeAdViewSrcObject(this.gameObject);
        }
        switch (sceneState) {
            case SCENE_STATE.MAIN:
                break;
            case SCENE_STATE.QUIT_WAIT:
                sceneState = SCENE_STATE.QUIT;
                break;
            case SCENE_STATE.QUIT:
                sceneState = SCENE_STATE.END;
                //Application.Quit();
                break;
            case SCENE_STATE.END:
                break;
        }
    }

    void MovieNativeAdViewCallback(ArrayList vars) {

        Debug.Log("MovieNativeAdViewCallback");
        int stateName = (int)vars[0];
        string appID = (string)vars[1];
        string errorCode = (string)vars[2];


        AdfurikunMovieNativeAdViewUtility.ADF_MovieStatus state = (AdfurikunMovieNativeAdViewUtility.ADF_MovieStatus)stateName;
        switch (state) {

            case AdfurikunMovieNativeAdViewUtility.ADF_MovieStatus.LoadFinish:
                //"ネイティブ広告の読み込み成功"
                Debug.Log("Load Success");
                setNativeMovieAD();
                break;
            case AdfurikunMovieNativeAdViewUtility.ADF_MovieStatus.LoadError:
                //"ネイティブ広告の読み込み失敗"
                Debug.Log("--- LoadError+" + errorCode);
                break;
            default:
                Debug.Log("--- other State");
                adMovieNativeUtil.loadMovieNativeAdView();
                return;
        }
    }
}

