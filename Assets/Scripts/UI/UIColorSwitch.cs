using UnityEngine;
using UnityEngine.UI;

public class UIColorSwitch : MonoBehaviour
{
    public Image cyanIcon;
    public Image magentaIcon;
    public Image yellowIcon;

    public void SetIconActive(Filter filter)
    {
        switch(filter)
        {
            case Filter.Cyan:
                cyanIcon.color = Color.cyan;
                magentaIcon.color = new Color(magentaIcon.color.r, magentaIcon.color.g, magentaIcon.color.b, 0);
                yellowIcon.color = new Color(yellowIcon.color.r, yellowIcon.color.g, yellowIcon.color.b, 0);
                break;

            case Filter.Magenta:
                cyanIcon.color = new Color(cyanIcon.color.r, cyanIcon.color.g, cyanIcon.color.b, 0);
                magentaIcon.color = Color.magenta;
                yellowIcon.color = new Color(yellowIcon.color.r, yellowIcon.color.g, yellowIcon.color.b, 0);
                break;

            case Filter.Yellow:
                cyanIcon.color = new Color(cyanIcon.color.r, cyanIcon.color.g, cyanIcon.color.b, 0);
                magentaIcon.color = new Color(magentaIcon.color.r, magentaIcon.color.g, magentaIcon.color.b, 0);
                yellowIcon.color = new Color(1, 1, 0, 1);
                break;
        }
    }
}
