using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile Info")]
     public bool isFilled;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Color Info")]
    [HideInInspector] public Color fillColor;
    private Color startColor;

    private void Start()
    {
        startColor = spriteRenderer.color;
    }

    public void FillTile(bool shouldFill)
    {
        isFilled = shouldFill;

        if (isFilled)
        {
            spriteRenderer.color = fillColor;
        }
        else
        {
            spriteRenderer.color = startColor;
        }
    }
}
