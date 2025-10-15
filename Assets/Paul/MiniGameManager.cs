using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    [Header("Liste des mini-jeux (GameObjects à activer/désactiver)")]
    public List<GameObject> miniGames = new List<GameObject>();

    [Header("Durée de chaque mini-jeu (en secondes, même ordre que miniGames)")]
    public List<float> durations = new List<float>();

    [Header("UI de transition (Image noire en overlay)")]
    public Image fadeImage;

    [Header("Texte d’instruction du mini-jeu")]
    public TextMeshProUGUI gameNameText;
    public float displayGameNameDuration = 2f;
    public float textFadeDuration = 0.7f;

    [Header("Paramètres du fondu visuel")]
    public float fadeDuration = 1f;

    [Header("Paramètres du fondu sonore")]
    public float audioFadeDuration = 1f;

    [Header("Boucler après le dernier mini-jeu ?")]
    public bool loop = true;

    [Header("Son de transition (joué entre deux mini-jeux)")]
    public AudioClip transitionSound;
    public float transitionVolume = 1f;

    [Header("Délai avant que le premier mini-jeu commence (pour lire l’instruction)")]
    public float firstGameDelay = 2f;

    private int currentIndex = 0;
    private float timer = 0f;
    private bool isTransitioning = false;

    private AudioSource currentAudio;
    private AudioSource nextAudio;
    private AudioSource transitionAudioSource;

    void Start()
    {
        if (miniGames.Count == 0)
        {
            Debug.LogWarning("Aucun mini-jeu n'est assigné !");
            return;
        }

        while (durations.Count < miniGames.Count)
            durations.Add(10f);

        // Désactive tous les mini-jeux
        for (int i = 0; i < miniGames.Count; i++)
            miniGames[i].SetActive(false);

        // Prépare l’audio du premier mini-jeu
        currentAudio = miniGames[0].GetComponent<AudioSource>();
        if (currentAudio != null)
            currentAudio.volume = 1f;

        // Source pour les sons de transition
        transitionAudioSource = gameObject.AddComponent<AudioSource>();
        transitionAudioSource.playOnAwake = false;
        transitionAudioSource.volume = transitionVolume;

        // Fade visuel et texte
        if (fadeImage != null)
            SetFadeAlpha(1f); // Commence noir
        if (gameNameText != null)
        {
            gameNameText.gameObject.SetActive(true);
            SetTextAlpha(0f);
            gameNameText.text =  miniGames[0].name;
        }

        // Lance la première instruction avec fade-in et début du mini-jeu après delay
        StartCoroutine(FirstGameStart());
    }

    private System.Collections.IEnumerator FirstGameStart()
    {
        // Fade-in du texte d’instruction
        if (gameNameText != null)
            yield return StartCoroutine(FadeText(1f));

        // Petite pause pour que le joueur lise
        yield return new WaitForSeconds(firstGameDelay);

        // Active le premier mini-jeu
        miniGames[0].SetActive(true);
        timer = durations[0];

        // Fade-out du texte
        if (gameNameText != null)
            yield return StartCoroutine(FadeText(0f));

        // Fade-in visuel de l’écran
        if (fadeImage != null)
            yield return StartCoroutine(Fade(0f));

        // Joue l’audio du premier mini-jeu si disponible
        if (currentAudio != null)
            currentAudio.Play();
    }

    void Update()
    {
        if (isTransitioning || miniGames.Count == 0)
            return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
            StartCoroutine(TransitionToNextGame());

    }

    private System.Collections.IEnumerator TransitionToNextGame()
    {
        isTransitioning = true;

        // Fade-out visuel et audio du mini-jeu actuel
        var fadeVisual = Fade(1f);
        var fadeAudioOut = FadeAudio(currentAudio, 0f);
        yield return StartCoroutine(fadeVisual);
        if (fadeAudioOut != null) yield return StartCoroutine(fadeAudioOut);

        // Son de transition
        if (transitionSound != null)
            transitionAudioSource.PlayOneShot(transitionSound, transitionVolume);

        // Désactive le mini-jeu actuel
        miniGames[currentIndex].SetActive(false);

        // Passe au suivant
        currentIndex++;
        if (currentIndex >= miniGames.Count)
        {
            if (loop)
                currentIndex = 0;
            else
            {
                if (fadeImage != null)
                    yield return StartCoroutine(Fade(1f));
                yield break;
            }
        }

        // Active le nouveau mini-jeu
        miniGames[currentIndex].SetActive(true);
        timer = durations[currentIndex];

        // Joue le son du mini-jeu
        nextAudio = miniGames[currentIndex].GetComponent<AudioSource>();
        if (nextAudio != null)
        {
            nextAudio.volume = 0f;
            nextAudio.Play();
        }

        // Affiche l’instruction du mini-jeu
        if (gameNameText != null)
        {
            gameNameText.text = /*"Prochain jeu : " + */miniGames[currentIndex].name;
            gameNameText.gameObject.SetActive(true);
            yield return StartCoroutine(FadeText(1f));
        }

        yield return new WaitForSeconds(transitionSound != null ? transitionSound.length * 0.3f : 0.3f);

        // Fade-in visuel et audio
        var fadeVisualIn = Fade(0f);
        var fadeAudioIn = FadeAudio(nextAudio, 1f);
        yield return StartCoroutine(fadeVisualIn);
        if (fadeAudioIn != null) yield return StartCoroutine(fadeAudioIn);

        // Masque le texte après displayGameNameDuration
        if (gameNameText != null)
        {
            yield return new WaitForSeconds(displayGameNameDuration);
            yield return StartCoroutine(FadeText(0f));
            gameNameText.gameObject.SetActive(false);
        }

        currentAudio = nextAudio;
        isTransitioning = false;
    }

    // Fade visuel
    private System.Collections.IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break;
        float startAlpha = fadeImage.color.a;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetFadeAlpha(Mathf.Lerp(startAlpha, targetAlpha, Mathf.Clamp01(t / fadeDuration)));
            yield return null;
        }
        SetFadeAlpha(targetAlpha);
    }

    // Fade audio
    private System.Collections.IEnumerator FadeAudio(AudioSource audio, float targetVolume)
    {
        if (audio == null) yield break;
        float startVolume = audio.volume;
        float t = 0f;
        while (t < audioFadeDuration)
        {
            t += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, targetVolume, Mathf.Clamp01(t / audioFadeDuration));
            yield return null;
        }
        audio.volume = targetVolume;
        if (targetVolume == 0f) audio.Stop();
    }

    // Fade texte
    private System.Collections.IEnumerator FadeText(float targetAlpha)
    {
        if (gameNameText == null) yield break;
        float startAlpha = gameNameText.color.a;
        float t = 0f;
        while (t < textFadeDuration)
        {
            t += Time.deltaTime;
            SetTextAlpha(Mathf.Lerp(startAlpha, targetAlpha, Mathf.Clamp01(t / textFadeDuration)));
            yield return null;
        }
        SetTextAlpha(targetAlpha);
    }

    private void SetFadeAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }

    private void SetTextAlpha(float alpha)
    {
        Color c = gameNameText.color;
        c.a = alpha;
        gameNameText.color = c;
    }
}