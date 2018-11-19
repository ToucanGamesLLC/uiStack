
public static class DialogHelper {

	public static T DisplayDialog<T>(BaseDialog.Options _options, string _dialogName = null) 
		where T : BaseDialog {

		T result = null;

		if(string.IsNullOrEmpty(_dialogName)) {
			_dialogName = typeof(T).Name;
		}

		result = ViewCanvas.Instance.DisplayDialog<T>(_dialogName);
		if (result != null) {
			result.Populate(_options);
		}

		return result;
	}
}
