using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameState : ModifiedSingleton<GameState>
{
    
    [SerializeField] private UnityEvent _onWordCollected;
    public State State;
    private LetterBox[] _letterBoxes;

    private void Start()
    {
        _letterBoxes = FindObjectsOfType<LetterBox>();
    }

    public void CheckWordOnCollection()
    {
        if (_letterBoxes.All(x => x.LetterBoxState != LetterBoxState.Empty))
        {
            _onWordCollected?.Invoke();
        }
    }
}
