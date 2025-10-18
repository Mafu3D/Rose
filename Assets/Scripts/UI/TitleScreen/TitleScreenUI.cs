using Project.SceneManagement;
using UnityEngine;

namespace Project.UI.TitleScreen
{
    public class TitleScreenUI : MonoBehaviour
    {
        public void StartGame()
        {
            Debug.Log("Game Started");
            SceneLoader.Load(2);
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
            SceneLoader.Quit();
        }
    }
}

namespace Project.UI.GameOver
{
}