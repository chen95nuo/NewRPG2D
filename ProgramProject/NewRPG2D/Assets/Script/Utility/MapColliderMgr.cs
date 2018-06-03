using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using HiSocket;
using UnityEngine;

namespace Assets.Script.Utility
{
    public class MapColliderMgr : TSingleton<MapColliderMgr>
    {
        private List<Vector3> colliderPositionList;
        private ByteArray mByteArray;
        public override void Init()
        {
            base.Init();
            mByteArray = new ByteArray();
            colliderPositionList = new List<Vector3>();
            LoadMapData();
        }


        public override void Dispose()
        {
            base.Dispose();
            mByteArray = null;
            colliderPositionList = null;
        }

        public bool CheckCollider(Vector3 position)
        {
            for (int i = 0; i < colliderPositionList.Count; i++)
            {
                if ((colliderPositionList[i] - position).magnitude < 1.0f)
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadMapData()
        {
            string path = GetXmlPath();
            GameLogic.Instance.StartCoroutine(wwwLoad(path, ReadMapData));
        }

        private IEnumerator wwwLoad(string _path, Action<byte[]> action)
        {

            WWW www = new WWW(_path);
            yield return www;

            DebugHelper.LogError("MapColliderMgr wwwLoad error " + www.error);
            if (www.error != null)
            {
                yield break;
            }
            action(www.bytes);
        }

        private string GetXmlPath()
        {
            string path = "";
            path = string.Format("{0}/MapData", Application.streamingAssetsPath);
            return path;
        }

        private void ReadMapData(byte[] data)
        {
            mByteArray.Write(data, data.Length);
            byte X_Count = mByteArray.ReadByte<byte>();
            byte Z_Count = mByteArray.ReadByte<byte>();
            short totalCount = mByteArray.ReadByte<short>();
            while (mByteArray.Length - 3 > 0)
            {
                float x = mByteArray.ReadByte<byte>() * 0.5f;
                float y = mByteArray.ReadByte<byte>() * 0.5f;
                Vector3 position = new Vector3(x, 0, y);
                colliderPositionList.Add(position);
                byte value = mByteArray.ReadByte<byte>();

            }
            for (int i = 0; i < X_Count; i++)
            {
                Vector3 position = new Vector3(i * 0.5f, 0, 0);
                colliderPositionList.Add(position);
                position = new Vector3(i * 0.5f, 0, Z_Count * 0.5f);
                colliderPositionList.Add(position);
            }
            for (int i = 0; i < Z_Count; i++)
            {
                Vector3 position = new Vector3(0, 0, i * 0.5f);
                colliderPositionList.Add(position);
                position = new Vector3(X_Count * 0.5f, 0, i * 0.5f);
                colliderPositionList.Add(position);
            }
        }

    }
}
