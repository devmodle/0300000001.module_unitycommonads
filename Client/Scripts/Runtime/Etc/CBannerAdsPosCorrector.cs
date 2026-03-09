using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
/** 배너 광고 위치 보정자 */
public partial class CBannerAdsPosCorrector : CComponent {
	#region 변수
	[SerializeField] private Vector3 m_stPosOffset = Vector3.zero;

	private Vector3 m_stOriginPos = Vector3.zero;
	private Vector3 m_stCorrectPos = Vector3.zero;
	#endregion // 변수

	#region 프로퍼티
#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_LATEST;
#endif // #if UNITY_EDITOR
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CScheduleManager.Inst.AddComponent(this);
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		m_stOriginPos = (this.transform as RectTransform).anchoredPosition.ExTo3D();
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
			CFunc.ShowLogWarning($"CBannerAdsPosCorrector.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			var stPos = m_stOriginPos + (m_stPosOffset + new Vector3(KCDefine.B_VAL_0_REAL, (CPluginInfoTable.Inst.BannerAdsPos == EBannerAdsPos.DOWN) ? CAdsManager.Inst.BannerAdsHeight : -CAdsManager.Inst.BannerAdsHeight, KCDefine.B_VAL_0_REAL));

			// 보정 위치와 다를 경우
			if(!stPos.ExIsEquals(m_stCorrectPos)) {
				m_stCorrectPos = stPos;
				(this.transform as RectTransform).anchoredPosition = stPos;
			}
		}
	}
	#endregion // 함수
}

/** 배너 광고 위치 보정자 - 접근 */
public partial class CBannerAdsPosCorrector : CComponent {
	#region 함수
	/** 원본 위치를 변경한다 */
	public void SetOriginPos(Vector3 a_stPos) {
		m_stOriginPos = a_stPos;
	}
	#endregion // 함수
}
#endif // #if ADS_MODULE_ENABLE
