using UnityEngine;

#if UNITY_EDITOR
public class SceneTest : MonoBehaviour
{
    public GameManager gameManeger;
    public SoundManager soundManager;
    private void Awake()
    {
        Instantiate(gameManeger);
        Instantiate(soundManager);
    }
}
#endif
