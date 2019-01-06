using UnityEngine;
using System.Collections;

namespace KodGames
{
	public class RefCountClass
	{
		public int RefCount { get { return mRefCount; } }

		~RefCountClass()
		{
			Debug.Assert(mRefCount == 0, "Deleting referenced object");
		}

		public void AddRef()
		{
			mRefCount++;
		}

		public void ReleaseRef()
		{
			Debug.Assert(mRefCount > 0, "mRefCount > 0");
			mRefCount--;
		}

		private int mRefCount = 0;
	}
}

