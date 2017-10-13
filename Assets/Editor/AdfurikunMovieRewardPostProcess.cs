#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
#else

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using System.IO;

public class AdfurikunMovieRewardPostProcess {

	//このスクリプトを使うか
	private static bool useThis = true;
	//iOS9 SDK以上か?
	private static bool is_iOS9orOver = true;

	private static bool use_AppLovin = true; //AppLovinを有効にしたい場合はtrue
	private static bool use_AdColony = true; //AdColonyを有効にしたい場合はtrue
	private static bool use_UnityAds = true; //UnityAdsを有効にしたい場合はtrue
	private static bool use_Maio = true; //Maioを有効にしたい場合はtrue
	private static bool use_Tapjoy = true; //Tapjoyを有効にしたい場合はtrue
	private static bool use_Vungle = true; //Vungleを有効にしたい場合はtrue
	private static bool use_SmaadVideo = true; //SMAAD Videoを有効にしたい場合はtrue
	private static bool use_Five = true; //Fiveを有効にしたい場合はtrue

	// ビルド時に実行される
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path) {

		if (buildTarget == BuildTarget.iOS) {
			if(!useThis){return;}

			string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
			PBXProject proj = new PBXProject();
			proj.ReadFromString(File.ReadAllText(projPath));

			string plistPath = Path.Combine (path, "Info.plist");
			var plist = new PlistDocument ();
			plist.ReadFromFile (plistPath);

			string target = proj.TargetGuidByName("Unity-iPhone");
			string frameworkSuffix = (is_iOS9orOver) ? "tbd" : "dylib";

			//Adfurikunの、必要な標準frameworkを追加します。
			proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
			proj.AddFrameworkToProject(target, "AdSupport.framework", false);
			proj.AddFrameworkToProject(target, "WebKit.framework", false);
			//
			//アドネットワークごとの、必要な標準frameworkを追加します。
			//AppLovin
			if(use_AppLovin){
				proj.AddFrameworkToProject(target, "AdSupport.framework", false);
				proj.AddFrameworkToProject(target, "AVFoundation.framework", false);
				proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
				proj.AddFrameworkToProject(target, "CoreGraphics.framework", false);
				proj.AddFrameworkToProject(target, "CoreMedia.framework", false);
				proj.AddFrameworkToProject(target, "MediaPlayer.framework", false);
				proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
				proj.AddFrameworkToProject(target, "StoreKit.framework", false);
				proj.AddFrameworkToProject(target, "UIKit.framework", false);
			}

			//AdColony
			if(use_AdColony){
				proj.AddFrameworkToProject(target, "AdSupport.framework", false);
				proj.AddFrameworkToProject(target, "AudioToolbox.framework", false);
				proj.AddFrameworkToProject(target, "AVFoundation.framework", false);
				proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
				proj.AddFrameworkToProject(target, "EventKit.framework", false);
				proj.AddFrameworkToProject(target, "JavaScriptCore.framework", true); //Optional
				proj.AddFrameworkToProject(target, "MessageUI.framework", false);
				proj.AddFrameworkToProject(target, "Social.framework", false);
				proj.AddFrameworkToProject(target, "StoreKit.framework", false);
				proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
				proj.AddFrameworkToProject(target, "WatchConnectivity.framework", true);//Optional
				proj.AddFrameworkToProject(target, "WebKit.framework", true);//Optional
				proj.AddFileToBuild(target, proj.AddFile(
					"usr/lib/libz.1.2.5."+frameworkSuffix, "libz.1.2.5."+frameworkSuffix, PBXSourceTree.Sdk));

				//プライバシー設定
				plist.root.SetString("NSCalendarsUsageDescription", "Adding events");
				plist.root.SetString("NSPhotoLibraryUsageDescription", "Taking selfies");
				plist.root.SetString("NSCameraUsageDescription", "Taking selfies");
				plist.root.SetString("NSMotionUsageDescription", "Interactive ad controls");
			}

			//UnityAds
			if(use_UnityAds){
				proj.AddFrameworkToProject(target, "AdSupport.framework", false);
				proj.AddFrameworkToProject(target, "AVFoundation.framework", false);
				proj.AddFrameworkToProject(target, "StoreKit.framework", false);
				proj.AddFrameworkToProject(target, "CFNetwork.framework", false);
				proj.AddFrameworkToProject(target, "CoreFoundation.framework", false);
				proj.AddFrameworkToProject(target, "CoreMedia.framework", false);
				proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
				proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
			}

			//Maio
			if(use_Maio){
				proj.AddFrameworkToProject(target, "MobileCoreServices.framework", false);
				proj.AddFrameworkToProject(target, "CoreMedia.framework", false);
			}

			//TapJoy
			if(use_Tapjoy){
				proj.AddFrameworkToProject(target, "AdSupport.framework", false);
				proj.AddFrameworkToProject(target, "CFNetwork.framework", false);
				proj.AddFrameworkToProject(target, "CoreData.framework", false);
				proj.AddFrameworkToProject(target, "CoreGraphics.framework", false);
				proj.AddFrameworkToProject(target, "CoreLocation.framework", false);
				proj.AddFrameworkToProject(target, "CoreMotion.framework", false);
				proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
				proj.AddFrameworkToProject(target, "EventKit.framework", false);
				proj.AddFrameworkToProject(target, "EventKitUI.framework", false);
				proj.AddFrameworkToProject(target, "Foundation.framework", false);
				proj.AddFrameworkToProject(target, "MapKit.framework", false);
				proj.AddFrameworkToProject(target, "MediaPlayer.framework", false);
				proj.AddFrameworkToProject(target, "MessageUI.framework", false);
				proj.AddFrameworkToProject(target, "MobileCoreServices.framework", false);
				proj.AddFrameworkToProject(target, "PassKit.framework", true);//Optional
				proj.AddFrameworkToProject(target, "QuartzCore.framework", false);
				proj.AddFrameworkToProject(target, "Security.framework", false);
				proj.AddFrameworkToProject(target, "Social.framework", true);//Optional
				proj.AddFrameworkToProject(target, "StoreKit.framework", false);
				proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
				proj.AddFrameworkToProject(target, "Twitter.framework", false);
				proj.AddFrameworkToProject(target, "UIKit.framework", false);
				proj.AddFrameworkToProject(target, "ImageIO.framework", false);
				proj.AddFileToBuild(target, proj.AddFile(
					"usr/lib/libxml2."+frameworkSuffix, "libxml2."+frameworkSuffix, PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile(
					"usr/lib/libc++."+frameworkSuffix, "libc++."+frameworkSuffix, PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile(
					"usr/lib/libz.1.2.5."+frameworkSuffix, "libz.1.2.5."+frameworkSuffix, PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile(
					"usr/lib/libsqlite3.0."+frameworkSuffix, "libsqlite3.0."+frameworkSuffix, PBXSourceTree.Sdk));
			}

			//Vungle
			if(use_Vungle){
				proj.AddFrameworkToProject(target, "AdSupport.framework", false);
				proj.AddFrameworkToProject(target, "AudioToolbox.framework", false);
				proj.AddFrameworkToProject(target, "AVFoundation.framework", false);
				proj.AddFrameworkToProject(target, "CFNetwork.framework", false);
				proj.AddFrameworkToProject(target, "CoreGraphics.framework", false);
				proj.AddFrameworkToProject(target, "CoreMedia.framework", false);
				proj.AddFrameworkToProject(target, "Foundation.framework", false);
				proj.AddFrameworkToProject(target, "MediaPlayer.framework", false);
				proj.AddFrameworkToProject(target, "QuartzCore.framework", false);
				proj.AddFrameworkToProject(target, "StoreKit.framework", false);
				proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
				proj.AddFrameworkToProject(target, "UIKit.framework", false);
				proj.AddFrameworkToProject(target, "WebKit.framework", true);//Optional
				proj.AddFileToBuild(target, proj.AddFile(
					"usr/lib/libz.1.2.5."+frameworkSuffix, "libz.1.2.5."+frameworkSuffix, PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile(
					"usr/lib/libsqlite3.0."+frameworkSuffix, "libsqlite3.0."+frameworkSuffix, PBXSourceTree.Sdk));
			}

			//SMAAD Video
			if(use_SmaadVideo){
				proj.AddFrameworkToProject(target, "MediaPlayer.framework", false);
				proj.AddFrameworkToProject(target, "AdSupport.framework", false);
				proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
				//SmaADは、バージョン1.1.3の時点ではbitcodeに対応しておりません
				proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
			}

			//Five
			if(use_Five){
				proj.AddFrameworkToProject(target, "AdSupport.framework", false);
				proj.AddFrameworkToProject(target, "AVFoundation.framework", false);
				proj.AddFrameworkToProject(target, "CoreMedia.framework", false);
				proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
				proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
			}

			// フレームワーク検索パスの設定
			proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
			// Frameworks/Plugins/iOS/**/*.framework
			// Use ** to make search setting in Xcode recursive
			proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks/Plugins/iOS/**");

			//エクスポート時に文字列のパスが加えられること(Unity由来の事象)への対応
			proj.UpdateBuildProperty (target, "HEADER_SEARCH_PATHS",
				new string[]{"$(SRCROOT)/Classes", "$(SRCROOT)"}, new string[]{"\"$(SRCROOT)/Classes\"", "\"$(SRCROOT)\""});
			proj.UpdateBuildProperty (target, "LIBRARY_SEARCH_PATHS",
				new string[]{"$(SRCROOT)/Classes", "$(SRCROOT)"}, new string[]{"\"$(SRCROOT)/Classes\"", "\"$(SRCROOT)\""});
			// Set a custom link flag
			proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
			proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-fobjc-arc");

			// ソースのコンパイルフラグに "-fobjc-arc" を追加
			foreach (string filePath in new string[]{
				"Libraries/Plugins/iOS/ADFMovieRewardAdViewController.mm",
				"Libraries/Plugins/iOS/ADFMovieRewardUnityAdapter.m",
				"Libraries/Plugins/iOS/ADFMovieInterstitialAdViewController.mm",
				"Libraries/Plugins/iOS/ADFMovieInterstitialUnityAdapter.m",
				"Libraries/Plugins/iOS/ADFMovieNativeAdViewManager.mm",
				"Libraries/Plugins/iOS/ADFMovieNativeAdViewUnityAdapter.m"
			}) {
				string fileGuid = proj.FindFileGuidByProjectPath(filePath);
				var flags = proj.GetCompileFlagsForFile(target, fileGuid);
				flags.Remove("-fobjc-arc");
				flags.Add("-fobjc-arc");
				proj.SetCompileFlagsForFile(target, fileGuid, flags);
			}
			//Debug.Log(target);
			File.WriteAllText(projPath, proj.WriteToString());
			plist.WriteToFile (plistPath);
		}
	}
}
#endif
