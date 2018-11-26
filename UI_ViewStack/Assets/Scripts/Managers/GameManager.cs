using ToucanEngine;
using System.Collections;
using UnityEngine;
using System;

public class GameManager : BaseMonobehaviorGameManager<GameManager> {

	#region Delegates

	private delegate IEnumerator IEnumeratorVoid();

	#endregion


	#region Monobehavior

	private void Start() {
		StartCoroutine(InitializeGameManagers());
	}

	#endregion

	#region BaseMonobehaviorGameManager

	private IEnumerator PreInitializeManagers() {
		//PrefabManager.Instance.name = "(S)" + PrefabManager.Instance.GetType().Name;
		//PrefabManager.Instance.transform.SetParent(this.transform);

		yield return RunManagerFunctionBlock(typeof(PrefabManager),
			PrefabManager.Instance.Preinitialize
		);

		yield return RunManagerFunctionBlock(typeof(LocalizationManager),
			LocalizationManager.Instance.Preinitialize
		);
	}

	private IEnumerator InitializeManagers() {
		yield return RunManagerFunctionBlock(typeof(PrefabManager),
			PrefabManager.Instance.Initialize
		);

		yield return RunManagerFunctionBlock(typeof(LocalizationManager),
			LocalizationManager.Instance.Initialize
		);
	}

	private IEnumerator InitializeGameManagers() {
		yield return base.Initialize();

		yield return PreInitializeManagers();

		yield return InitializeManagers();

		if (ViewCanvas.Instance == null) {
			ViewCanvas viewCanvasPrefab = 
				PrefabManager.Instance.LoadPrefabWithComponent<ViewCanvas>();

			if (viewCanvasPrefab != null) {
				if (GameObject.Instantiate(viewCanvasPrefab.gameObject) == null) {
					LogHelper.LogError("Failed to instantiate "
						+ typeof(ViewCanvas).Name,
						this
					);
				}
			}
		} else {
			LogHelper.LogError(typeof(ViewCanvas).Name + " already instantiated", this);
		}
		
		m_initialized = true;

		SetDirty();
	}

	#endregion


	private IEnumerator RunManagerFunctionBlock(Type managerType, IEnumeratorVoid managerFunction) {
		if (managerFunction != null) {
			int step = 0;

			IEnumerator enumerator = managerFunction();
			while(enumerator.MoveNext()) {
				step++;
				LogHelper.LogMessage(managerType.Name
					+ "_" 
					+ managerFunction.Method.Name 
					+ "_" 
					+ step
				);

				yield return null;
			}
		}

		yield return null;

	}

}
