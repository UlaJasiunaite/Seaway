using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    None = 0,
    MainScene = 1,
    StartScene = 2,
    GameplayScene = 3,
    LoadingScene = 4,
}

public class SceneLoader : MonoBehaviour
{
    public static Scenes SceneToLoad;
    public static Scenes SceneToUnload;
    public static bool LegacyUnload;

    private static SceneLoader _instance;
    private static Animator _transitionAnimator;
    private static readonly int StartTransition = Animator.StringToHash("StartTransition");
    private static Scenes _currentScene = Scenes.None;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _transitionAnimator = gameObject.GetComponentInChildren<Animator>();

        LoadScene(Scenes.StartScene, Scenes.None);
    }

    public static void LoadScene(Scenes sceneToLoad, Scenes sceneToUnload)
    {
        _instance.StartCoroutine(StartLoadingScene());
        SceneToLoad = sceneToLoad;
        SceneToUnload = sceneToUnload;
        
        _currentScene = sceneToLoad;
    }

    public static void LoadScene(Scenes sceneToLoad, Scenes sceneToUnload, bool legacyUnload)
    {
        LoadScene(sceneToLoad, sceneToUnload);
        LegacyUnload = legacyUnload;
    }
    
    public static void LoadScene(Scenes sceneToLoad)
    {
        LoadScene(sceneToLoad, _currentScene);
    }

    private static IEnumerator StartLoadingScene()
    {
        _transitionAnimator.SetTrigger(StartTransition);
        yield return new WaitForSeconds(_transitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }

    public static IEnumerator FinalizeLoadingScene(AsyncOperation operation)
    {
        _transitionAnimator.SetTrigger(StartTransition);
        yield return new WaitForSeconds(_transitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        operation.allowSceneActivation = true;

        if (SceneToUnload != Scenes.None)
        {
            if (LegacyUnload)
                SceneManager.UnloadScene(SceneToUnload.ToString());
            else
                SceneManager.UnloadSceneAsync(SceneToUnload.ToString());

            SceneToUnload = Scenes.None;
        }

        if (LegacyUnload)
            SceneManager.UnloadScene(Scenes.LoadingScene.ToString());
        else
            SceneManager.UnloadSceneAsync(Scenes.LoadingScene.ToString());

        Cleanup();
    }

    public static void FinalizeUnLoadingAdditiveScene()
    {
        if (SceneToUnload != Scenes.None)
        {
            SceneManager.UnloadSceneAsync(SceneToUnload.ToString());
            SceneToUnload = Scenes.None;
        }

        SceneManager.UnloadSceneAsync(Scenes.LoadingScene.ToString());

        Cleanup();
    }

    private static void Cleanup()
    {
        SceneToLoad = Scenes.None;
        LegacyUnload = false;
        Resources.UnloadUnusedAssets();
    }
}