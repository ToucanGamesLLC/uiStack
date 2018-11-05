using System.Collections;

public interface IGameManager {

	IEnumerator Preinitialize();
	IEnumerator Initialize();
	IEnumerator Reset();

	bool initialzied { get; }

}
