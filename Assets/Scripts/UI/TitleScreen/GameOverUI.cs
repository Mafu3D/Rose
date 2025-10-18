using Project.SceneManagement;
using UnityEngine;

namespace Project.UI.GameOver
{
    public class GameOverUI : MonoBehaviour
    {
        public void RestartGame()
        {
            Debug.Log("Game Started");
            SceneLoader.Load(0);
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
            SceneLoader.Quit();
        }
    }
}