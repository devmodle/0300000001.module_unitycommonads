using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
/** 보상 광고 상호 작용 처리자 */
public partial class CRewardAdsTouchInteractable : CComponent {
	#region 변수
	[SerializeField] private EAdsPlatform m_eAdsPlatform = EAdsPlatform.NONE;
	private CTouchInteractable m_oTouchInteractable = null;
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CScheduleManager.Inst.AddComponent(this);

		// 터치 상호 작용 처리자를 설정한다
		m_oTouchInteractable = this.GetComponentInChildren<CTouchInteractable>();
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				CScheduleManager.Inst.RemoveComponent(this);
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CRewardAdsTouchInteractable.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			var eAdsPlatform = m_eAdsPlatform.ExIsValid() ? m_eAdsPlatform : CCommonAppInfoStorage.Inst.AdsPlatform;
			m_oTouchInteractable?.SetInteractable(CAdsManager.Inst.IsLoadRewardAds(eAdsPlatform));
		}
	}
	#endregion // 함수
}

/** 보상 광고 상호 작용 처리자 - 접근 */
public partial class CRewardAdsTouchInteractable : CComponent {
	#region 함수
	/** 광고 플랫폼을 변경한다 */
	public void SetAdsPlatform(EAdsPlatform a_eType) {
		m_eAdsPlatform = a_eType;
	}
	#endregion // 함수
}
#endif // #if ADS_MODULE_ENABLE
