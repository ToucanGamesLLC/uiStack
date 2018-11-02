using UnityEngine;

public static class GameObjectHelper {
	
	public static GameObject AddChildren(
		GameObject prefab, 
		Transform parent, 
		bool instantiateIWorldSpace = false
	) {
		GameObject result = null;

		if (parent != null) {
			if (prefab != null) {
				result = GameObject.Instantiate(
					prefab,
					parent, 
					instantiateIWorldSpace
				);
			} else {
				LogHelper.LogError(
					"Cannot add child object; " 
					+ nameof(prefab) 
					+ " is not set"
				);
			}
		} else {
			LogHelper.LogError(
					"Cannot add child object; "
					+ nameof(parent)
					+ " is not set"
				);
		}

		return result;
	}

	public static T AddChildrenWithComponent<T>(
		T prefab, 
		Transform parent, 
		bool instantiateIWorldSpace = false
	) where T : BaseDialog {
		T result = null;

		if (prefab != null) {
			GameObject gameObject = AddChildren(
				prefab.gameObject, 
				parent, 
				instantiateIWorldSpace
			);

			if (gameObject != null) {
				result = gameObject.GetComponent<T>();

				if (result == null) {
					LogHelper.LogError(
						"Cannot add child object; " 
						+ gameObject.name 
						+ " does not have component of type " 
						+ typeof(T).Name
					);
				} else {
					Destroy(gameObject, true);
				}
			}
		} else {
			LogHelper.LogError(
				"Cannot add child object; " 
				+ nameof(prefab) 
				+ " is not set"
			);
		}

		return result;
	}

	public static void Destroy(GameObject gameObject, bool inmedatly = false) {
		if (gameObject != null) {
			if (inmedatly) {
				GameObject.DestroyImmediate(gameObject);
			} else {
				GameObject.Destroy(gameObject);
			}
		}
	}

	public static void Destroy(GameObject gameObject, float delay) {
		if (gameObject != null) {
			if (delay > 0.0f) {
				GameObject.Destroy(gameObject, delay);
			} else {
				GameObject.Destroy(gameObject);
			}
		}
	}
}
