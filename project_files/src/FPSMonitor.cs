using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VNyanInterface;
using VNyanMonitorPlus;

namespace VNyanMonitorPlus
{
    public class FPSMonitor : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [Header("Window Components")]
        [Tooltip("Object for background and outline")]
        [SerializeField] private GameObject Background;
        [Tooltip("Text for title")]
        [SerializeField] private TMP_Text Title;
        [Tooltip("Top right close button")]
        [SerializeField] private Button closeButton;

        [Header("Plugin Window")]
        [Tooltip("Prefab for UI window.")]
        [SerializeField] private GameObject windowPrefab;

        private RectTransform mainRect;
        [SerializeField] private GameObject GraphArea;
        private RectTransform GraphAreaRect;

        [Header("FPS Display")]
        private float averageFPS = 0f;
        [SerializeField] private float RefreshFrequency = 0.4f;
        private float TimeSinceUpdate = 0f;
        [SerializeField] private float SmoothingFactor = 0.9f;
        [SerializeField] private TMP_Text FPSText;

        [Header("FPS Graph")]
        [SerializeField] private GameObject DataContainer;
        [SerializeField] private GameObject axis;
        [SerializeField] private GameObject DataPointPrefab;
        [SerializeField] private int DataSize = 80;
        private float spaceBetweenPoint;
        private float axisHeightLimits = 100f;
        [SerializeField] private float MaxFPS = 120f;
        private float[]? DataArray;

        public void AddNewPoint(Vector3 position, int i)
        {
            GameObject datum = Instantiate<GameObject>(DataPointPrefab);
            datum.name = "point " + i;
            datum.transform.position = position;
            datum.transform.SetParent(DataContainer.transform);
        }

        void Start()
        {
            // To transform the window we need to get the transform component of the type RectTransform!
            mainRect = GetComponent(typeof(RectTransform)) as RectTransform;
            GraphAreaRect = GraphArea.GetComponent(typeof(RectTransform)) as RectTransform;

            // calculate space between points based on graph area size
            spaceBetweenPoint = GraphAreaRect.rect.width / (float)DataSize - 0.04f;
            axisHeightLimits = GraphAreaRect.rect.height;

            DataArray = new float[DataSize];
            for (int i = 0; i < DataSize; i++)
            {
                Vector3 pointPosition = new Vector3(1f * i, 0f, 0f);
                AddNewPoint(pointPosition, i);
            }

            // Link Close button
            closeButton.onClick.AddListener(delegate { CloseButtonClicked(); });

            if (!Application.isEditor)
            {
                changeThemeSettings();
                VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings; // Re-init colors when this event fires
            }
        }

        void Update()
        {
            // Exponentially weighted moving average (EWMA)
            averageFPS = SmoothingFactor * averageFPS + (1f - SmoothingFactor) * 1f / Time.unscaledDeltaTime;

            if (TimeSinceUpdate < RefreshFrequency)
            {
                TimeSinceUpdate += Time.deltaTime;
                return;
            }

            // reset time since update
            TimeSinceUpdate = 0f;

            // Get position of axis for reference
            float xAxisPosition = axis.transform.position.x;
            float yAxisPosition = axis.transform.position.y - axisHeightLimits / 2;

            // Process data value
            float yPosition = averageFPS / MaxFPS * axisHeightLimits;

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

                Vector3 new_position = new Vector3(spaceBetweenPoint * j + xAxisPosition + 2, data_point + yAxisPosition, 0);

                DataContainer.transform.GetChild(j).transform.position = new_position;
            }
            FPSText.text = averageFPS.ToString("0.0");
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
            // Set UI Colors from VNyan
            Background.GetComponent<Image>().color = Panel;
            Background.GetComponent<Outline>().effectColor = Borders;
            closeButton.GetComponent<Image>().color = Panel;
            Title.color = TextColor;
            closeButton.GetComponent<Outline>().effectColor = Borders;
            closeButton.GetComponentInChildren<TMP_Text>().color = TextColor;
            FPSText.GetComponent<TMP_Text>().color = TextColor;
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
    }
}
