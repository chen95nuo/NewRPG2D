
//功能： 泛型单例
//创建者: 胡海辉
//创建时间：

using System;
public class TSingleton<T>
{
    private static T s_instance;

    public static T instance
    {
        get
        {
            if (s_instance == null)
            {
                CreateInstance();
            }
            return s_instance;
        }
    }

    public static void CreateInstance()
    {
        if (s_instance == null)
        {
            s_instance = Activator.CreateInstance<T>();
            if (s_instance is TSingleton<T>)
            {
                (s_instance as TSingleton<T>).Init();
            }
        }
    }

    public static void DestroyInstance()
    {
        if (s_instance != null)
        {
            (s_instance as TSingleton<T>).Dispose();
            s_instance = (T)((object)null);
        }
    }

    public static T GetInstance()
    {
        if (s_instance == null)
        {
            CreateInstance();
        }
        return s_instance;
    }

    public static bool HasInstance()
    {
        return s_instance != null;
    }

    public virtual void Init()
    {
    }

    public virtual void Update(float deltaTime)
    { }

    public virtual void Dispose()
    {
    }

}