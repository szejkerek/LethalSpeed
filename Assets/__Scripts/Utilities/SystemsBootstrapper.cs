using UnityEngine;
using UnityEngine.AddressableAssets;

public static class SystemsBootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Object.DontDestroyOnLoad(Addressables.InstantiateAsync("PersistentSystems").WaitForCompletion());
    }
}
