using UnityEngine;

public static class LogHelper {

	public static void LogMessage(string message, Object logger = null) {
		Debug.Log(message, logger);
	}

	public static void LogWarning(string message, Object logger = null) {
		Debug.LogWarning(message, logger);
	}

	public static void LogError(string message, Object logger = null) {
		Debug.LogError(message, logger);
	}

}
