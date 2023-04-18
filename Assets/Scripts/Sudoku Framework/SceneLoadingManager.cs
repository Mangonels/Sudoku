using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoMa.Sudoku.Framework
{
    /// <summary>
    /// Manages scene loading globally.
    /// This class could get more sofisticated the more complex the game became.
    /// Loading scenes with the addressables system may also be considered.
    /// </summary>
    public class SceneLoadingManager : Singleton<SceneLoadingManager>
    {
        private void Awake()
        {
            base.Awake(true);
        }

        internal void LoadScene(int index, float delay = 0.0f) 
        {
            if (delay == 0.0f) SceneManager.LoadScene(index);
            else StartCoroutine(LoadSceneDelayed(index, delay));
        }

        private IEnumerator LoadSceneDelayed(int index, float delay)
        {
            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(index);
        }
    }
}