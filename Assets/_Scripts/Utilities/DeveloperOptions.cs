using UnityEditor;
using UnityEngine;

public class DeveloperOptions : MonoBehaviour
{
    private const string _developerFolder = "Developer/";

//Keep all dev options here to be able to create builds
#if UNITY_EDITOR

    [MenuItem(_developerFolder + "Enemy/Kill all enemies")]
    public static void KillAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Debug.Log($"{enemy.gameObject.name} was killed!");
            enemy.TakeDamage();
        }
    }

#endif
}

