using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : BaseMonobehaviorGameManager<PrefabManager> {

	


	#region public fields

	public List<GameObject> prefabs;

	#endregion


	#region private fields

	private Dictionary<string, GameObject> m_prefabsByName;

	#endregion


	#region BaseMonobehaviorGameManager

	public override IEnumerator Preinitialize() {
		yield return base.Preinitialize();

		m_prefabsByName = new Dictionary<string, GameObject>();
		
		if (prefabs != null) {
			m_prefabsByName = new Dictionary<string, GameObject>();

			for(int i = 0; i < prefabs.Count; i++) {
				GameObject prefab = prefabs[i];

				if (prefab != null) {
					string prefabName = prefab.name;

					if (!m_prefabsByName.ContainsKey(prefabName)) {
						m_prefabsByName[prefabName] = prefab;
					}
				} else {
					LogHelper.LogWarning("Cannot cache prefab at index " 
						+ i 
						+ "; " 
						+ nameof(prefab) 
						+ " is not set", 
						this
					);
				}
			}
		}

		yield return null;
	}

	public override IEnumerator Initialize() {
		yield return base.Initialize();

		int cachedPrefabs = (m_prefabsByName != null) ? m_prefabsByName.Count : 0;

		LogHelper.LogMessage(GetType().Name 
			+ " initialized completed : " 
			+ nameof(m_prefabsByName)
			+ " size : "
			+ cachedPrefabs, 
			this
		);

		m_initialized = true;

		SetDirty();

		yield return null;
	}

	#endregion


	#region Prefab loading 

	private GameObject LoadPrefabFromCacheData(string _prefabName) {
		GameObject result = null;

		if (!string.IsNullOrEmpty(_prefabName)) {
			if (m_prefabsByName != null) {
				if (!m_prefabsByName.TryGetValue(_prefabName, out result)
					|| result == null
				) {
					LogHelper.LogWarning("Failed to load prefab with name "
						+ _prefabName
						+ "; prefab cannot be found",
						this
					);
				}
			}
		} else {
			LogHelper.LogWarning("Cannot load prefab; "
				+ nameof(_prefabName)
				+ " is not set",
				this
			);
		}

		return result;
	}

	public GameObject LoadPrefab(string _prefabName) {
		GameObject result = null;
		
		if (result == null) {
			result = LoadPrefabFromCacheData(_prefabName);

			if (result == null) {
				LogHelper.LogError("Failed to load prefab "
					+ _prefabName 
					+ " from "
					+ GetType().Name
					+ " cached data",
					this
				);
			}
		}

		return result;
	}

	public T LoadPrefabWithComponent<T>(string _prefabName = null) where T : MonoBehaviour {
		T result = null;

		if (string.IsNullOrEmpty(_prefabName)) {
			_prefabName = typeof(T).Name;
		}

		GameObject prefabObject = LoadPrefab(_prefabName);
		if (prefabObject != null) {
			result = prefabObject.GetComponent<T>();
			if (result == null) {
				LogHelper.LogError("Failed to load prefab wih component "
					+ typeof(T).Name
					+ "; missing component in prefab "
					+ _prefabName,
					this
				);
			}
		}

		return result;
	}

	#endregion

}
