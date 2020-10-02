using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextLevelUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    public void Show(int currentLevel, List<LetterBox> openedWords, int wordAmount)
    {
        _levelText.text = $"Level {currentLevel}";
    }
}
