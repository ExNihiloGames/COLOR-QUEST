using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelDataGroup", menuName = "ScriptableObjects/LevelData/LevelDataGroup", order = 1)]
public class LevelDataGroup : ScriptableObject
{
    public string displayName => m_displayName;
    [SerializeField] string m_displayName;
    public List<LevelData> levelDatas => m_levelDatas;
    [SerializeField] private List<LevelData> m_levelDatas;
}
