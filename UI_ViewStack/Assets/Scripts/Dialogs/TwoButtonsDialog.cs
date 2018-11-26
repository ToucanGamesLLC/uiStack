using TMPro;
using UnityEngine.UI;

public class TwoButtonsDialog : BaseDialog {

	public new class Options : BaseDialog.Options {

		public string messageText;

		public bool showRightButton;
		public string rightButtonText;
		public VoidBaseDialogDelegate onRightButtonClick;

		public bool showLeftButton;
		public string leftButtonText;
		public VoidBaseDialogDelegate onLeftButtonClick;
		
	}


	#region Public properties

	public TMP_Text messageText;

	public TMP_Text rightButtonText;
	public Button rightButton;

	public TMP_Text leftButtonText;
	public Button leftButton;

	#endregion


	#region Private properties

	private VoidBaseDialogDelegate m_onRightButtonClick;
	private VoidBaseDialogDelegate m_onLeftButtonClick;

	#endregion


	#region BaseDialog

	public override void Populate(BaseDialog.Options _options) {
		base.Populate(_options);

		Options twoButtonOptions = _options as TwoButtonsDialog.Options;
		if (twoButtonOptions != null) {
			if (rightButton != null) {
				SetActive(rightButton.gameObject, twoButtonOptions.showRightButton);
				m_onRightButtonClick = twoButtonOptions.onRightButtonClick;
			}
			SetText(rightButtonText, twoButtonOptions.rightButtonText);

			if (leftButton != null) {
				SetActive(leftButton.gameObject, twoButtonOptions.showLeftButton);
				m_onLeftButtonClick = twoButtonOptions.onLeftButtonClick;
			}
			SetText(leftButtonText, twoButtonOptions.leftButtonText);

			SetText(messageText, twoButtonOptions.messageText);
		} else {
			LogHelper.LogWarning("Failed to populate "
				+ GetType().Name
				+ "; "
				+ nameof(_options)
				+ " is not of type "
				+ typeof(TwoButtonsDialog.Options).Name,
				this
			);
		}
	}

	#endregion


	#region UI Callbacks

	public void OnRightButtonClick() {
		if (m_onRightButtonClick != null) {
			m_onRightButtonClick(this);
		}

		CloseDialog();
	}

	public void OnLeftButtonClick() {
		if (m_onLeftButtonClick != null) {
			m_onLeftButtonClick(this);
		}

		CloseDialog();
	}

	#endregion

}
