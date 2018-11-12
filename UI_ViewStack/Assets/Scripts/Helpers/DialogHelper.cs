
public static class DialogHelper {

	public static T DisplayDialog<T>(BaseDialog.Options options, string dialogName = null) 
		where T : BaseDialog {

		T result = null;

		if(string.IsNullOrEmpty(dialogName)) {
			dialogName = typeof(T).Name;
		}

		result = ViewCanvas.Instance.DisplayDialog<T>(dialogName);
		if (result != null) {
			result.Populate(options);
		}

		return result;
	}
}
