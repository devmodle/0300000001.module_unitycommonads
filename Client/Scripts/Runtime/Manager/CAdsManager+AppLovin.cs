using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
/** 광고 관리자 - 앱 로빈 */
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	/** 앱 로빈 배너 광고를 로드한다 */
	private void LoadAppLovinBannerAds() {
		CAccess.Assert(this.IsValidAppLovinBannerAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		MaxSdk.CreateBanner(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID, string.Empty), (this.Params.m_eBannerAdsPos == EBannerAdsPos.UP) ? MaxSdkBase.BannerPosition.TopCenter : MaxSdkBase.BannerPosition.BottomCenter);
		MaxSdk.SetBannerExtraParameter(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID, string.Empty), KCDefine.U_KEY_ADS_M_APP_LOVIN_IS_ADAPTIVE_BANNER, KCDefine.B_TEXT_FALSE.ToLower());
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 보상 광고를 로드한다 */
	private void LoadAppLovinRewardAds() {
		CAccess.Assert(this.IsValidAppLovinRewardAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		MaxSdk.LoadRewardedAd(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID, string.Empty));
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 전면 광고를 로드한다 */
	private void LoadAppLovinFullscreenAds() {
		CAccess.Assert(this.IsValidAppLovinFullscreenAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		MaxSdk.LoadInterstitial(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID, string.Empty));
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 배너 광고를 출력한다 */
	private void ShowAppLovinBannerAds() {
		CAccess.Assert(this.IsValidAppLovinBannerAdsID() && this.Params.m_eAdsPlatform == EAdsPlatform.APP_LOVIN);

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		MaxSdk.ShowBanner(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID, string.Empty));
		this.SetupBannerAdsHeight(MaxSdkUtils.GetAdaptiveBannerHeight());
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 보상 광고를 출력한다 */
	private void ShowAppLovinRewardAds() {
		CAccess.Assert(this.IsValidAppLovinRewardAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		MaxSdk.ShowRewardedAd(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID, string.Empty));
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 전면 광고를 출력한다 */
	private void ShowAppLovinFullscreenAds() {
		CAccess.Assert(this.IsValidAppLovinFullscreenAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		MaxSdk.ShowInterstitial(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID, string.Empty));
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 배너 광고를 닫는다 */
	private void CloseAppLovinBannerAds() {
		CAccess.Assert(this.IsValidAppLovinBannerAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		MaxSdk.HideBanner(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID, string.Empty));
		MaxSdk.DestroyBanner(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID, string.Empty));

		m_bIsLoadAppLovinBannerAds = false;
		this.SetupBannerAdsHeight(KCDefine.B_VAL_0_REAL);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}
	#endregion // 함수

	#region 접근자 함수
	/** 앱 로빈 초기화 여부를 검사한다 */
	private bool IsInitAppLovin() {
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return m_bIsInitAppLovin;
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAppLovinAdsID(string a_oID) {
		CAccess.Assert(a_oID.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return this.Params.m_oAppLovinAdsIDDict.TryGetValue(a_oID, out string oAdsID) && oAdsID.ExIsValid();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 배너 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAppLovinBannerAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return this.IsValidAppLovinAdsID(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 보상 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAppLovinRewardAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return this.IsValidAppLovinAdsID(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 전면 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAppLovinFullscreenAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return this.IsValidAppLovinAdsID(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 배너 광고 로드 여부를 검사한다 */
	private bool IsLoadAppLovinBannerAds() {
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return this.IsValidAppLovinBannerAdsID() && m_bIsLoadAppLovinBannerAds;
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 보상 광고 로드 여부를 검사한다 */
	private bool IsLoadAppLovinRewardAds() {
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return this.IsValidAppLovinRewardAdsID() && MaxSdk.IsRewardedAdReady(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID, string.Empty));
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}

	/** 앱 로빈 전면 광고 로드 여부를 검사한다 */
	private bool IsLoadAppLovinFullscreenAds() {
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
		return this.IsValidAppLovinFullscreenAdsID() && MaxSdk.IsInterstitialReady(this.Params.m_oAppLovinAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID, string.Empty));
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	}
	#endregion // 접근자 함수

	#region 조건부 함수
#if(UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	/** 앱 로빈이 초기화 되었을 경우 */
	private void OnInitAppLovin(MaxSdkBase.SdkConfiguration a_oConfig) {
		CFunc.ShowLog($"CAdsManager.OnInitAppLovin", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_INIT_CALLBACK, () => {
			this.IsInit = a_oConfig.IsSuccessfullyInitialized;
			m_bIsInitAppLovin = a_oConfig.IsSuccessfullyInitialized;

			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, EAdsPlatform.APP_LOVIN, m_bIsInitAppLovin);
		});
	}

	/** 앱 로빈 배너 광고가 로드 되었을 경우 */
	private void OnLoadAppLovinBannerAds(string a_oAdsID, MaxSdkBase.AdInfo a_oAdsInfo) {
		CFunc.ShowLog("CAdsManager.OnLoadAppLovinBannerAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_BANNER_ADS_LOAD_CALLBACK, () => {
			m_bIsLoadAppLovinBannerAds = true;
			this.HandleLoadAppLovinBannerAdsResult();
		});
	}

	/** 앱 로빈 배너 광고 로드에 실패했을 경우 */
	private void OnLoadFailAppLovinBannerAds(string a_oAdsID, MaxSdkBase.ErrorInfo a_oErrorInfo) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAppLovinBannerAds: {a_oErrorInfo.MediatedNetworkErrorCode}, {a_oErrorInfo.MediatedNetworkErrorMessage}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_BANNER_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailBannerAdsInfo(EAdsPlatform.APP_LOVIN, this.LoadBannerAds));
	}
	
	/** 앱 로빈 보상 광고가 로드 되었을 경우 */
	private void OnLoadAppLovinRewardAds(string a_oAdsID, MaxSdkBase.AdInfo a_oAdsInfo) {
		CFunc.ShowLog("CAdsManager.OnLoadAppLovinRewardAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_LOAD_CALLBACK, () => {
			// Do Something
		});
	}

	/** 앱 로빈 보상 광고 로드에 실패했을 경우 */
	private void OnLoadFailAppLovinRewardAds(string a_oAdsID, MaxSdkBase.ErrorInfo a_oErrorInfo) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAppLovinRewardAds: {a_oErrorInfo.MediatedNetworkErrorCode}, {a_oErrorInfo.MediatedNetworkErrorMessage}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailRewardAdsInfo(EAdsPlatform.APP_LOVIN, this.LoadRewardAds));
	}

	/** 앱 로빈 보상 광고가 닫혔을 경우 */
	private void OnCloseAppLovinRewardAds(string a_oAdsID, MaxSdkBase.AdInfo a_oAdsInfo) {
		CFunc.ShowLog("CAdsManager.OnCloseAppLovinRewardAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_CLOSE_CALLBACK, () => {
			this.HandleCloseRewardAdsResult(EAdsPlatform.APP_LOVIN);
			this.LoadRewardAds(EAdsPlatform.APP_LOVIN);
		});
	}

	/** 앱 로빈 광고 보상을 수신했을 경우 */
	private void OnReceiveAppLovinAdsReward(string a_oAdsID, MaxSdkBase.Reward a_stReward, MaxSdkBase.AdInfo a_oAdsInfo) {
		CFunc.ShowLog($"CAdsManager.OnReceiveAppLovinAdsReward: {a_stReward.Label}, {a_stReward.Amount}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_RECEIVE_ADS_REWARD_CALLBACK, () => {
			this.HandleRewardAdsResult(EAdsPlatform.APP_LOVIN, new STAdsRewardInfo() {
				m_oID = a_stReward.Label.ExIsValid() ? a_stReward.Label : string.Empty, m_oVal = $"{a_stReward.Amount}"
			}, true);
		});
	}

	/** 앱 로빈 전면 광고가 로드 되었을 경우 */
	private void OnLoadAppLovinFullscreenAds(string a_oAdsID, MaxSdkBase.AdInfo a_oAdsInfo) {
		CFunc.ShowLog("CAdsManager.OnLoadAppLovinFullscreenAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_LOAD_CALLBACK, () => {
			// Do Something
		});
	}

	/** 앱 로빈 전면 광고 로드에 실패했을 경우 */
	private void OnLoadFailAppLovinFullscreenAds(string a_oAdsID, MaxSdkBase.ErrorInfo a_oErrorInfo) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAppLovinFullscreenAds: {a_oErrorInfo.MediatedNetworkErrorCode}, {a_oErrorInfo.MediatedNetworkErrorMessage}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailFullscreenAdsInfo(EAdsPlatform.APP_LOVIN, this.LoadFullscreenAds));
	}

	/** 앱 로빈 전면 광고가 닫혔을 경우 */
	private void OnCloseAppLovinFullscreenAds(string a_oAdsID, MaxSdkBase.AdInfo a_oAdsInfo) {
		CFunc.ShowLog("CAdsManager.OnCloseAppLovinFullscreenAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_CLOSE_CALLBACK, () => {
			this.HandleCloseFullscreenAdsResult(EAdsPlatform.APP_LOVIN);
			this.LoadFullscreenAds(EAdsPlatform.APP_LOVIN);
		});
	}

	/** 앱 로빈 배너 광고 로드 결과를 처리한다 */
	private void HandleLoadAppLovinBannerAdsResult() {
		this.ExLateCallFunc((a_oSender) => this.ShowBannerAds(EAdsPlatform.APP_LOVIN, null));
	}

	/** 광고 식별자를 생성한다 */
	private List<string> MakeAppLovinAdsIDs(Dictionary<string, string> a_oAdsIDDict) {
		var oAdsIDList = new List<string>();

		foreach(var stKeyVal in a_oAdsIDDict) {
			// 광고 식별자가 유효 할 경우
			if(stKeyVal.Value.ExIsValid()) {
				oAdsIDList.ExAddVal(stKeyVal.Value);
			}
		}

		return oAdsIDList;
	}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APP_LOVIN_ADS_ENABLE
	#endregion // 조건부 함수
}
#endif // #if ADS_MODULE_ENABLE
