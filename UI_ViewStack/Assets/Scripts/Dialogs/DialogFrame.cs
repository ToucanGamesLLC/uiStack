using UnityEngine;

public class DialogFrame : MonoBehaviour {

	public enum DisplayAnimType {
		None,
		EnableDsiable
	}

	#region public fields

	public RectTransform dialogPanel;
	public BaseDialog dialogContent { get; private set; }

	#endregion


	#region Populate methods

	public BaseDialog TryInstantiateDialogContent(BaseDialog baseDialogPrefa) {
		dialogContent = null;

		if (baseDialogPrefa != null) {
			dialogContent = GameObjectHelper.AddChildrenWithComponent<BaseDialog>(
				baseDialogPrefa,
				dialogPanel
			);

			if (dialogContent != null) {
				dialogContent.dialogFrame = this;
				RectTransform rectTransform = GetComponent<RectTransform>();

				if (rectTransform != null) {
					rectTransform.offsetMax = new Vector2(-dialogContent.rightOffset, -dialogContent.topOffset);
					rectTransform.offsetMin = new Vector2(dialogContent.leftOffset, dialogContent.bottomOffset);
				}
			}
		} else {
			LogHelper.LogWarning("Failed to set base dialog; " 
				+ nameof(baseDialogPrefa) 
				+ " is not set",
				this
			);
		}

		return dialogContent;
	}

	#endregion


	public void Show() {
		if (dialogContent != null) {
			switch (dialogContent.displayAnimType) {
				case DisplayAnimType.EnableDsiable:
					gameObject.SetActive(true);
					break;

				default:
					LogHelper.LogWarning("Unsupported animtion type to show dialog "
						+ dialogContent.GetType().Name
						+ "; enabling game object",
						this
					);
					break;
			}
		}
	}

	public void Hide() {
		if (dialogContent != null) {
			switch (dialogContent.displayAnimType) {
				case DisplayAnimType.EnableDsiable:
					gameObject.SetActive(false);
					break;

				default:
					LogHelper.LogWarning("Unsupported animtion type to show dialog "
						+ dialogContent.GetType().Name
						+ "; enabling game object",
						this
					);
					break;
			}
		}
	}
}
