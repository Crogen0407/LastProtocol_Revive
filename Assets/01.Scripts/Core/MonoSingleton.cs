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
                    Debug.LogError($"�̷�, '{typeof(T).Name}'�� ����ִ� ������Ʈ�� �����.\n" +
                        $"{typeof(T).Name}�� ���� ������Ʈ�� ������ּ���.");
                }
            }
            return instance;
        }
    }
}
