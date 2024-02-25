using System.Linq;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[assembly: MelonInfo(typeof(SheepRecolor.SheepRecolor), "Sheep Recolor", "1.0.0", "Jonyboylovespie")]
[assembly: MelonGame("HawkesByte", "Flocking Frenzy")]

namespace SheepRecolor
{
    public class SheepRecolor : MelonMod
    {
        private static GameObject SelectColorButton;
        private GameObject panelObject;
        private Color selectedColorWhite;
        private Color selectedColorBlack;
        private Slider redSliderWhite;
        private Slider greenSliderWhite;
        private Slider blueSliderWhite;
        private Slider redSliderBlack;
        private Slider greenSliderBlack;
        private Slider blueSliderBlack;
        private Image colorDisplayImageWhite;
        private Image colorDisplayImageBlack;

        private GameObject CreatePanelWhite(GameObject parent)
        {
            GameObject newPanel = new GameObject("ColorSelectorPanelWhite");
            RectTransform rectTransform = newPanel.AddComponent<RectTransform>();
            rectTransform.SetParent(parent.transform);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            colorDisplayImageWhite = newPanel.AddComponent<Image>();
            colorDisplayImageWhite.rectTransform.sizeDelta = new Vector2(80, 80);
            colorDisplayImageWhite.color = Color.white;
            rectTransform.localPosition = new Vector3(-10, 100);
            redSliderWhite = CreateSlider(newPanel, "Red", Color.red);
            redSliderWhite.gameObject.transform.localPosition = new Vector3(50, -150);
            greenSliderWhite = CreateSlider(newPanel, "Green", Color.green);
            greenSliderWhite.gameObject.transform.localPosition = new Vector3(50, -200);
            blueSliderWhite = CreateSlider(newPanel, "Blue", Color.blue);
            blueSliderWhite.gameObject.transform.localPosition = new Vector3(50, -250);
            return newPanel;
        }
        
        private GameObject CreatePanelBlack(GameObject parent)
        {
            GameObject newPanel = new GameObject("ColorSelectorPanelBlack");
            RectTransform rectTransform = newPanel.AddComponent<RectTransform>();
            rectTransform.SetParent(parent.transform);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            colorDisplayImageBlack = newPanel.AddComponent<Image>();
            colorDisplayImageBlack.rectTransform.sizeDelta = new Vector2(80, 80);
            colorDisplayImageBlack.color = Color.white;
            rectTransform.localPosition = new Vector3(-10, 0);
            redSliderBlack = CreateSlider(newPanel, "Red", Color.red);
            redSliderBlack.gameObject.transform.localPosition = new Vector3(50, -150);
            greenSliderBlack = CreateSlider(newPanel, "Green", Color.green);
            greenSliderBlack.gameObject.transform.localPosition = new Vector3(50, -200);
            blueSliderBlack = CreateSlider(newPanel, "Blue", Color.blue);
            blueSliderBlack.gameObject.transform.localPosition = new Vector3(50, -250);
            return newPanel;
        }

        private Slider CreateSlider(GameObject parent, string name, Color color)
        {
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            GameObject Canvas = null;
            foreach (var gameObject in allGameObjects)
            {
                if (gameObject.name == "Canvas")
                {
                    Canvas = gameObject;
                }
            }
            GameObject sliderObject = Object.Instantiate(Canvas.transform.Find("Settings/Image/Settings/Audio/SFX Slider/Slider").gameObject, parent.transform);
            sliderObject.name = name + "Slider";
            Slider slider = sliderObject.GetComponent<Slider>();
            slider.colors = new ColorBlock { normalColor = color, highlightedColor = color, pressedColor = color, selectedColor = color, disabledColor = color };
            if (parent.name == "ColorSelectorPanelWhite")
            {
                slider.onValueChanged.AddListener(_ => UpdateColorWhite());
            }
            if (parent.name == "ColorSelectorPanelBlack")
            {
                slider.onValueChanged.AddListener(_ => UpdateColorBlack());
            }
            return slider;
        }

        private void UpdateColorWhite()
        {
            selectedColorWhite = new Color(redSliderWhite.value, greenSliderWhite.value, blueSliderWhite.value);
            colorDisplayImageWhite.color = selectedColorWhite;
        }
        
        private void UpdateColorBlack()
        {
            selectedColorBlack = new Color(redSliderBlack.value, greenSliderBlack.value, blueSliderBlack.value);
            colorDisplayImageBlack.color = selectedColorBlack;
        }
        
        public override void OnUpdate()
        {
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (var gameObject in allGameObjects)
            {
                if (gameObject.name == "Sheep")
                {
                    ChangeChildColor(gameObject.transform);
                }
                if (gameObject.name == "Sheep(Clone)")
                {
                    ChangeChildColor(gameObject.transform);
                }
            }

            if (GameObject.Find("Canvas/Main Menu/MenuButtons"))
            {
                if (SelectColorButton == null)
                {
                    GameObject.Find("Canvas/Main Menu").GetComponent<RectTransform>().sizeDelta = new Vector2(370, 450);
                    SelectColorButton = Object.Instantiate(GameObject.Find("Canvas/Main Menu/MenuButtons/Credits"), GameObject.Find("Canvas/Main Menu/MenuButtons").transform, false);
                    SelectColorButton.GetComponent<Button>().onClick.AddListener(OnClick);
                    SelectColorButton.name = "SetColor";
                    SelectColorButton.GetComponent<Image>().color = new Color(0, 0.502f, 0.502f);
                    SelectColorButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Set Color");
                    GameObject.Find("Canvas/Main Menu/MenuButtons/Credits").GetComponent<Button>().onClick.AddListener(ReverseOnClick);
                }
            }
        }

        public void ReverseOnClick()
        {
            GameObject.Find("Canvas/Credits/Title/Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Credits";
            GameObject.Find("Canvas/Credits/Text (TMP)").SetActive(true);
            GameObject.Find("Canvas/Credits/Title").GetComponent<Image>().color = new Color(0.9608f, 0.6118f, 0.3961f);
            foreach (var i in Enumerable.Range(0, GameObject.Find("Canvas/Credits").transform.childCount).ToList())
            {
                var child = GameObject.Find("Canvas/Credits").transform.GetChild(i);
                if (child.name is "ColorSelectorPanelWhite" or "ColorSelectorPanelBlack")
                {
                    MelonLogger.Msg("destroy");
                    Object.Destroy(child.gameObject);
                }
            }

        }
        public void OnClick()
        {
            GameObject.Find("Canvas/Credits/Title/Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Set Color";
            GameObject.Find("Canvas/Credits/Text (TMP)").SetActive(false);
            CreatePanelWhite(GameObject.Find("Canvas/Credits"));
            CreatePanelBlack(GameObject.Find("Canvas/Credits"));
            GameObject.Find("Canvas/Credits/Title").GetComponent<Image>().color = new Color(0, 0.502f, 0.502f);

        }
        
        public void ChangeChildColor(Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (child.GetComponent<MeshRenderer>() != null)
                {
                    if (child.GetComponent<MeshRenderer>().material.color == Color.white)
                    {
                        child.GetComponent<MeshRenderer>().material.color = selectedColorWhite;
                    }
                    if (child.GetComponent<MeshRenderer>().material.name == "Black (Instance)")
                    {
                        child.GetComponent<MeshRenderer>().material.color = selectedColorBlack;
                    }
                }

                ChangeChildColor(child);
            }
        }
    }
}
