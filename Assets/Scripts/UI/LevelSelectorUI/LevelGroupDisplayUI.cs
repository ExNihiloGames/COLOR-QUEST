using System;
using UnityEngine;

public class LevelGroupDisplayUI : MonoBehaviour
{
    [SerializeField] MinimizedLevelDisplayUI m_levelListElementPrefab;
    [SerializeField] Transform m_levelListParent;
    MinimizedLevelDisplayUI[] m_levelListElements;
    public static Action<int> onLevelIndexSelected;

    private void OnEnable()
    {
        LevelSelectorUI.onLevelSelected += Init;
        MinimizedLevelDisplayUI.onLevelDisplaySelected += OnLevelClicked;
    }

    private void OnDisable()
    {
        LevelSelectorUI.onLevelSelected -= Init;
        MinimizedLevelDisplayUI.onLevelDisplaySelected -= OnLevelClicked;
    }

    private void OnLevelClicked(int index)
    {
        onLevelIndexSelected?.Invoke(index);
    }

    void Init(LevelDataGroupCollection collection, int groupIndex, int levelIndex)
    {
        Clear();
        m_levelListElements = new MinimizedLevelDisplayUI[collection.levelDataGroups[groupIndex].levelDatas.Count];
        //Instanciate a MinimizedLevelDisplayUI for each LevelData in the LevelDataGroup
        for (int i = 0; i < m_levelListElements.Length; i++)
        {
            m_levelListElements[i] = Instantiate(m_levelListElementPrefab, m_levelListParent);
            m_levelListElements[i].Init(collection.levelDataGroups[groupIndex].levelDatas[i].minimizedName, i);
        }
    }

    void Clear()
    {
        for (int i = 0; i < m_levelListParent.childCount; i++)
        {
            Destroy(m_levelListParent.GetChild(i).gameObject);
        }
    }
}
