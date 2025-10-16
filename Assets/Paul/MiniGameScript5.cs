using UnityEngine;

public class MiniGameScript5 : MonoBehaviour
{
    public float vitesse = 5f;
    public GameObject clothes;
    public Transform[] spawnAreas;

    [Header("Points gagnés à la réussite")]
    public int pointsReward = 10;
    private bool gameCompleted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SpawnerEnnemi", 2);
    }

    // Update is called once per frame
    void Update()
    {
        float mouvement = Input.GetAxisRaw("Horizontal"); // -1 (gauche), 0, 1 (droite)
        transform.Translate(Vector3.right * mouvement * vitesse * Time.deltaTime);

    }
    void SpawnerEnnemi()
    {
        if (spawnAreas.Length == 0 || clothes == null)
        {
            Debug.LogWarning("Aucun point de spawn ou prefab défini !");
            return;
        }

        int index = Random.Range(0, spawnAreas.Length);
        Transform spawnPoint = spawnAreas[index];
        clothes.transform.position = spawnPoint.position;
        clothes.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Clothes")
        {
            print("ça touche");
            vitesse = 0;
            clothes = null;
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
