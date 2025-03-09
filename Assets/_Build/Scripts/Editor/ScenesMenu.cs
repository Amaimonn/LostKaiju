using UnityEditor;
using UnityEditor.SceneManagement;

using LostKaiju.Game.Constants;

namespace LostKaiju.Editor
{
    public static class ScenesMenu
    {
        [MenuItem("Scenes/EntryPoint")]
        private static void Entry()
        {
            OpenScene(Scenes.ENTRY_POINT);
        }

        [MenuItem("Scenes/MainMenu")]
        private static void MainMenu()
        {
            OpenScene(Scenes.MAIN_MENU);
        }

        [MenuItem("Scenes/Hub")]
        private static void Hub()
        {
            OpenScene(Scenes.HUB);
        }

        private static void OpenScene(string sceneRelativePath)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene($"Assets/_Build/Scenes/{sceneRelativePath}.unity");
        }
    }
}