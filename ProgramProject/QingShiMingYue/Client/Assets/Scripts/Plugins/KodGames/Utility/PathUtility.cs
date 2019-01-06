using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KodGames
{
	public static class PathUtility
	{
		// Directory separator char
		public static char DirChar
		{
			get { return Path.AltDirectorySeparatorChar; }
		}

		public static string UnifyPath(string path)
		{
			return UnifyPath(path, true);
		}

		public static string UnifyPath(string path, bool toLower)
		{
			string unifiedPath = (toLower ? path.ToLower() : path).Trim(new char[] { '/', '\\' }).Replace("//", "/");
			unifiedPath = unifiedPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			unifiedPath = unifiedPath.Trim(Path.DirectorySeparatorChar);
			unifiedPath = ParsePath(unifiedPath, "..");
			return unifiedPath;
		}

		public static string ParsePath(string path, string formatChar)
		{
			List<string> strs = new List<string>();
			string[] pathstrs = path.Split('/');
			int formatCount = 0;
			for (int i = 0; i < pathstrs.Length; i++)
			{
				if (!pathstrs[i].Equals(".."))
				{
					formatCount = i;
					break;
				}
			}

			for (int i = 0; i < pathstrs.Length; i++)
			{
				if (i < formatCount)
					continue;
				strs.Add(pathstrs[i]);
			}
			if (strs.Contains(formatChar))
			{
				string pathstr = string.Empty;
				if (strs != null && strs.Count > 0)
				{
					for (int i = 0; i < strs.Count; i++)
					{
						if ((strs[i].Equals(formatChar) || strs[i] == formatChar))
						{
							pathstr = pathstr.Substring(0, pathstr.LastIndexOf('/'));
						}
						else
							pathstr += "/" + strs[i];
					}

				}
				for (int i = 0; i < formatCount; i++)
				{
					pathstr = "/" + formatChar + pathstr;
				}
				return ParsePath(pathstr.IndexOf('/') == 0 ? pathstr.Substring(1) : pathstr, formatChar);
			}
			else
			{
				return path;
			}
		}

		public static string TripExtension(string path)
		{
			if (Path.HasExtension(path))
			{
				return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
			}
			else
			{
				return path;
			}
		}

		public static string GetExtension(string path)
		{
			if (Path.HasExtension(path))
			{
				return Path.GetExtension(path);
			}
			else
			{
				return "";
			}
		}

		public static string GetSubPath(string path, string inPath)
		{
			path = UnifyPath(path);
			inPath = UnifyPath(inPath);

			if (path.StartsWith(inPath))
				return UnifyPath(path.Substring(inPath.Length));
			else
				return "";
		}

		// Combine path.		
		public static string Combine(bool toLower, params string[] pathes)
		{
			if (pathes == null || pathes.Length == 0)
				return "";

			string res = "";

			foreach (string p in pathes)
				res += p + "/";

			return UnifyPath(res, toLower);
		}

		public static string Combine(params string[] pathes)
		{
			return Combine(true, pathes);
		}

		public static bool CreateDirectory(string directory)
		{
			if (Directory.Exists(directory) == false)
			{
				Directory.CreateDirectory(directory);
				return Directory.Exists(directory);
			}
			else
			{
				return true;
			}
		}

		public static void CopyFile(string sourceFile, string targetFile)
		{
#if ENABLE_BUILD_LOG
		Debug.Log(sourceFile + " " + targetFile);
#endif
			// Convert to full path
			sourceFile = Path.GetFullPath(sourceFile);
			targetFile = Path.GetFullPath(targetFile);

			if (Directory.Exists(sourceFile))
			{
				// Copy directory
				DirectoryCopy(sourceFile, targetFile, true);
			}
			else
			{
				// Copy file
				// Create target path if it doesn't exist
				string targetPath = Path.GetDirectoryName(targetFile);
				if (PathUtility.CreateDirectory(targetPath) == false)
				{
					Debug.LogError("Create target path failed : " + targetPath);
					return;
				}

				// Notice can not use AssetDatabase.DeleteAsset, AssetDatabase.CopyAsset
				// 1. DeleteAsset will cause GUID changed which is used in ezgui
				// 2. CopyAsset will cause a internal asset of unity.
				File.Delete(targetFile);
				File.Copy(sourceFile, targetFile);
			}
		}

		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the source directory does not exist, throw an exception.
			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			// If the destination directory does not exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}


			// Get the file contents of the directory to copy.
			FileInfo[] files = dir.GetFiles();

			foreach (FileInfo file in files)
			{
				// Create the path to the new copy of the file.
				string temppath = Path.Combine(destDirName, file.Name);

				// Copy the file.
				file.CopyTo(temppath, true);
			}

			// If copySubDirs is true, copy the subdirectories.
			if (copySubDirs)
			{

				foreach (DirectoryInfo subdir in dirs)
				{
					// Skip svn folder
					if (string.Compare(subdir.Name, ".svn", true) == 0)
						continue;

					// Create the subdirectory.
					string temppath = Path.Combine(destDirName, subdir.Name);

					// Copy the subdirectories.
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}

		public static void RemoveFile(string filePath)
		{
			if (Directory.Exists(filePath))
				Directory.Delete(filePath, true);
			else if (File.Exists(filePath))
				File.Delete(filePath);
		}

		public static string FindFileWithExtension(string filePath)
		{
			// Find the file with extension
			List<string> fileWithExt = new List<string>(Directory.GetFiles(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + ".*"));

			// Remove meta file
			for (int i = 0; i < fileWithExt.Count; ++i)
			{
				if (Path.GetExtension(fileWithExt[i]).Equals(".meta", System.StringComparison.CurrentCultureIgnoreCase))
				{
					fileWithExt.RemoveAt(i);
					--i;
				}
			}

			if (fileWithExt.Count == 0)
			{
				Debug.LogError("Can not find file : " + filePath);
				return null;
			}

			if (fileWithExt.Count != 1)
			{
				System.Text.StringBuilder sb = new StringBuilder();
				sb.AppendLine("There are more than one file : \n");
				foreach (var file in fileWithExt)
					sb.AppendLine(file);

				Debug.LogError(sb);

				return null;
			}

			return fileWithExt[0];
		}

		public static string GetAssetNameKeepInBuddle(string assetName)
		{
			assetName = UnifyPath(assetName);

			return assetName.Replace('/', '_');
		}
	}
}
