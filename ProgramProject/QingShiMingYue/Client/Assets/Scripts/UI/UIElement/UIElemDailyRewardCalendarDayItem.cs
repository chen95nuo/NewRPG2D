	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemDailyRewardCalendarDayItem : MonoBehaviour
{
	public List<SpriteText> dayLabels;
	public GameObjectPool signIconPool;
	private int rowIndex;
	public int RowIndex { get { return rowIndex; } }

	public void SetData(int rowIndex)
	{
		this.rowIndex = rowIndex;

		for (int index = 0; index < UIDlgDailyReward.MaxColunm; index++)
		{
			int dayNum = (rowIndex - 1) * UIDlgDailyReward.MaxColunm + index + 1 - CalculateDataOffset();

			bool isVilidDay = IsVilidDay(dayNum);
			dayLabels[index].Hide(isVilidDay == false);

			if (isVilidDay)
			{
				// Set the day num.
				dayLabels[index].Text = dayNum.ToString();

				// Set the day color.
				if (SysLocalDataBase.Inst.LoginInfo.NowDateTime.Day == dayNum|| UIDlgDailyReward.SignInState(dayNum))
					dayLabels[index].SetColor(GameDefines.txColorWhite);
				else
					dayLabels[index].SetColor(GameDefines.txColorSignDayColor);

				// Set the Sign icon if this day has signed.
				bool hasSigned = UIDlgDailyReward.SignInState(dayNum);

				//	SetSignAnimation(hasSigned, false, index);

				if (hasSigned)
				{
					if (dayLabels[index].transform.childCount <= 0)
					{
						GameObject signIconObject = signIconPool.AllocateItem();
						signIconObject.transform.parent = dayLabels[index].transform;
						signIconObject.transform.localPosition = new Vector3(0f, 2f, -0.001f);
						signIconObject.transform.localScale = new Vector3(1f, 1f, 1f);
					}
				}
				else
				{
					if (dayLabels[index].transform.childCount > 0)
						signIconPool.ReleaseItem(dayLabels[index].transform.GetChild(0).gameObject);
				}

			}
			else
			{
				dayLabels[index].Text = "";
				dayLabels[index].Hide(true);
				SetSignIconPool(false, index);
			}
		}
	}

	public void SetSignIconPool(int dayNum)
	{
		for (int index = 0; index < dayLabels.Count; index++)
		{
			if (dayLabels[index].Text.Equals(dayNum.ToString()))
			{
				SetSignIconPool(true, index);
				break;
			}
		}
	}

	private void SetSignIconPool(bool show, int index)
	{
		if (show)
		{
		    GameObject signIconObject = signIconPool.AllocateItem();
		    signIconObject.transform.parent = dayLabels[index].transform;
		    signIconObject.transform.localPosition = new Vector3(0f, 2f, -0.001f);
		    signIconObject.transform.localScale = new Vector3(1f, 1f, 1f);
			dayLabels[index].SetColor(GameDefines.txColorWhite);
		    
		}
		else
		{
		    if (dayLabels[index].transform.childCount > 0)
		        signIconPool.ReleaseItem(dayLabels[index].transform.GetChild(0).gameObject);
		}
	}

	private bool IsVilidDay(int dayNum)
	{
		int daysInMonth = System.DateTime.DaysInMonth(SysLocalDataBase.Inst.LoginInfo.NowDateTime.Year, SysLocalDataBase.Inst.LoginInfo.NowDateTime.Month);

		if (dayNum < 1 || dayNum > daysInMonth)
			return false;

		return true;
	}

	private int CalculateDataOffset()
	{
		// TODO : use DayOfWeek instead.
		int year = SysLocalDataBase.Inst.LoginInfo.NowDateTime.Year;
		int month = SysLocalDataBase.Inst.LoginInfo.NowDateTime.Month;
		int day = 1;

		if (month == 1 || month == 2)
		{
			year--;
			month += 12;
		}

		//		W= (d+2*m+3*(m+1)/5+y+y/4-y/100+y/400) mod 7 
		int week = (day + 2 * month + 3 * (month + 1) / 5 + year + year / 4 - year / 100 + year / 400) % 7;


		if (week == 6)
			return 0;

		return week + 1;
	}
}
