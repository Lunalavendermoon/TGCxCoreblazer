using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class CutsceneManager : MonoBehaviour
{
    public GameObject linePresenter, lineBG, BG;
    public DialogueRunner dialogueRunner;
    public TextMeshProUGUI dialogue;
    public float fadeDuration = 0.5f;
    public GameObject startingCutscene;
    private VideoPlayer starting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starting = startingCutscene.GetComponent<VideoPlayer>();
        SpecialFormat(true);
        dialogueRunner.AddCommandHandler<bool>("special_format", SpecialFormat);
        dialogueRunner.AddCommandHandler("fade_out_text", FadeOut);
        dialogueRunner.AddCommandHandler("play_cutscene", PlayCutsene);
        starting.loopPointReached += OnVideoEnd;
    }

    public void PlayCutsene()
    {
        BG.SetActive(false);
        startingCutscene.SetActive(true);
        starting.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SpecialFormat(false);
        vp.gameObject.SetActive(false);
    }
    public void SpecialFormat(bool special)
    {
        if (special)
        {
            linePresenter.transform.position += new Vector3(0f, 5f, 0f);
            lineBG.SetActive(false);
            BG.SetActive(true);
            AudioManager.Instance.PlayBGM("cutscene");
        }
        else
        {
            linePresenter.transform.position += new Vector3(0f, -5f, 0f);
            lineBG.SetActive(true);
            BG.SetActive(false);
            AudioManager.Instance.PlayBGM("main");
        }
    }

    public void FadeIn() => StartCoroutine(FadeTo(1f));
    public void FadeOut() => StartCoroutine(FadeTo(0f));

    private IEnumerator FadeTo(float targetAlpha)
    {
        Color startColor = dialogue.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, targetAlpha, time / fadeDuration);
            dialogue.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        dialogue.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }
}
