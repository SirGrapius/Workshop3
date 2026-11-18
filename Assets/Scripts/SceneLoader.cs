using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private ScreenFade screenFade;
    void Start()
    {
        screenFade = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<ScreenFade>();
    }

    public void StartLoad(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        StartCoroutine(screenFade.FadeOut(2));
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneIndex);
        yield return null;
    }

    public void Quit()
    {
        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame()
    {
        StartCoroutine(screenFade.FadeOut(2));
        yield return new WaitForSeconds(2);
        Application.Quit();
        yield return null;
    }
}
