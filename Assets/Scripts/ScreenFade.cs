using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FadeIn()
    {
        yield return null;
    }

    public IEnumerator FadeOut()
    {
        yield return null;
    }

    public IEnumerator FadeInAndOut()
    {
        yield return null;
    }
}
