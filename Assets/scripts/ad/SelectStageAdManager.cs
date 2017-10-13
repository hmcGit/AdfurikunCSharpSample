using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageAdManager : MonoBehaviour {

    private SnapScrollRect snapScroll;
    //private int prevPageIndex = 0;
    private bool isHidedMovieNative = false;
    private void Awake() {

        Debug.Log("SelectStage::Awake");
        if (snapScroll == null) snapScroll = GameObject.Find("Scroll View").GetComponent<SnapScrollRect>();
    }

    public void onChangePageAdAction() {
        int ipage = snapScroll.hIndex;
        MovieNativeADSelectStageManager.Instance.resetNativeMovieAD(ipage);

    }

}
