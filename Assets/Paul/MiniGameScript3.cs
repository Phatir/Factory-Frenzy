using DG.Tweening;
using UnityEngine;

public class MiniGameScript3 : MonoBehaviour
{
    public float vitesseRotation;
    public Transform player;
    public float playerDistance = 3;
    public float playerSpeed;
    [Header("Points gagnés à la réussite")]
    public int pointsReward = 10;
    private bool gameCompleted = false;
    void Update()
    {
        transform.Rotate(Vector3.forward, vitesseRotation * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.DOMoveX(playerDistance, playerSpeed);
        }
        if (gameCompleted)
        {
            CompleteMiniGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("ça touche");
            vitesseRotation = 0;
            gameCompleted = true;
        }
    }

    void CompleteMiniGame()
    {
        gameCompleted = true;
        Debug.Log("Mini-jeu terminé !");
        ScoreManager.Instance.AddScore(pointsReward);
        gameCompleted = false;
    }

}
