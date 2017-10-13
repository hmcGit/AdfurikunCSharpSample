using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieInterAdManager : MonoBehaviour {

    private AdfurikunMovieInterstitialUtility adutil;
    private bool initialized = false;

    private enum SCENE_STATE { MAIN, QUIT_WAIT, QUIT, END };
    private SCENE_STATE sceneState = SCENE_STATE.MAIN;

    [SerializeField]
    private Text resultText;

    public void Awake() {

        if (adutil == null) adutil = GameObject.Find("AdfurikunMovieInterstitialUtility").GetComponent<AdfurikunMovieInterstitialUtility>();
    }

	// Update is called once per frame
	void Update () {
        if (!initialized) {
            initialized = true;
            adutil.setMovieInterstitialSrcObject(this.gameObject);
        }
        switch (this.sceneState) {
            case SCENE_STATE.MAIN:
                break;
            case SCENE_STATE.QUIT_WAIT:
                this.sceneState = SCENE_STATE.QUIT;
                break;
            case SCENE_STATE.QUIT:
                this.sceneState = SCENE_STATE.END;
                //Application.Quit();
                break;
            case SCENE_STATE.END:
                break;
        }
    }
    /// <summary>
    /// 動画インタースティシャル広告を再生する
    /// </summary>
    public IEnumerator showInterAdMovie() {

        while (!adutil.isPreparedMovieInterstitial()) {
            yield return new WaitForSeconds(0.1f);
        }
#if UNITY_IOS
        //BGMをとめる
#endif
        adutil.playMovieInterstitial();
    }
    /// <summary>
    /// リワード動画再生時のアドフリ君処理
    /// </summary>
    /// <returns></returns>
    IEnumerator StartMovie() {

        while ( !adutil.isPreparedMovieInterstitial() ) {
            yield return new WaitForSeconds(0.1f);
        }

        adutil.playMovieInterstitial();
    }

    public void MovieInterstitialCallback(ArrayList vars) {
        int stateName = (int)vars[0];
        string appID = (string)vars[1];
        string adnetworkKey = (string)vars[2];

        AdfurikunMovieInterstitialUtility.ADF_MovieStatus state = (AdfurikunMovieInterstitialUtility.ADF_MovieStatus)stateName;

        switch (state) {
            case AdfurikunMovieInterstitialUtility.ADF_MovieStatus.PrepareSuccess:
                //準備完了
                Debug.Log("インタースティシャル動画：準備完了");
                resultText.text += "インタースティシャル動画：準備完了\n";
                break;
            case AdfurikunMovieInterstitialUtility.ADF_MovieStatus.StartPlaying:
                //再生開始
                Debug.Log("インタースティシャル動画：再生開始");
                resultText.text += "インタースティシャル動画：再生開始\n";
                break;
            case AdfurikunMovieInterstitialUtility.ADF_MovieStatus.FinishedPlaying:
                //再生完了
                Debug.Log("インタースティシャル動画：再生完了");
                Screen.orientation = ScreenOrientation.Portrait;
                resultText.text += "インタースティシャル動画：再生完了\n";
                break;
            case AdfurikunMovieInterstitialUtility.ADF_MovieStatus.FailedPlaying:
                //再生失敗
                Debug.Log("インタースティシャル動画：再生失敗");
                Screen.orientation = ScreenOrientation.Portrait;
                resultText.text += "インタースティシャル動画：再生失敗\n";
                break;
            case AdfurikunMovieInterstitialUtility.ADF_MovieStatus.AdClose:
                //動画を閉じた
                Debug.Log("インタースティシャル動画：動画を閉じた");
                Screen.orientation = ScreenOrientation.Portrait;
                resultText.text += "インタースティシャル動画：動画を閉じた\n";
                break;
            case AdfurikunMovieInterstitialUtility.ADF_MovieStatus.NotPrepared:
                Debug.Log("インタースティシャル動画：再生準備が出来ていない");
                resultText.text += "インタースティシャル動画：再生準備が出来ていない\n";
                break;
            default:
                Debug.Log("インタースティシャル動画：その他");
                resultText.text += "インタースティシャル動画：その他\n";
                Screen.orientation = ScreenOrientation.Portrait;
                return;
        }
    }


}
