using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "ScriptableObjects/LevelData/LevelData", order = 2)]
public class LevelData : ScriptableObject
{
    public string fullName => m_fullName;
    [SerializeField] string m_fullName;
    public string minimizedName => m_minimizedName;
    [SerializeField] string m_minimizedName;
    public int buildIndex => m_buildIndex;
    [SerializeField] int m_buildIndex;
    public Sprite screenshot => m_screenshot;
    [SerializeField] Sprite m_screenshot;

}
