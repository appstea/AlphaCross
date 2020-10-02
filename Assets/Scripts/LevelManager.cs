using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class LevelManager : MonoBehaviour
{
    [SerializeField] private NextLevelUi _nextLevelUi;
    [Header("Spawn Area Size")]
    [SerializeField] private Vector3 _spawnArea;
    [SerializeField] private Vector3 _spawnAreaMargin;

    [Space(20), Header("Letters Color")] [SerializeField]
    private List<Color> _lettersColors;
    
    [SerializeField, ReorderableList] private List<Level> _levels = new List<Level>();
    [SerializeField, ReadOnly] private int _currentLevel;
    [SerializeField] private int _levelToChange = 1;
    [SerializeField] private List<LetterBox> _letterBoxes;
    private Level InGameLevel => _levels[_currentLevel-1];
    private int _currentWordNumber = 0;
    private string CurrentWord => InGameLevel.Words[_currentWordNumber];
    private Letter[] _allLetters;

    public void WordCollected()
    {
        _currentWordNumber++;
        if (InGameLevel.Words.Count <= _currentWordNumber)
        {
            _nextLevelUi.Show(_currentLevel, string.Join(" ", InGameLevel.Words));
            _currentLevel++;
            _currentWordNumber = 0;
            _nextLevelUi.gameObject.SetActive(true);
            //TODO: Загрузка нового уровня
        }
        else
        {
            LoadWord();
        }
    }
    private void Start()
    {
        _allLetters = Resources.LoadAll<Letter>("Alphabet");
        Debug.Log($"Загружено {_allLetters.Length} символов.");

        LoadLevel();
    }

    #region EditorPart
    [Button] private void SetLevel()
    {
        if (_levels.Count < _levelToChange || _levelToChange < 1)
        {
            Debug.LogError("Введённый уровень недопустим.");
            return;
        }
        PlayerPrefs.SetInt("CurrentLevel", _levelToChange);
        _currentLevel = _levelToChange;
    }

    private void OnValidate()
    {
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position+_spawnAreaMargin, _spawnArea);
    }
    #endregion

    public void LoadLevel()
    {
        //TODO: Логика уровня
        
        var allLettersInLevel = InGameLevel.Words.SelectMany(x => x.ToLower().ToCharArray()).Where(x=>x != ' ').ToList();
        foreach (var letter in allLettersInLevel)
        {
            for (int i = 0; i < InGameLevel.MainLettersAmount; i++)
            {
                var alphabetLetterGo = Instantiate(_allLetters.First(x => x.AlphabetLetter == letter), new Vector3(100, 100), Quaternion.identity, transform);
                alphabetLetterGo.Initialize(_spawnArea, transform.position + _spawnAreaMargin, _lettersColors[Random.Range(0, _lettersColors.Count-1)]);
            }
        }

        var otherLetters = _allLetters.Select(x => x.AlphabetLetter).Where(x => !allLettersInLevel.Contains(x)).ToList();
        for (int i = 0; i < InGameLevel.OtherLettersAmount; i++)
        {
            var randNum = Random.Range(0, otherLetters.Count - 1);
            char otherLetter = otherLetters[randNum];
            var alphabetLetterGo = Instantiate(_allLetters.First(x => x.AlphabetLetter == otherLetter), new Vector3(100, 100), Quaternion.identity, transform);
            alphabetLetterGo.Initialize(_spawnArea, transform.position + _spawnAreaMargin, _lettersColors[Random.Range(0, _lettersColors.Count-1)]);
        }
        
        LoadWord();
    }

    private void LoadWord()
    {
        for (int i = 0; i < _letterBoxes.Count; i++)
        {
            var letterToInsert = CurrentWord.Length <= i ? ' ' : CurrentWord[i];
            _letterBoxes[i].Initialize(letterToInsert);
        }
    }
}
