using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerControl : MonoBehaviour
{
    [SerializeField] private UnityEvent _timeOver;
    [SerializeField] private TextMeshProUGUI _clock;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Transform _placeForTimerLines;
    [SerializeField] private GameObject _timerLinePrefab;
    [SerializeField] private GameObject _timerLineFreePrefab;
    private readonly List<GameObject> _timerLines = new List<GameObject>();
    private DateTime _dateTimeEnd;
    private bool _isPause;
    private Coroutine _timer;

    public void Initialize(Level level, int levelNumber)
    {
        _isPause = false;
        if (_timer != null)
        {
            StopCoroutine(_timer);
        }

        if (_timerLines.Any())
        {
            _timerLines.ForEach(Destroy);
        }

        _levelText.text = $"Level {levelNumber}";
        _dateTimeEnd = DateTime.Now.Add(TimeSpan.FromMinutes(level.Minutes)).Add(TimeSpan.FromSeconds(level.Seconds));
        _timer = StartCoroutine(Timer());
        level.Words.ForEach(x =>
        {
            _timerLines.Add(Instantiate(_timerLineFreePrefab, _placeForTimerLines));
        });
    }

    public void EndLevel(int wordNumber)
    {
        Destroy(_timerLines[wordNumber]);
        _timerLines[wordNumber] = Instantiate(_timerLinePrefab, _placeForTimerLines);
        _timerLines[wordNumber].transform.SetSiblingIndex(wordNumber);
    }

    public void ChangeTimerPause()
    {
        _isPause = !_isPause;
        GameState.Instance.State = _isPause ? State.Pause : State.InGame;
    }
    

    private IEnumerator Timer()
    {
        var estimateTime = _dateTimeEnd - DateTime.Now;
        while (estimateTime.TotalSeconds > 0)
        {
            while (_isPause)
            {
                _dateTimeEnd = _dateTimeEnd.AddSeconds(1);
                yield return new WaitForSeconds(1);
            }
            _clock.text = estimateTime.ToString(@"mm\:ss");
            yield return new WaitForSeconds(1);
            estimateTime = _dateTimeEnd - DateTime.Now;
            _clock.text = estimateTime.ToString(@"mm\:ss");
            while (_isPause)
            {
                _dateTimeEnd = _dateTimeEnd.AddSeconds(1);
                yield return new WaitForSeconds(1);
            }
        }
        _timeOver?.Invoke();
    }
}
