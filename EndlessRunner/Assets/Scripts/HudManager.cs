using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    [SerializeField] TMP_Text textLife;

    public void UpdateLifes(int value)
    {
        textLife.text = value.ToString();
    }
}
