using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    /// <summary>
    /// 重写的字典集合，可以同步一个List
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    class YuanServerActor<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private List<TValue> myList;
        public YuanServerActor(List<TValue> mList)
            : base()
        {
            myList = mList;
        }

        /// <summary>
        /// 重写Add方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
			if(!base.ContainsKey (key))
			{
	            base.Add(key, value);
	            if (!myList.Contains(value))
	            {
	                myList.Add(value);
	            }
			}
        }


        /// <summary>
        /// 重写Remove方法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool Remove(TKey key)
        {
            if (this.ContainsKey(key) && myList.Contains(this[key]))
            {
                myList.Remove(this[key]);
            }
            return base.Remove(key);
        }

        
    }

