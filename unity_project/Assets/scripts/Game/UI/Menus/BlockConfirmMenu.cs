using UnityEngine;
using System.Collections;

public class BlockConfirmMenu : AutoConfirmMenu {

	public UISprite	blockSprite;

	void Awake()
	{
		GameSystem.GetInstance().gameUI.blockConfirmMenu = this;
	}

	public override void Show (bool active)
	{
		if (active)
		{
			GameSystem.GetInstance().gameUI.confirmMenu.Show(false);
		}
		base.Show (active);
	}

	protected override int CustomContentHeight ()
	{
		return Mathf.Max(blockSprite.height, messageLabel.height);
	}

	protected override void CustomContent ()
	{
		Vector3 messageLabelPos = messageLabel.transform.localPosition;
		Vector3 blockSpritePos = blockSprite.transform.localPosition;
		blockSpritePos.y = messageLabelPos.y - messageLabel.height / 2 + 9;
		blockSprite.transform.localPosition = blockSpritePos;
	}
}
