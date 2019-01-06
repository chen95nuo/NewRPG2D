using UnityEngine;

namespace KodGames
{
	public class iAdViewer : MonoBehaviour 
	{
#if UNITY_IPHONE
		private static iAdViewer instance;
		public static iAdViewer Instance { get { return instance; } }

		public static ADBannerView.BannerWasClickedDelegate onBannerWasClicked;
		public static ADBannerView.BannerWasClickedDelegate onBannerWasLoaded;

		private ADBannerView banner = null;

		private bool visable = false;
		public bool Visable
		{
			get { return visable; }
			set 
			{
				if (banner == null)
					return;

				// 如果banner没有加载成功, 先记录状态
				visable = value;
				// 禁用的时候不设置状态, 在开启的时候会设置
				if (this.enabled && banner.loaded)
					banner.visible = visable;
			}
		}

		public static iAdViewer CreateOnObject(GameObject go, ADBannerView.Type type, ADBannerView.Layout layout)
		{
			if (instance != null)
				return instance;

			instance = go.AddComponent<iAdViewer>();
			instance.banner = new ADBannerView(type, layout);

			return instance;
		}

		void Awake()
		{
			// 启动时增加回调
			ADBannerView.onBannerWasClicked += OnBannerClicked;
			ADBannerView.onBannerWasLoaded  += OnBannerLoaded;
		}

		void OnDestroy()
		{
			// 销毁的时候关闭广告, 同时注销回调
			Visable = false;
			banner = null;
			
			ADBannerView.onBannerWasClicked -= OnBannerClicked;
			ADBannerView.onBannerWasLoaded  -= OnBannerLoaded;
		}

		void OnEnable()
		{
			// 开启的时候设置为保持的状态
			if (banner != null && banner.loaded)
				banner.visible = visable;
		}

		void OnDisable()
		{
			// 禁用时隐藏广告,但是不改变存储状态
			if (banner != null && banner.loaded)
				banner.visible = false;
		}

		void OnBannerClicked()
		{
			if (onBannerWasClicked != null)
				onBannerWasClicked();
		}
		
		void OnBannerLoaded()
		{
			// 设置显示状态
			Visable = visable;

			if (onBannerWasLoaded != null)
				onBannerWasLoaded();
		}
#endif
	}
}
