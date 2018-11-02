using UnityEngine;

public class DialogFrame : MonoBehaviour {

	#region public fields

	public RectTransform dialogPanel;

	public BaseDialog dialogContent { get; private set; }

	#endregion


	#region Populate methods

	public bool TryInstantiateDialogContent(BaseDialog baseDialogPrefa) {
		bool result = false;

		if (baseDialogPrefa != null) {
			dialogContent = GameObjectHelper.AddChildrenWithComponent<BaseDialog>(
				baseDialogPrefa, 
				dialogPanel
			);

			if (dialogContent != null) {
				RectTransform rectTransform = transform as RectTransform;
				if (rectTransform != null) {
					rectTransform.sizeDelta =
						new Vector2(dialogContent.width, dialogContent.height);
				}
			}

			result = (dialogContent != null);
		} else {
			LogHelper.LogWarning("Failed to set base dialog; " 
				+ nameof(dialogContent) 
				+ " is not set",
				this
			);
		}

		return result;
	}

	#endregion

}
