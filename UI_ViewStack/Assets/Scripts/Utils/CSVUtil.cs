using System.Collections.Generic;

public static class CSVUtil {
	
	public static string [][] DeserializeCSV(string _csvText) {
		string[][] result = null;

		if (!string.IsNullOrEmpty(_csvText)) {
			_csvText = _csvText.Replace("\r", "");
			string[] lines = _csvText.Split('\n');

			if (lines != null && lines.Length > 0) {
				result = new string[lines.Length][];
				for(int i = 0; i < lines.Length; i++) {
					result[i] = lines[i].Split(',');
				}
			}

			LogHelper.LogMessage("Done deserializing csv data: "
				+ result.Length
				+ " entries"
			);
		} else {
			LogHelper.LogError("Failed to deserialize data; "
				+ nameof(_csvText)
				+ " is not set"
			);
		}

		return result;
	}
}
