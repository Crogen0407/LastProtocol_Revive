using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"이런, '{typeof(T).Name}'가 들어있는 오브젝트가 없어요.\n" +
                        $"{typeof(T).Name}를 넣은 오브젝트를 만들어주세요.");
                }
            }
            return instance;
        }
    }
}
