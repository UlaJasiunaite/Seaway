using UnityEngine;

public class GameplayScreenActions : MonoBehaviour
{
    [SerializeField] private CanvasGroup _miniMapModal;
    [SerializeField] private CanvasGroup _inventoryModal;
    [SerializeField] private GameObject _pauseMenuModal;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMapVisibility();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryVisibility();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenuVisibility();
        }
    }

    private void ToggleInventoryVisibility()
    {
        _inventoryModal.alpha = _inventoryModal.alpha == 1 ? 0 : 1;
    }

    private void ToggleMapVisibility()
    {
        _miniMapModal.alpha = _miniMapModal.alpha == 1 ? 0 : 1;
    }

    private void TogglePauseMenuVisibility()
    {
        if (_pauseMenuModal.gameObject.activeSelf)
        {
            _pauseMenuModal.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            _pauseMenuModal.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    
}