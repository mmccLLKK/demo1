/// <summary>
/// 单例基类
/// </summary>
public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance;

    public static T inst
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }
}