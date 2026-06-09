using System;
using System.Collections.Generic;
using UnityEngine;
using VNyanInterface;

namespace VNyanMonitorPlus
{
    public class LZUIManager : MonoBehaviour, IButtonClickedHandler
    {
        [Header("Plugin Window")]
        [Tooltip("Prefab for main UI window (should contain LZMainWindow)")]
        [SerializeField] public GameObject windowPrefab;
        [Tooltip("Name to show up in VNyan's plugin menu")]
        [SerializeField] private string PluginMenuName = "VNyan Parameter Graph";



        private GameObject window;

        private static Dictionary<string, string> settings = new Dictionary<string, string>();

        public static Dictionary<string, string> getSettingsDict()
        {
            return settings;
        }

        void Awake()
        {
            if (!Application.isEditor)
            {
                // Register UI button
                VNyanInterface.VNyanInterface.VNyanUI.registerPluginButton(PluginMenuName, (IButtonClickedHandler)this);
                this.window = (GameObject)VNyanInterface.VNyanInterface.VNyanUI.instantiateUIPrefab((object)this.windowPrefab);
            }

            if ((UnityEngine.Object)this.window != (UnityEngine.Object)null)
            {
                this.window.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
                this.window.SetActive(false);
            }
        }

        public void pluginButtonClicked()
        {
            if ((UnityEngine.Object)this.window == (UnityEngine.Object)null)
                return;
            this.window.SetActive(!this.window.activeSelf);
            if (!this.window.activeSelf)
                return;
            this.window.transform.SetAsLastSibling();
        }

        /// <summary>
        /// Method to convert VNyan's theme component hex color to a Color.
        /// </summary>
        /// <param name="hex">#000000 Hex color format</param>
        /// <param name="alpha">Transparency between 0-255</param>
        /// <returns></returns>
        public static Color hexToColor(string hex, byte alpha = 255)
        {
            // conversion from hex to rgb, needed to read from VNyan theme components.
            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF

            byte a = alpha; //assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Takes a color and darkens it by some amount.
        /// </summary>
        /// <param name="color">Color input</param>
        /// <param name="amount">byte value to darken each r/g/b by</param>
        /// <returns></returns>
        public static Color32 darkenColor(Color32 color, byte amount)
        {
            byte r = (byte)Mathf.Max(0, color.r - amount);
            byte g = (byte)Mathf.Max(0, color.g - amount);
            byte b = (byte)Mathf.Max(0, color.b - amount);

            return new Color32(r, g, b, color.a);
        }
    }
}