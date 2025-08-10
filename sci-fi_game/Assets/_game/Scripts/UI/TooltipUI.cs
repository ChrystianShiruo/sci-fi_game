using Game.Input;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }


    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private CanvasGroup _canvasGroup;
    //[SerializeField] private LayoutElement _layoutElement;
    //[SerializeField] private int _characterWrapLimit = 60;

    private RectTransform _rectTransform;


    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _rectTransform = GetComponent<RectTransform>();

        ShowTooltip(false);
    }
    private void Update() {
        transform.position = InputHandler.Instance.UIActions.Point.ReadValue<Vector2>();
    }
    public void ShowTooltip(bool show, string header = "", string content = "") {
        if(!show) {
            _canvasGroup.alpha = 0;
            return;
        }

        if(string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content)) {
            return;
        }
        _headerText.text = header;
        _contentText.text = content;


        _canvasGroup.alpha = 1;
    }
}
