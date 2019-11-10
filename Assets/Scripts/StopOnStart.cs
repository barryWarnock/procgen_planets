using UnityEngine;

public class StopOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<ParticleSystem>().Pause();        
    }
}
