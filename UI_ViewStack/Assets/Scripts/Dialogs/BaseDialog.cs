using UnityEngine;

public abstract class BaseDialog : MonoBehaviour {

    #region Constants

    private const float REFRESH_INTERVAL = 1.0f;

	#endregion


	#region Public fields

	public DialogFrame dialogFrame;
	public DialogFrame.DisplayAnimType displayAnimType;
	public float topOffset;
	public float bottomOffset;
	public float leftOffset;
	public float rightOffset;

	#endregion


	#region Accessors 

	public class Options {
		public string titleText;
        public bool allowRefrehInterval;
    }

    public Options options { get; private set; }
	
	#endregion


	#region Private fields

	private float m_updateInterval;

    #endregion


    #region Monobehavior

	protected virtual void Awake() {

	}

    private void Update() {
        if (options != null && options.allowRefrehInterval) {
            m_updateInterval += Time.deltaTime;

            if (m_updateInterval >= REFRESH_INTERVAL) {
                RefreshOnInterval();
            }
        }
    }

    #endregion


    public virtual void Populate(Options options) {
        this.options = options;
    }

    public virtual void RefreshOnInterval() { }
}
