using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.SceneManagement
{
    static class SceneLoader
    {
        public static void Load(int index)
        {
            SceneManager.LoadScene(index);
        }

        public static void GameOver()
        {
            Load(1);
        }

        public static void RestartGame()
        {
            Load(1);
        }

        public static void Quit()
        {
            Application.Quit();
        }
    }
}
