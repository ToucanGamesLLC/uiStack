using System.Collections.Generic;
using UnityEngine;

public class ViewCanvas : BaseMonobehaviorManager<ViewCanvas> {

	#region private fields

	public Stack<BaseDialog> dialogStack { get; private set; }

	#endregion


	#region public fields and accessors

	public RectTransform dialogCanvas;

	public BaseDialog activeDialog { get; private set; }

	#endregion


	public T DisplayDialog<T>(string dialogName = null) where T : BaseDialog {
		T result = LoadDialog<T>(dialogName);

		if (result != null) {
			if (activeDialog != null) {
				activeDialog.dialogFrame.Hide();
				PushDialog(activeDialog);
			}

			activeDialog = result;
			SetDirty();
		}

		return result;
	}

	private T LoadDialog<T>(string dialogName = null) where T : BaseDialog {
		T result = null;

		if (dialogCanvas != null) {
			dialogName = (!string.IsNullOrEmpty(dialogName))
				? dialogName
				: typeof(T).Name;

			if (!string.IsNullOrEmpty(dialogName)) {
				GameObject baseDialogPrefab =
					PrefabManager.Instance.LoadPrefab(typeof(T).Name);

				T dialogPrefab = (baseDialogPrefab != null)
					? baseDialogPrefab.GetComponent<T>()
					: null;

				if (dialogPrefab != null) {
					if (dialogPrefab.dialogFrame != null) {
						DialogFrame dialoFrame = 
							GameObjectHelper.AddChildrenWithComponent<DialogFrame>(
							dialogPrefab.dialogFrame, 
							dialogCanvas.transform
						);

						if (dialoFrame != null) {
							result = dialoFrame.TryInstantiateDialogContent(dialogPrefab) as T;

							if (result == null) {
								LogHelper.LogError(
									"Failed to instantiate dialog content for "
									+ dialogName,
									this
								);

								GameObjectHelper.Destroy(result.gameObject, true);
							} 
						}
					} else {
						LogHelper.LogWarning("Failed to load "
							+ typeof(T).Name
							+ "; missing "
							+ nameof(dialogPrefab.dialogFrame),
							this
						);
					}
				}
			}
		} else {
			LogHelper.LogError("Failed to load dialog; "
				+ nameof(dialogCanvas)
				+ " is not set",
				this
			);
		}

		return result;
	}

	private void PushDialog(BaseDialog dialog) {
		if (dialog != null) {
			if (dialogStack == null) {
				dialogStack = new Stack<BaseDialog>();
			}
			
			dialogStack.Push(dialog);
		}
	}

	private BaseDialog PopDialog() {
		BaseDialog result = null;

		if (activeDialog == null) {
			if (dialogStack != null && dialogStack.Count > 0) {
				result = dialogStack.Pop();
			}
		} else {
			LogHelper.LogError("Failed to popr "
				+ typeof(BaseDialog).Name
				+ "; "
				+ nameof(activeDialog)
				+ " is already assigned",
				this
			);
		}

		return result;
    }
}
