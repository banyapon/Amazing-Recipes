using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KitchenChaos.Core
{
    public static partial class Loader
    {
        static Scene _targetScene;

        public static void LoadScene(Scene targetScene)
        {
            _targetScene = targetScene;
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void LoadSceneNetwork(Scene targetScene)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(_targetScene.ToString());
        }
    }
}
