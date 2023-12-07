/// <summary>
/// 单例基类
/// </summary>
public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance;
    private static readonly object lockObject = new object();

    public static T inst
    {
        get
        {
            if (instance == null)
            {
                //Unity没有多线程.其实这个并不需要
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }

            return instance;
        }
    }
}