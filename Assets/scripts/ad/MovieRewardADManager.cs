using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieRewardADManager : MonoBehaviour {

    private AdfurikunMovieRewardUtility adutil;
    private bool initialized = false;
    private enum SCENE_STATE { MAIN, QUIT_WAIT, QUIT, END };
    private SCENE_STATE sceneState = SCENE_STATE.MAIN;

    [SerializeField]
    private Text resultText;
    public void Awake() {

        if (adutil == null) adutil = GameObject.Find("AdfurikunMovieRewardUtility").GetComponent<AdfurikunMovieRewardUtility>();

    }


    // Use this for initialization
    void Start() {
        adutil.initializeMovieReward();
    }

    /// <summary>
    /// リワード動画を再生する
    /// </summary>
    public void playRewardMovie() {

        StartCoroutine(StartMovie());
    }

    /// <summary>
    /// リワード動画再生時のアドフリ君処理
    /// </summary>
    /// <returns></returns>
    IEnumerator StartMovie() {

#if UNITY_EDITOR
        yield return null;
#elif UNITY_IOS
        BgmManager.Instance.StopImmediately();
        while (!adutil.isPreparedMovieReward()) {
            yield return new WaitForSeconds(0.2f);
        }

        StartCoroutine(freeHint());
        adutil.playMovieReward();
#else
        //リワード動画の準備ができるまでWaitして再生開始

        while (!adutil.isPreparedMovieReward()) {
            yield return new WaitForSeconds(0.2f);
        }

        adutil.playMovieReward();
#endif
    }

    // Update is called once per frame
    void Update() {
        if (!initialized) {
            initialized = true;
            adutil.setMovieRewardSrcObject(this.gameObject);
        }
        switch (this.sceneState) {
            case SCENE_STATE.MAIN:
                break;
            case SCENE_STATE.QUIT_WAIT:
                this.sceneState = SCENE_STATE.QUIT;
                break;
            case SCENE_STATE.QUIT:
                this.sceneState = SCENE_STATE.END;
                break;
            case SCENE_STATE.END:
                break;
        }
    }

    void MovieRewardCallback(ArrayList vars) {
        int stateName = (int)vars[0];
        string appID = (string)vars[1];
        string adnetworkKey = (string)vars[2];

        AdfurikunMovieRewardUtility.ADF_MovieStatus state = (AdfurikunMovieRewardUtility.ADF_MovieStatus)stateName;
        switch (state) {
            case AdfurikunMovieRewardUtility.ADF_MovieStatus.PrepareSuccess:
                //"準備完了"
                resultText.text += "リワード動画：準備完了\n";
                break;
            case AdfurikunMovieRewardUtility.ADF_MovieStatus.StartPlaying:
                //"再生開始"
                resultText.text += "リワード動画：再生開始\n";
                break;
            case AdfurikunMovieRewardUtility.ADF_MovieStatus.FinishedPlaying:
                //"再生完了"
                Screen.orientation = ScreenOrientation.Portrait;
                resultText.text += "リワード動画：再生完了\n";
                //ここで報酬を付与します
                break;
            case AdfurikunMovieRewardUtility.ADF_MovieStatus.FailedPlaying:
                //"再生失敗"
                Screen.orientation = ScreenOrientation.Portrait;
                resultText.text += "リワード動画：再生失敗\n";
                break;
            case AdfurikunMovieRewardUtility.ADF_MovieStatus.AdClose:
                //"動画を閉じた"
                Screen.orientation = ScreenOrientation.Portrait;
                resultText.text += "リワード動画：動画を閉じた\n";
                break;
            default:
                resultText.text += "リワード動画：その他\n";
                return;
        }
    }
}

