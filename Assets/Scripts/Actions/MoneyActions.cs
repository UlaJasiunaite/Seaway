using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyActions : MonoBehaviour
{
    [SerializeField] private CanvasGroup _popUpMoneyModal;
    [SerializeField] private TextMeshProUGUI _popUpMoneyText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    private PlayerStatistics _playerStatistics;
    private AudioManager _audioManager;

    private void Start()
    {
        _playerStatistics = FindFirstObjectByType<PlayerStatistics>();
        _audioManager = FindFirstObjectByType<AudioManager>();

        _moneyText.text = _playerStatistics.MoneyOwned.ToString();
    }

    public void AddMoney(int moneyToAdd)
    {
        _audioManager.PlayEffect(SoundsList.MoneySound);

        _playerStatistics.MoneyOwned += moneyToAdd;
        
        _popUpMoneyText.text = moneyToAdd.ToString();
        StartCoroutine(MoneyAddition());
    }

    private IEnumerator MoneyAddition()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            _popUpMoneyModal.alpha = i;
            yield return null;
        }
        _popUpMoneyModal.alpha = 1;
        
        _moneyText.text = _playerStatistics.MoneyOwned.ToString();
        yield return new WaitForSeconds(1f);

        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            _popUpMoneyModal.alpha = i;
            yield return null;
        }
        _popUpMoneyModal.alpha = 0;
        yield return null;
    }
}
