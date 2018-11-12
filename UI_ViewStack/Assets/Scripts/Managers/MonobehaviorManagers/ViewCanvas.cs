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


	#region Display methods

	public T DisplayDialog<T>(string dialogName = null) where T : BaseDialog {
		T result = LoadDialog<T>(dialogName);

		if (result != null) {
			result.dialogFramePrefab.gameObject.SetActive(false);

			if (activeDialog != null) {
				BaseDialog dialogToHide = activeDialog;
				PushDialog(dialogToHide);

				activeDialog = result;

				dialogToHide.dialogFramePrefab.Hide(() => {
					activeDialog.dialogFramePrefab.Show();
				});
			} else {
				activeDialog = result;
				activeDialog.dialogFramePrefab.Show();
			}

			SetDirty();
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

	#endregion


	public void TryCloseDialog(BaseDialog dialog) {
		if (dialog != null) {
			if (activeDialog != null) {
				if(dialog == activeDialog) {
					activeDialog = PopDialog();
					if (activeDialog != null) {
						activeDialog.dialogFramePrefab.gameObject.SetActive(false);
					}

					dialog.dialogFramePrefab.Hide(() => {
						GameObjectHelper.Destroy(dialog.dialogFramePrefab.gameObject, 0.25f);
						if(activeDialog!= null) {
							activeDialog.dialogFramePrefab.Show();
						}
					});

					SetDirty();
				} else {
					LogHelper.LogError("Cannot close dialog "
						+ dialog.GetType() 
						+ " "
						+ dialog.name
						+ "; "
						+ nameof(activeDialog)
						+ " and "
						+ nameof(dialog)
						+ " are not the same",
						this
					);
				}
			} else {
				LogHelper.LogError("Cannot close dialog "
					+ dialog.GetType().Name
					+ " "
					+ dialog.name 
					+ "; " 
					+ nameof(activeDialog) 
					+ " is not set",
					this
				);
			}
		}
	}
	
	private BaseDialog PopDialog() {
		BaseDialog result = null;

		if (dialogStack != null && dialogStack.Count > 0) {
			result = dialogStack.Pop();
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
					if (dialogPrefab.dialogFramePrefab != null) {
						DialogFrame dialoFrame =
							GameObjectHelper.AddChildrenWithComponent<DialogFrame>(
							dialogPrefab.dialogFramePrefab,
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
							+ nameof(dialogPrefab.dialogFramePrefab),
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

}
