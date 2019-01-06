//#define ENALBE_RESOURDE_DOWNLOADER_LOG
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using ClientServerCommon;
using KodGames.ExternalCall;

/// <summary>
/// 资源下载管理器
/// 使用WWW类下载远程的资源,以非压缩方式保存在本地,要求资源是以BZip2格式压缩的
/// </summary>
public class ResourceDownloader : SysModule
{

	class Task
	{
		public Task(){}
		public enum _State
		{
			Initialized,
			Loading,
			Finished,
			Failed,
		}

		//protected int downloadedSize;
		public virtual int DownloadedSize
		{
			get
			{
				return 0;
			}
		}
		protected ClientManifest.FileInfo fileInfo;
		public ClientManifest.FileInfo FileInfo { get { return fileInfo; } }

		protected _State state;
		public _State State { 
			get { return state; } 
			set
			{
				state = value;
			}
		}

		public bool Started
		{
			get { return state == _State.Loading; }
		}

		public bool Finished
		{
			get { return state == _State.Finished; }
		}

		public bool Failed
		{
			get { return state == _State.Failed; }
		}

		public virtual bool Start(){ return false; }
		public virtual void Update(){}

	}

	class DownloadingTask : Task
	{
		private string wwwUrl;
		private WWW www;

		public override int DownloadedSize
		{
			get
			{
				switch (state)
				{
					case _State.Loading:
						return www != null ? (int)(www.progress * fileInfo.fileSize) : 0;

					case _State.Finished:
						return fileInfo.fileSize;

					default:
						return 0;
				}
			}
		}

		public DownloadingTask(ClientManifest.FileInfo fileInfo, _State initState)
		{
			this.fileInfo = fileInfo;
			this.state = initState;
		}

		public override string ToString()
		{
			return string.Format("{0}|{1}|{2}|{3}", fileInfo.assetName, fileInfo.fileName, fileInfo.fileSize, wwwUrl);
		}

		public override bool Start()
		{
			if (state == _State.Initialized)
			{
				// Create WWW
				wwwUrl = ResourceDownloader.Instance.GetRemoteFileUrl(fileInfo.fileName);
				www = new WWW(wwwUrl);

#if ENALBE_RESOURDE_DOWNLOADER_LOG
			Debug.Log(string.Format("[ResourceDownloader] Start downloading file : {0}|{1}", this, wwwUrl));
#endif

				state = _State.Loading;

				return true;
			}

			return false;
		}

		public override void Update()
		{
			if (state == _State.Loading)
			{
				// Check error
				if (www.error != null && www.error != "")
				{
					Debug.LogError(string.Format("[ResourceDownloader] Task error : {0}|{1}", this, www.error));
					state = _State.Failed;
				}
				// Check downloading
				else if (www.isDone)
				{
					try
					{
						// Save to HD
						SaveToLocal();
					}
					catch (System.Exception e)
					{
						Debug.LogError(e);
					}

					// Release www
					www = null;

#if ENALBE_RESOURDE_DOWNLOADER_LOG
				Debug.Log(string.Format("[ResourceDownloader] Task finished : {0}|{1}", this, wwwUrl));
#endif
					state = _State.Finished;
				}
			}
		}

		private void SaveToLocal()
		{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
			Debug.Log(string.Format("[ResourceDownloader] Saving AB to Local : {0} size : {1}", this, www.bytes.Length));
#endif

			// Create directory
			string rootPath = ResourceManager.Instance.GetLocalFileDirectory();
			Debug.LogError("rootPath : "+rootPath);
			if (!Directory.Exists(rootPath))
				Directory.CreateDirectory(rootPath);

			// Save file
			string filePath = ResourceManager.Instance.GetLocalFilePath(fileInfo.fileName);
			Debug.LogError("filePath : "+filePath +"      "+www.url+"     "+www.text);
			using (FileStream file = new FileStream(filePath, FileMode.Create))
			{
				if (fileInfo.isStreamAsset == false)
					ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(new MemoryStream(www.bytes, false), file, false);
				else
					file.Write(www.bytes, 0, www.bytes.Length);
			}
		}
	}

	class AndroidDownloadingTask : Task
	{
		public AndroidDownloadingTask(ClientManifest.FileInfo fileInfo, _State initState)
		{
			this.fileInfo = fileInfo;
			this.state = initState;
		}

		public override int DownloadedSize
		{
			get
			{
				switch (state)
				{
					case _State.Loading:
						return 0;

					case _State.Finished:
						return fileInfo.fileSize;

					default:
						return 0;
				}
			}
		}

		public override string ToString()
		{
			return string.Format("{0}|{1}|{2}", fileInfo.assetName, fileInfo.fileName, fileInfo.fileSize);
		}

		public override bool Start()
		{
			if (state == _State.Initialized)
			{

				state = _State.Loading;

				return true;
			}

			return false;
		}

	}


	public static ResourceDownloader Instance { get { return SysModuleManager.Instance.GetSysModule<ResourceDownloader>(); } }
	private string baseRemotePath;

	private string localKodNameFileName = "kodNames.kod";
	private int maxIntercurrentTaskCount = 5;
	public int MaxIntercurrentTaskCount
	{
		get { return maxIntercurrentTaskCount; }
		set { maxIntercurrentTaskCount = value; }
	}
	//下载GameAssets的方式（从Android下载还是从Unity下载）
	private bool downloadFromPlatfrom;
	public bool DownloadFromPlatfrom
	{
		get { return downloadFromPlatfrom; }
		set { downloadFromPlatfrom = value; }
	}

	private int totleFileSize = 0;
	private LinkedList<Task> pendingTasks = new LinkedList<Task>();
	private LinkedList<Task> downloadingTasks = new LinkedList<Task>();
	private LinkedList<Task> finishedTasks = new LinkedList<Task>();
	private LinkedList<Task> failedTasks = new LinkedList<Task>();
	public string GetRemoteFileUrl(string file)
	{
		return Path.Combine(baseRemotePath, file);
	}

	private bool CheckLocalAsset(string fileName, int fileSize)
	{
		string filePath = ResourceManager.Instance.GetLocalFilePath(fileName);
		FileInfo _fileInfo = new FileInfo(filePath);

#if ENALBE_RESOURDE_DOWNLOADER_LOG
		Debug.Log(string.Format("CheckLocalAsset : {0}({1}) exist:{2}({3})", filePath, fileSize, _fileInfo.Exists, _fileInfo.Exists ? _fileInfo.Length : 0));
#endif
		return _fileInfo.Exists && _fileInfo.Length == fileSize;
	}

	private bool CheckLocalAsset(ClientManifest.FileInfo fileInfo)
	{
		return CheckLocalAsset(fileInfo.fileName, fileInfo.uncompressedFileSize);
	}

	/*
	 * Upgrade Game Config
	 */
	public bool CheckLocalGameConfig(ClientManifest.FileInfo fileInfo)
	{
		return CheckLocalAsset(fileInfo);
	}

	public void StartUpgradingGameConfig(string baseRemotePath, ClientManifest.FileInfo fileInfo, bool addFinishedTask)
	{
		this.baseRemotePath = baseRemotePath;
		SetDownloadFromPlatform(false);
		ClearTask();
		AddTask(fileInfo, addFinishedTask);
	}

	/*
	 * Upgrade asset
	 */
	public bool CheckLocalAssets()
	{
		foreach (var fileInfo in ConfigDatabase.DefaultCfg.ClientManifest.fileInfos)
			if (CheckLocalAsset(fileInfo) == false)
				return false;

		return true;
	}

	private void DeleteLocalFile(string fileName)
	{
		string filePath = ResourceManager.Instance.GetLocalFilePath(fileName);
		FileInfo _fileInfo = new FileInfo(filePath);

		if (_fileInfo.Exists)
			_fileInfo.Delete();

		_fileInfo.Refresh();
	}

	public void DeleteGameConfig(ClientManifest.FileInfo fileInfo)
	{
		DeleteLocalFile(fileInfo.fileName);
	}

	public void DeleteLocalAssets()
	{
		foreach (var fileInfo in ConfigDatabase.DefaultCfg.ClientManifest.fileInfos)
			DeleteLocalFile(fileInfo.fileName);
	}

	public void StartUpgradingAssets(string baseRemotePath, bool addFinishedTask)
	{
		this.baseRemotePath = baseRemotePath;

		ClearTask();
		SetDownloadFromPlatform(true);
		foreach (var fileInfo in ConfigDatabase.DefaultCfg.ClientManifest.fileInfos)
			AddTask(fileInfo, addFinishedTask);
	}

	private void ClearTask()
	{
		totleFileSize = 0;
		pendingTasks.Clear();
		downloadingTasks.Clear();
		finishedTasks.Clear();
		failedTasks.Clear();
	}

	//设置从平台下载（目前是从Android下载）
	public void SetDownloadFromPlatform(bool downloadMode)
	{
		if (!downloadMode)
		{
			DownloadFromPlatfrom = downloadMode;
			maxIntercurrentTaskCount = 5;
		}
		else if (KodConfigPlugin.GetBoolValue("DownloadBackground"))
		{
			DownloadFromPlatfrom = downloadMode;
			maxIntercurrentTaskCount = 10000;
		}
	}

	private void AddTask(ClientManifest.FileInfo fileInfo, bool addFinishedTask)
	{
		if (CheckLocalAsset(fileInfo) == false)
		{
            Debug.LogError("0000000000000000000000");
			// Add to pending list 
#if UNITY_ANDROID
			if(DownloadFromPlatfrom)
				pendingTasks.AddLast(new AndroidDownloadingTask(fileInfo, AndroidDownloadingTask._State.Initialized));
			else
				pendingTasks.AddLast(new DownloadingTask(fileInfo, DownloadingTask._State.Initialized));

#else
			pendingTasks.AddLast(new DownloadingTask(fileInfo, DownloadingTask._State.Initialized));
#endif

			totleFileSize += fileInfo.fileSize;
		}
		else if (addFinishedTask)
		{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
			Debug.Log(string.Format("[ResourceDownloader] Add finished task : {0}|{1}", fileInfo.assetName, fileInfo.fileName));
#endif
			// Add to finished list
			finishedTasks.AddLast(new DownloadingTask(fileInfo, DownloadingTask._State.Finished));
			totleFileSize += fileInfo.fileSize;
		}
	}

	/*
	 * Upgrading
	*/
	public bool IsLoading()
	{
		return downloadingTasks.Count != 0 || pendingTasks.Count != 0;
	}

	public int GetTotalSize()
	{
		return totleFileSize;
	}

	public int GetDownloadedSize()
	{
		int size = 0;
		foreach (var task in finishedTasks)
			size += task.DownloadedSize;

		foreach (var task in downloadingTasks)
			size += task.DownloadedSize;

		return size;
	}

	public int GetTotalFileCount()
	{
		return GetDownloadedFileCount() + downloadingTasks.Count;
	}

	public int GetDownloadedFileCount()
	{
		return finishedTasks.Count;
	}

	// 下载失败的任务数量
	public int GetFailedFileCount()
	{
		return failedTasks.Count;
	}

	// 损坏文件的任务数量
	public int GetDamagedFileCount()
	{
		int damageFileCount = 0;

		foreach (var finishTask in finishedTasks)
		{
			string filePath = ResourceManager.Instance.GetLocalFilePath(finishTask.FileInfo.fileName);
			FileInfo _fileInfo = new FileInfo(filePath);

			if (!_fileInfo.Exists || _fileInfo.Length != finishTask.FileInfo.uncompressedFileSize)
				damageFileCount++;
		}

		return damageFileCount;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if(DownloadFromPlatfrom)
		{
			UpdateBackgrandTasks();
		}

		// Update loading
		int intercurrentTaskCount = 0;
		LinkedListNode<Task> node = downloadingTasks.First;
		while (node != null)
		{
			intercurrentTaskCount++;

			LinkedListNode<Task> next = node.Next;

			Task task = node.Value;
			if (task != null)
			{
				task.Update();

				if (task.Finished)
				{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
					Debug.Log(string.Format("[ResourceDownloader] Move task to finished list : {0}", task));
#endif
					// Finished, move to finished list
					node.List.Remove(node);
					finishedTasks.AddLast(node);
				}
				else if (task.Failed)
				{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
					Debug.Log(string.Format("[ResourceDownloader] Move task to failed list : {0}", task));
#endif
					// Failed, move to failed list
					node.List.Remove(node);
					failedTasks.AddLast(node);
				}
			}

			node = next;
		}

		// Start loading
		LinkedList<Task> startingTask = new LinkedList<Task>();
		LinkedListNode<Task> pendingNode = pendingTasks.First;
		while (intercurrentTaskCount < maxIntercurrentTaskCount && pendingNode != null)
		{
			intercurrentTaskCount++;

			LinkedListNode<Task> next = pendingNode.Next;

			Task task = pendingNode.Value;
			if (task != null && task.Start())
			{
				// Started, move to downloading list
				pendingNode.List.Remove(pendingNode);
				downloadingTasks.AddLast(pendingNode);

				if (startingTask == null)
					startingTask = new LinkedList<Task>();
				startingTask.AddLast(pendingNode.Value);
			}

			pendingNode = next;
		}

		if(DownloadFromPlatfrom && (startingTask.Count > 0))
			StartBackgrandTasks(startingTask);
		startingTask.Clear();
	}

	//开启从平台（Android）下载GameAssets
	private void StartBackgrandTasks(LinkedList<Task> startingTask)
	{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
		Debug.Log(string.Format("[ResourceDownloader bxt]  pendingTasks.count : {0}", pendingTasks.Count));
#endif
		if (startingTask != null)
		{
			string[] fileNames = new string[startingTask.Count];
			bool[] isStream = new bool[startingTask.Count];

			int index = 0;
			foreach (var task in startingTask)
			{
				isStream[index] = task.FileInfo.isStreamAsset;
				fileNames[index++] = task.FileInfo.fileName;
			}
#if ENALBE_RESOURDE_DOWNLOADER_LOG
		Debug.Log(string.Format("[ResourceDownloader bxt]  DownLoadUtil.DownLoadGameAsset"));
#endif
			//Save kodNames to Local
			// Create directory
			string rootPath = ResourceManager.Instance.GetLocalFileDirectory();
			if (!Directory.Exists(rootPath))
				Directory.CreateDirectory(rootPath);

			using (TextWriter sw = File.CreateText(ResourceManager.Instance.GetLocalFilePath(localKodNameFileName)))
			{
				for (int i = 0; i < fileNames.Length; i++)
				{
					sw.Write (fileNames[i]);
					if(isStream[i])
						sw.WriteLine ("&1");
					else
						sw.WriteLine ("&0");
				}
				sw.Close();
			}

			//start platform download
			DownLoadUtil.DownLoadGameAsset(baseRemotePath, ResourceManager.Instance.GetLocalFileDirectory(), localKodNameFileName);

			fileNames = null;
			isStream = null;
#if ENALBE_RESOURDE_DOWNLOADER_LOG
		Debug.Log(string.Format("[ResourceDownloader bxt]  DownLoadUtil.DownLoadGameAsset end"));
#endif
		}
	}

	//获取平台（Android）后台下载完毕的和下载失败的文件
	private void UpdateBackgrandTasks()
	{
		// Process finished tasks
		string[] finishKodName = DownLoadUtil.GetFinishKodName();
		if(finishKodName != null)
		{
			foreach(var name in finishKodName)
			{
				if(name != null)
				{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
					Debug.Log(string.Format("[ResourceDownloader] GetFinishKodName : {0}", name));
#endif
					foreach (var task in downloadingTasks)
					{
						if(name.Equals(task.FileInfo.fileName))
						{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
							Debug.Log(string.Format("[ResourceDownloader] Move to FinishTask GetFinishKodName : {0}", name));
#endif
							task.State = DownloadingTask._State.Finished;
							break;
						}
					}
					
				}
			}
		}	
		//Process failed tasks
		string[] failedKodName = DownLoadUtil.GetFailedKodName();
		if(failedKodName == null)
			return;
		foreach(var name in failedKodName)
		{
			if(name != null)
			{
#if ENALBE_RESOURDE_DOWNLOADER_LOG
				Debug.Log(string.Format("[ResourceDownloader] GetFailedKodName : {0}", name));
#endif
				foreach (var task in downloadingTasks)
				{
					if(name.Equals(task.FileInfo.fileName))
					{
						task.State = DownloadingTask._State.Failed;
						break;
					}
				}
			}
		}
	}	
}
