using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VNyanInterface;

namespace VNyanMonitorPlus
{
    public class LZMenuItem : MonoBehaviour
    {
        [SerializeField] private Button MenuButton;
        [SerializeField] private TMP_Text MenuDescription;

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
            Color32 ButtonColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Button));
            Color32 ButtonColorHighlight = LZUIManager.darkenColor(ButtonColor, 10);
            Color32 ButtonColorSelect = LZUIManager.darkenColor(ButtonColor, 30);
            Color32 ButtonTextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.ButtonText));
            Color32 TextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));

            ColorBlock cb = MenuButton.colors;
            MenuButton.GetComponent<Image>().color = ButtonColor;
            cb.normalColor = ButtonColor;
            cb.highlightedColor = ButtonColorHighlight;
            cb.selectedColor = ButtonColorSelect;

            MenuButton.GetComponentInChildren<TMP_Text>().color = ButtonTextColor;

            MenuDescription.color = TextColor;
        }
    }
}
