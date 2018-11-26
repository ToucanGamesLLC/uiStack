using TMPro;
using UnityEngine;

public class UIComponentHelper : MonoBehaviour {

	protected void SetActive(GameObject uiObject, bool value) {
		if (uiObject != null) {
			uiObject.SetActive(value);
		} else {
			LogHelper.LogWarning("Failed to set game object active value; "
				+ nameof(uiObject)
				+ " is not set",
				this
			);
		}
	}

	protected void SetText(TMP_Text uiText, string value) {
		if (uiText != null) {
			uiText.text = value;
		} else {
			LogHelper.LogWarning("Failed to set game object active value; "
				+ nameof(uiText)
				+ " is not set",
				this
			);
		}
	}
}
