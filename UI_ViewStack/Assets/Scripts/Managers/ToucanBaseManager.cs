
public class ToucanBaseManager {

	#region static fields

	private static ToucanBaseManager m_instance = new ToucanBaseManager();
	public static ToucanBaseManager instance { get { return m_instance; } }

	#endregion

	protected ToucanBaseManager() { }

}