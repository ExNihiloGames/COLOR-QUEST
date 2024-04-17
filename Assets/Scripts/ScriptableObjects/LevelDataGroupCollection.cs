using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelDataGroupCollection", menuName = "ScriptableObjects/LevelData/LevelDataGroupCollection", order = 0)]
public class LevelDataGroupCollection : ScriptableObject
{
    public List<LevelDataGroup> levelDataGroups => m_levelDataGroups;
    [SerializeField] private List<LevelDataGroup> m_levelDataGroups;
}
