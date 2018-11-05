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

	private void DisplayActiveDialogs(ViewCanvas viewCanvas) {
		EditorGUILayout.BeginVertical(EditorStyles.helpBox);
		
		DisplayBaseDialog("Active Dialog", viewCanvas.activeDialog);

		Stack<BaseDialog> dialogStack = viewCanvas.dialogStack;
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

	private void DisplayBaseDialog(string label, BaseDialog dialog) {
		if (dialog != null) {
			label = string.Format("{0} : {1} ({2})", 
				label, 
				dialog.name, 
				dialog.GetType().Name
			);
		}

		EditorGUILayout.LabelField(label);
	}
}
