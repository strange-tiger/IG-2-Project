using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if(_instance == null )
                {
                    GameObject newGameObject = new GameObject();
                    newGameObject.AddComponent<T>();
                    DontDestroyOnLoad(newGameObject);
                }
                else
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }

            }

            return _instance;
        }
    }

    protected void Awake()
    {
        T instance = FindObjectOfType<T>();

        if (instance != null && instance != _instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}