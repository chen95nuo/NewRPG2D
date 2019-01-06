using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

//剧情战斗：角色特殊变化Round
public class Custom_RoleTweenRound : BattleRound
{
	public enum TweenType
	{
		Scale,//大小变化
	}

	class RoleTweenTransaction
	{
		public BattleRole targetRole;//目标角色
		public TweenType tweenType;
		public float tweenTime = 0;//变化总时间
		public float timer = 0;//计时器
		public bool ignoreTimeScale = false;//是否忽略时间缩放
		public object startValue, endValue;//变化的开始数值和结束数值，用于插值计算
		public EZAnimation.Interpolator interpolator;//插值函数
	}

	List<RoleTweenTransaction> tweenTransactions = new List<RoleTweenTransaction>();

	protected float RealDeltaTime
	{
		get { return Time.timeScale == 0 ? 0 : Time.deltaTime / Time.timeScale; }
	}

	public Custom_RoleTweenRound(RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
		tweenTransactions.Clear();

		Debug.Assert(roundRecord != null, "[Custom_RoleTweenRound] roundRecord is null");

		if (roundRecord.configParameterDic == null)
			return;

		foreach (string parameter in roundRecord.configParameterDic.Values)
		{
			RoleTweenTransaction rtt = new RoleTweenTransaction();
			if (GenTransaction(parameter, rtt))
				tweenTransactions.Add(rtt);
		}

	}

	//从配置中解析变化事务
	bool GenTransaction(string parameter, RoleTweenTransaction rtt)
	{
		rtt.timer = 0;

		//parameter
		//Value开起来是这样的：TargetRoleIdx=3 TweenType=Scale TweenTime=0.724 IgnoreTimeScale=false EndValue=2
		//一下解析Value
		string[] lv1Split = parameter.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		int temp;
		float temp1;
		string endValue = "";
		try
		{
			//pair
			//key=value
			foreach (string pair in lv1Split)
			{
				string[] config = pair.Trim().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
				switch (config[0])
				{
					case "TargetRoleIdx":
						temp = StrParser.ParseDecIntEx(config[1], -1);
						if (temp == -1)
							throw new Exception("配置错误:" + pair);

						rtt.targetRole = battleRecordPlayer.BattleRoles[temp];
						break;

					case "TweenType":
						rtt.tweenType = (TweenType)Enum.Parse(typeof(TweenType), config[1], true);
						break;

					case "TweenTime":
						temp1 = StrParser.ParseFloat(config[1], -1);
						if (temp1 < 0)
							throw new Exception("配置错误:" + pair);
						rtt.tweenTime = temp1;
						break;

					case "EndValue":
						endValue = config[1];
						break;

					case "IgnoreTimeScale":
						rtt.ignoreTimeScale = StrParser.ParseBool(config[1], false);
						break;
				}
			}

			//parse EndValue by TweenType
			switch (rtt.tweenType)
			{
				case TweenType.Scale:
					temp1 = StrParser.ParseFloat(endValue, -1);
					if (temp1 < 0)
						throw new Exception("配置错误: endValue=" + endValue);

					rtt.endValue = temp1;
					rtt.startValue = rtt.targetRole.CachedTransform.localScale.x;
					break;
			}

			if (rtt.tweenTime == 0)
				throw new Exception("配置错误：TewwnTime=0");

			rtt.interpolator = EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default);

		}
		catch (System.Exception ex)
		{
			Debug.LogError("[Custom_RoleTweenRound] ConfigParameter process error.：" + parameter + "    " + ex.Message);
			return false;
		}

		return true;
	}

	public override void Update()
	{
		base.Update();

		if (RoundState == _RoundState.NotStarted || RoundState == _RoundState.Finished)
			return;

		List<int> transacToBeRemoved = new List<int>();
		for (int i = 0; i < tweenTransactions.Count; i++)
		{
			var transac = tweenTransactions[i];

			switch (transac.tweenType)
			{
				case TweenType.Scale:
					if (transac.timer > transac.tweenTime)
					{
						transac.targetRole.CachedTransform.localScale = Vector3.one * (float)transac.endValue;
						if (!transacToBeRemoved.Contains(i))
							transacToBeRemoved.Add(i);
					}
					else
					{
						//transac.targetRole.CachedTransform.localScale = Vector3.one * Mathf.Lerp((float)transac.startValue, (float)transac.endValue, transac.timer / transac.tweenTime);
						transac.targetRole.CachedTransform.localScale = Vector3.one * transac.interpolator(transac.timer, (float)transac.startValue, (float)transac.endValue - (float)transac.startValue, transac.tweenTime);

						if (transac.ignoreTimeScale)
							transac.timer += RealDeltaTime;
						else
							transac.timer += Time.deltaTime;
					}
					break;
			}
		}

		transacToBeRemoved.Sort((m, n) =>
		{
			return n - m;
		});

		for (int j = 0; j < transacToBeRemoved.Count; j++)
			tweenTransactions.RemoveAt(transacToBeRemoved[j]);
	}

	public override bool CanStartAfterRound()
	{
		if (!base.CanStartAfterRound())
			return false;

		return tweenTransactions.Count == 0;
	}
}
