using UnityEngine;

public class ClassManager : MonoBehaviour
{
    public PlayerClassData mageData;
    public PlayerClassData warriorData;
    public PlayerClassData marksmanData;

    public static ClassManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
