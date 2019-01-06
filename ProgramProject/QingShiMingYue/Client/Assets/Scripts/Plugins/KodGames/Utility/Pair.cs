﻿using System;
using System.Collections.Generic;

namespace KodGames
{
	public struct Pair<T1, T2>
	{
		public T1 first;
		public T2 second;

		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}
	}
}

