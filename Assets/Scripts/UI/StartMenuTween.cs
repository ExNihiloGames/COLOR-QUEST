using UnityEngine;


public class StartMenuTween : MonoBehaviour
{
    [SerializeField] 
    GameObject titleImage, startMenu,startGameButton,optionsButton, creditsButton, backToDesktopButton;
    public GameObject generalTween;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        LeanTween.moveLocal(titleImage, new Vector3(647.5f, 300f, 0f), 0.5f).setDelay(0.5f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(startMenu, new Vector3(-600f, -142.5f, 0f), 0.5f).setDelay(0.75f).setEase(LeanTweenType.easeOutExpo);

        // Slide buttons in
        LeanTween.moveLocal(startGameButton, new Vector3(-470f, 200f, 0f), 0.75f).setDelay(1f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(optionsButton, new Vector3(-600f, 0f, 0f), 0.75f).setDelay(1.25f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(creditsButton, new Vector3(-715f, -200f, 0f), 0.75f).setDelay(1.5f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(backToDesktopButton, new Vector3(-380f, -400f, 0f), 0.75f).setDelay(1.75f).setEase(LeanTweenType.easeOutExpo);

        //Scale buttons in
        //LeanTween.scale(startGameButton, Vector3.one, 0.75f).setDelay(1.25f).setEase(LeanTweenType.easeOutElastic);
        //LeanTween.scale(optionsButton, Vector3.one, 0.75f).setDelay(1.5f).setEase(LeanTweenType.easeOutElastic);
        //LeanTween.scale(creditsButton, Vector3.one, 0.75f).setDelay(1.75f).setEase(LeanTweenType.easeOutElastic);
        //LeanTween.scale(backToDesktopButton, Vector3.one, 0.75f).setDelay(2f).setEase(LeanTweenType.easeOutElastic);
    }

    public void SlideStartMenuIn()
    {
        LeanTween.moveLocal(startMenu, new Vector3(-600f, -142.5f, 0f), 0.5f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(startGameButton, new Vector3(-470f, 200f, 0f), 0.75f).setDelay(0.25f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(optionsButton, new Vector3(-600f, 0f, 0f), 0.75f).setDelay(0.5f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(creditsButton, new Vector3(-715f, -200f, 0f), 0.75f).setDelay(0.75f).setEase(LeanTweenType.easeOutExpo);
        LeanTween.moveLocal(backToDesktopButton, new Vector3(-380f, -400f, 0f), 0.75f).setDelay(1f).setEase(LeanTweenType.easeOutExpo);
    }

    public void SlideStartMenuOut()
    {
        LeanTween.moveLocal(startGameButton, new Vector3(-1300f, 200f, 0f), 0.75f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocal(optionsButton, new Vector3(-1400f, 0, 0f), 0.75f).setDelay(0.25f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocal(creditsButton, new Vector3(-1500f, -200f, 0f), 0.75f).setDelay(0.5f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocal(backToDesktopButton, new Vector3(-1200f, -400f, 0f), 0.75f).setDelay(0.75f).setEase(LeanTweenType.easeInBack);
        LeanTween.moveLocal(startMenu, new Vector3(-1416, -142.5f, 0f), 0.5f).setDelay(1f).setEase(LeanTweenType.easeInBack);
    }

    public void StartGameButton()
    {
        LeanTween.scale(startGameButton, Vector3.one*0.75f,0f);
        LeanTween.scale(startGameButton, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(SlideStartMenuOut);
        LeanTween.moveLocal(titleImage, new Vector3(1272.5f, 300f, 0f), 0.25f).setDelay(1.25f).setEase(LeanTweenType.easeInBack);
    }

    public void OptionsButton()
    {
        LeanTween.scale(optionsButton, Vector3.one * 0.75f, 0f);
        LeanTween.scale(optionsButton, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(SlideStartMenuOut);
    }
}
