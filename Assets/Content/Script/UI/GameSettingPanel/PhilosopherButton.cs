using System.Collections;
using System.Collections.Generic;
using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhilosopherButton : MonoBehaviour
{
    private static string[] colorHexList = { "8C7B27", "F2C335", "F2B33D", "D97855", "A61B1B" };
    [SerializeField]
    public static List<Color> colorList = new List<Color>();

    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private Image image;
    [SerializeField]
    private CleanButton buttonRef;
    public string categoryName { set => textMesh.text = value; get => textMesh.text; }
    public Color color { set => image.color = value ; get => image.color; }

    public Color RandomColor()
    {
        Color color =  Random.ColorHSV();
        bool isBlack = .222 * color.r + .707 * color.g + .071 * color.b > 128;
        textMesh.color = isBlack ? Color.black : Color.white;
        return color;

    }
    private void Start()
    {
        colorList.Clear();
        foreach (var hex in colorHexList)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#7F7E83FF", out color);
            colorList.Add(color);
        }
        buttonRef.onClick.AddListener(HandleGameStart);
    }

    private void HandleGameStart()
    {
        UIManager.Ins.PushPanel("InGamePanel");
        var inGamePanelRef = UIManager.Ins.GetPanel("InGamePanel") as InGamePanel;
        if (inGamePanelRef)
            inGamePanelRef.OnGameStart(categoryName);
    }
}
