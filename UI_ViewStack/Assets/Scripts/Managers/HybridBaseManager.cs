using UnityEngine;

public class HybridBaseManager<T1, T2> : BaseMonobehaviorManager<T2> 
	where T1 : ToucanBaseManager
	where T2 : MonoBehaviour {

	public static T1 manager { get { return ToucanBaseManager.instance as T1; } }

}
