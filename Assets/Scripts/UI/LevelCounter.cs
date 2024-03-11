using UnityEngine;
using TMPro;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text textDisplay;

    // Start is called before the first frame update
    void Start()
    {
        string[] rawTxt = GameManager.CurrentScene.ToString().Split("_");
        textDisplay.text = rawTxt[1];
    }
}
