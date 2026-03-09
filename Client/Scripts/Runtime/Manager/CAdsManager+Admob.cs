using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
#if ADMOB_ADS_ENABLE
using GoogleMobileAds.Api;
#endif // #if ADMOB_ADS_ENABLE

/** 광고 관리자 - 애드 몹 */
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	/** 애드 몹 배너 광고를 로드한다 */
	private void LoadAdmobBannerAds() {
		CAccess.Assert(this.IsValidAdmobBannerAdsID());

		this.ExLateCallFunc((a_oSender) => {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
			this.AdmobBannerAds.LoadAd(this.AdmobRequest);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		});
	}

	/** 애드 몹 보상 광고를 로드한다 */
	private void LoadAdmobRewardAds() {
		CAccess.Assert(this.IsValidAdmobRewardAdsID());

		this.ExLateCallFunc((a_oSender) => {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
			RewardedAd.Load(this.Params.m_oAdmobAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID, string.Empty), this.AdmobRequest, this.OnLoadAdmobRewardAds);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		});
	}

	/** 애드 몹 전면 광고를 로드한다 */
	private void LoadAdmobFullscreenAds() {
		CAccess.Assert(this.IsValidAdmobFullscreenAdsID());

		this.ExLateCallFunc((a_oSender) => {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
			InterstitialAd.Load(this.Params.m_oAdmobAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID, string.Empty), this.AdmobRequest, this.OnLoadAdmobFullscreenAds);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		});
	}

	/** 애드 몹 배너 광고를 출력한다 */
	private void ShowAdmobBannerAds() {
		CAccess.Assert(this.IsValidAdmobBannerAdsID() && this.Params.m_eAdsPlatform == EAdsPlatform.ADMOB);

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.AdmobBannerAds.Show();
		this.SetupBannerAdsHeight(this.AdmobBannerAds.GetHeightInPixels().ExPixelsToDPIPixels());
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 보상 광고를 출력한다 */
	private void ShowAdmobRewardAds() {
		CAccess.Assert(this.IsValidAdmobRewardAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		m_oAdmobRewardAds.Show(this.OnReceiveAdmobAdsReward);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 전면 광고를 출력한다 */
	private void ShowAdmobFullscreenAds() {
		CAccess.Assert(this.IsValidAdmobFullscreenAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		m_oAdmobFullscreenAds.Show();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 배너 광고를 닫는다 */
	private void CloseAdmobBannerAds() {
		CAccess.Assert(this.IsValidAdmobBannerAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.AdmobBannerAds?.Hide();
		this.AdmobBannerAds?.Destroy();

		this.ResetAdmobBannerAds();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}
	#endregion // 함수

	#region 조건부 함수
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	/** 애드 몹 배너 광고 상태를 리셋한다 */
	private void ResetAdmobBannerAds() {
		CAccess.AssignVal(ref m_oAdmobBannerAds, null);
		m_bIsLoadAdmobBannerAds = false;

		this.SetupBannerAdsHeight(KCDefine.B_VAL_0_REAL);
	}

	/** 애드 몹 보상 광고 상태를 리셋한다 */
	private void ResetAdmobRewardAds() {
		CAccess.AssignVal(ref m_oAdmobRewardAds, null);
	}

	/** 애드 몹 전면 광고 상태를 리셋한다 */
	private void ResetAdmobFullscreenAds() {
		CAccess.AssignVal(ref m_oAdmobFullscreenAds, null);
	}

	/** 애드 몹이 초기화 되었을 경우 */
	private void OnInitAdmob(InitializationStatus a_oStatus) {
		string oStr = a_oStatus.getAdapterStatusMap().ExToStr(KCDefine.B_TOKEN_COMMA);
		CFunc.ShowLog($"CAdsManager.OnInitAdmob: {oStr}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_INIT_CALLBACK, () => {
			this.IsInit = true;
			m_bIsInitAdmob = true;

			MobileAds.SetRequestConfiguration(new RequestConfiguration() {
				TestDeviceIds = this.Params.m_oAdmobTestDeviceIDList
			});

			MobileAds.GetRequestConfiguration().TestDeviceIds.ExAddVal(AdRequest.TestDeviceSimulator);
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, EAdsPlatform.ADMOB, m_bIsInitAdmob);
		});
	}

	/** 애드 몹 배너 광고가 로드 되었을 경우 */
	private void OnLoadAdmobBannerAds() {
		CFunc.ShowLog("CAdsManager.OnLoadAdmobBannerAds", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_CALLBACK, () => { m_bIsLoadAdmobBannerAds = true; this.HandleLoadAdmobBannerAdsResult(); });
	}

	/** 애드 몹 배너 광고 로드에 실패했을 경우 */
	private void OnLoadFailAdmobBannerAds(LoadAdError a_oError) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAdmobBannerAds: {a_oError?.GetCode()}, {a_oError?.GetMessage()}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailBannerAdsInfo(EAdsPlatform.ADMOB, this.LoadBannerAds));
	}

	/** 애드 몹 보상 광고가 로드 되었을 경우 */
	private void OnLoadAdmobRewardAds(RewardedAd a_oRewardAds, LoadAdError a_oError) {
		CFunc.ShowLog($"CAdsManager.OnLoadAdmobRewardAds: {a_oError?.GetCode()}, {a_oError?.GetMessage()}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_LOAD_CALLBACK, () => {
			m_oAdmobRewardAds = (a_oRewardAds != null && a_oError == null) ? a_oRewardAds : null;

			// 로드 되었을 경우
			if(a_oRewardAds != null && a_oError == null) {
				m_oAdmobRewardAds.OnAdFullScreenContentClosed += this.OnCloseAdmobRewardAds;
			} else {
				this.AddLoadFailRewardAdsInfo(EAdsPlatform.ADMOB, this.LoadRewardAds);
			}
		});
	}

	/** 애드 몹 배너 광고 로드에 실패했을 경우 */
	private void OnLoadFailAdmobRewardAds(object a_oSender, AdFailedToLoadEventArgs a_oEventArgs) {
		// Do Something
	}

	/** 애드 몹 보상 광고가 닫혔을 경우 */
	private void OnCloseAdmobRewardAds() {
		CFunc.ShowLog("CAdsManager.OnCloseAdmobRewardAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_CLOSE_CALLBACK, () => {
			this.ResetAdmobRewardAds();
			this.HandleCloseRewardAdsResult(EAdsPlatform.ADMOB);

			this.LoadRewardAds(EAdsPlatform.ADMOB);
		});
	}

	/** 애드 몹 광고 보상을 수신했을 경우 */
	private void OnReceiveAdmobAdsReward(Reward a_oReward) {
		CFunc.ShowLog($"CAdsManager.OnReceiveAdmobAdsReward: {a_oReward?.Type}, {a_oReward?.Amount}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_RECEIVE_ADS_REWARD_CALLBACK, () => {
			this.HandleRewardAdsResult(EAdsPlatform.ADMOB, (a_oReward != null) ? new STAdsRewardInfo() {
				m_oID = a_oReward.Type.ExIsValid() ? a_oReward.Type : string.Empty, m_oVal = $"{a_oReward.Amount}"
			} : STAdsRewardInfo.INVALID, true);
		});
	}

	/** 애드 몹 전면 광고가 로드 되었을 경우 */
	private void OnLoadAdmobFullscreenAds(InterstitialAd a_oFullscreenAds, LoadAdError a_oError) {
		CFunc.ShowLog($"CAdsManager.OnLoadAdmobFullscreenAds: {a_oError?.GetCode()}, {a_oError?.GetMessage()}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_CALLBACK, () => {
			m_oAdmobFullscreenAds = (a_oFullscreenAds != null && a_oError == null) ? a_oFullscreenAds : null;

			// 로드 되었을 경우
			if(a_oFullscreenAds != null && a_oError == null) {
				m_oAdmobFullscreenAds.OnAdFullScreenContentClosed += this.OnCloseAdmobFullscreenAds;
			} else {
				this.AddLoadFailFullscreenAdsInfo(EAdsPlatform.ADMOB, this.LoadFullscreenAds);
			}
		});
	}

	/** 애드 몹 전면 광고 로드에 실패했을 경우 */
	private void OnLoadFailAdmobFullscreenAds(object a_oSender, AdFailedToLoadEventArgs a_oEventArgs) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAdmobFullscreenAds: {a_oEventArgs?.LoadAdError.GetCode()}, {a_oEventArgs?.LoadAdError.GetMessage()}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => {
			this.ResetAdmobFullscreenAds();
			this.AddLoadFailFullscreenAdsInfo(EAdsPlatform.ADMOB, this.LoadFullscreenAds);
		});
	}

	/** 애드 몹 전면 광고가 닫혔을 경우 */
	private void OnCloseAdmobFullscreenAds() {
		CFunc.ShowLog("CAdsManager.OnCloseAdmobFullscreenAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_CLOSE_CALLBACK, () => {
			this.ResetAdmobFullscreenAds();
			this.HandleCloseFullscreenAdsResult(EAdsPlatform.ADMOB);

			this.LoadFullscreenAds(EAdsPlatform.ADMOB);
		});
	}

	/** 애드 몹 배너 광고 로드 결과를 처리한다 */
	private void HandleLoadAdmobBannerAdsResult() {
		m_bIsLoadAdmobBannerAds = true;
		this.ExLateCallFunc((a_oSender) => this.ShowBannerAds(EAdsPlatform.ADMOB, null));
	}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	#endregion // 조건부 함수
}

/** 광고 관리자 - 애드 몹 접근 */
public partial class CAdsManager : CSingleton<CAdsManager> {
	#region 함수
	/** 애드 몹 초기화 여부를 검사한다 */
	private bool IsInitAdmob() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return m_bIsInitAdmob;
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobAdsID(string a_oID) {
		CAccess.Assert(a_oID.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.Params.m_oAdmobAdsIDDict.TryGetValue(a_oID, out string oAdsID) && oAdsID.ExIsValid();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 배너 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobBannerAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobAdsID(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 보상 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobRewardAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobAdsID(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 전면 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobFullscreenAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobAdsID(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 배너 광고 로드 여부를 검사한다 */
	private bool IsLoadAdmobBannerAds() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobBannerAdsID() && (m_oAdmobBannerAds != null && m_bIsLoadAdmobBannerAds);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 보상 광고 로드 여부를 검사한다 */
	private bool IsLoadAdmobRewardAds() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobRewardAdsID() && (m_oAdmobRewardAds != null && m_oAdmobRewardAds.CanShowAd());
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 전면 광고 로드 여부를 검사한다 */
	private bool IsLoadAdmobFullscreenAds() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobFullscreenAdsID() && (m_oAdmobFullscreenAds != null && m_oAdmobFullscreenAds.CanShowAd());
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}
	#endregion // 함수
}
#endif // #if ADS_MODULE_ENABLE
