using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcuts : MonoBehaviour {

	private void Update() {
		if(Input.GetKeyUp(KeyCode.A)) {
			Debug.Log("Calling key up A");
			DialogHelper.DisplayDialog<ConfirmDialog>(new BaseDialog.Options());
		}
	}
}
