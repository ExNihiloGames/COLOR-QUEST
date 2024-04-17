using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorDebugger : MonoBehaviour
{
    private void OnEnable()
    {
        LevelSelectorUI.onLevelLoadRequest += OnLevelLoad;
    }

    private void OnDisable()
    {
        LevelSelectorUI.onLevelLoadRequest -= OnLevelLoad;
    }

    private void OnLevelLoad(LevelData levelData)
    {
        Debug.Log($"Trying to load : {levelData.fullName}");
    }
}
