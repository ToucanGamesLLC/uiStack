using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ViewCanvas))]
public class ViewCanvasEditorTool : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (Application.isPlaying) {
			ViewCanvas viewCanvas = (ViewCanvas)target;

			if (viewCanvas != null) {
				DisplayActiveDialogs(viewCanvas);
			}
		}
	}

	private void DisplayActiveDialogs(ViewCanvas _viewCanvas) {
		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		
		DisplayBaseDialog("Active Dialog", _viewCanvas.activeDialog);

		Stack<BaseDialog> dialogStack = _viewCanvas.dialogStack;
		int activeDialogs = (dialogStack != null) ? dialogStack.Count : 0;
		EditorGUILayout.LabelField("Stacked Dialogs : " + activeDialogs);
		if (activeDialogs > 0) {
			int count = 1;
			foreach(BaseDialog dialog in dialogStack) {
				DisplayBaseDialog(count.ToString(), dialog);
				count++;
			}
		}

		EditorGUILayout.EndVertical();
	}

	private void DisplayBaseDialog(string _label, BaseDialog _dialog) {
		if (_dialog != null) {
			_label = string.Format("{0} : {1} ({2})", 
				_label, 
				_dialog.name, 
				_dialog.GetType().Name
			);
		}

		EditorGUILayout.LabelField(_label);
	}
}
