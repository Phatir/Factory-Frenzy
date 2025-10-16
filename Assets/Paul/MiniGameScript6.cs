using UnityEngine;

public class MiniGameScript6 : MonoBehaviour
{
    public float vitesseX = 5f;
    public float vitesseY = 5f;
    [Header("Points gagnés à la réussite")]
    public int pointsReward = 10;
    private bool gameCompleted = false;


    void Update()
    {
        float mouvement = Input.GetAxisRaw("Horizontal"); // -1 (gauche), 0, 1 (droite)
        transform.Translate(Vector3.right * mouvement * vitesseX * Time.deltaTime);
        transform.Translate(Vector3.down * vitesseY * Time.deltaTime);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sol")
        {
            vitesseY = 0;
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
}

