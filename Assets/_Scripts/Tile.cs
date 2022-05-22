using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isFilled;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color startColor;

    private void Start()
    {
        startColor = spriteRenderer.color;
    }

    public void HandleTile(bool shouldFill)
    {
        isFilled = shouldFill;

        if (isFilled)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = startColor;
        }
    }
}
