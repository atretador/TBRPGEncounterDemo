using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicEntityInfoDisplay : MonoBehaviour
{
    public TMP_Text entityName;
    public Slider healthBar;
    public Slider actionBar;

    public void UpdateHeathBar(float value)
    {
        healthBar.value = value;
    }

    public void UpdateActionBar(float value)
    {
        actionBar.value = value;
    }
}
