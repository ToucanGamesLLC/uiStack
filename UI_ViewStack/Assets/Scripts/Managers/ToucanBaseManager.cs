using UnityEngine;

public class ToucanBaseManager<T1, T2> : BaseMonobehaviorManager<T2> 
	where T1 : new()
	where T2 : MonoBehaviour {

	private static T1 m_manager = new T1();
	public static T1 manager { get { return m_manager; } }

}
