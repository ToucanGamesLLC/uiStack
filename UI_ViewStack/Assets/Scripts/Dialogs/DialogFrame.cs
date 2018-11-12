using UnityEngine;
using UnityEngine.UI;

public class DialogFrame : MonoBehaviour {

	#region Delegates

	public delegate void VoidDelegate();

	#endregion


	public enum DisplayAnimType {
		None,
		EnableDsiable
	}

	#region public fields

	public RectTransform dialogPanel;
	public Button closeButon;

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
				dialogContent.dialogFramePrefab = this;
				RectTransform rectTransform = GetComponent<RectTransform>();

				if (rectTransform != null) {
					rectTransform.offsetMax = new Vector2(-dialogContent.rightOffset, -dialogContent.topOffset);
					rectTransform.offsetMin = new Vector2(dialogContent.leftOffset, dialogContent.bottomOffset);
				}

				if (closeButon != null) {
					if (dialogContent.options != null) {
						closeButon.gameObject.SetActive(dialogContent.options.showCloseButton);
					}
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


	#region Show and hide methods

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

	public void Hide(VoidDelegate onHideCompleted = null) {
		if (dialogContent != null) {
			switch (dialogContent.displayAnimType) {
				case DisplayAnimType.EnableDsiable:
					gameObject.SetActive(false);

					if(onHideCompleted != null) {
						onHideCompleted();
					}

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
	#endregion

	public void OnCloseButtonClick() {
		if(dialogContent != null) {
			dialogContent.CloseDialog();
		}
	}
}
