using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VNyanInterface;

namespace VNyanMonitorPlus
{
    public class LZDictKeyItem : MonoBehaviour
    {
        [SerializeField] private TMP_InputField KeyField;
        [SerializeField] private Button KeyButton;
        [SerializeField] private TMP_InputField KeyValueField;

        void Start()
        {
            if (!Application.isEditor)
            {
                changeThemeSettings();
                VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings; // Re-init colors when this event fires
            }
        }

        public void changeThemeSettings()
        {
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

            ColorBlock cb = KeyButton.colors;
            KeyButton.GetComponent<Image>().color = ButtonColor;
            cb.normalColor = ButtonColor;
            cb.highlightedColor = ButtonColorHighlight;
            cb.selectedColor = ButtonColor;

            KeyButton.GetComponentInChildren<TMP_Text>().color = ButtonTextColor;

            ColorBlock textValueField = KeyField.colors;
            KeyField.GetComponent<Image>().color = ComponentColor;
            textValueField.normalColor = ComponentColor;
            textValueField.highlightedColor = ComponentHighlight;
            textValueField.selectedColor = ComponentColor;
            KeyField.textComponent.color = TextColor;
            KeyField.placeholder.color = PanelComponentTextColorTransparent;

            ColorBlock KeyValueFieldColrs = KeyValueField.colors;
            KeyValueFieldColrs.normalColor = ComponentColor;
            KeyValueFieldColrs.highlightedColor = ComponentHighlight;
            KeyValueFieldColrs.selectedColor = ComponentColor;
            KeyValueField.textComponent.color = TextColor;
            KeyValueField.placeholder.color = PanelComponentTextColorTransparent;

        }
    }
}
