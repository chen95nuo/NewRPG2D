using UnityEngine;
using System;

namespace KodGames
{
	public static class Math
	{
		public static int RoundToInt(double value)
		{
			if (value > 0)
				return (int)(value + 0.5);
			else
				return -((int)(System.Math.Abs(value) + 0.5));
		}

		public static double Ceil(double value)
		{
			return (double)CeilToInt(value);
		}

		public static double Floor(double value)
		{
			return (double)FloorToInt(value);
		}

		public static long CeilToInt(double value)
		{
			if (value >= 0)
				return (value - (long)value) != 0 ? (long)(value + 1) : (long)value;
			else
				return (long)value;
		}

		public static long FloorToInt(double value)
		{
			if (value >= 0)
				return (long)value;
			else
				return (value - (long)value) != 0 ? (long)(value - 1) : (long)value;
		}

		/// <summary>
		/// Clamp a angle in 0 to 360
		/// </summary>
		public static float ClampAngle(float angle)
		{
			angle = angle - (((int)angle) / 360) * 360;
			if (angle < 0)
				angle += 360;

			return angle >= 0 ? angle : angle + 360;
		}

		public static float LerpWithoutClamp(float from, float to, float t)
		{
			return from + (to - from) * t;
		}

		public static float LerpAngleWithoutClamp(float from, float to, float t)
		{
			return ClampAngle(LerpWithoutClamp(from, to, t));
		}

		//////////////////////////////////////////////////////////////////////////
		// http://wiki.unity3d.com/index.php/Mathfx		
		/// <summary>
		/// his method will interpolate while easing in and out at the limits.
		/// http://wiki.unity3d.com/images/0/02/Mathfx-Hermite.png
		/// </summary>
		public static float Hermite(float start, float end, float value)
		{
			return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
		}

		/// <summary>
		/// Sinerp - Short for 'sinusoidal interpolation', this method will interpolate while easing around the end, when value is near one.
		/// http://wiki.unity3d.com/images/4/44/Mathfx-Sinerp.png
		/// </summary>
		public static float Sinerp(float start, float end, float value)
		{
			return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
		}

		/// <summary>
		/// Coserp - Similar to Sinerp, except it eases in, when value is near zero, instead of easing out (and uses cosine instead of sine).
		/// </summary>
		public static float Coserp(float start, float end, float value)
		{
			return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
		}

		/// <summary>
		/// Berp - Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest.
		/// http://wiki.unity3d.com/images/a/a9/Mathfx-Berp.png
		/// </summary>
		public static float Berp(float start, float end, float value)
		{
			value = Mathf.Clamp01(value);
			value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
			return start + (end - start) * value;
		}

		/// <summary>
		/// SmoothStep - Works like Lerp, but has ease-in and ease-out of the values.
		/// </summary>
		public static float SmoothStep(float x, float min, float max)
		{
			x = Mathf.Clamp(x, min, max);
			float v1 = (x - min) / (max - min);
			float v2 = (x - min) / (max - min);
			return -2 * v1 * v1 * v1 + 3 * v2 * v2;
		}

		/// <summary>
		/// Lerp - Short for 'linearly interpolate', this method is equivalent to Unity's Mathf.Lerp, included for comparison.
		/// http://wiki.unity3d.com/images/c/c8/Mathfx-Lerp.png
		/// </summary>
		public static float Lerp(float start, float end, float value)
		{
			return ((1.0f - value) * start) + (value * end);
		}

		/// <summary>
		/// NearestPoint - Will return the nearest point on a line to a point. Useful for making an object follow a track.
		/// </summary>
		public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
			float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
			return lineStart + (closestPoint * lineDirection);
		}

		/// <summary>
		/// NearestPointStrict - Works like NearestPoint except the end of the line is clamped.
		/// </summary>
		public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
		{
			Vector3 fullDirection = lineEnd - lineStart;
			Vector3 lineDirection = Vector3.Normalize(fullDirection);
			float closestPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);
			return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
		}

		/// <summary>
		/// Bounce - Returns a value between 0 and 1 that can be used to easily make bouncing GUI items (a la OS X's Dock)
		/// http://wiki.unity3d.com/images/e/e8/Mathfx-Bounce.png
		/// </summary>
		public static float Bounce(float x)
		{
			return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
		}

		// test for value that is near specified float (due to floating point inprecision)
		// all thanks to Opless for this!
		public static bool Approx(float val, float about, float range)
		{
			return ((Mathf.Abs(val - about) < range));
		}

		// test if a Vector3 is close to another Vector3 (due to floating point inprecision)
		// compares the square of the distance to the square of the range as this 
		// avoids calculating a square root which is much slower than squaring the range
		public static bool Approx(Vector3 val, Vector3 about, float range)
		{
			return ((val - about).sqrMagnitude < range * range);
		}

		/*
		  * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
		  * This is useful when interpolating eulerAngles and the object
		  * crosses the 0/360 boundary.  The standard Lerp function causes the object
		  * to rotate in the wrong direction and looks stupid. Clerp fixes that.
		  */
		public static float Clerp(float start, float end, float value)
		{
			float min = 0.0f;
			float max = 360.0f;
			float half = Mathf.Abs((max - min) / 2.0f);//half the distance between min and max
			float retval = 0.0f;
			float diff = 0.0f;

			if ((end - start) < -half)
			{
				diff = ((max - start) + end) * value;
				retval = start + diff;
			}
			else if ((end - start) > half)
			{
				diff = -((max - end) + start) * value;
				retval = start + diff;
			}
			else retval = start + (end - start) * value;

			// Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
			return retval;
		}

		public static int CombineValue(int high, int low)
		{
			return ((int)((high & 0x0000FFFF) << 16)) | (low & 0x0000FFFF);
		}

		public static void SplitValue(int value, out int high, out int low)
		{
			high = (int)((value & 0xFFFF0000) >> 16);
			low = value & 0x0000FFFF;
		}

		//计算相对于指定Transform的上下、前后、左右指定大小的偏移值
		public static Vector3 RelativeOffset(Transform target, Vector3 offset, bool useVector3Up)
		{
			if (useVector3Up)
				return target.right.normalized * offset.x + Vector3.up.normalized * offset.y + target.forward.normalized * offset.z;
			else
				return target.right.normalized * offset.x + target.up.normalized * offset.y + target.forward.normalized * offset.z;
		}
	}
}
