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
    public class ARKitMonitor : MonoBehaviour, IDragHandler, IPointerDownHandler
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

        [Header("Blendshape Texts")]
        [SerializeField] private TMP_Text Blendshapes_Col1;
        [SerializeField] private TMP_Text Blendshapes_Col2;
        [SerializeField] private TMP_Text Blendshapes_Col3;
        [SerializeField] private TMP_Text Blendshapes_Col4;

        [SerializeField] private TMP_InputField Blendshapes_Selectable;

        [Header("Controls")]
        [SerializeField] private TMP_InputField ThresholdField;
        [SerializeField] private Button ThresholdButton;
        [SerializeField] private Button PauseButton;

        private StringBuilder stringlargeBlendshapes = new StringBuilder();

        private Color32 Panel;
        private Color32 Borders;
        private Color32 ComponentColor;
        private Color32 ComponentHighlight;
        private Color32 TextColor;
        private Color32 PanelComponentTextColor;
        private Color32 PanelComponentTextColorTransparent;
        private Color32 ButtonColor;
        private Color32 ButtonColorHighlight;
        private Color32 ButtonColor2;
        private Color32 ButtonColorHighlight2;
        private Color32 ButtonTextColor;


        // Dictionary for current snapshot of all blendshapes
        private Dictionary<string, float> CurrentBlendshapes;
        private float threshold = 30;
        private bool pauseFlag = false;

        public float getThreshold() => threshold;
        public float setThreshold(float value) => threshold = value;

        public bool isPaused() => pauseFlag;
        public void togglePause() { pauseFlag = !pauseFlag; }


        void Start()
        {
            mainRect = GetComponent(typeof(RectTransform)) as RectTransform;

            // Link Close button
            closeButton.onClick.AddListener(delegate { CloseButtonClicked(); });

            ThresholdButton.onClick.AddListener(delegate { ThresholdButtonPressCheck(); });
            PauseButton.onClick.AddListener(delegate { PauseButtonPressCheck(); });

            if (!Application.isEditor)
            {
                changeThemeSettings();
                VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings;
                CurrentBlendshapes = VNyanInterface.VNyanInterface.VNyanAvatar.getBlendshapesInstant();
            }
        }

        void Update()
        {
            if (!isPaused())
            {
                CurrentBlendshapes = VNyanInterface.VNyanInterface.VNyanAvatar.getBlendshapesInstant();

                List<StringBuilder> ListOfBlendshape = listBlendshapes();

                Blendshapes_Col1.text = ListOfBlendshape[0].ToString();
                Blendshapes_Col2.text = ListOfBlendshape[1].ToString();
                Blendshapes_Col3.text = ListOfBlendshape[2].ToString();
                Blendshapes_Col4.text = ListOfBlendshape[3].ToString();

                Blendshapes_Selectable.text = ListOfBlendshape[4].ToString();
            }
        }


        private List<StringBuilder> listBlendshapes()
        {
            List<StringBuilder> blendshapeStrings = new List<StringBuilder>();
            blendshapeStrings.Add(new StringBuilder());
            blendshapeStrings.Add(new StringBuilder());
            blendshapeStrings.Add(new StringBuilder());
            blendshapeStrings.Add(new StringBuilder());
            blendshapeStrings.Add(new StringBuilder());

            int blendIndex = 0;
            int blendIndexOffset = 0;

            string[] ARKit_Blendshapes = new string[] { "BrowDownLeft", "BrowDownRight", "BrowInnerUp", "BrowOuterUpLeft", "BrowOuterUpRight", "CheekPuff", "CheekSquintLeft", "CheekSquintRight", "EyeBlinkLeft", "EyeBlinkRight", "EyeLookDownLeft", "EyeLookDownRight", "EyeLookInLeft", "EyeLookInRight", "EyeLookOutLeft", "EyeLookOutRight", "EyeLookUpLeft", "EyeLookUpRight", "EyeSquintLeft", "EyeSquintRight", "EyeWideLeft", "EyeWideRight", "JawForward", "JawLeft", "JawOpen", "JawRight", "MouthClose", "MouthDimpleLeft", "MouthDimpleRight", "MouthFrownLeft", "MouthFrownRight", "MouthFunnel", "MouthLeft", "MouthLowerDownLeft", "MouthLowerDownRight", "MouthPressLeft", "MouthPressRight", "MouthPucker", "MouthRight", "MouthRollLower", "MouthRollUpper", "MouthShrugLower", "MouthShrugUpper", "MouthSmileLeft", "MouthSmileRight", "MouthStretchLeft", "MouthStretchRight", "MouthUpperUpLeft", "MouthUpperUpRight", "NoseSneerLeft", "NoseSneerRight", "TongueOut" };

            foreach (string blendshape in ARKit_Blendshapes)
            {
                if (CurrentBlendshapes.ContainsKey(blendshape))
                {
                    blendIndex++;
                    if (blendIndex <= 26)
                    {
                        blendIndexOffset = 0;
                    }
                    else
                    {
                        blendIndexOffset = 2;
                    }

                    float shapeValue = CurrentBlendshapes[blendshape] * 100;
                    if (shapeValue >= getThreshold())
                    {
                        blendshapeStrings[0 + blendIndexOffset].AppendLine(">" + blendshape + ": ");
                        blendshapeStrings[4].Append(blendshape + ";");
                    }
                    else
                    {
                        blendshapeStrings[0 + blendIndexOffset].AppendLine(blendshape + ": ");
                    }
                    blendshapeStrings[1 + blendIndexOffset].AppendLine(shapeValue.ToString("0.0"));
                }
            }

            return blendshapeStrings;
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

        void PauseButtonPressCheck()
        {
            togglePause();
        }
        void ThresholdButtonPressCheck()
        {
            if (float.TryParse(ThresholdField.text, out float fieldValue))
            {
                setThreshold(fieldValue);
            }
            else
            {
                ThresholdField.text = getThreshold().ToString();
            }
        }

        public void changeThemeSettings()
        {
            // Set common colours to pull from
            TextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));
            Panel = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Panel));
            Borders = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Borders));

            ComponentColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Component));
            ComponentHighlight = LZUIManager.darkenColor(ComponentColor, 10);

            ButtonColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Button));
            ButtonColorHighlight = LZUIManager.darkenColor(ButtonColor, 10);

            ButtonColor2 = LZUIManager.darkenColor(ButtonColor, 40);
            ButtonColorHighlight2 = LZUIManager.darkenColor(ButtonColor, 50);

            ButtonTextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.ButtonText));

            PanelComponentTextColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.PanelComponentText));
            PanelComponentTextColorTransparent = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.PanelComponentText), 70);

            // Set UI Colors from VNyan
            Background.GetComponent<Image>().color = Panel;
            Background.GetComponent<Outline>().effectColor = Borders;
            Title.color = TextColor;
            closeButton.GetComponent<Image>().color = Panel;
            closeButton.GetComponent<Outline>().effectColor = Borders;
            closeButton.GetComponentInChildren<TMP_Text>().color = TextColor;

            // Blendshape list text
            Blendshapes_Col1.color = TextColor;
            Blendshapes_Col2.color = TextColor;
            Blendshapes_Col3.color = TextColor;
            Blendshapes_Col4.color = TextColor;

            // Selectable Field colors
            ColorBlock selectableField = Blendshapes_Selectable.colors;
            Blendshapes_Selectable.GetComponent<Image>().color = ComponentColor;
            selectableField.normalColor = ComponentColor;
            selectableField.highlightedColor = ComponentHighlight;
            selectableField.selectedColor = ComponentColor;
            Blendshapes_Selectable.textComponent.color = TextColor;
            Blendshapes_Selectable.placeholder.color = PanelComponentTextColorTransparent;


            // Set Param Name input colours
            // Input field colors
            ColorBlock cbNameField = ThresholdField.colors;
            ThresholdField.GetComponent<Image>().color = ComponentColor;
            cbNameField.normalColor = ComponentColor;
            cbNameField.highlightedColor = ComponentHighlight;
            cbNameField.selectedColor = ComponentColor;

            ThresholdField.textComponent.color = TextColor;
            ThresholdField.placeholder.color = PanelComponentTextColorTransparent;

            // Button colors
            ColorBlock cbNameButton = ThresholdButton.colors;
            ThresholdButton.GetComponent<Image>().color = ButtonColor;
            cbNameButton.normalColor = ButtonColor;
            cbNameButton.highlightedColor = ButtonColorHighlight;
            cbNameButton.selectedColor = ButtonColor;

            ThresholdButton.GetComponentInChildren<TMP_Text>().color = ButtonTextColor;

        }

        public void ChangeButtonColor(bool boolbuttonState)
        {
            // If the input is true make it green
            if (boolbuttonState)
            {
                ColorBlock cb = PauseButton.colors;
                PauseButton.GetComponent<Image>().color = ButtonColor;
                cb.normalColor = ButtonColor;
                cb.highlightedColor = ButtonColorHighlight;
                cb.pressedColor = ButtonColor;
                cb.selectedColor = ButtonColor;
                PauseButton.colors = cb;

                PauseButton.GetComponentInChildren<TMP_Text>().color = ButtonTextColor;
            }
            // Else make it red! 
            else
            {
                ColorBlock cb = PauseButton.colors;
                PauseButton.GetComponent<Image>().color = ButtonColor2;
                cb.normalColor = ButtonColor2;
                cb.highlightedColor = ButtonColorHighlight2;
                cb.pressedColor = ButtonColor2;
                cb.selectedColor = ButtonColor2;
                PauseButton.colors = cb;

                PauseButton.GetComponentInChildren<TMP_Text>().color = ButtonTextColor;
            }
        }

    }
}
