using UnityEditor;
using UnityEngine;

public class DeveloperOptions : MonoBehaviour
{
    private const string _developerFolder = "Developer/";

//Keep all dev options here to be able to create builds
#if UNITY_EDITOR

    [MenuItem(_developerFolder + "Player/Teleport to spawnpoint")]
    public static void TeleportToSpawnPoint()
    {
        Debug.Log("Random Msg");
    }

#endif
}

