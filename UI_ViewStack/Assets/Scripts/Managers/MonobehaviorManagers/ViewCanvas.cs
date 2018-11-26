using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ViewCanvas : MonoBehaviour { 

	#region Static fields

	public static ViewCanvas Instance = null;

	#endregion

	#region public fields and accessors

	public RectTransform dialogCanvas;

	public BaseDialog activeDialog { get; private set; }
	public Stack<BaseDialog> dialogStack { get; private set; }

	#endregion


	#region Monobehavior

	private void Awake() {
		Instance = this;
	}

	private void OnDestroy() {
		Instance = null;
	}

	#endregion


	#region Display methods

	public T DisplayDialog<T>(string _dialogName = null) where T : BaseDialog {
		T result = LoadDialog<T>(_dialogName);

		if (result != null) {
			result.dialogFrame.gameObject.SetActive(false);

			if (activeDialog != null) {
				BaseDialog dialogToHide = activeDialog;
				PushDialog(dialogToHide);

				activeDialog = result;

				dialogToHide.dialogFrame.Hide(() => {
					activeDialog.dialogFrame.Show();
				});
			} else {
				activeDialog = result;
				activeDialog.dialogFrame.Show();
			}

			SetDirty();
		}

		return result;
	}

	private void PushDialog(BaseDialog _dialog) {
		if (_dialog != null) {
			if (dialogStack == null) {
				dialogStack = new Stack<BaseDialog>();
			}
			
			dialogStack.Push(_dialog);
		}
	}

	#endregion


	public void TryCloseDialog(BaseDialog _dialog) {
		if (_dialog != null) {
			if (activeDialog != null) {
				if(_dialog == activeDialog) {
					activeDialog = PopDialog();
					if (activeDialog != null) {
						activeDialog.dialogFrame.gameObject.SetActive(false);
					}

					_dialog.dialogFrame.Hide(() => {
						GameObjectHelper.Destroy(_dialog.dialogFrame.gameObject, 0.25f);
						if(activeDialog!= null) {
							activeDialog.dialogFrame.Show();
						}
					});

					SetDirty();
				} else {
					LogHelper.LogError("Cannot close dialog "
						+ _dialog.GetType() 
						+ " "
						+ _dialog.name
						+ "; "
						+ nameof(activeDialog)
						+ " and "
						+ nameof(_dialog)
						+ " are not the same",
						this
					);
				}
			} else {
				LogHelper.LogError("Cannot close dialog "
					+ _dialog.GetType().Name
					+ " "
					+ _dialog.name 
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

	private T LoadDialog<T>(string _dialogName = null) where T : BaseDialog {
		T result = null;

		if (dialogCanvas != null) {
			_dialogName = (!string.IsNullOrEmpty(_dialogName))
				? _dialogName
				: typeof(T).Name;

			if (!string.IsNullOrEmpty(_dialogName)) {
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
									+ _dialogName,
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

	private void SetDirty() {

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif

	}

}
