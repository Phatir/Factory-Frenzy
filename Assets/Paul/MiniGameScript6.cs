using UnityEngine;

public class MiniGameScript6 : MonoBehaviour
{
    public float vitesseX = 5f;
    public float vitesseY = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

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
        }
    }
}

