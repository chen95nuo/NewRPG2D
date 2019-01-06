//#define ENABLE_NOTIFICATION_LOG
//#define DISABLE_LOCAL_NOTIFICATION
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ExternalCall;

public class SysNotification : SysModule
{
	public static SysNotification Instance
	{
		get { return SysModuleManager.Instance.GetSysModule<SysNotification>(); }
	}

#if UNITY_IPHONE
	private bool remoteNotificationRegistered = false;
	private bool waitingForRemoteNotificationRegisterResult = false;
#endif
	public override bool Initialize()
	{
#if UNITY_IPHONE
#if ENABLE_NOTIFICATION_LOG
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("[SysNotification]Initializing notification");
			sb.AppendFormat("Local notifications count : {0}", NotificationServices.localNotifications.Length);
			sb.AppendLine();
			foreach (var notif in NotificationServices.localNotifications)
			{
				sb.AppendFormat("\tNotification : {0}", Log(notif));
				sb.AppendLine();
			}
			
			sb.AppendLine();
			sb.AppendFormat("Scheduled local notifications count : {0}", NotificationServices.scheduledLocalNotifications.Length);
			sb.AppendLine();
			foreach (var notif in NotificationServices.scheduledLocalNotifications)
			{
				sb.AppendFormat("\tNotification : {0}", Log(notif));
				sb.AppendLine();
			}
			
			sb.AppendFormat("Remote notifications count : {0}", NotificationServices.remoteNotifications.Length);
			sb.AppendLine();
			foreach (var notif in NotificationServices.remoteNotifications)
			{
				sb.AppendFormat("\tNotification : {0}", Log(notif));
				sb.AppendLine();
			}
			
			Debug.Log(sb);
		}
#endif
		// Clear received local notification
//#if DISABLE_LOCAL_NOTIFICATION
		// notif.userInfo is not valid , clear all instead
		NotificationServices.CancelAllLocalNotifications();
//#endif
		NotificationServices.ClearLocalNotifications();

		// Clear received remote notification
		// Reset iphone app icon badge number, ClearLocalNotifications can not clear badge number.
		NotificationServices.ClearRemoteNotifications();
//		MessagePlugin.SetApplicationIconBadgeNumber(0);

		// Schedule login notification
		//		RescheduleAllLoginNotification();
#endif

		return true;
	}

	public void RegisterRemoteNotification()
	{
#if ENABLE_NOTIFICATION_LOG
		Debug.Log("[SysNotification] RegisterRemoteNotification");
#endif

#if UNITY_IPHONE
		// Register remote notification
		remoteNotificationRegistered = NotificationServices.deviceToken != null;
		if (remoteNotificationRegistered == false)
		{
			waitingForRemoteNotificationRegisterResult = true;
			NotificationServices.RegisterForRemoteNotificationTypes(
				RemoteNotificationType.Alert |
				RemoteNotificationType.Badge |
				RemoteNotificationType.Sound);
		}
		else
		{
			// Send notification token
			RequestMgr.Inst.Request(new SendAPNTokenRequest(NotificationServices.deviceToken));
		}
#endif
	}

	public override void Dispose()
	{
	}

	public override void Run(object userData)
	{

	}

	public override void OnUpdate()
	{
#if UNITY_IPHONE
		// Check APS registration
		if (waitingForRemoteNotificationRegisterResult)
		{
			// Error occurs when register APS
			if (NotificationServices.registrationError != null)
			{
#if ENABLE_NOTIFICATION_LOG
				Debug.LogWarning("[SysNotification] Register push notification failed : " + NotificationServices.registrationError);
#endif
				waitingForRemoteNotificationRegisterResult = false;
			}
			else if (NotificationServices.deviceToken != null)
			{
				// Register success
#if ENABLE_NOTIFICATION_LOG				
				Debug.Log("[SysNotification] Register push notification success");
#endif
				waitingForRemoteNotificationRegisterResult = false;
				remoteNotificationRegistered = true;

				// Send notification token
				RequestMgr.Inst.Request(new SendAPNTokenRequest(NotificationServices.deviceToken));
			}
		}
#endif
	}

	public void RescheduleAllLoginNotification()
	{
		CancelScheduleLocalNotification(LocalNotificationConfig._NotificationType.Login);

		foreach (var notifCfg in ConfigDatabase.DefaultCfg.LocalNotificationConfig.GetNotificationsByType(LocalNotificationConfig._NotificationType.Login))
		{
			if (notifCfg.isOpen == false)
				continue;

			ScheduleLocalNotification(
				System.DateTime.Now.AddMilliseconds(notifCfg.delayTime),
				notifCfg.messageBody,
				notifCfg.appIconBadageNumber,
				notifCfg.hasAction,
				notifCfg.actionTitle,
				notifCfg.type,
				0,
				notifCfg.delayTime);
		}
	}

	public void ScheduleLocalNotification(System.DateTime dateTime, string message, int appBadgeNumber, bool hasAction, string actionTitle, int type, int id, long delay)
	{
		ScheduleLocalNotification(dateTime, message, appBadgeNumber, hasAction, actionTitle, type, id, _TimeDurationType.Era);
	}

	public void ScheduleLocalNotification(System.DateTime dateTime, string message, int appBadgeNumber, bool hasAction, string actionTitle, int type, int id, int repeatInterval)
	{
#if UNITY_IPHONE
		// Skip notification between 23h~9h
		int timeHour = dateTime.Hour;
		if (timeHour >= 23 || timeHour <= 9)
		{
			Debug.Log("Skip notification : " + dateTime);
			return;
		}

		// Skip disabled notification

		var notif = new LocalNotification();
		notif.fireDate = dateTime;
		notif.alertBody = ConfigDatabase.DefaultCfg.StringsConfig.GetString("Notification", message);
		notif.applicationIconBadgeNumber = appBadgeNumber;
		notif.hasAction = hasAction;		
		notif.alertAction = ConfigDatabase.DefaultCfg.StringsConfig.GetString("Notification", actionTitle);
		notif.repeatInterval = GameUtility.TimeDurationType2CalendarUnit(repeatInterval);

		// Set userInfo by Local.
		var userInfo = new Dictionary<string, int>();
		userInfo.Add(userInfoKey_Type, type);
		userInfo.Add(userInfoKey_ID, id);
		notif.userInfo = userInfo;

		// There is some problem in editor, the userinfo is null after set value, it's ok on IOS device
		if (notif.userInfo != null)
		{
#if ENABLE_NOTIFICATION_LOG
			Debug.Log(string.Format("[SysNotification] Local Notification Scheduled : {0},{1},{2}", type, id, Log(notif)));
#endif
			
#if !DISABLE_LOCAL_NOTIFICATION
			NotificationServices.ScheduleLocalNotification(notif);
#endif
		}
		else
		{
#if !UNITY_EDITOR // Disable log for editor
			Debug.LogError("[SysNotification] userInfo is null");
#endif
		}
#endif
	}

	public void CancelScheduleLocalNotification(int type)
	{
#if UNITY_IPHONE
		foreach (var notif in NotificationServices.scheduledLocalNotifications)
		{
			if (notif.userInfo != null && notif.userInfo.Contains(userInfoKey_Type) && (int)notif.userInfo[userInfoKey_Type] == type)
			{
#if ENABLE_NOTIFICATION_LOG				
				Debug.Log("[SysNotification] Cancel scheduled local Notification : " + Log(notif));
#endif

				NotificationServices.CancelLocalNotification(notif);

				return;
			}
		}
#endif
	}

	public void CancelScheduleLocalNotification(int type, int id)
	{
#if UNITY_IPHONE
		foreach (var notif in NotificationServices.scheduledLocalNotifications)
		{
			if (notif.userInfo != null &&
				notif.userInfo.Contains(userInfoKey_Type) && (int)notif.userInfo[userInfoKey_Type] == type &&
					notif.userInfo.Contains(userInfoKey_ID) && (int)notif.userInfo[userInfoKey_ID] == id)
			{
#if ENABLE_NOTIFICATION_LOG				
				Debug.Log("[SysNotification] Cancel scheduled local Notification : " + Log(notif));
#endif

				NotificationServices.CancelLocalNotification(notif);

				return;
			}
		}
#endif
	}

#if ENABLE_NOTIFICATION_LOG
	private string Log(LocalNotification notif)
	{
		object type = notif.userInfo != null && notif.userInfo.Contains(userInfoKey_Type) ? notif.userInfo[userInfoKey_Type] : 0;
		object id = notif.userInfo != null && notif.userInfo.Contains(userInfoKey_ID) ? notif.userInfo[userInfoKey_ID] : 0;
		
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append("LocalNotification Data :");
		if (notif.userInfo != null)
		{
			sb.AppendFormat("Count {0}", notif.userInfo.Count);
			foreach (System.Collections.DictionaryEntry kvp in notif.userInfo)
				sb.AppendFormat("({0},{1})",  kvp.Key, kvp.Value);
		}
		else
		{
			sb.Append("userInfo is null");
		}
		
		return string.Format("fireDate({0},{1}),timeZone({2}),repeatInterval({3}),repeatCalendar({4}),alertBody({5}),alertAction({6}),hasAction({7}),alertLaunchImage({8}),applicationIconBadgeNumber({9}),soundName({10}),userInfo({11}),type({12}),id({13})",
			notif.fireDate,
			notif.fireDate.Kind,
			notif.timeZone,
			notif.repeatInterval,
			notif.repeatCalendar,
			notif.alertBody,
			notif.alertAction,
			notif.hasAction,
			notif.alertLaunchImage,
			notif.applicationIconBadgeNumber,
			notif.soundName,
			sb,
			type,
			id);
	}
	
	private string Log(RemoteNotification notif)
	{
		return string.Format("{0},{1},{2},{3}",
			notif.alertBody,
			notif.hasAction,
			notif.applicationIconBadgeNumber,
			notif.soundName);
	}
#endif

	private const string userInfoKey_Type = "Type";
	private const string userInfoKey_ID = "ID";
}