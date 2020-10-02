using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _clock;
    private DateTime _dateTimeEnd;

    private void Start()
    {
        _clock.text = "15:00";
        _dateTimeEnd = DateTime.Now.Add(TimeSpan.FromMinutes(15));
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            var estimateTime = _dateTimeEnd - DateTime.Now;
            _clock.text = $"{estimateTime.Minutes}:{estimateTime.Seconds}";
            yield return new WaitForSeconds(1);
        }
    }
}
