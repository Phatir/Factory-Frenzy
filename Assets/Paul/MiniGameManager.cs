using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [Header("Liste des mini-jeux (GameObjects à activer/désactiver)")]
    public List<GameObject> miniGames = new List<GameObject>();

    [Header("Durée de chaque mini-jeu (en secondes, même ordre que miniGames)")]
    public List<float> durations = new List<float>();

    [Header("UI de transition (Image noire en overlay)")]
    public Image fadeImage;

    [Header("Paramètres du fondu visuel")]
    public float fadeDuration = 1f;

    [Header("Paramètres du fondu sonore")]
    public float audioFadeDuration = 1f;

    [Header("Boucler après le dernier mini-jeu ?")]
    public bool loop = true;

    [Header("Son de transition (joué entre deux mini-jeux)")]
    public AudioClip transitionSound;
    public float transitionVolume = 1f;

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

        // Complète la liste des durées manquantes
        while (durations.Count < miniGames.Count)
            durations.Add(10f);

        // Désactive tout sauf le premier
        for (int i = 0; i < miniGames.Count; i++)
        {
            miniGames[i].SetActive(i == 0);
        }

        // Configure l'audio du premier mini-jeu
        currentAudio = miniGames[0].GetComponent<AudioSource>();
        if (currentAudio != null)
        {
            currentAudio.volume = 1f;
            if (!currentAudio.isPlaying)
                currentAudio.Play();
        }

        // Prépare la source sonore pour les transitions
        transitionAudioSource = gameObject.AddComponent<AudioSource>();
        transitionAudioSource.playOnAwake = false;
        transitionAudioSource.volume = transitionVolume;

        timer = durations[0];

        // Initialise le fondu visuel
        if (fadeImage != null)
            SetFadeAlpha(0f);
    }

    void Update()
    {
        if (isTransitioning || miniGames.Count == 0)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            StartCoroutine(TransitionToNextGame());
        }
    }

    private System.Collections.IEnumerator TransitionToNextGame()
    {
        isTransitioning = true;

        // Lancer le fade visuel et audio du mini-jeu actuel
        var fadeVisual = Fade(1f);
        var fadeAudioOut = FadeAudio(currentAudio, 0f);

        yield return StartCoroutine(fadeVisual);
        if (fadeAudioOut != null)
            yield return StartCoroutine(fadeAudioOut);

        // Joue le son de transition (whoosh, click, etc.)
        if (transitionSound != null && transitionAudioSource != null)
        {
            transitionAudioSource.PlayOneShot(transitionSound, transitionVolume);
        }

        // Désactive le mini-jeu actuel
        miniGames[currentIndex].SetActive(false);

        // Passe au suivant
        currentIndex++;

        // Si on arrive à la fin
        if (currentIndex >= miniGames.Count)
        {
            if (loop)
            {
                currentIndex = 0;
            }
            else
            {
                Debug.Log("Tous les mini-jeux sont terminés !");
                yield return new WaitForSeconds(transitionSound != null ? transitionSound.length : 0.5f);
                if (fadeImage != null)
                    yield return StartCoroutine(Fade(1f)); // écran noir final
                yield break;
            }
        }

        // Active le nouveau mini-jeu
        miniGames[currentIndex].SetActive(true);
        timer = durations[currentIndex];

        // Gère l’audio du nouveau mini-jeu
        nextAudio = miniGames[currentIndex].GetComponent<AudioSource>();
        if (nextAudio != null)
        {
            nextAudio.volume = 0f;
            nextAudio.Play();
        }

        // Attendre un petit moment pour laisser respirer la transition sonore
        if (transitionSound != null)
            yield return new WaitForSeconds(transitionSound.length * 0.3f);

        // Fade-in visuel et audio du nouveau jeu
        var fadeVisualIn = Fade(0f);
        var fadeAudioIn = FadeAudio(nextAudio, 1f);

        yield return StartCoroutine(fadeVisualIn);
        if (fadeAudioIn != null)
            yield return StartCoroutine(fadeAudioIn);

        currentAudio = nextAudio;
        isTransitioning = false;
    }

    private System.Collections.IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null)
            yield break;

        float startAlpha = fadeImage.color.a;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / fadeDuration);
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, blend);
            SetFadeAlpha(alpha);
            yield return null;
        }

        SetFadeAlpha(targetAlpha);
    }

    private System.Collections.IEnumerator FadeAudio(AudioSource audio, float targetVolume)
    {
        if (audio == null)
            yield break;

        float startVolume = audio.volume;
        float t = 0f;

        while (t < audioFadeDuration)
        {
            t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / audioFadeDuration);
            audio.volume = Mathf.Lerp(startVolume, targetVolume, blend);
            yield return null;
        }

        audio.volume = targetVolume;

        if (targetVolume == 0f)
            audio.Stop();
    }

    private void SetFadeAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }
}