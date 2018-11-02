using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : BaseMonobehaviorManager<PrefabManager> {

	#region public fields

	public List<GameObject> prefabs;

	#endregion


	#region private fields

	private Dictionary<string, GameObject> m_prefabsByName;

	#endregion


	private void Awake() {
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
	}

	private GameObject LoadPrefabFromCacheData(string prefabName) {
		GameObject result = null;

		if (!string.IsNullOrEmpty(prefabName)) {
			if (m_prefabsByName != null) {
				if (!m_prefabsByName.TryGetValue(prefabName, out result)
					|| result == null
				) {
					LogHelper.LogWarning("Failed to load prefab with name "
						+ prefabName
						+ "; prefab cannot be found",
						this
					);
				}
			}
		} else {
			LogHelper.LogWarning("Cannot load prefab; "
				+ nameof(prefabName)
				+ " is not set",
				this
			);
		}

		return result;
	}

	public GameObject LoadPrefab(string prefabName) {
		GameObject result = null;

		if (result == null) {
			result = LoadPrefabFromCacheData(prefabName);
		}

		return result;
	}
}
