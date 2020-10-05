using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiShower : MonoBehaviour
{
    private ParticleSystem _confetti;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _confetti = GetComponentInChildren<ParticleSystem>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        StartCoroutine(ConfettiShowing());
    }

    private IEnumerator ConfettiShowing()
    {
        GameState.Instance.State = State.Pause;

        while (_canvasGroup.alpha<1)
        {
            _canvasGroup.alpha += 0.05f;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        _confetti.Play();
        yield return new WaitForSeconds(1);
        while (_canvasGroup.alpha>0)
        {
            _canvasGroup.alpha -= 0.05f;
            yield return new WaitForEndOfFrame();
        }
        
        GameState.Instance.State = State.InGame;
    }
}
