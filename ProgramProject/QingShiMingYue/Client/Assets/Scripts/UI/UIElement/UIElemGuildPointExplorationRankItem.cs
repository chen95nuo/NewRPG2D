using UnityEngine;
using System.Collections;

public class UIElemGuildPointExplorationRankItem : MonoBehaviour 
{
	public SpriteText rankLabel;
	public SpriteText nameLabel;	
	public SpriteText explorLabel;
	public SpriteText moneyLabel;

	public void SetData(string rank, string name, string explor, string money)
	{
		rankLabel.Text = rank;
		nameLabel.Text = name;
		explorLabel.Text = explor;
		moneyLabel.Text = money;
	}
}
