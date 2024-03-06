using System;
using System.Collections;
using UnityEngine;

public class MainMenu : MenuBase
{
    public GameObject leanTweenObj;
    private StartMenuTween startMenuTween;

    void Start()
    {
        startMenuTween = leanTweenObj.GetComponent<StartMenuTween>();
    }

    public void StartGame()
    {
        startMenuTween.StartGameButton();
        StartCoroutine(LaunchGame(2.5f));
    }

    public void ShowCredits()
    {
        onCredits?.Invoke();
    }

    public void ExitGame()
    {
        onExitGame?.Invoke();
    }

    IEnumerator LaunchGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onLaunchTutorial?.Invoke();
    }
}
