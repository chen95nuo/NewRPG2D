using UnityEngine;
using System.Collections;

namespace KodGames
{
	public class ImageFileLoader
	{
		public static bool IsBMPFile(byte[] buffer)
		{
			int offset = 0;
			return ReadShort(buffer, ref offset) == 0x4d42;
		}

		public static bool IsPNGFile(byte[] buffer)
		{
			int offset = 0;
			return ReadUInt(buffer, ref offset) == 0x474E5089;
		}

		public static bool IsJPEGFile(byte[] buffer)
		{
			int offset = 0;
			return ReadUInt(buffer, ref offset) == 0xE0FFD8FF;
		}

		public static bool IsGIFFile(byte[] buffer)
		{
			//gr = new GifReader();
			//bool result = gr.IsGIFFile(buffer);
			//gr = null;
			//return result;
			return false;
		}

		private static int ReadInt(byte[] buffer, ref int offset)
		{
			if (offset + 3 >= buffer.Length)
				return 0;

			int byte0 = buffer[offset++];
			int byte1 = buffer[offset++];
			int byte2 = buffer[offset++];
			int byte3 = buffer[offset++];

			return (byte3 << 24) | (byte2 << 16) | (byte1 << 8) | (byte0 << 0);
		}

		public static uint ReadUInt(byte[] buffer, ref int offset)
		{
			if (offset + 3 >= buffer.Length)
				return 0;

			uint byte0 = buffer[offset++];
			uint byte1 = buffer[offset++];
			uint byte2 = buffer[offset++];
			uint byte3 = buffer[offset++];

			return (byte3 << 24) | (byte2 << 16) | (byte1 << 8) | (byte0 << 0);
		}

		public static uint ReadUIntLittle(byte[] buffer, ref int offset)
		{
			if (offset + 3 >= buffer.Length)
				return 0;

			uint byte0 = buffer[offset++];
			uint byte1 = buffer[offset++];
			uint byte2 = buffer[offset++];
			uint byte3 = buffer[offset++];

			return (byte3 << 0) | (byte2 << 8) | (byte1 << 16) | (byte0 << 24);
		}

		public static short ReadShort(byte[] buffer, ref int offset)
		{
			if (offset + 1 >= buffer.Length)
				return 0;

			int byte0 = buffer[offset++];
			int byte1 = buffer[offset++];

			return (short)((byte1 << 8) | (byte0 << 0));
		}

		public static byte ReadByte(byte[] buffer, ref int offset)
		{
			if (offset >= buffer.Length)
				return 0;

			return buffer[offset++];
		}

		#region GIF
		//private static GifReader gr = null;
		public static Texture2D LoadToTextureGIF(byte[] data)
		{
			//gr = new GifReader();
			//Texture2D result = gr.LoadToTexture(data);
			//gr = null;
			//return result;
			return null;
		}
		#endregion

		#region BITMAP
		private struct BITMAP_FILEHEADER
		{
			public short signature;
			public int size;
			public int reserved;
			public int bitsOffset;

			public void Read(byte[] buffer, ref int offset)
			{
				signature = ImageFileLoader.ReadShort(buffer, ref offset);
				size = ImageFileLoader.ReadInt(buffer, ref offset);
				reserved = ImageFileLoader.ReadInt(buffer, ref offset);
				bitsOffset = ImageFileLoader.ReadInt(buffer, ref offset);
			}
		};

		private struct BITMAP_HEADER
		{
			public int headerSize;
			public int width;
			public int height;
			public short planes;
			public short bitCount;
			public int compression;
			public int sizeImage;
			public int pelsPerMeterX;
			public int pelsPerMeterY;
			public int clrUsed;
			public int clrImportant;

			public void Read(byte[] buffer, ref int offset)
			{
				headerSize = ImageFileLoader.ReadInt(buffer, ref offset);
				width = ImageFileLoader.ReadInt(buffer, ref offset);
				height = ImageFileLoader.ReadInt(buffer, ref offset);
				planes = ImageFileLoader.ReadShort(buffer, ref offset);
				bitCount = ImageFileLoader.ReadShort(buffer, ref offset);
				compression = ImageFileLoader.ReadInt(buffer, ref offset);
				sizeImage = ImageFileLoader.ReadInt(buffer, ref offset);
				pelsPerMeterX = ImageFileLoader.ReadInt(buffer, ref offset);
				pelsPerMeterY = ImageFileLoader.ReadInt(buffer, ref offset);
				clrUsed = ImageFileLoader.ReadInt(buffer, ref offset);
				clrImportant = ImageFileLoader.ReadInt(buffer, ref offset);
			}
		};

		public static Texture2D LoadToTextureBMP(byte[] data)
		{
			int offset = 0;

			BITMAP_FILEHEADER bmpFileHeader = new BITMAP_FILEHEADER();
			bmpFileHeader.Read(data, ref offset);

			BITMAP_HEADER bmpHeader = new BITMAP_HEADER();
			bmpHeader.Read(data, ref offset);

			int widthStart = System.Math.Min(0, bmpHeader.width);
			int widthEnd = System.Math.Max(0, bmpHeader.width);
			int heightStart = System.Math.Min(0, bmpHeader.height);
			int heightEnd = System.Math.Max(0, bmpHeader.height);

			if (bmpHeader.compression != 0)
			{
				Debug.LogError("Does not support compressed bmp file.");
				return null;
			}

			//		Debug.Log(string.Format("Width:{0} Height:{1}", bmpHeader.width, bmpHeader.height));

			if (bmpHeader.bitCount == 24)
			{
				// Read pixel
				Texture2D texture = new Texture2D(widthEnd - widthStart, heightEnd - heightStart, TextureFormat.ARGB32, false);

				offset = bmpFileHeader.bitsOffset;

				for (int y = heightStart; y < heightEnd; ++y)
				{
					for (int x = widthStart; x < widthEnd; ++x)
					{
						byte b = ReadByte(data, ref offset);
						byte g = ReadByte(data, ref offset);
						byte r = ReadByte(data, ref offset);
						byte a = 255;

						Color32 color = new Color32(r, g, b, a);
						texture.SetPixel(x > 0 ? x : -x, y > 0 ? y : -y, color);
					}
				}
				texture.Apply();
				return texture;
			}
			else if (bmpHeader.bitCount == 8)
			{
				Texture2D texture = new Texture2D(widthEnd - widthStart, heightEnd - heightStart, TextureFormat.ARGB32, false);
				int colourInfoBegin = offset;

				offset = bmpFileHeader.bitsOffset;
				Debug.Log("offset " + offset);
				Debug.Log("colourInfoBegin " + colourInfoBegin);

				for (int y = heightStart; y < heightEnd; ++y)
				{
					for (int x = widthStart; x < widthEnd; ++x)
					{
						int index = ReadByte(data, ref offset);
						//Debug.Log("index " + index);
						int colourInfoOffset = colourInfoBegin + index * 4;
						//Debug.Log("colourInfoOffset " + colourInfoOffset);

						byte b = ReadByte(data, ref colourInfoOffset);
						byte g = ReadByte(data, ref colourInfoOffset);
						byte r = ReadByte(data, ref colourInfoOffset);
						byte a = 255;

						Color32 color = new Color32(r, g, b, a);
						texture.SetPixel(x > 0 ? x : -x, y > 0 ? y : -y, color);
					}
				}
				texture.Apply();
				return texture;
			}
			else
			{
				Debug.LogError("Only support 24 bit or 8 bit bmp file : " + bmpHeader.bitCount);
				return null;
			}
		}

		#endregion
	}
}

