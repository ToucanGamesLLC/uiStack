using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcuts : MonoBehaviour {

	private void Update() {
		if(Input.GetKeyUp(KeyCode.A)) {
			Debug.Log("Calling key up A => " + typeof(TwoButtonsDialog).Name);

			DialogHelper.DisplayAlertDialog(new TwoButtonsDialog.Options() {
				titleText = 
					LocalizationManager.Instance.GetPrefixText("AlertTitle_",
						"Test", 
						null,
						true
					),
				messageText =
					LocalizationManager.Instance.GetPrefixText("AlertMessage_",
						"Test", 
						null, 
						true
					),
				leftButtonText = LocalizationManager.Instance.GetText("Alert", 
					null,
					true
				)
			});
		}
	}
}
