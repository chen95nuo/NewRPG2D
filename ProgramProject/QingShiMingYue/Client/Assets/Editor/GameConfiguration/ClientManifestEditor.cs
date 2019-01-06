using UnityEngine;
using UnityEditor;
using System.Security;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using KodGames;
using ICSharpCode.SharpZipLib.BZip2;

namespace ClientServerCommon
{
	public class ClientManifestEditor : ClientManifest
	{
		public List<string> fileNamesInWorkspace = new List<string>();

		public static ClientManifestEditor BuildClientManifest(string outputPath, string clientManifestFile, List<string> ignorePaths, bool streamAllAssets, int baseAppRevision, bool compress, List<Object> additionalFiles, List<string> additionalFilePathInResource, List<string> additionalFilePathInWorkspace, List<string> keepNamePaths)
		{
			string resPath = "Assets/Resources";
			string streamingAssetPath = "Assets/StreamingAssets";

			ClientManifestEditor clientManifest = new ClientManifestEditor();

			Generate(clientManifest, ignorePaths, streamAllAssets, baseAppRevision, resPath, streamingAssetPath, additionalFiles, additionalFilePathInResource, additionalFilePathInWorkspace, keepNamePaths);
			BuildAssetBundle(clientManifest, outputPath, compress, additionalFiles, additionalFilePathInResource);
			BuildStreamingAsset(clientManifest, outputPath, compress, streamingAssetPath);
			SaveToFile(clientManifest, clientManifestFile);
			return clientManifest;
		}

		private static void Generate(ClientManifestEditor clientManifest, List<string> ignorePaths, bool streamAllAssets, int baseAppRevision, string resPath, string streamingAssetPath, List<Object> additionalFiles, List<string> additionalFilePathInResource, List<string> additionalFilePathInWorkspace, List<string> keepNamePaths)
		{
			// Clear old data
			clientManifest.fileInfos.Clear();

			// Generate additional files
			System.Console.Write(string.Format("------------------ Collecting additional files revision {0} {1} ------------------", baseAppRevision, additionalFiles.Count));
			System.Console.WriteLine();
			GenerateAdditionalFiles(clientManifest, baseAppRevision, additionalFiles, additionalFilePathInResource, additionalFilePathInWorkspace, keepNamePaths);
			System.Console.Write(string.Format("------------------ Finished ------------------"));
			System.Console.WriteLine();

			// Full build need not create asset AB
			if (streamAllAssets == false && baseAppRevision == 0)
				return;

			System.Console.Write(string.Format("------------------ Collecting changed file in resource folder from revision {0} ------------------", baseAppRevision));
			System.Console.WriteLine();
			DoGenerate(clientManifest, ignorePaths, streamAllAssets, baseAppRevision, resPath, resPath, false, keepNamePaths);
			System.Console.Write(string.Format("------------------ Finished ------------------"));
			System.Console.WriteLine();

			System.Console.Write(string.Format("------------------ Collecting changed file in streamingAsset folder from revision {0} ------------------", baseAppRevision));
			System.Console.WriteLine();
			DoGenerate(clientManifest, ignorePaths, streamAllAssets, baseAppRevision, streamingAssetPath, streamingAssetPath, true, keepNamePaths);
			System.Console.Write(string.Format("------------------ Finished ------------------"));
			System.Console.WriteLine();
		}

		private static void GenerateAdditionalFiles(ClientManifestEditor clientManifest, int baseAppRevision, List<Object> additionalFiles, List<string> additionalFilePathInResource, List<string> additionalFilePathInWorkspace, List<string> keepNamePaths)
		{
			if (additionalFiles == null || additionalFilePathInResource == null || additionalFilePathInWorkspace == null)
				return;

			for (int i = 0; i < additionalFiles.Count; ++i)
			{
				var fileInfo = DoGenerate(clientManifest, false, baseAppRevision, additionalFilePathInResource[i], additionalFilePathInWorkspace[i], false, keepNamePaths);
				if (fileInfo != null)
					clientManifest.fileInfos.Add(fileInfo);
			}
		}

		private static void DoGenerate(ClientManifestEditor clientManifest, List<string> ignorePaths, bool streamAllAssets, int baseAppRevision, string basePath, string path, bool isStreamAsset, List<string> keepNamePaths)
		{
			DirectoryInfo dir = new DirectoryInfo(path);

			// Check files
			foreach (var file in dir.GetFiles())
			{
				// Ignore System file.
				if (string.IsNullOrEmpty(file.Name) || file.Name.StartsWith("."))
					continue;

				// Ignore meta file
				if (file.Extension.Equals(".meta", System.StringComparison.OrdinalIgnoreCase))
					continue;

				// Ignore svn file
				if (file.Extension.Equals(".svn", System.StringComparison.OrdinalIgnoreCase))
					continue;

				string fileNameInWorkspace = file.FullName.Substring(Directory.GetCurrentDirectory().Length + 1);
				string fileNameInResource = PathUtility.GetSubPath(Path.Combine(path, isStreamAsset ? Path.GetFileName(fileNameInWorkspace) : Path.GetFileNameWithoutExtension(fileNameInWorkspace)), basePath);
				var fileInfo = DoGenerate(clientManifest, !streamAllAssets, baseAppRevision, fileNameInResource, fileNameInWorkspace, isStreamAsset, keepNamePaths);
				if (fileInfo != null)
					clientManifest.fileInfos.Add(fileInfo);
			}

			// Check sub directories
			foreach (var subDir in dir.GetDirectories())
			{
				// Ignore svn dir
				if (subDir.Name.Equals(".svn", System.StringComparison.OrdinalIgnoreCase))
					continue;

				string subDirName = Path.Combine(path, subDir.Name);

				// Ignore specific dir
				bool contains = false;
				foreach (var ignorePath in ignorePaths)
				{
					if (ignorePath.Equals(PathUtility.UnifyPath(subDirName)))
					{
						contains = true;
						break;
					}
				}

				if (contains)
					continue;

				DoGenerate(clientManifest, ignorePaths, streamAllAssets, baseAppRevision, basePath, subDirName, isStreamAsset, keepNamePaths);
			}
		}

		private static FileInfo DoGenerate(ClientManifestEditor clientManifest, bool doCheckRevision, int baseAppRevision, string filePathInResource, string filePathInWorkspace, bool isStreamAsset, List<string> keepNamePaths)
		{
			int fileRevision = 0;
			if (doCheckRevision && CheckIfFileIsNewerThanClientRevision(clientManifest, baseAppRevision, filePathInWorkspace, isStreamAsset, ref fileRevision) == false)
				return null;

			System.Console.Write(string.Format("{0} : {1} : {2}", baseAppRevision, filePathInResource, filePathInWorkspace));
			System.Console.WriteLine();

			FileInfo fileInfo = new FileInfo();
			fileInfo.assetName = PathUtility.UnifyPath(filePathInResource);
			fileInfo.fileName = Cryptography.Md5Hash(fileInfo.assetName);
			fileInfo.fileSize = 0;
			fileInfo.isStreamAsset = isStreamAsset;
			fileInfo.keepFileName = KeepAssetFileName(keepNamePaths, filePathInWorkspace);

			clientManifest.fileNamesInWorkspace.Add(filePathInWorkspace);

			return fileInfo;
		}

		private static bool CheckIfFileIsNewerThanClientRevision(ClientManifestEditor clientManifest, int baseAppRevision, string fileNameInWorkspace, bool isStreamAsset, ref int fileRevision)
		{
			try
			{
				// Check if target file is newer can last build

				// Check source file				
				string[] dependencies = AssetDatabase.GetDependencies(new string[] { fileNameInWorkspace });
				if (dependencies.Length == 0)
				{
					//					Debug.LogError("Invalid asset : " + fileNameInWorkspace);
					System.Console.Write("Invalid asset : " + fileNameInWorkspace);
					System.Console.WriteLine();
					return false;
				}

				fileRevision = GetHighestVersionNumberOfFiles(dependencies);
				if (fileRevision == 0)
					return false;

				// Check meta file, StreamingAsset是直接加载的, 不需要meta文件
				if (isStreamAsset == false)
				{
					string metaFile = fileNameInWorkspace + ".meta";
					int metaFileRevision = SVNHelper.GetFileRevision(metaFile);
					if (metaFileRevision == 0)
					{
						//					Debug.LogError("SVN warning : " + metaFile + "," + SVNHelper.GetLastErrorMsg());
						System.Console.Write("SVN warning : " + metaFile + "," + SVNHelper.GetLastErrorMsg());
						System.Console.WriteLine();
						return false;
					}

					//				System.Console.Write(string.Format("{0}|{1}|{2}|{3}|{4}", filePathInWorkspace, fileRevision, metaFileRevision, dependencies.Length, dependencies));
					//				System.Console.WriteLine();

					fileRevision = Mathf.Max(fileRevision, metaFileRevision);
				}

				return fileRevision > baseAppRevision;
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
			}

			return false;
		}

		public static int GetHighestVersionNumberOfFiles(string[] fileList)
		{
			int highestRevision = 0;
			foreach (string name in fileList)
			{
				int revision = SVNHelper.GetFileRevision(name);
				if (revision == 0)
				{
					//					Debug.LogError("SVN warning : " + name + "," + SVNHelper.GetLastErrorMsg());
					System.Console.Write("SVN warning : " + name + "," + SVNHelper.GetLastErrorMsg());
					System.Console.WriteLine();
					return 0;
				}

				highestRevision = Mathf.Max(highestRevision, revision);
			}

			return highestRevision;
		}

		private static void BuildAssetBundle(ClientManifestEditor config, string path, bool compress, List<Object> additionalFiles, List<string> additionalFilePathInResource)
		{
			PathUtility.CreateDirectory(path);

			var additionFile = new Dictionary<string, Object>();
			for (int i = 0; i < additionalFiles.Count; ++i)
				additionFile.Add(PathUtility.UnifyPath(additionalFilePathInResource[i]), additionalFiles[i]);

			foreach (var fileInfo in config.fileInfos)
			{
				if (fileInfo.isStreamAsset)
					continue;

				Object obj = null;
				additionFile.TryGetValue(fileInfo.assetName, out obj);
				BuildAssetBundle(fileInfo, path, compress, obj);
			}
		}

		private static void BuildAssetBundle(ClientManifestEditor.FileInfo fileInfo, string path, bool compress, Object obj)
		{
			if (obj == null)
				obj = ResourcesWrapper.Load(fileInfo.assetName);

			if (obj == null)
				throw new System.Exception("Missing asset : " + fileInfo.assetName);

			// Build asset bundle
			string targetFile = PathUtility.Combine(path, fileInfo.fileName);
			UnityEditor.BuildPipeline.BuildAssetBundle(obj, null, PathUtility.Combine(path, fileInfo.fileName),
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle,
				EditorUserBuildSettings.activeBuildTarget);

			// Replace file name with file hash name
			byte[] bytes = File.ReadAllBytes(targetFile);
			fileInfo.uncompressedFileSize = bytes.Length;

			fileInfo.fileName = string.Format("{0}.kod", fileInfo.keepFileName ? PathUtility.GetAssetNameKeepInBuddle(fileInfo.assetName) : KodGames.Cryptography.Md5Hash(bytes));
			string renamedFileName = PathUtility.Combine(path, fileInfo.fileName);
			using (FileStream renamedFile = new FileStream(renamedFileName, FileMode.Create))
				if (compress)
					BZip2.Compress(new MemoryStream(bytes), renamedFile, false, 9);
				else
					renamedFile.Write(bytes, 0, bytes.Length);
			PathUtility.RemoveFile(targetFile);

			// Get file size
			System.IO.FileInfo _fileInfo = new System.IO.FileInfo(renamedFileName);
			fileInfo.fileSize = (int)_fileInfo.Length;
		}

		private static void BuildStreamingAsset(ClientManifestEditor config, string outputPath, bool compress, string streamingAssetPath)
		{
			PathUtility.CreateDirectory(outputPath);

			foreach (var fileInfo in config.fileInfos)
			{
				if (fileInfo.isStreamAsset == false)
					continue;

				BuildStreamingAsset(fileInfo, outputPath, streamingAssetPath);
			}
		}

		private static void BuildStreamingAsset(ClientManifestEditor.FileInfo fileInfo, string outputPath, string streamingAssetPath)
		{
			string sourceFilePath = PathUtility.Combine(streamingAssetPath, fileInfo.assetName);

			// Replace file name with file hash name
			byte[] bytes = File.ReadAllBytes(sourceFilePath);
			fileInfo.uncompressedFileSize = bytes.Length;

			// 使用原始的扩展名用于下载之后的文件加载, 
			if (fileInfo.keepFileName)
				fileInfo.fileName = PathUtility.GetAssetNameKeepInBuddle(fileInfo.assetName);
			else
				fileInfo.fileName = string.Format("{0}{1}", KodGames.Cryptography.Md5Hash(bytes), Path.GetExtension(fileInfo.assetName));
			string targetFilePath = PathUtility.Combine(outputPath, fileInfo.fileName);
			using (FileStream targetFile = new FileStream(targetFilePath, FileMode.Create))
				targetFile.Write(bytes, 0, bytes.Length);

			// Get file size
			System.IO.FileInfo _fileInfo = new System.IO.FileInfo(targetFilePath);
			fileInfo.fileSize = (int)_fileInfo.Length;
		}

		private static bool KeepAssetFileName(List<string> keepNamePaths, string sourceFilePath)
		{
			var dirRoot = new System.IO.FileInfo(sourceFilePath).Directory.FullName.Substring(Directory.GetCurrentDirectory().Length + 1);
			dirRoot = PathUtility.UnifyPath(dirRoot);

			foreach (var path in keepNamePaths)
				if (path.Equals(dirRoot))
					return true;

			return false;
		}

		private static void SaveToFile(ClientManifestEditor config, string fileName)
		{
			// Create XML
			System.Xml.XmlDocument cfgDoc = new System.Xml.XmlDocument();
			cfgDoc.AppendChild(cfgDoc.CreateXmlDeclaration("1.0", "utf-8", null));

			XmlElement rootNode = cfgDoc.CreateElement("ClientManifest");
			cfgDoc.AppendChild(rootNode);

			// Add content
			foreach (var fileInfo in config.fileInfos)
			{
				XmlElement fileNode = cfgDoc.CreateElement("FileInfo");
				fileNode.SetAttribute("AssetName", fileInfo.assetName);
				fileNode.SetAttribute("FileName", fileInfo.fileName);
				fileNode.SetAttribute("FileSize", fileInfo.fileSize.ToString());
				fileNode.SetAttribute("UncompressedFileSize", fileInfo.uncompressedFileSize.ToString());
				fileNode.SetAttribute("IsStreamAsset", fileInfo.isStreamAsset.ToString());
				fileNode.SetAttribute("KeepFileName", fileInfo.keepFileName.ToString());

				rootNode.AppendChild(fileNode);
			}

			// Save the XML
			System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
			settings.Indent = true;
			settings.Encoding = new System.Text.UTF8Encoding(false); // Encode the temp XML with utf-8 without BOM flag
			settings.NewLineChars = System.Environment.NewLine;

			System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(fileName, settings);
			cfgDoc.Save(writer);
			writer.Close();

			// Refresh Asset Database
			UnityEditor.AssetDatabase.ImportAsset(fileName); // AssetDatabase.Refresh() can not update the file
		}

		//		[UnityEditor.MenuItem("Tools/Test ClientManifestEditor")]
		//		public static void Test()
		//		{
		//			ConfigDatabase.Initialize(new MathParserFactory(), false, true);
		//			ConfigSetting cfgSetting = new ConfigSetting(Configuration._FileFormat.Xml);
		//			GameDefines.SetupConfigSetting(cfgSetting);
		//			IFileLoader fileLoader = new FileLoaderFromResourceFolder();
		//			ConfigDatabase.DefaultCfg.LoadConfig<ClientConfig>(fileLoader, cfgSetting);
		//			ConfigDatabase.DefaultCfg.LoadGameConfig(fileLoader, cfgSetting);
		//
		//			ClientManifestEditor config = new ClientManifestEditor();
		//			BuildClientManifest("AssetBundle",
		//				"ClientManifest.xml",
		//				new List<string>(),
		//				true,
		//				0,
		//				true,
		//				new List<Object>(),
		//				new List<string>(),
		//				new List<string>());
		//		}
	}
}