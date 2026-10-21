using UnityEngine;

public class Scoremanager : MonoBehaviour
{
    public static Scoremanager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
            return;
        }
        
        Instance = this;

    }
}
