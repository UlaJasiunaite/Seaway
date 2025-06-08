using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenActions : MonoBehaviour
{
    private void Start()
    {
        if (UnloadAdditive())
        {
            UnloadAdditiveScene();
            return;
        }

        StartCoroutine(LoadScene());
    }

    private static bool UnloadAdditive()
    {
        return SceneLoader.SceneToLoad == Scenes.None && SceneLoader.SceneToUnload != Scenes.None;
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        var isLoadingFinalizing = false;
        var asyncOperation =
            SceneManager.LoadSceneAsync(SceneLoader.SceneToLoad.ToString(), LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                if (!isLoadingFinalizing)
                {
                    isLoadingFinalizing = true;
                    StartCoroutine(SceneLoader.FinalizeLoadingScene(asyncOperation));
                }
            }

            yield return null;
        }
    }

    private static void UnloadAdditiveScene()
    {
        SceneLoader.FinalizeUnLoadingAdditiveScene();
    }
}