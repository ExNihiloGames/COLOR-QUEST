using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_levelNameText;
    [SerializeField] Image m_screenShotImage;
    [SerializeField] Button m_loadButton;
    public static event Action onConfirmLoad;

    private void OnEnable()
    {
        LevelSelectorUI.onLevelSelected += DisplayLevelData;
        m_loadButton.onClick.AddListener(ConfirmLoad);
    }

    private void OnDisable()
    {
        LevelSelectorUI.onLevelSelected -= DisplayLevelData;
        m_loadButton.onClick.RemoveAllListeners();
    }

    private void ConfirmLoad()
    {
        onConfirmLoad?.Invoke();
    }

    private void DisplayLevelData(LevelDataGroupCollection collection, int groupIndex, int levelIndex)
    {
        LevelData levelData = collection.levelDataGroups[groupIndex].levelDatas[levelIndex];
        m_levelNameText.text = levelData.fullName;
        m_screenShotImage.sprite = levelData.screenshot;
    }
}
