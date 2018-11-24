using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseMonobehaviorGameManager<GameManager> {

	private delegate IEnumerator IEnumeratorVoid();

	public List<IGameManager> m_gameManagers;

	private void Start() {
		StartCoroutine(InitializeGameManagers());
	}


	#region BaseMonobehaviorGameManager

	private IEnumerator InitializeGameManagers() {
		yield return base.Initialize();
		
		PrefabManager.Instance.name = "(S)" + PrefabManager.Instance.GetType().Name;
		PrefabManager.Instance.transform.SetParent(this.transform);
		
		yield return RunManagerFunctionBlock<PrefabManager>(PrefabManager.Instance.Preinitialize);

		yield return RunManagerFunctionBlock<PrefabManager>(PrefabManager.Instance.Initialize);

		if (ViewCanvas.Instance == null) {
			GameObject gameCanvas = PrefabManager.Instance.LoadPrefab(typeof(ViewCanvas).Name);
			if (gameCanvas != null) {
				GameObject gameCanvasObject = GameObject.Instantiate(gameCanvas);

			}
		} else {
			LogHelper.LogError(typeof(ViewCanvas).Name + " already instantiated", this);
		}

		// yield return RunManagerFunctionBlock<ViewCanvas>(ViewCanvas.Instance.Preinitialize);

		// yield return RunManagerFunctionBlock<ViewCanvas>(ViewCanvas.Instance.Initialize);

		m_initialized = true;

		SetDirty();
	}

	#endregion


	private IEnumerator RunManagerFunctionBlock<T>(IEnumeratorVoid managerFunction) {
		if (managerFunction != null) {
			int step = 0;

			IEnumerator enumerator = managerFunction();
			while(enumerator.MoveNext()) {
				step++;
				LogHelper.LogMessage(typeof(T).Name + "_" + managerFunction.Method.Name + "_" + step);
				yield return null;
			}
		}

		yield return null;

	}

}
