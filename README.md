# AdfurikunCSharpSample

UnityでのAdfurikun（アドフリくん）動画広告C#サンプルコード

インタースティシャル動画、リワード動画、ネイティブ動画広告に対応しています。

## Description

Unityで[Adfurikun（アドフリくん）](https://adfurikun.jp/adfurikun/)を利用する場合
動作サンプルにC#がないので作成しました。

なんでjsしかないの？アドフリさん。以下に対応しています

- インタースティシャル動画広告
- リワード動画広告
- ネイティブ動画広告
***
<動作環境>

- Unity5.6.3f1
- adfurikunSDK_Unity_moviereward_2_11_0
- Android7.1.2(Nexus5x)
- iOS11.0.2/10.3.3(iPhone6Plus/iPhone5c)

***DEMO:***

![adfuri](https://user-images.githubusercontent.com/7788005/31538021-2cc88f02-b03f-11e7-946b-2e263d5545fe.gif)

動画ネイティブの表示（Android)

スクロールビューでのページ切り替えに追従させています

## Features

- ネイティブ動画広告表示
- インタースティシャル動画広告表示
- リワード動画広告表示


## Requirement

-[Adfurikunの動画SDK Unity](https://adfurikun.jp/adfurikun/download)を導入して下さい

## Installation

    $ git clone git@github.com:hmcGit/AdfurikunCSharpSample.git

1. Android/iOSにSwitchPlatformします
2. AdfurikunのUnitySDKを導入　[Adfurikun動画SDK Unity](https://adfurikun.jp/adfurikun/download)　


adfurikunPlugin_googlelib/adfurikunPlugins_2_xx_x.unitypackageなどからインポートします。

AndroidManifest.xmlはインポートされる物をそのまま利用してビルドできます。
 
3.testAdMainシーンを開きます

もしも2.のアドフリSDKを導入前にシーンを開いた場合、下記のヒエラルキーにあるPrefabが壊れていると思います。

その場合はAssets/Plugins/Adfurikunからヒエラルキー上にセットしてください。

- AdfurikunMovieNativeAdViewUtility
- AdfurikunMovieInterstitialUtility
- AdfurikunMovieRewardUtility


![h1](https://user-images.githubusercontent.com/7788005/31546764-af0a5d9a-b05f-11e7-9c71-bec53fc352ae.jpg)

4.AdfurikunXXXUtilityに広告IDをセットします

![h2](https://user-images.githubusercontent.com/7788005/31546960-7e16f742-b060-11e7-90e2-df0573d69451.jpg)

それぞれインスペクター上でIDをセットしてください。テスト用のIDほしいですよね。アドフリさんお願いします。

## Usage

1.実行したら各ボタンを押して下さい。
画像はエディター実行時のものです。

便宜上画像では3画面を1つにくっつけていますが
各画面はスワイプで移動できます。
![h3](https://user-images.githubusercontent.com/7788005/31547260-931ade6e-b061-11e7-9759-ea04d9d5a1d7.jpg)

## Specification

1.スクリプト本体はscripts/ad以下のものです

- MovieInterAdManager.cs インタースティシャル動画広告用

　利用例）コルーチンでの呼び出しとして定義しています。
~~~
MovieInterAdManager movieAd = GameObject.Find("MovieInterAdManager").GetComponent<MovieInterAdManager>();
StartCoroutine(movieAd.showInterAdMovie());
~~~

---

- MovieRewardAdManager.cs　リワード動画広告用

 利用例）通常のメソッドです。
~~~
MovieRewardADManager rewardAd = GameObject.Find("MovieRewardAdManager").GetComponent<MovieRewardADManager>();
rewardAd.playRewardMovie();
~~~

---

- MovieNativeAdSelectStageManager.cs ネイティブ動画広告用
　（名前変え忘れた・・・）
 
 　利用例）シングルトンとして定義しています。引数はページ番号です。
~~~
MovieNativeADSelectStageManager.Instance.startNativeMovieAD(0);
~~~
 
 呼び出し例は、scripts/TestAdfuri.csを参照してください。

---
 
- スクロールビューについて

　[SnapScrollRect](https://github.com/cyario/SnapScroll)を利用しています。ピタッとスナップして気持ちよいです。

---
 
- ページ切り替え時のネイティブ動画位置調整について
　
 
 Canvas-Scroll ViewのonValueChanged()にてSelectStageAdManager.onChangePageAdAction()を呼んでいます。
 
 onChangePageAdAction()では、MovieNativeADSelectStageManager::resetNativeMovieAd(int page)を呼んでいます。
 
 ネイティブ動画の表示位置を設定するために「movieAdFrame0～2」のオブジェクトが各ページに設定されています。
　
## Notice

1.インスペクターでのアタッチについて

　MovieInterAdManager.cs/MovieRewardADManager.cs/MovieNativeADSelectStageManager.csは
 
 それぞれ別のオブジェクトにアタッチしています。
 
 これは同じオブジェクトにアタッチすると、それぞれのAdfuriSDKのCallbackが上手く呼ばれなくなるためです。
 
 setMovieNativeAdViewSrcObjectなど、CallbackのあるObjectを設定する際に工夫すればいけるかもしれません。
 
 ---
 
 2.iOSで動画再生時にBGMがかぶる
 
 これはAndroidでは発生しないのですが、iOSではリワード・インタースティシャル動画が再生時に、
 
 アプリ側のBGMが再生されてままでかぶってしまいます。
 
 解決方法がわからなかったので、iOSのみBGM停止処理、終了後に再生とすることで対処しました。
 
 ---
 
 3.iOSでのビルド
 
 Editor/AdfurikunMovieRewardPostProcessにて以下を設定してビルドしました。
 ~~~
 private static bool useThis = true;
 private static bool is_iOS9orOver = true;
 ~~~
 
 - コンパイルフラグの設定
 Plugins/iOS/adnetworks以下にある各アドネットワークフォルダにそれぞれ以下のファイルがあります。
 
 MovieInterStitial600x.mm/MovieReward600x.mm/MovieNative600x.mm

上記には「-fobjc-arc」追記しました。

基本的にはこれだけでビルドできます。以前よりずいぶん楽になった印象です。
## Author

[@hmc_j](https://twitter.com/hmc_j)

[www.alice3.net](http://www.alice3.net)


