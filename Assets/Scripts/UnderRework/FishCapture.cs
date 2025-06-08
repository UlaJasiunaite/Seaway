using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;

public class FishCapture : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fishingUICanvasGroup;
    [SerializeField] private int _fishCountInACluster;
    [SerializeField] private float _timerDuration;

    [Header("Fishing UI:")]
    [SerializeField] private TextMeshProUGUI _fishCountText;
    [SerializeField] private GameObject _startFishingButton;
    [SerializeField] private GameObject _quicktimeEventView;
    [SerializeField] private List<QuicktimeKey> _quicktimeKeys;
    [SerializeField] private Image _timeCounterVisual;
    [SerializeField] private Sprite _successCheckmark;
    [SerializeField] private Sprite _failureCross;
    
    private GameObject _playerShip;
    int FishMax = 6;
    private bool _isFishing;
    bool IsPlayerInProximity = false;
    
    private static readonly string excludedLetters = "WASDIM";
    private static string availableLetters;

    private NotificationActions _notificationActions;
    private RandomEventManager _randomEventManager;
    private FishingManager _fishingManager;

    private void Start()
    {
        _randomEventManager = FindFirstObjectByType<RandomEventManager>();
        _notificationActions = FindFirstObjectByType<NotificationActions>();
        _fishingManager = FindFirstObjectByType<FishingManager>();
        _playerShip = FindFirstObjectByType<ShipMovementActions>().gameObject;
        
        SetAvailableLetters();
        _fishCountInACluster = UnityEngine.Random.Range(1, FishMax + 1);
    }
    
    private static void SetAvailableLetters()
    {
        for (var c = 'A'; c <= 'Z'; c++)
        {
            if (!excludedLetters.Contains(c.ToString()))
            {
                availableLetters += c;
            }
        }
    }

    private void TriggerFishingUI()
    {
        
    }

    private void DestroyFishCluster()
    {
        _randomEventManager.DestroyRandomEvent(gameObject);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _playerShip.transform.position) < 5)
        {
            if (!IsPlayerInProximity)
            {
                //In case of player moving in and out of the area, stops the previous coroutine
                StopCoroutine(FadeUI(true));
                StartCoroutine(FadeUI(false));
                IsPlayerInProximity = true;
            }

            if (Input.GetKeyDown(KeyCode.F) && !_isFishing && _fishCountInACluster != 0)
            {
                if (!_fishingManager.IsFishInventoryFull())
                {
                    StartCoroutine(QuicktimeEvent());
                    _startFishingButton.SetActive(false);
                    _quicktimeEventView.SetActive(true);
                }
                else _notificationActions.ShowNotification(NotificationTypes.MaxFishInventory);
            }
        }
        else
        {
            //Starts UI fade out
            if (IsPlayerInProximity == true)
            {
                //In case of player moving in and out of the area, stops the previous coroutine
                StopCoroutine(FadeUI(false));
                StartCoroutine(FadeUI(true));
                IsPlayerInProximity = false;
            }
        }
        
        if (_fishCountInACluster == 0)
        {
            _randomEventManager.DestroyRandomEvent(gameObject);
        }
    }

    private static char GetRandomLetter()
    {
        return availableLetters[Random.Range(0, availableLetters.Length)];
    }

    private void SetQuicktimeKeyLetters()
    {
        foreach (var quicktimeKey in _quicktimeKeys)
        {
            quicktimeKey.KeyText.text = GetRandomLetter().ToString();
        }
    }

    private void OnKeyPress()
    {
        
    }
    
    private IEnumerator QuicktimeEvent()
    {
        _isFishing = true;
        var currentKeyIndex = 1;

        SetQuicktimeKeyLetters();
        
        var normalizedTime = 1f;
        while (normalizedTime >= 0f)
        {
            _timeCounterVisual.fillAmount = normalizedTime;
            normalizedTime -= Time.deltaTime / _timerDuration;
            yield return null;

            //Key1 Sequence
            if (currentKeyIndex == 1 && Input.anyKeyDown)
            {
                var pressedKey = Input.inputString;

                if (pressedKey.ToUpper() == Key1.text)
                {
                    Key1.text = " ";
                    Success1.gameObject.SetActive(true); //Activates tick
                    Success1.sprite = _successCheckmark;
                    currentKeyIndex++;
                    yield return null;
                }
                else
                {
                    Key1.text = " ";
                    Success1.gameObject.SetActive(true); //Activates cross
                    Success1.sprite = _failureCross;
                    yield return new WaitForSeconds(1f);

                    FishingFinish();
                    yield break;
                }
            }

            //Key2 Sequence
            if (currentKeyIndex == 2 && Input.anyKeyDown)
            {
                string pressedKey = Input.inputString;

                if (pressedKey.ToUpper() == Key2.text)
                {
                    Key2.text = " ";
                    Success2.gameObject.SetActive(true);
                    Success2.sprite = _successCheckmark;
                    currentKeyIndex++;
                    yield return null;
                }
                else
                {
                    Key2.text = " ";
                    Success2.gameObject.SetActive(true);
                    Success2.sprite = _failureCross;
                    yield return new WaitForSeconds(1f);

                    FishingFinish();
                    yield break;
                }
            }

            //Key3 Sequence
            if (currentKeyIndex == 3 && Input.anyKeyDown)
            {
                string pressedKey = Input.inputString;

                if (pressedKey.ToUpper() == Key3.text)
                {
                    Key3.text = " ";
                    Success3.gameObject.SetActive(true);
                    Success3.sprite = _successCheckmark;

                    _fishingManager.AddFishToInventory(1);
                    FishCommendation?.Invoke(); //Adds +1 to fish commendation

                    yield return new WaitForSeconds(1f);

                    FishingFinish();
                    yield break;
                }
                else
                {
                    Key3.text = " ";
                    Success3.gameObject.SetActive(true);
                    Success3.sprite = _failureCross;
                    yield return new WaitForSeconds(1f);

                    FishingFinish();
                    yield break;
                }
            }
        }

        //When timer runs out
        Key1.text = " ";
        Key2.text = " ";
        Key3.text = " ";
        Success1.gameObject.SetActive(true);
        Success2.gameObject.SetActive(true);
        Success3.gameObject.SetActive(true);
        Success1.sprite = _failureCross;
        Success2.sprite = _failureCross;
        Success3.sprite = _failureCross;
        yield return new WaitForSeconds(1f);

        FishingFinish();
        yield return null;
    }

    private void FishingFinish()
    {
        FishNum.text = (int.Parse(FishNum.text) - 1).ToString();
        _isFishing = false;

        //Reset the toggle view
        FishStartToggle.GetComponent<CanvasGroup>().alpha = 1; //Toggle Main on
        FishKeys.GetComponent<CanvasGroup>().alpha = 0; //Toggle Fishing off

        //Empty out the fields
        Key1.text = " ";
        Key2.text = " ";
        Key3.text = " ";

        //Deactivate ticks and crosses
        Success1.gameObject.SetActive(false);
        Success2.gameObject.SetActive(false);
        Success3.gameObject.SetActive(false);
    }

    private IEnumerator FadeUI(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime * 2)
            {
                _fishingUICanvasGroup.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
            //Ensure that the UI is fully off
            _fishingUICanvasGroup.GetComponent<CanvasGroup>().alpha = 0;
            yield return null;
        }
        // fade from transparent to opaque
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime * 2)
            {
                _fishingUICanvasGroup.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
            //Ensure that UI is fully on
            _fishingUICanvasGroup.GetComponent<CanvasGroup>().alpha = 1;
            yield return null;
        }
    }

    [Serializable]
    private class QuicktimeKey
    {
        public TextMeshProUGUI KeyText;
        public Image KeyResolutionVisual;
    }
}