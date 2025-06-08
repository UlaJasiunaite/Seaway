using System.Collections;
using UnityEngine;

public class StartScreenActions : MonoBehaviour
{
    [SerializeField] private CanvasGroup _achievementsTab;
    [SerializeField] private CanvasGroup _controlsTab;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindFirstObjectByType<AudioManager>();
        _audioManager.PlayPrimarySound(SoundsList.OceanSound);
    }

    public void OnPlayButtonClick()
    {
        SceneLoader.LoadScene(Scenes.GameplayScene, Scenes.StartScene);
    }

    public void OnAchievementsButtonClick()
    {
        _achievementsTab.blocksRaycasts = true;
        StartCoroutine(FadeUIElement(false, _achievementsTab));
    }

    public void OnAchievementsExitButtonClick()
    {
        _achievementsTab.blocksRaycasts = false;
        StartCoroutine(FadeUIElement(true, _achievementsTab));
    }

    public void OnControlsButtonClick()
    {
        _controlsTab.blocksRaycasts = true;
        StartCoroutine(FadeUIElement(false, _controlsTab));
    }

    public void OnControlsExitButtonClick()
    {
        _controlsTab.blocksRaycasts = false;
        StartCoroutine(FadeUIElement(true, _controlsTab));
    }

    public void OnExitGameButtonClick()
    {
        Application.Quit();
    }

    private static IEnumerator FadeUIElement(bool fade, CanvasGroup element)
    {
        if (fade)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                element.alpha = i;
                yield return null;
            }
            element.alpha = 0;
            yield return null;
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                element.alpha = i;
                yield return null;
            }
            element.alpha = 1;
            yield return null;
        }
    }
}
