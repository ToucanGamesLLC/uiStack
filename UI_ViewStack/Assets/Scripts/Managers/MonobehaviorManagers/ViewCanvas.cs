using System.Collections.Generic;
using UnityEngine;

public class ViewCanvas : Singleton<ViewCanvas> {

	#region constants fields

	private const string DIALOGS_RESOURCE_PATH = "Dialogs/";
	#endregion


	#region private fields

	private Stack<BaseDialog> m_dialogStack;

	#endregion


	#region public fields and accessors

	public RectTransform dialogCanvas;

	public BaseDialog activeDialog { get; private set; }

	#endregion


	private DialogFrame LoadDialog(string dialogName) {
		DialogFrame result = null;

		if (!string.IsNullOrEmpty(dialogName)) { 
			BaseDialog dialogPrefab = Resources.Load<BaseDialog>(
				DIALOGS_RESOURCE_PATH + dialogName
			);

			if (dialogPrefab != null) {
				if (dialogPrefab.dialogFrame != null) {
					result = Instantiate<DialogFrame>(dialogPrefab.dialogFrame);

					if (result != null) {
						if (!result.TryInstantiateDialogContent(dialogPrefab)) {
							LogHelper.LogError(
								"Failed to instantiate dialog content for " 
								+ dialogName, 
								this
							);

							GameObjectHelper.Destroy(result.gameObject, true);
						}
					}
				}
			}
		}

		return result;
	}

	public void PushDialog(BaseDialog dialog) {
		if (dialog != null) {
			if (activeDialog != null) {
				if (m_dialogStack == null) {
					m_dialogStack = new Stack<BaseDialog>();
				}

				m_dialogStack.Push(activeDialog);
			}

			activeDialog = dialog;
		}
	}

    public void PopActiveDialog(BaseDialog dialog) {
        if (activeDialog != null) {
			GameObjectHelper.Destroy(dialog.gameObject);
        }

		if (m_dialogStack != null && m_dialogStack.Count > 0) {
			activeDialog = m_dialogStack.Pop();
		}
    }
}
