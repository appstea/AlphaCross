using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class MoveControl : MonoBehaviour
{
    [SerializeField] private UnityEvent _onWordCollected;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private float _distanceAboveFloor = 1;
    [SerializeField] private List<LetterBox> _letterBoxes;
    private PointerEventData _pointerEventData;
    private Camera _camera;
    private Vector3 _cameraFloorPosition;
    private float _trianglesDelta;
    private Transform _attachedLetter;
    private Letter _letter;

    private void Start()
    {
        _camera = Camera.main;
        _cameraFloorPosition = _camera.transform.position - new Vector3(0,_camera.transform.position.y);
        _trianglesDelta = _distanceAboveFloor / _camera.transform.position.y;
    }

    private void Update()
    {
        if (GameState.Instance.State != State.InGame)
        {
            return;
        }

        MoveLetter();
    }

    private void MoveLetter()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseButtonDown();
        }
        else if (Input.GetMouseButtonUp(0) && _attachedLetter != null)
        {
            OnMouseButtonUp();
        }
        else if (Input.GetMouseButton(0) && _attachedLetter != null)
        {
            OnMouseButton();
        }
    }

    private void OnMouseButtonDown()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100, 1<<0)) 
        {
            if (hit.rigidbody.TryGetComponent(out Letter letter))
            {
                hit.rigidbody.isKinematic = true;
                _attachedLetter = letter.transform;
                _letter = letter;
            }
        }
    }

    private void OnMouseButtonUp()
    {
        LetterBox letterBox = GetLetterBox();
        if (letterBox != null && letterBox.Letter == _letter.AlphabetLetter && letterBox.LetterBoxState!= LetterBoxState.Filled)
        {
            letterBox.FillCell();
            Destroy(_attachedLetter.gameObject);
            _attachedLetter = null;
            //Проверка на полностью составленное слово
            if (_letterBoxes.All(x => x.LetterBoxState != LetterBoxState.Empty))
            {
                _onWordCollected?.Invoke();
            }
        } else if (letterBox != null)
        {
            var letterRig = _attachedLetter.GetComponent<Rigidbody>();
            letterRig.isKinematic = false;
            letterRig.AddForce(Vector3.forward*2, ForceMode.Impulse);
            _attachedLetter = null;
        }
        else
        {
            _attachedLetter.GetComponent<Rigidbody>().isKinematic = false;
            _attachedLetter = null;
        }
    }

    private void OnMouseButton()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 30, 1<<8))
        {
            //Position
            var deltaLength = Vector3.Lerp(_cameraFloorPosition, hit.point, 1 - _trianglesDelta);
            _attachedLetter.position = Vector3.Lerp(_attachedLetter.position, deltaLength + new Vector3(0, _distanceAboveFloor, 0) - _letter.CenterPoint, Time.deltaTime*7);
                
            //Rotation
            _attachedLetter.rotation = Quaternion.Lerp(_attachedLetter.rotation, Quaternion.Euler(-90,180,0), Time.deltaTime * 5);
        }
    }

    private LetterBox GetLetterBox()
    {
        _pointerEventData = new PointerEventData(_eventSystem) {position = Input.mousePosition};
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        _raycaster.Raycast(_pointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            if(result.gameObject.TryGetComponent(out LetterBox lb))
            {
                return lb;
            }
        }

        return null;
    }
}
