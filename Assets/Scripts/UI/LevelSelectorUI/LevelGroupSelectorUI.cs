using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelGroupSelectorUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_groupNameText;
    [SerializeField] Button m_lastGroupButton;
    [SerializeField] Button m_nextGroupButton;
    public static event Action<bool> onIncreaseGroup;

    private void OnEnable()
    {
        m_lastGroupButton.onClick.AddListener(() => onIncreaseGroup?.Invoke(false));
        m_nextGroupButton.onClick.AddListener(() => onIncreaseGroup?.Invoke(true));
        LevelSelectorUI.onLevelSelected += Init;
    }

    private void OnDisable()
    {
        m_lastGroupButton.onClick.RemoveAllListeners();
        m_nextGroupButton.onClick.RemoveAllListeners();
        LevelSelectorUI.onLevelSelected -= Init;
    }

    void Init(LevelDataGroupCollection groupCollection, int groupIndex, int levelIndex)
    {
        m_lastGroupButton.interactable = !(groupIndex == 0);
        m_nextGroupButton.interactable = !(groupIndex == groupCollection.levelDataGroups.Count - 1);
        m_groupNameText.text = groupCollection.levelDataGroups[groupIndex].displayName;
    }

}
