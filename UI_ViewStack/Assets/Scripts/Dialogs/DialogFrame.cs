using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogFrame : UIComponentHelper {

	#region Delegates

	public delegate void VoidDelegate();

	#endregion


	public enum DisplayAnimType {
		None,
		EnableDsiable
	}

	#region public fields and accessors

	public TMP_Text titleText;
	public Button closeButon;
	public RectTransform dialogPanel;

	public BaseDialog dialogContent { get; private set; }

	#endregion


	#region Populate methods

	public BaseDialog TryInstantiateDialogContent(BaseDialog _baseDialogPrefa) {
		if (dialogContent == null) {
			if (_baseDialogPrefa != null) {
				dialogContent = GameObjectHelper.AddChildrenWithComponent<BaseDialog>(
					_baseDialogPrefa,
					dialogPanel
				);

				if (dialogContent != null) {
					RectTransform rectTransform = GetComponent<RectTransform>();

					if (rectTransform != null) {
						rectTransform.offsetMax =
							new Vector2(-dialogContent.rightOffset, -dialogContent.topOffset);

						rectTransform.offsetMin =
							new Vector2(dialogContent.leftOffset, dialogContent.bottomOffset);
					}

					dialogContent.dialogFrame = this;
				}
			} else {
				LogHelper.LogWarning("Failed to set base dialog; "
					+ nameof(_baseDialogPrefa)
					+ " is not set",
					this
				);
			}
		} else {
			LogHelper.LogWarning("Failed to set " 
				+ nameof(dialogContent) 
				+ "; already exists", 
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

	public void Hide(VoidDelegate _onHideCompleted = null) {
		if (dialogContent != null) {
			switch (dialogContent.displayAnimType) {
				case DisplayAnimType.EnableDsiable:
					gameObject.SetActive(false);

					if(_onHideCompleted != null) {
						_onHideCompleted();
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
