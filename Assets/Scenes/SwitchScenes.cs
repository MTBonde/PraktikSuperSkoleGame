using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SwitchScenes : MonoBehaviour
    {   
        public static void SwitchToMainWorld() => SceneManager.LoadScene(SceneNames.Main);
        public static void SwitchToPlayerHouseScene() => SceneManager.LoadScene(SceneNames.House);
        public static void SwitchToWordFactoryLoadingScene() => SceneManager.LoadScene(SceneNames.FactoryLoading);
        public static void SwitchToWordFactory() => SceneManager.LoadScene(SceneNames.Factory);
        // TODO : Change this when we have a racing scene
        public static void SwitchToRacingScene() => SceneManager.LoadScene(SceneNames.House);    
        public static void SwitchToSymbolEaterScene() => SceneManager.LoadScene(SceneNames.Eater);    
        public static void SwitchToSymbolEaterLoaderScene() => SceneManager.LoadScene(SceneNames.EaterLoading);
        public static void SwitchToTowerScene() => SceneManager.LoadScene(SceneNames.Tower);
        public static void SwitchToTowerLoaderScene() => SceneManager.LoadScene(SceneNames.TowerLoading);
        public static void SwitchToRacerLoaderScene() => SceneManager.LoadScene(SceneNames.RacerLoading);
        public static void SwitchToRacerScene() => SceneManager.LoadScene(SceneNames.Racer);
        public static void SwitchToArcadeScene() => SceneManager.LoadScene(SceneNames.Arcade);

    }
}
