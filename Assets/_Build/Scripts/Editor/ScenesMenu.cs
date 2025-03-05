using UnityEditor;
using UnityEditor.SceneManagement;

namespace LostKaiju.Editor
{
    public static class ScenesMenu
    {
        [MenuItem("Scenes/EntryPoint")]
        private static void Entry()
        {
            EditorSceneManager.OpenScene($"Assets/_Build/Scenes/{Scenes.ENTRY_POINT}.unity");
        }

        [MenuItem("Scenes/MainMenu")]
        private static void MainMenu()
        {
            EditorSceneManager.OpenScene($"Assets/_Build/Scenes/{Scenes.MAIN_MENU}.unity");
        }

        [MenuItem("Scenes/Hub")]
        private static void Hub()
        {
            EditorSceneManager.OpenScene($"Assets/_Build/Scenes/{Scenes.HUB}.unity");
        }
    }
}