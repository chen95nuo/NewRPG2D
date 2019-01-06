using System.Security.Cryptography;

namespace KodGames
{
	public static class Cryptography
	{
		public static string Md5Hash(string input)
		{
			return Md5Hash(System.Text.Encoding.UTF8.GetBytes(input));
		}

		public static string Md5Hash(byte[] input)
		{
			MD5 md5Hash = new MD5CryptoServiceProvider();

			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hash.ComputeHash(input);

			// Create a new StringBuilder to collect the bytes and create a string.
			System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();

			// Loop through each byte of the hashed data  and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
				sBuilder.Append(data[i].ToString("x2"));

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}

		public static int SDBMHash(string input)
		{
			uint hash = 0;

			foreach (var c in input)
			{
				hash = c + (hash << 6) + (hash << 16) - hash;
			}

			return (int)(hash & 0x7FFFFFFF);
		}
	}
}
