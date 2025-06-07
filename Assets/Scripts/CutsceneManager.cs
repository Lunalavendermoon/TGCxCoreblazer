using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class CutsceneManager : MonoBehaviour
{
    public GameObject linePresenter, lineBG, BG;
    public DialogueRunner dialogueRunner;
    //public DialogueManager dialogueManager;
    public TextMeshProUGUI dialogue;
    public float fadeDuration = 0.5f;
    public GameObject startingCutscene, badCutscene, goodCutscene;
    private VideoPlayer starting, badEnd, goodEnd;
    public int testNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starting = startingCutscene.GetComponent<VideoPlayer>();
        badEnd = badCutscene.GetComponent<VideoPlayer>();
        goodEnd = goodCutscene.GetComponent<VideoPlayer>();
        SpecialFormat(true);
        dialogueRunner.AddCommandHandler<bool>("special_format", SpecialFormat);
        dialogueRunner.AddCommandHandler("fade_out_text", FadeOut);
        dialogueRunner.AddCommandHandler<string>("play_cutscene", PlayCutscene);
        starting.loopPointReached += OnVideoEnd;
        badEnd.loopPointReached += OnVideoEnd;
        goodEnd.loopPointReached += OnVideoEnd;
    }
    public CanvasGroup bgCanvasGroup; // Drag your BG panel here in the inspector
    public float fadeOutDuration = 0.5f; // Adjustable fade time

    public void PlayCutscene(string name)
    {
        StartCoroutine(FadeOutBGAndPlayCutscene(name));
    }

    private IEnumerator FadeOutBGAndPlayCutscene(string name)
    {
        // make sure the BG actually exists
        bgCanvasGroup.alpha = 1f;

        // Then play the video
        switch (name)
        {
            case "start":
                startingCutscene.SetActive(true);
                starting.Play();
                break;
            case "bad":
                badCutscene.SetActive(true);
                badEnd.Play();
                break;
            case "good":
                goodCutscene.SetActive(true);
                goodEnd.Play();
                break;
        }
        // Fade out the BG instead of SetActive(false)
        float time = 0f;
        float startAlpha = bgCanvasGroup.alpha;

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            // float alpha = Mathf.Lerp(startAlpha, 0f, time / fadeOutDuration);
            // bgCanvasGroup.alpha = alpha;
            yield return null;
        }

        bgCanvasGroup.alpha = 0f;
        bgCanvasGroup.interactable = false;
        bgCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator PlayWhenPrepared(VideoPlayer vp)
    {
        vp.Prepare();

        // Wait until the video is ready
        while (!vp.isPrepared)
            yield return null;

        vp.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("video ended");
        if (vp == starting)
        {
            SpecialFormat(false);
        }
        if (vp == badEnd)
        {
            if (/**dialogueManager.savedNPCs.Count < **/ testNum < 5)
            {
                dialogueRunner.StartDialogue("BadEnd");
                bgCanvasGroup.alpha = 1f;
            }
            else
            {
                PlayCutscene("good");
            }
        }
        vp.gameObject.SetActive(false);
    }
    public void SpecialFormat(bool special)
    {
        if (special)
        {
            linePresenter.transform.position += new Vector3(0f, 5f, 0f);
            lineBG.SetActive(false);
            bgCanvasGroup.alpha = 1f;
            AudioManager.Instance.PlayBGM("cutscene");
        }
        else
        {
            linePresenter.transform.position += new Vector3(0f, -5f, 0f);
            lineBG.SetActive(true);
            bgCanvasGroup.alpha = 0f;
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
