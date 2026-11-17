using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FadeCoroutine(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            renderer.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        Color startColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
        Color targetColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);

        yield return FadeCoroutine(startColor, targetColor, duration);
    }

    public IEnumerator FadeOut(float duration)
    {
        Color startColor = new Color(renderer.color.r,  renderer.color.g, renderer.color.b, 0);
        Color targetColor = new Color(renderer.color.r, renderer.color.g, renderer .color.b, 1);

        yield return FadeCoroutine(startColor, targetColor, duration);
    }

    public IEnumerator FadeInAndOut(float duration)
    {
        Color startColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        Color targetColor = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
        yield return FadeCoroutine(startColor, targetColor, duration);
        yield return new WaitForSeconds(duration);
        yield return FadeCoroutine(targetColor, startColor, duration);
    }
}
