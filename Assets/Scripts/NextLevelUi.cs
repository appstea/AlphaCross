using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextLevelUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _wordsText;

    public void Show(int currentLevel, string openedWords)
    {
        _levelText.text = $"Level {currentLevel}";
        _wordsText.text = openedWords;
    }
}
