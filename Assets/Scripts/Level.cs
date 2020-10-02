using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Уровни/Создать новый уровень", order = 10)]
public class Level : ScriptableObject
{
    [ReorderableList] public List<string> Words = new List<string>(4);
    [Label("Дубликатов основных букв"), Range(1,20)]
    public int MainLettersAmount = 2;
    [Label("Остальных букв на поле")]
    [Range(0, 500)] public int OtherLettersAmount = 60;
}