using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VNyanInterface;

namespace VNyanMonitorPlus
{
    class LZ_ParamValue : MonoBehaviour
    {
        public Text parameterValueText;

        void Start()
        {
            changeThemeSettings();
            VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings; // Re-init colors when this event fires
        }

        void Update()
        {
            //parameterValueText.text = ParamGraphMonitor.ParamMonitor.getParamValue().ToString("0.000");
        }

        public void changeThemeSettings()
        {
            parameterValueText.GetComponent<Text>().color = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));
        }
    }
}
