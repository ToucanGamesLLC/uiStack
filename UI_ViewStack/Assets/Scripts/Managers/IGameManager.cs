using System.Collections;

namespace ToucanEngine {

	public interface IGameManager {

		IEnumerator Preinitialize();
		IEnumerator Initialize();
		IEnumerator Reset();

		bool initialzied { get; }

	}

}
