
using System.IO;
using UnityEngine;

namespace Assets.Script.Utility
{
    public class CaptureScreenMgr : TSingleton<CaptureScreenMgr>
    {

        /// <summary>  
        /// 对相机截图。   
        /// </summary>  
        /// <returns>The screenshot2.</returns>  
        /// <param name="camera">Camera.要被截屏的相机</param>  
        /// <param name="rect">Rect.截屏的区域</param>  
        public Texture2D CaptureCamera(Camera camera, Rect rect, int roleId)
        {
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
            camera.targetTexture = rt;
            camera.Render();
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();
            camera.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string dirPath = string.Format("{0}/{1}", Application.persistentDataPath, roleId);
            DirectoryInfo mydir = new DirectoryInfo(dirPath);
            if (!mydir.Exists)
                Directory.CreateDirectory(dirPath);

            System.IO.File.WriteAllBytes(dirPath + "/Icon.png", bytes);

            return screenShot;
        }
    }
}
