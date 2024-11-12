using UnityEngine;

using LostKaiju.Architecture.Providers;
using LostKaiju.Architecture.Services;

namespace LostKaiju.Architecture.Entry
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
