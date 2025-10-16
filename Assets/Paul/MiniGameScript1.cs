using System.Collections;
using UnityEngine;

public class MiniGameScript1 : MonoBehaviour
{

    public Rigidbody barre;
    private float delai;
    [Header("Points gagnés à la réussite")]
    public int pointsReward = 10;

    private bool gameCompleted = false;

    void Update()
    {
        // Exemple : si on appuie sur E, on considère le mini-jeu réussi
        if (gameCompleted)
        {
            CompleteMiniGame();
        }
    }

    void CompleteMiniGame()
    {
        gameCompleted = true;
        Debug.Log("Mini-jeu terminé !");
        ScoreManager.Instance.AddScore(pointsReward);
        gameCompleted = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hand" && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espace pressé !");
            barre.isKinematic = !barre.isKinematic;
            gameCompleted = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand" && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espace pressé !");
            barre.isKinematic = !barre.isKinematic;
            gameCompleted = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hand" && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espace pressé !");
            barre.isKinematic = !barre.isKinematic;
        }
    }

    private void Start()
    {
        barre.useGravity = false;
        delai = Random.Range(2, 5);
        print(delai);
        StartCoroutine(LancementBarre(delai));
    }

    IEnumerator LancementBarre(float temps)
    {
        yield return new WaitForSeconds(temps);
        barre.useGravity = true;
    }

}


