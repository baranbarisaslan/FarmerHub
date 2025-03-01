using UnityEngine;

public class ObjectHelper : MonoBehaviour
{
    public static void SetOpacity(GameObject targetObject, float opacity)
    {
        if (targetObject == null)
        {
            Debug.LogError("OpacityHelper: Hedef nesne bo�!");
            return;
        }

        SpriteRenderer spriteRenderer = targetObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("OpacityHelper: Nesnede SpriteRenderer bulunamad�!");
            return;
        }

        opacity = Mathf.Clamp(opacity, 0f, 1f); // 0 ile 1 aras�nda s�n�rla
        Color color = spriteRenderer.color;
        color.a = opacity;
        spriteRenderer.color = color;
    }
}
