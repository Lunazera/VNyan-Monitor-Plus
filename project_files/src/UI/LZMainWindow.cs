using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VNyanInterface;

namespace VNyanMonitorPlus
{
    class LZMainWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [Header("Plugin Manifest")]
        private string PluginVersion = "v1";
        private string PluginTitle = "LZ's Monitor Plus";
        private string PluginAuthor = "Lunazera";
        private string PluginWebsite = "https://git.gay/lunazera/VNyan-Monitor-Plus";

        [Header("Window Components")]
        [Tooltip("Object for background and outline")]
        [SerializeField] private GameObject Background;
        [Tooltip("Text for plugin title")]
        [SerializeField] private TMP_Text Title;
        [Tooltip("Text for version and author credit")]
        [SerializeField] private TMP_Text Version;
        [Tooltip("Free text field 1")]
        [SerializeField] private TMP_Text Desc1;

        [Header("Close Button")]
        [Tooltip("Top right close button")]
        [SerializeField] private Button closeButton;

        [Header("Plugin Window")]
        [Tooltip("Prefab for main UI window.")]
        [SerializeField] private GameObject windowPrefab;

        [Header("Sub Windows")]
        //[SerializeField] public GameObject ARKitPrefab;
        //[SerializeField] public GameObject MiniPrefab;
        [SerializeField] public GameObject ParamGraphPrefab;
        [SerializeField] private Button ParamGraphButton;

        [SerializeField] public GameObject ParamMiniPrefab;
        [SerializeField] private Button ParamMiniButton;

        [SerializeField] public GameObject TextMonitorPrefab;
        [SerializeField] private Button TextMonitorButton;

        [SerializeField] public GameObject ARKitPrefab;
        [SerializeField] private Button ARKitButton;

        [SerializeField] public GameObject DictMonitorPrefab;
        [SerializeField] private Button DictMonitorButton;

        [SerializeField] public GameObject BlendshapeGraphPrefab;
        [SerializeField] private Button BlendshapeGraphButton;


        private RectTransform mainRect;
        private Button VersionURLButton;

        void Start()
        {
            // Set info from manifest
            Title.text = PluginTitle;
            Version.text = PluginVersion + " - " + PluginAuthor;

            // To transform the window we need to get the transform component of the type RectTransform!
            mainRect = GetComponent(typeof(RectTransform)) as RectTransform;

            // Link Close button
            closeButton.onClick.AddListener(delegate { CloseButtonClicked(); });

            VersionURLButton = Version.GetComponent<Button>();
            VersionURLButton.onClick.AddListener(delegate { VersionClicked(); });

            // Link subwindow buttons
            ParamGraphButton.onClick.AddListener(OnParamGraphButtonClicked);
            ParamMiniButton.onClick.AddListener(OnMiniButtonClicked);
            TextMonitorButton.onClick.AddListener(OnTextMonitorButtonClicked);
            ARKitButton.onClick.AddListener(OnARKitButtonClicked);
            DictMonitorButton.onClick.AddListener(OnDictMonitorButtonClicked);
            BlendshapeGraphButton.onClick.AddListener(OnBlendshapeGraphButtonClicked);

            // Theme applies if we aren't in editor
            if (!Application.isEditor)
            {
                changeThemeSettings();
                VNyanInterface.VNyanInterface.VNyanUI.colorThemeChanged += changeThemeSettings; // Re-init colors when this event fires
            }
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
        public void VersionClicked()
        {
            Application.OpenURL(PluginWebsite);
        }
        public void OnParamGraphButtonClicked()
        {
            if ((UnityEngine.Object)ParamGraphPrefab == (UnityEngine.Object)null)
                return;
            ParamGraphPrefab.SetActive(!ParamGraphPrefab.activeSelf);
            if (!ParamGraphPrefab.activeSelf)
                return;
            ParamGraphPrefab.transform.SetAsLastSibling();
        }
        public void OnMiniButtonClicked()
        {
            if ((UnityEngine.Object)ParamMiniPrefab == (UnityEngine.Object)null)
                return;
            ParamMiniPrefab.SetActive(!ParamMiniPrefab.activeSelf);
            if (!ParamMiniPrefab.activeSelf)
                return;
            ParamMiniPrefab.transform.SetAsLastSibling();
        }
        public void OnTextMonitorButtonClicked()
        {
            if ((UnityEngine.Object)TextMonitorPrefab == (UnityEngine.Object)null)
                return;
            TextMonitorPrefab.SetActive(!TextMonitorPrefab.activeSelf);
            if (!TextMonitorPrefab.activeSelf)
                return;
            TextMonitorPrefab.transform.SetAsLastSibling();
        }
        public void OnARKitButtonClicked()
        {
            if ((UnityEngine.Object)ARKitPrefab == (UnityEngine.Object)null)
                return;
            ARKitPrefab.SetActive(!ARKitPrefab.activeSelf);
            if (!ARKitPrefab.activeSelf)
                return;
            ARKitPrefab.transform.SetAsLastSibling();
        }
        public void OnDictMonitorButtonClicked()
        {
            if ((UnityEngine.Object)DictMonitorPrefab == (UnityEngine.Object)null)
                return;
            DictMonitorPrefab.SetActive(!DictMonitorPrefab.activeSelf);
            if (!DictMonitorPrefab.activeSelf)
                return;
            DictMonitorPrefab.transform.SetAsLastSibling();
        }

        public void OnBlendshapeGraphButtonClicked()
        {
            if ((UnityEngine.Object)BlendshapeGraphPrefab == (UnityEngine.Object)null)
                return;
            BlendshapeGraphPrefab.SetActive(!BlendshapeGraphPrefab.activeSelf);
            if (!BlendshapeGraphPrefab.activeSelf)
                return;
            BlendshapeGraphPrefab.transform.SetAsLastSibling();
        }

        /// <summary>
        /// Method to change colours of the UI's visual components 
        /// </summary>
        public void changeThemeSettings()
        {
            // Set UI Colors from VNyan
            Background.GetComponent<Image>().color = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Panel));
            Background.GetComponent<Outline>().effectColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Borders));
            Title.color = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));
            Version.color = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));
            Desc1.color = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));
            closeButton.GetComponent<Image>().color = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Panel));
            closeButton.GetComponent<Outline>().effectColor = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Borders));
            closeButton.GetComponentInChildren<TMP_Text>().color = LZUIManager.hexToColor(VNyanInterface.VNyanInterface.VNyanUI.getCurrentThemeColor(ThemeComponent.Text));
        }
    }
}