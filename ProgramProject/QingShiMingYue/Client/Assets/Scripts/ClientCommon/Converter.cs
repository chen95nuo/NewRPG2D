using UnityEngine;

namespace ClientServerCommon
{
	public static class Converter
	{
		public static Vector3 ToVector3(vector3 v)
		{
			Vector3 vec3;
			vec3.x = v.x;
			vec3.y = v.y;
			vec3.z = v.z;
			return vec3;
		}

		public static Color ToColor(color c)
		{
			Color color;
			color.r = c.r;
			color.g = c.g;
			color.b = c.b;
			color.a = c.a;
			return color;
		}

		public static Rect ToRect(rect r)
		{
			Rect rect = default(Rect);

			if (r == null)
			{
				rect.x = 0;
				rect.y = 0;
				rect.xMax = 1;
				rect.yMax = 1;
			}
			else
			{
				rect.x = r.x;
				rect.y = r.y;
				rect.xMax = r.xMax;
				rect.yMax = r.yMax;
			}

			return rect;
		}

		public static rect ToKodRect(Rect r)
		{
			rect rect = new rect();
			rect.x = r.x;
			rect.y = r.y;
			rect.xMax = r.xMax;
			rect.yMax = r.yMax;

			return rect;
		}
	}
}