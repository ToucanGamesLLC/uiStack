
public static class DialogHelper {

	#region TwoButtonsDialog methods

	public static TwoButtonsDialog DisplayAlertDialog(TwoButtonsDialog.Options _options) {
		TwoButtonsDialog result = null;

		if (_options != null) {
			_options.showCloseButton = false;
			_options.showLeftButton = true;
			_options.showRightButton = false;

			result = DisplayDialog<TwoButtonsDialog>(_options);
		} else {
			LogHelper.LogWarning("Cannot display " 
				+ typeof(TwoButtonsDialog).Name
				+ "; "
				+ nameof(_options)
				+ " is not set"
			);
		}

		return result;
	}

	#endregion


	private static T DisplayDialog<T>(BaseDialog.Options _options, string _dialogName = null) 
		where T : BaseDialog {

		T result = null;

		if(string.IsNullOrEmpty(_dialogName)) {
			_dialogName = typeof(T).Name;
		}

		result = ViewCanvas.Instance.DisplayDialog<T>(_dialogName);
		if (result != null) {
			result.Populate(_options);
		} else {
			LogHelper.LogWarning("Failed to display "
				+ typeof(T).Name
				+ " dialog named "
				+ _dialogName
			);
		}

		return result;
	}

}
