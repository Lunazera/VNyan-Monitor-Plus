using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VNyanInterface;

namespace VNyanMonitorPlus
{
    public class BlendshapeGraph : MonoBehaviour, IDragHandler, IPointerDownHandler
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

        [SerializeField] private GameObject GraphArea;
        private RectTransform GraphAreaRect;

        [Header("Graph Display")]
        [SerializeField] private GameObject DataContainer;
        [SerializeField] private GameObject axis;
        [SerializeField] private GameObject DataPointPrefab;
        [SerializeField] int DataSize = 300;
        private static float maxValue = 100;
        private float spaceBetweenPoint;
        private float axisHeightLimits = 100;
        [SerializeField] public TMP_Text parameterValueText;
        [SerializeField] private TMP_Text axis0;
        [SerializeField] private TMP_Text axis100;

        [Header("Controls")]
        [SerializeField] private TMP_Text axisMax;
        [SerializeField] private TMP_InputField ParamNameField;
        [SerializeField] private Button ParamNameButton;


        private float[]? DataArray;

        private string currentParameterName = "";

        public void setParamName(string name) => currentParameterName = name;

        public float getParamValue()
        {
            if (!string.IsNullOrEmpty(currentParameterName))
            {
                return VNyanInterface.VNyanInterface.VNyanAvatar.getBlendshapeInstant(currentParameterName) * 100;
            }
            return 0f;
        }
        
        public void AddNewPoint(Vector3 position, int i)
        {
            GameObject datum = Instantiate<GameObject>(DataPointPrefab);
            datum.name = "point " + i;
            datum.transform.position = position;
            datum.transform.SetParent(DataContainer.transform);
        }

        public static void SetAxisScale(float scale)
        {
            maxValue = scale;
        }
        public void setAxisLabels(float value)
        {
            axisMax.text = value.ToString();
        }

        void Start()
        {
            // To transform the window we need to get the transform component of the type RectTransform!
            mainRect = GetComponent(typeof(RectTransform)) as RectTransform;
            GraphAreaRect = GraphArea.GetComponent(typeof(RectTransform)) as RectTransform;

            // calculate space between points based on graph area size
            spaceBetweenPoint = GraphAreaRect.rect.width / (float)DataSize - 0.02f;
            axisHeightLimits = GraphAreaRect.rect.height;

            // Link Close button
            closeButton.onClick.AddListener(delegate { CloseButtonClicked(); });

            ParamNameButton.onClick.AddListener(delegate { ParamNameButtonPressCheck(); });

            DataArray = new float[DataSize];

            for (int i = 0; i < DataSize; i++)
            {
                Vector3 pointPosition = new Vector3(1f * i, 0f, 0f);
                AddNewPoint(pointPosition, i);
            }

            // Theme applies if we aren't in editor
            if (!Application.isEditor)
            {
                changeThemeSettings();
                VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings; // Re-init colors when this event fires
            }
        }
        void Update()
        {
            float paramVal = getParamValue();

            // Get position of axis for reference
            spaceBetweenPoint = GraphAreaRect.rect.width / (float)DataSize - 0.02f;
            axisHeightLimits = GraphAreaRect.rect.height;

            float xAxisPosition = axis.transform.position.x;
            float yAxisPosition = axis.transform.position.y - axisHeightLimits/2;

            // Change text value
            parameterValueText.text = paramVal.ToString("0.0");

            // Process data value
            float yPosition = paramVal / maxValue * axisHeightLimits;
            
            // Shift all values in the data array
            for (int i = 0; i < DataSize - 1; i++)
            {
                DataArray[i] = DataArray[i + 1];
            }

            // Add in the new data point
            DataArray[DataSize - 1] = yPosition;

            // Set objects according to array
            for (int j = 0; j < DataArray.Length; j++)
            {
                float data_point = DataArray[j];

                // constrain data to be in the graph area
                if (data_point > axisHeightLimits)
                {
                    data_point = axisHeightLimits;
                }
                else if (data_point < 0)
                {
                    data_point = 0;
                }

                Vector3 new_position = new Vector3(spaceBetweenPoint * j + xAxisPosition + 5, data_point + yAxisPosition, 0);

                DataContainer.transform.GetChild(j).transform.position = new_position;
            }
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

            axis0.color = PanelComponentTextColor;
            axis100.color = PanelComponentTextColor;

            // Text for parameter display
            parameterValueText.GetComponent<TMP_Text>().color = TextColor;

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
