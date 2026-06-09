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
    public class DictMonitor : MonoBehaviour, IDragHandler, IPointerDownHandler
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

        [Header("Dictionary readout")]
        private string currentDictionaryName = "";

        
        

        [Header("Controls")]
        [SerializeField] private TMP_InputField ParamNameField;
        [SerializeField] private Button ParamNameButton;

        private string currentKey1 = "";
        [SerializeField] private TMP_InputField currentKey1Value;
        [SerializeField] private TMP_InputField Key1Field;
        [SerializeField] private Button Key1Button;

        private string currentKey2 = "";
        [SerializeField] private TMP_InputField currentKey2Value;
        [SerializeField] private TMP_InputField Key2Field;
        [SerializeField] private Button Key2Button;

        private string currentKey3 = "";
        [SerializeField] private TMP_InputField currentKey3Value;
        [SerializeField] private TMP_InputField Key3Field;
        [SerializeField] private Button Key3Button;

        private string currentKey4 = "";
        [SerializeField] private TMP_InputField currentKey4Value;
        [SerializeField] private TMP_InputField Key4Field;
        [SerializeField] private Button Key4Button;

        private string currentKey5 = "";
        [SerializeField] private TMP_InputField currentKey5Value;
        [SerializeField] private TMP_InputField Key5Field;
        [SerializeField] private Button Key5Button;

        public void setParamName(string name) => currentDictionaryName = name;
        public void setKey1(string key) => currentKey1 = key;
        public void setKey2(string key) => currentKey2 = key;
        public void setKey3(string key) => currentKey3 = key;
        public void setKey4(string key) => currentKey4 = key;
        public void setKey5(string key) => currentKey5 = key;

        public string getVNyanDictionaryValue(string keyName)
        {
            if (!string.IsNullOrEmpty(currentDictionaryName))
            {
                return VNyanInterface.VNyanInterface.VNyanParameter.getVNyanDictionaryValue(currentDictionaryName, keyName);
            }
            return "";
        }

        void Start()
        {
            mainRect = GetComponent(typeof(RectTransform)) as RectTransform;

            // Link Close button
            closeButton.onClick.AddListener(delegate { CloseButtonClicked(); });

            ParamNameButton.onClick.AddListener(delegate { ParamNameButtonPressCheck(); });
            Key1Button.onClick.AddListener(delegate { Key1ButtonPressCheck(); });
            Key2Button.onClick.AddListener(delegate { Key2ButtonPressCheck(); });
            Key3Button.onClick.AddListener(delegate { Key3ButtonPressCheck(); });
            Key4Button.onClick.AddListener(delegate { Key4ButtonPressCheck(); });
            Key5Button.onClick.AddListener(delegate { Key5ButtonPressCheck(); });

            if (!Application.isEditor)
            {
                changeThemeSettings();
                VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings; // Re-init colors when this event fires
            }
        }

        void Update()
        {
            currentKey1Value.text = getVNyanDictionaryValue(currentKey1);
            currentKey2Value.text = getVNyanDictionaryValue(currentKey2);
            currentKey3Value.text = getVNyanDictionaryValue(currentKey3);
            currentKey4Value.text = getVNyanDictionaryValue(currentKey4);
            currentKey5Value.text = getVNyanDictionaryValue(currentKey5);
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

        void Key1ButtonPressCheck()
        {
            setKey1(Key1Field.text);
        }
        void Key2ButtonPressCheck()
        {
            setKey2(Key2Field.text);
        }
        void Key3ButtonPressCheck()
        {
            setKey3(Key3Field.text);
        }
        void Key4ButtonPressCheck()
        {
            setKey4(Key4Field.text);
        }
        void Key5ButtonPressCheck()
        {
            setKey5(Key5Field.text);
        }
    }
}
