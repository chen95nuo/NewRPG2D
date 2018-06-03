//功能： 字符串转化，主要为了优化int转为string的GC
//创建者: 胡海辉
//创建时间：
using System;
using System.Text;
namespace Assets.Script.Tools
{
    public class StringHelper : TSingleton<StringHelper>, IDisposable
    {
        private const int maxNum = 4096;
        public StringBuilder builder;
        public string[] StringNum = new string[maxNum];
        public override void Init()
        {
            base.Init();
            builder = new StringBuilder();
            for (int i = 0; i < maxNum; i++)
            {
                StringNum[i] = i.ToString();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            builder = null;
        }

        public void ClearBuilder()
        {
            builder.Remove(0, builder.Length);
        }

        public string IntToString(int num)
        {
            if (num >= 0 && num < maxNum)
            {
                return StringNum[num];
            }
            else
            {
                return num.ToString();
            }
        }

        public string StringToSpecilString(string str, int instanceId)
        {
            return str + "_" + IntToString(instanceId);
        }

        public string GetPerfabName(string str, int instanceId)
        {
            return str + "_" + IntToString(instanceId);
        }


    }
}
