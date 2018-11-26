using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : BaseMonobehaviorGameManager<LocalizationManager> {

	#region Test property 

	private const string LANGUAGE = "English";

	#endregion


	#region const fields

	private const string SPRTES_CSV_FILE_NAME = "Localization";

	#endregion


	#region Public fields

	public TextAsset dataFile;

	#endregion


	#region Private fields

	private Dictionary<string, int> m_languageKeyToIndex;
	private Dictionary<string, List<string>> m_localizedValues;

	#endregion

	#region BaseMonobehaviorGameManager

	public override IEnumerator Preinitialize() {
		yield return base.Preinitialize();

		if (dataFile != null) {
			CacheTranslationData(dataFile.text);
		} else {
			LogHelper.LogWarning("Failed to initialize "
				+ this.GetType()
				+ "; "
				+ nameof(dataFile)
				+ " is not set",
				this
			);
		}
	}

	public override IEnumerator Initialize() {
		yield return base.Initialize();
	}

	#endregion


	#region Localized Text methods

	private int GetPlayerLanguageIndex(string _language, bool _logError) {
		int result = -1;

		if (!string.IsNullOrEmpty(_language)) {
			if (m_languageKeyToIndex == null
				|| !m_languageKeyToIndex.TryGetValue(_language, out result)
			) {
				result = -1;
			}

			if (result < 0 && _logError) {
				LogHelper.LogWarning("Failed to get language index for "
					+ LANGUAGE
					+ "; data is not set",
					this
				);
			}
		} else if (_logError) {
			LogHelper.LogWarning("Failed to get language index; "
				+ nameof(_language)
				+ " is not set",
				this
			);
		}

		return result;
	}

	private string GetLocalizedText(string _localizationKey,
		int _languageIndex,
		bool _logError
	) {
		string result = null;

		if (!string.IsNullOrEmpty(_localizationKey)) {
			if (m_localizedValues != null) {
				List<string> translationData = null;
				if (m_localizedValues.TryGetValue(_localizationKey, out translationData)
					&& translationData != null
				) {
					if (_languageIndex >= 0 && _languageIndex < translationData.Count) {
						result = translationData[_languageIndex];
					} else if (_logError) {
						LogHelper.LogWarning("Failed to get localized text; "
							+ _languageIndex
							+ " is out of range for key "
							+ _localizationKey,
							this
						);
					}
				} else if (_logError) {
					LogHelper.LogWarning("Failed to get localized text; "
						+ nameof(translationData)
						+ " is not set for key "
						+ _localizationKey,
						this
					);
				}
			} else if (_logError) {
				LogHelper.LogWarning("Failed to get localized text; "
					+ nameof(m_localizedValues)
					+ " is not set",
					this
				);
			}
		} else if (_logError) {
			LogHelper.LogWarning("Failed to get localized text; "
				+ nameof(_localizationKey)
				+ " is not set",
				this
			);
		}

		return result;
	}

	private string BuildLocalizedText(string _localizedText,
		Dictionary<string, string> _localizationReplacements
	) {
		string result = null;

		if (!string.IsNullOrEmpty(_localizedText)) {
			result = _localizedText;

			if (_localizationReplacements != null) {
				foreach (KeyValuePair<string, string> keyValuePair in _localizationReplacements) {
					result = result.Replace(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		return result;
	}

	public string GetText(string _localizationKey, 
		Dictionary<string, string> _localizationReplacements = null, 
		bool _logError = false
	) {
		string result = null;

		if (!string.IsNullOrEmpty(_localizationKey)) {
			int languageIndex = GetPlayerLanguageIndex(LANGUAGE, _logError);

			if (languageIndex >= 0) {
				string loclizedText = GetLocalizedText(_localizationKey, languageIndex, _logError);

				if (!string.IsNullOrEmpty(loclizedText)) {
					result = BuildLocalizedText(loclizedText, _localizationReplacements);
				}

				if (string.IsNullOrEmpty(result)) {
					result = "<" + _localizationKey  + ">";
				}
			}
		} else if (_logError) {
			LogHelper.LogWarning("Failed to get localized text; "
				+ nameof(_localizationKey)
				+ " is not set",
				this
			);
		}
		

		return result;
	}

	public string GetPrefixText(string _prefix,
		string _localizationKey,
		Dictionary<string, string> _localizationReplacements = null,
		bool _logError = false
	) {
		return GetText(_prefix + _localizationKey, 
			_localizationReplacements, 
			_logError
		);
	}

	#endregion


	#region Deserialization methods

	private void CacheTranslationData(string _serializedData) {
		string[][] deserializedData = CSVUtil.DeserializeCSV(_serializedData);

		m_languageKeyToIndex = new Dictionary<string, int>();
		m_localizedValues = new Dictionary<string, List<string>>();

		if (deserializedData != null && deserializedData.Length > 0) {
			for (int i = 0; i < deserializedData.Length; i++) {
				if (i == 0) {
					DeserializeLanguages(deserializedData[i]);
				} else {
					DeserializeTranslations(deserializedData[i]);
				}
			}
		} else {
			LogHelper.LogWarning("Failed to initialize "
				+ this.GetType()
				+ "; "
				+ nameof(dataFile)
				+ " is not set",
				this
			);
		}
	}

	private void DeserializeLanguages(string [] _languages) {
		if (_languages != null && _languages.Length > 0) {
			m_languageKeyToIndex = new Dictionary<string, int>();

			for(int i = 1; i < _languages.Length; i++) {
				string langugage = _languages[i];

				if (!string.IsNullOrEmpty(langugage)) {
					if (!m_languageKeyToIndex.ContainsKey(langugage)) {
						m_languageKeyToIndex.Add(langugage, (i - 1));
					} else {
						LogHelper.LogWarning("Failed to set language index; "
							+ langugage
							+ " is duplicated on index "
							+ i,
							this
						);
					}
				} else {
					LogHelper.LogWarning("Failed to set language index; "
						+ nameof(langugage)
						+ " is not set for index "
						+ i,
						this
					);
				}
			}
		} else {
			LogHelper.LogWarning("Failed to deserialize languages; "
				+ nameof(_languages)
				+ " is not set",
				this
			);
		}
	}

	private void DeserializeTranslations(string [] _translations) {
		if (_translations != null && _translations.Length > 0) {
			if (m_localizedValues == null) {
				m_localizedValues = new Dictionary<string, List<string>>();
			}

			string translationKey = _translations[0];
			if (!string.IsNullOrEmpty(translationKey)) {
				if (!m_localizedValues.ContainsKey(translationKey)) {
					m_localizedValues.Add(translationKey, 
						new List<string>(new List<string>(_translations.Length - 1))
					);
					
					for (int i = 1; i < _translations.Length; i++) {
						string translation = _translations[i];
						m_localizedValues[translationKey].Add(translation);
					}
				} else {
					LogHelper.LogWarning("Failed to cache translation data; "
						+ translationKey
						+ " is duplicated",
						this
					);
				}
			} else {
				LogHelper.LogWarning("Failed to cache translation data; "
					+ nameof(translationKey)
					+ " is not set",
					this
				);
			}
		} else {
			LogHelper.LogWarning("Failed to deserialize languages; "
				+ nameof(_translations)
				+ " is not set",
				this
			);
		}
	}

	#endregion

}
