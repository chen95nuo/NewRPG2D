// #region 使用道具
//using UnityEngine;
//
//public class UseItem : MonoBehaviour {
//	/// <summary>
//	/// 使用道具
//	/// </summary>
//	/// <param name="?"></param>
//	public void OperationUseItem(OperationRequest operationRequest)
//	{
//	    string itemID = (string)operationRequest.Parameters[(byte)ParameterType.ItemID];
//	    UseItem(itemID, operationRequest.OperationCode);
//	}
//
//	/// <summary>
//	/// 使用道具
//	/// </summary>
//	/// <param name="mItemID">道具ID</param>
//	public void UseItem(string mItemID, byte operationCode)
//	{
//	    try
//	    {
//	        YuanTable getItem = null;
//	        if (YuanServerApplication.PubGameItem.TryGetValue(mItemID, out getItem))
//	        {
//	            //使用包裹中的物品用 AnalysePlayerInventoryMin方法，道具用AnalyseForItemMin
//	           
//				if (mItemID.Substring(0, 3) == "881")//战神卡
//	            {
//	                string itemStr = AnalyseForItemMin(mItemID.Split(',')[0]);//使用道具方法，返回为“0”表示没有该道具
//	                if (itemStr != "0")
//	                {
//						myTable[0]["GOWCard"].YuanColumnText = (int.Parse(myTable[0]["GOWCard"].YuanColumnText) + 900).ToString();
//						string GOWCardValue = getItem[0]["ItemValue"].YuanColumnText;
//						myTable[0]["GOWCardValue"].YuanColumnText = GOWCardValue;
//						myTable[0]["Item"].YuanColumnText = itemStr;
//						Dictionary<string, string> dicTemp = new Dictionary<string, string>();
//						dicTemp["Item"] = myTable[0]["Item"].YuanColumnText;
//
//	                    //构造数据库已改信息，准备发送回客户端相关代码
//	                    string[] strKey = null;
//	                    string[] strValue = null;
//	                    Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//	                    SetTable(dicTemp, out strKey, out strValue, dicSend);
//	                    dicSend[(byte)ParameterType.ItemID] = mItemID;
//	                    //使用道具成功，返回消息给客户端
//	                    OperationResponse response = new OperationResponse(operationCode, dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//	                    SendOperationResponse(response, new SendParameters());
//	                }
//	                else
//	                {
//	                    //使用道具失败，包裹中没有该道具
//	                    OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//	                    SendOperationResponse(response, new SendParameters());
//	                }
//	            }
//				else
//				if (mItemID.Substring(0, 3) == "882")//经验石
//				{
//					string itemStr = AnalyseForItemMin(mItemID.Split(',')[0]);//使用道具方法，返回为“0”表示没有该道具
//					if (itemStr != "0")
//					{
//						int exp = int.Parse(getItem[0]["ItemValue"].YuanColumnText);
//						AddExp(exp, 1, (byte)OperationCode.AddExperience);
//						myTable[0]["Item"].YuanColumnText = itemStr;
//						Dictionary<string, string> dicTemp = new Dictionary<string, string>();
//						dicTemp["Item"] = myTable[0]["Item"].YuanColumnText;
//						
//						//构造数据库已改信息，准备发送回客户端相关代码
//						string[] strKey = null;
//						string[] strValue = null;
//						Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//						SetTable(dicTemp, out strKey, out strValue, dicSend);
//						dicSend[(byte)ParameterType.ItemID] = mItemID;
//						//使用道具成功，返回消息给客户端
//						OperationResponse response = new OperationResponse(operationCode, dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//					else
//					{
//						//使用道具失败，包裹中没有该道具
//						OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//				}
//				else
//					if (mItemID.Substring(0, 3) == "883")//双倍经验卡
//				{
//					string itemStr = AnalyseForItemMin(mItemID.Split(',')[0]);//使用道具方法，返回为“0”表示没有该道具
//					if (itemStr != "0")
//					{
//						int Attr = int.Parse(getItem[0]["ItemValue"].YuanColumnText);
//						myTable[0]["DoubleCard"].YuanColumnText = (int.Parse(myTable[0]["DoubleCard"].YuanColumnText) + Attr*60).ToString();
//						
//						myTable[0]["Item"].YuanColumnText = itemStr;
//						Dictionary<string, string> dicTemp = new Dictionary<string, string>();
//						dicTemp["Item"] = myTable[0]["Item"].YuanColumnText;
//						
//						//构造数据库已改信息，准备发送回客户端相关代码
//						string[] strKey = null;
//						string[] strValue = null;
//						Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//						SetTable(dicTemp, out strKey, out strValue, dicSend);
//						dicSend[(byte)ParameterType.ItemID] = mItemID;
//						//使用道具成功，返回消息给客户端
//						OperationResponse response = new OperationResponse(operationCode, dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//					else
//					{
//						//使用道具失败，包裹中没有该道具
//						OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//				}
//				else
//					if (mItemID.Substring(0, 3) == "884")//体力药
//				{
//					string itemStr = AnalyseForItemMin(mItemID.Split(',')[0]);//使用道具方法，返回为“0”表示没有该道具
//					if (itemStr != "0")
//					{
//						int Power = int.Parse(getItem[0]["ItemValue"].YuanColumnText);
//						myTable[0]["Power"].YuanColumnText = (int.Parse(myTable[0]["Power"].YuanColumnText) + Power).ToString(); 
//						
//						myTable[0]["Item"].YuanColumnText = itemStr;
//						Dictionary<string, string> dicTemp = new Dictionary<string, string>();
//						dicTemp["Item"] = myTable[0]["Item"].YuanColumnText;
//						
//						//构造数据库已改信息，准备发送回客户端相关代码
//						string[] strKey = null;
//						string[] strValue = null;
//						Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//						SetTable(dicTemp, out strKey, out strValue, dicSend);
//						dicSend[(byte)ParameterType.ItemID] = mItemID;
//						//使用道具成功，返回消息给客户端
//						OperationResponse response = new OperationResponse(operationCode, dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//					else
//					{
//						//使用道具失败，包裹中没有该道具
//						OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//				}
//				else
//				if (mItemID.Substring(0, 3) == "885")//能力强化卡
//				{
//					string itemStr = AnalyseForItemMin(mItemID.Split(',')[0]);//使用道具方法，返回为“0”表示没有该道具
//					if (itemStr != "0")
//					{
//						int NonPoint = int.Parse(getItem[0]["ItemValue"].YuanColumnText);
//						myTable[0]["NonPoint"].YuanColumnText = (int.Parse(myTable[0]["NonPoint"].YuanColumnText) + NonPoint).ToString(); 
//						
//						myTable[0]["Item"].YuanColumnText = itemStr;
//						Dictionary<string, string> dicTemp = new Dictionary<string, string>();
//						dicTemp["Item"] = myTable[0]["Item"].YuanColumnText;
//						
//						//构造数据库已改信息，准备发送回客户端相关代码
//						string[] strKey = null;
//						string[] strValue = null;
//						Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//						SetTable(dicTemp, out strKey, out strValue, dicSend);
//						dicSend[(byte)ParameterType.ItemID] = mItemID;
//						//使用道具成功，返回消息给客户端
//						OperationResponse response = new OperationResponse(operationCode, dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//					else
//					{
//						//使用道具失败，包裹中没有该道具
//						OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//				}
//				else
//				if (mItemID.Substring(0, 3) == "886")//重生十字
//				{
//					string itemStr = AnalyseForItemMin(mItemID.Split(',')[0]);//使用道具方法，返回为“0”表示没有该道具
//					if (itemStr != "0")
//					{
////						提示使用重生十字成功
//						myTable[0]["Item"].YuanColumnText = itemStr;
//						Dictionary<string, string> dicTemp = new Dictionary<string, string>();
//						dicTemp["Item"] = myTable[0]["Item"].YuanColumnText;
//						
//						//构造数据库已改信息，准备发送回客户端相关代码
//						string[] strKey = null;
//						string[] strValue = null;
//						Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//						SetTable(dicTemp, out strKey, out strValue, dicSend);
//						dicSend[(byte)ParameterType.ItemID] = mItemID;
//						//使用道具成功，返回消息给客户端
//						OperationResponse response = new OperationResponse(operationCode, dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//					else
//					{
//						//使用道具失败，包裹中没有该道具
//						OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//				}
//				else
//				if (mItemID.Substring(0, 3) == "887")//等级奖励礼包
//				{
//					string itemStr = AnalyseForItemMin(mItemID.Split(',')[0]);//使用道具方法，返回为“0”表示没有该道具
//					if (itemStr != "0")
//					{
////						提示使用等级奖励礼包成功
//						myTable[0]["Item"].YuanColumnText = itemStr;
//						Dictionary<string, string> dicTemp = new Dictionary<string, string>();
//						dicTemp["Item"] = myTable[0]["Item"].YuanColumnText;
//						
//						//构造数据库已改信息，准备发送回客户端相关代码
//						string[] strKey = null;
//						string[] strValue = null;
//						Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//						SetTable(dicTemp, out strKey, out strValue, dicSend);
//						dicSend[(byte)ParameterType.ItemID] = mItemID;
//						//使用道具成功，返回消息给客户端
//						OperationResponse response = new OperationResponse(operationCode, dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//					else
//					{
//						//使用道具失败，包裹中没有该道具
//						OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//						SendOperationResponse(response, new SendParameters());
//					}
//				}
//			}
//	        else
//	        {
//	            //服务器中通过数据库查不到此道具信息
//	            OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//	            SendOperationResponse(response, new SendParameters());
//	        }
//	    }
//	    catch (Exception ex)
//	    {
//	        //执行出错
//	        OperationResponse response = new OperationResponse(operationCode) { ReturnCode = (short)ReturnCode.Error, DebugMessage = ex.ToString() };
//	        SendOperationResponse(response, new SendParameters());
//	    }
//	}
//
//	/// <summary>
//	/// 检测道具(减)id不带逗号和个数
//	/// </summary>
//	string AnalyseForItemMin(string itemID)
//	{
//	    string[] str = myTable[0]["Item"].YuanColumnText.Split(';');
//	    for (int i=0;i<str.Length;i++)
//	    {
//	        if (str[i].IndexOf(itemID) != -1)
//	        {
//	            int num = int.Parse(str[i].Split(',')[1]) - 1;
//	            if (num > 0)
//	            {
//	                str[i] = string.Format("{0},{1}", str[i].Split(',')[0], num.ToString());
//	            }
//	            else
//	            {
//	                str[i] = "";
//	            }
//	            StringBuilder itemStr = new StringBuilder();
//	            foreach (string item in str)
//	            {
//	                itemStr.AppendFormat("{0};", item);
//	            }
//	            return itemStr.ToString();
//	        }
//	    }
//	    return "0";
//	}
//}
//
//        #endregion