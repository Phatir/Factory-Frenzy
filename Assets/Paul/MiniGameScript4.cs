using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MiniGameScript4 : MonoBehaviour
{
    public float vitesse = 5f;
    public GameObject enemyPrefab;
    public Transform[] spawnAreas;
    public float spawnTime = 2f;

    [Header("Points gagnés à la réussite")]
    public int pointsReward = 10;
    private bool gameCompleted = false;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnerEnnemi), 1f, spawnTime);
        CompleteMiniGame();
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnerEnnemi));
    }
    void Update()
    {
        float mouvement = Input.GetAxisRaw("Horizontal"); // -1 (gauche), 0, 1 (droite)
        transform.Translate(Vector3.right * mouvement * vitesse * Time.deltaTime);
    }
    void SpawnerEnnemi()
    {
        if (spawnAreas.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("Aucun point de spawn ou prefab défini !");
            return;
        }

        int index = Random.Range(0, spawnAreas.Length);
        Transform spawnPoint = spawnAreas[index];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("ça touche");
            ScoreManager.Instance.AddScore(-pointsReward);


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
