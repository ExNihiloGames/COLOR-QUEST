using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToMainMenu()
    {
        gameManager.LoadTitleScreen();
    }

    public void ExitGame()
    {
        gameManager.ExitGame();
    }
}
