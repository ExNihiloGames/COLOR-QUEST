using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinimizedLevelDisplayUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;
    [SerializeField] Button m_button;
    int m_index;
    public static event Action<int> onLevelDisplaySelected;

    public void Init(string text, int index)
    {
        m_text.text = text;
        m_index = index;
        m_button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        onLevelDisplaySelected?.Invoke(m_index);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveAllListeners();
    }


}
