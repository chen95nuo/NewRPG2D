using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall
{
	public class IAPPlugin
	{
#if UNITY_EDITOR || UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void UnityCall_IAP_Initialize();

		[DllImport("__Internal")]
		private static extern void UnityCall_IAP_Finialize();

		[DllImport("__Internal")]
		private static extern void UnityCall_IAP_RequestProduct(string productID);
#endif

		[StructLayout(LayoutKind.Sequential)]
		public class ProductInfo
		{
			public const int BUFFER_SIZE = 64;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
			public string id;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
			public string localizeTitle;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
			public string localizeDescription;
			public int price;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
			public string localizePrice;
		}

#if UNITY_EDITOR || UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void UnityCall_IAP_GetProductInfo(string productID, [Out, MarshalAs(UnmanagedType.LPStruct)] ProductInfo productInfo);

		[DllImport("__Internal")]
		private static extern bool UnityCall_IAP_CanMakePayment();

		[DllImport("__Internal")]
		private static extern bool UnityCall_IAP_PayProduct(string productID, int count);

		[DllImport("__Internal")]
		private static extern IntPtr UnityCall_IAP_FetchPaymentTransactionReciept(string transactionID, ref int size);

		[DllImport("__Internal")]
		private static extern bool UnityCall_IAP_FinishTransition(string productID);
#endif

		public static void InitializeIAP()
		{
#if UNITY_EDITOR || UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				UnityCall_IAP_Initialize();
			}
#endif
		}

		public static void RequestProduct(string productID)
		{
#if UNITY_EDITOR || UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				UnityCall_IAP_RequestProduct(productID);
			}
#endif
		}

		public static bool CanMakePayment()
		{
#if UNITY_EDITOR || UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return UnityCall_IAP_CanMakePayment();
			}
#endif

			return false;
		}

		public static bool PayProduct(string productID, int count)
		{
#if UNITY_EDITOR || UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return UnityCall_IAP_PayProduct(productID, count);
			}
#endif

			return true;
		}

		public static byte[] FetchPaymentTransactionReciept(string transactionID)
		{
#if UNITY_EDITOR || UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				int size = -1;
				IntPtr byteBuffer = UnityCall_IAP_FetchPaymentTransactionReciept(transactionID, ref size);

				// Get buffer data
				byte[] receiptBuffer = new byte[size];
				for (int i = 0; i < size; ++i)
					receiptBuffer[i] = Marshal.ReadByte(byteBuffer, i);

				return receiptBuffer;
			}
#endif

			return new byte[0];
		}

		public static bool FinishTransition(string transactionID)
		{
#if UNITY_EDITOR || UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return UnityCall_IAP_FinishTransition(transactionID);
			}
#endif

			return true;
		}
	}

	public static class IAPHelper
	{
		public static bool ParsePaymentResponse(string message, out bool success, out string productID, out int quantity, out string transactionID)
		{
			ExternalCallParameterParser paramParser = new ExternalCallParameterParser(message);
			bool parseResult = true;
			parseResult &= paramParser.Parse(out success);
			parseResult &= paramParser.Parse(out productID);
			parseResult &= paramParser.Parse(out quantity);
			if (success)
				parseResult &= paramParser.Parse(out transactionID);
			else
				transactionID = "";

			return parseResult;
		}

		public static bool ParseRequestProductResponse(string message, out bool success, out int productID)
		{
			ExternalCallParameterParser paramParser = new ExternalCallParameterParser(message);
			bool parseResult = true;
			parseResult = paramParser.Parse(out success);
			parseResult = paramParser.Parse(out productID);
			return parseResult;
		}
	}
}
