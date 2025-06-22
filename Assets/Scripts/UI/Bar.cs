using Unity.Collections;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField]
    RectTransform fillBar;

    [SerializeField]
    float fillPercent = 1;

    public void SetFillPercent(float newValue)
    {
        fillPercent = newValue;
        fillBar.localScale = new Vector3(fillPercent, 1, 1);
    }

    public void ChangeFillPercent(float amount)
    {
        SetFillPercent(amount + fillPercent);
    }
}
