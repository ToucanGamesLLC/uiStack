using System.Collections;
using UnityEngine;
using UnityEditor;

public class BaseMonobehaviorGameManager<T> : Singleton<T>, IGameManager where T : MonoBehaviour, IGameManager {

	#region protected fields

	protected bool m_initialized;

	#endregion


	protected void SetDirty() {

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif

	}

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

}
