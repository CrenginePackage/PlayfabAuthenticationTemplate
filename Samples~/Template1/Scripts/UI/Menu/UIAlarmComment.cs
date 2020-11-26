using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAlarmComment : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private float alarmTime;

    private Coroutine alarmCoroutine;

    public void OpenAlarm(string _title, string _content)
    {
        if (alarmCoroutine != null) StopCoroutine(alarmCoroutine);
        alarmCoroutine = StartCoroutine(AlarmRoutine(_title, _content));
    }

    private IEnumerator AlarmRoutine(string _title, string _content)
    {
        canvasGroup.alpha = 1;
        SetAlarmText(_title, _content);
        yield return new WaitForSeconds(alarmTime);
        canvasGroup.alpha = 0;
    }

    private void SetAlarmText(string _title, string _content)
    {
        titleText.text = _title;
        contentText.text = _content;
    }
}
