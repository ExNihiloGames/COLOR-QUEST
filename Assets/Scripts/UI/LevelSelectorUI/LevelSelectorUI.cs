using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] LevelDataGroupCollection m_collection;
    int m_groupIndex;
    int m_levelIndex;
    [SerializeField] LevelDisplayUI m_levelDisplayUI;
    [SerializeField] LevelGroupSelectorUI m_levelGroupSelectorUI;
    [SerializeField] LevelGroupDisplayUI m_levelGroupDisplayUI;

    public static event Action<LevelDataGroupCollection, int, int> onLevelSelected;
    public static event Action<LevelData> onLevelLoadRequest;


    private void OnEnable()
    {
        m_groupIndex = 0;
        m_levelIndex = 0;
        LevelGroupSelectorUI.onIncreaseGroup += OnIncreaseGroupIndex;
        LevelGroupDisplayUI.onLevelIndexSelected += OnLevelSelected;
        LevelDisplayUI.onConfirmLoad += OnRequestLoad;
        onLevelSelected?.Invoke(m_collection, m_groupIndex, m_levelIndex);
    }

    private void OnDisable()
    {
        LevelGroupSelectorUI.onIncreaseGroup -= OnIncreaseGroupIndex;
        LevelGroupDisplayUI.onLevelIndexSelected -= OnLevelSelected;
        LevelDisplayUI.onConfirmLoad -= OnRequestLoad;
    }

    private void OnLevelSelected(int index)
    {
        m_levelIndex = index;
        onLevelSelected?.Invoke(m_collection, m_groupIndex, m_levelIndex);
    }

    private void OnIncreaseGroupIndex(bool increase)
    {
        m_groupIndex += increase ? 1 : -1;
        m_levelIndex = 0;
        onLevelSelected?.Invoke(m_collection, m_groupIndex, m_levelIndex);
    }

    private void OnRequestLoad()
    {
        onLevelLoadRequest?.Invoke(m_collection.levelDataGroups[m_groupIndex].levelDatas[m_levelIndex]);
    }
}
