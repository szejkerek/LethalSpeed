using UnityEngine;

public class Zone : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
