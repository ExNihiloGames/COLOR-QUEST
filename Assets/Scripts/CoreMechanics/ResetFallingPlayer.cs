using System;
using UnityEngine;

public class ResetFallingPlayer : MonoBehaviour
{
    public static Action onPlayerFall;
    private void OnTriggerEnter(Collider other)
    {
        onPlayerFall?.Invoke();
    }
}
