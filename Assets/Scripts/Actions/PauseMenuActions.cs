using UnityEngine;

public class PauseMenuActions : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuModal;
    [SerializeField] private GameObject _achievementsTab;
    [SerializeField] private GameObject _controlsTab;
    
    public void OnResumeButtonClick()
    {
        _achievementsTab.SetActive(false);
        _controlsTab.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnAchievementsButtonClick()
    {
        _achievementsTab.SetActive(true);
        _controlsTab.SetActive(false);
    }

    public void OnControlsButtonClick()
    {
        _achievementsTab.SetActive(false);
        _controlsTab.SetActive(true);
    }

    public void OnMainMenuButtonClick()
    {
        Time.timeScale = 1f;
        PersistenceManager.Instance.SaveGame();
        SceneLoader.LoadScene(Scenes.StartScene, Scenes.GameplayScene);
    }

    public void OnExitGameButtonClick()
    {
        PersistenceManager.Instance.SaveGame();
        Application.Quit();
    }
}
