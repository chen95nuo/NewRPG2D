using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDomineerPictureItem : MonoBehaviour
{
	public List<UIElemAssetIcon> cardBtns;
	public List<UIBox> cardSelectedBorders;

	public UIListItemContainer container;

	public void SetData(List<int> cardPictures)
	{
		for(int i = 0 ; i < cardSelectedBorders.Count; i++)
		{
			cardSelectedBorders[i].Hide(true);
		}

		container.Data = this;
		
		int index = 0;
		for (; index < cardPictures.Count; index++)
		{
			int cardId = cardPictures[index];

			cardBtns[index].SetData(cardId);

			cardBtns[index].Data = cardId;
			cardBtns[index].Hide(false);
		}

		for (; index < cardBtns.Count; index++)
		{
			cardBtns[index].Hide(true);
			cardSelectedBorders[index].Hide(true);
			cardBtns[index].Data = -1;// 清楚之前记录的数据
		}
	}

	public bool SetSelect(int cardId)
	{
		bool HasSelected = false;
		for(int i = 0 ; i < cardBtns.Count; i++)
		{
			if(cardBtns[i].Data != null && (int)cardBtns[i].Data == cardId)
			{
				cardSelectedBorders[i].Hide(false);
				HasSelected = true;
			}
			else
			{
				cardSelectedBorders[i].Hide(true);
			}
		}

		return HasSelected;
	}
	public void HideAllIcon()
	{
		for (int i = 0; i < cardBtns.Count;i++ )
		{
			cardSelectedBorders[i].Hide(true);
		}
	}
}
