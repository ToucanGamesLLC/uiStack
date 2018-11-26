using ToucanEngine;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class BaseMonobehaviorGameManager<T> : Singleton<T>, IGameManager 
	where T : MonoBehaviour, IGameManager {

	#region protected fields

	protected bool m_initialized;

	#endregion


	#region Monobehavior methods

	private void Awake() {
		gameObject.name = "(S)" + gameObject.name;
		
		this.transform.SetParent(GameManager.Instance.transform, false);
	}

	#endregion


	#region IGameManager

	public bool initialzied { get { return m_initialized; } }
	
	public virtual IEnumerator Initialize() {
		yield return null;
	}

	public virtual IEnumerator Preinitialize() {
		yield return null;
	}

	public virtual IEnumerator Reset() {
		yield return null;
	}

	#endregion


	protected void SetDirty() {

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif

	}
}
