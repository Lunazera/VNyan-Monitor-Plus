using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VNyanInterface;

namespace VNyanMonitorPlus
{
    public class TextMonitor : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [Header("Window Components")]
        [Tooltip("Object for background and outline")]
        [SerializeField] private GameObject Background;
        [Tooltip("Text for plugin title")]
        [SerializeField] private TMP_Text Title;
        [Tooltip("Top right close button")]
        [SerializeField] private Button closeButton;

        [Header("Plugin Window")]
        [Tooltip("Prefab for main UI window.")]
        [SerializeField] private GameObject windowPrefab;

        private RectTransform mainRect;

        [Header("Text Display")]
        private string currentParameterName = "";
        [SerializeField] private TMP_InputField parameterValueText;

        [Header("Controls")]
        [SerializeField] private TMP_InputField ParamNameField;
        [SerializeField] private Button ParamNameButton;

        public void setParamName(string name) => currentParameterName = name;
        public string getParamString()
        {
            if (!string.IsNullOrEmpty(currentParameterName))
            {
                return VNyanInterface.VNyanInterface.VNyanParameter.getVNyanParameterString(currentParameterName);
            }
            return "";
        }

        void Start()
        {
            mainRect = GetComponent(typeof(RectTransform)) as RectTransform;

            // Link Close button
            closeButton.onClick.AddListener(delegate { CloseButtonClicked(); });

            ParamNameButton.onClick.AddListener(delegate { ParamNameButtonPressCheck(); });

            if (!Application.isEditor)
            {
                changeThemeSettings();
                VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings; // Re-init colors when this event fires
            }
        }

        void Update()
        {
            parameterValueText.text = getParamString();
        }

        public void changeThemeSettings()
        {
            // Set common colours to pull from
            Color32 TextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));
            Color32 Panel = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Panel));
            Color32 Borders = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Borders));

            Color32 ComponentColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Component));
            Color32 ComponentHighlight = LZUIManager.darkenColor(ComponentColor, 10);

            Color32 ButtonColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Button));
            Color32 ButtonColorHighlight = LZUIManager.darkenColor(ButtonColor, 10);

            Color32 ButtonTextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.ButtonText));

            Color32 PanelComponentTextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.PanelComponentText));
            Color32 PanelComponentTextColorTransparent = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.PanelComponentText), 70);

            // Set UI Colors from VNyan
            Background.GetComponent<Image>().color = Panel;
            Background.GetComponent<Outline>().effectColor = Borders;
            Title.color = TextColor;
            closeButton.GetComponent<Image>().color = Panel;
            closeButton.GetComponent<Outline>().effectColor = Borders;
            closeButton.GetComponentInChildren<TMP_Text>().color = TextColor;

            ColorBlock textValueField = parameterValueText.colors;
            //parameterValueText.GetComponent<Image>().color = ComponentColor;
            textValueField.normalColor = ComponentColor;
            textValueField.highlightedColor = ComponentHighlight;
            textValueField.selectedColor = ComponentColor;
            parameterValueText.textComponent.color = TextColor;
            ParamNameField.placeholder.color = PanelComponentTextColorTransparent;

            // Set Param Name input colours
            // Input field colors
            ColorBlock cbNameField = ParamNameField.colors;
            ParamNameField.GetComponent<Image>().color = ComponentColor;
            cbNameField.normalColor = ComponentColor;
            cbNameField.highlightedColor = ComponentHighlight;
            cbNameField.selectedColor = ComponentColor;

            ParamNameField.textComponent.color = TextColor;
            ParamNameField.placeholder.color = PanelComponentTextColorTransparent;

            // Button colors
            ColorBlock cbNameButton = ParamNameButton.colors;
            ParamNameButton.GetComponent<Image>().color = ButtonColor;
            cbNameButton.normalColor = ButtonColor;
            cbNameButton.highlightedColor = ButtonColorHighlight;
            cbNameButton.selectedColor = ButtonColor;

            ParamNameButton.GetComponentInChildren<TMP_Text>().color = ButtonTextColor;
        }

        public void OnDrag(PointerEventData eventData)
        {
            mainRect.anchoredPosition += eventData.delta;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            mainRect.SetAsLastSibling();
        }
        public void CloseButtonClicked()
        {
            this.windowPrefab.SetActive(false);
        }
        void ParamNameButtonPressCheck()
        {
            setParamName(ParamNameField.text);
        }
    }
}
