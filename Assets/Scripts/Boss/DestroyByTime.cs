using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);    
    }
}
