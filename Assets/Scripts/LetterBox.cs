using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[RequireComponent(typeof(Image))]
public class LetterBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsHover;
    public char Letter;
    [SerializeField] private Sprite _emptyCell;
    [SerializeField] private Sprite _filledCell;
    [SerializeField] private Color _emptyColor;
    [SerializeField] private Color _filledColor;
    private TextMeshProUGUI _text;
    private Image _image;
    public LetterBoxState LetterBoxState = LetterBoxState.None;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponent<Image>();
        _image.sprite = _emptyCell;
        _text.color = _emptyColor;
    }

    public void Initialize(char letter)
    {
        Letter = letter;
        _image.sprite = _emptyCell;
        _text.color = _emptyColor;
        _text.text = letter.ToString();
        LetterBoxState = letter == ' ' ? LetterBoxState.None : LetterBoxState.Empty;
    }
    public void FillCell()
    {
        _image.sprite = _filledCell;
        _text.color = _filledColor;
        LetterBoxState = LetterBoxState.Filled;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHover = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(DelayHover());
    }

    private IEnumerator DelayHover()
    {
        yield return new WaitForSeconds(0.2f);
        IsHover = false;
    }
}
