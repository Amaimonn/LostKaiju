using UnityEngine;

public class EntryPoint
{
    private static EntryPoint _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void EnterTheGame()
    {
        _instance = new();
        _instance.Run();
    }

    public void Run()
    {
        ServiceLocator.Current.Register<IInputProvider>(new BaseInputProvider());
    }
}