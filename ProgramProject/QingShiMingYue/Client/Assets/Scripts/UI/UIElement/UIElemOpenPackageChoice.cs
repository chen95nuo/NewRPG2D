using UnityEngine;
using System.Collections;

using Group = ClientServerCommon.ItemConfig.Group;

public class UIElemOpenPackageChoice : MonoBehaviour 
{
	public SpriteText groupName;
	public SpriteText groupDesc;
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public UIStateToggleBtn selectBtn;
	public bool IsSelected{ get{ return selectBtn.StateName.Equals("On"); } }

	public void SetData(Group group, bool canUseItem)
	{
		groupName.Text = group.name;
		groupDesc.Text = group.desc;

		selectBtn.Hide(!canUseItem);
		if (selectBtn.IsHidden() == false)
			selectBtn.SetToggleState("Off");

		rewardList.ClearList(false);
		foreach (var reward in group.consumeReward)
		{
			UIListItemContainer container = rewardPool.AllocateItem().GetComponent<UIListItemContainer>();
			container.gameObject.GetComponent<SpriteText>().Text = GameUtility.FormatUIString("UINameXCount", ItemInfoUtility.GetAssetName(reward.id), reward.count.ToString());
			rewardList.AddItem(container);
		}
	}
}
