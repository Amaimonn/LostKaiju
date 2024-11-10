using UnityEngine;

using Assets._Build.Scripts.Architecture.Providers;
using Assets._Build.Scripts.Architecture.Services;

namespace Assets._Build.Scripts.Architecture.Entry
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;

        public GameEntryPoint()
        {
            ServiceLocator.Current.Register<IInputProvider>(new BaseInputProvider());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void EnterTheGame()
        {
            _instance = new();
            _instance.Run();
        }

        public void Run()
        {
            
        }
    }
}
