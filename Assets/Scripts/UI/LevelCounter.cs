using UnityEngine;
using TMPro;
using System.Linq;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text textDisplay;

    // Start is called before the first frame update
    void Start()
    {
        string[] rawTxt = GameManager.CurrentScene.ToString().Split("_");
#if UNITY_EDITOR
        if (rawTxt.Count() == 0) { rawTxt = new string[2] { "Niv", "00"}; }
#endif
        textDisplay.text = rawTxt[1];
    }
}
