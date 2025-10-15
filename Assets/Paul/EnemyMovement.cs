using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float vitesse = 2f;

    void Update()
    {
        transform.Translate(Vector3.down * vitesse * Time.deltaTime);
        if (transform.position.y < -10f)
        {
            Destroy(gameObject); // supprime l'ennemi quand il est hors écran
        }
    }

}
