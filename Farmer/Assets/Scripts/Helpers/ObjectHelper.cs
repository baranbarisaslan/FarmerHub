using UnityEngine;

public class ObjectHelper : MonoBehaviour
{
    public static void SetOpacity(GameObject targetObject, float opacity)
    {
        if (targetObject == null)
        {
            Debug.LogError("OpacityHelper: Hedef nesne boþ!");
            return;
        }

        SpriteRenderer spriteRenderer = targetObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("OpacityHelper: Nesnede SpriteRenderer bulunamadý!");
            return;
        }

        opacity = Mathf.Clamp(opacity, 0f, 1f); // 0 ile 1 arasýnda sýnýrla
        Color color = spriteRenderer.color;
        color.a = opacity;
        spriteRenderer.color = color;
    }
}
