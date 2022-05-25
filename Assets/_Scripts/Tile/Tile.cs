using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile Info")]
    public bool isFilled;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Color Info")]
    [HideInInspector] public Color fillColor;
    private Color _startColor;

    private void Start()
    {
        _startColor = spriteRenderer.color;
    }

    public void Fill_UnFill_Tile(bool shouldFill)
    {
        isFilled = shouldFill;

        if (isFilled)
        {
            spriteRenderer.color = fillColor;
        }
        else
        {
            spriteRenderer.color = _startColor;
        }
    }
}
