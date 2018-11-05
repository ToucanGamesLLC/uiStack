using System.Collections;
using UnityEngine;
using UnityEditor;

public class BaseMonobehaviorManager<T> : Singleton<T>, IGameManager where T : MonoBehaviour {

	#region protected fields

	protected bool _initialized;

	#endregion


	protected void SetDirty() {

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif

	}

	#region IGameManager

	public bool initialzied { get { return _initialized; } }
	
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
