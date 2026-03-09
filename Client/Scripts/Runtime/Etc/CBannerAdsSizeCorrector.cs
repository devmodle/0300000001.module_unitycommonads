using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
/** 배너 광고 크기 보정자 */
public partial class CBannerAdsSizeCorrector : CComponent {
	#region 변수
	[SerializeField] private Vector3 m_stSizeOffset = Vector3.zero;

	private Vector3 m_stOriginSize = Vector3.zero;
	private Vector3 m_stCorrectSize = Vector3.zero;
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
		m_stOriginSize = (this.transform as RectTransform).sizeDelta.ExTo3D();
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
			CFunc.ShowLogWarning($"CBannerAdsSizeCorrector.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			var stSize = m_stOriginSize + (m_stSizeOffset + new Vector3(KCDefine.B_VAL_0_REAL, -CAdsManager.Inst.BannerAdsHeight, KCDefine.B_VAL_0_REAL));

			// 보정 크기와 다를 경우
			if(!stSize.ExIsEquals(m_stCorrectSize)) {
				m_stCorrectSize = stSize;
				(this.transform as RectTransform).sizeDelta = stSize;
			}
		}
	}

	/** 원본 크기를 변경한다 */
	public void SetOriginSize(Vector3 a_stSize) {
		m_stOriginSize = a_stSize;
	}
	#endregion // 함수
}
#endif // #if ADS_MODULE_ENABLE
