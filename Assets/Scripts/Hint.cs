using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Hint : MonoBehaviour
{
    [SerializeField] private List<LetterBox> _letterBoxes;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OpenWord()
    {
        if (GameState.Instance.State != State.InGame)
        {
            return;
        }
        if (_letterBoxes.Any(x => x.LetterBoxState == LetterBoxState.Empty))
        {
            _button.interactable = false;
            var allFreeLetters = _letterBoxes.Where(x => x.LetterBoxState == LetterBoxState.Empty).ToList();
            var targetLetter = allFreeLetters[Random.Range(0, allFreeLetters.Count - 1)];
            targetLetter.FillCell();
            GameState.Instance.CheckWordOnCollection();
        }
    }

    public void Reset()
    {
        _button.interactable = true;
    }
}
