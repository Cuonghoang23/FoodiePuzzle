using UnityEngine;
using UnityEngine.UI;

public class UIOutlineEffect : MonoBehaviour
{
    private Image img;

    private Material runtimeMat;

    [SerializeField] private Color outlineColor = Color.white;

    [SerializeField] private float outlineSize = 1.5f;

    private void Awake()
    {
        img = GetComponent<Image>();

        // Clone material riêng cho image này
        runtimeMat = new Material(img.material);

        img.material = runtimeMat;

        SetOutline(false);
    }

    public void SetOutline(bool enable)
    {
        if (runtimeMat == null) return;

        if (enable)
        {
            runtimeMat.SetFloat("_OutlineSize", outlineSize);
            runtimeMat.SetColor("_OutlineColor", outlineColor);
        }
        else
        {
            runtimeMat.SetFloat("_OutlineSize", 0);
        }
    }

    private void OnDestroy()
    {
        if (runtimeMat != null)
        {
            Destroy(runtimeMat);
        }
    }
}