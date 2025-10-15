using DG.Tweening;
using UnityEngine;

public class MiniGameScript3 : MonoBehaviour
{
    public float vitesseRotation;
    public Transform player;
    public float playerDistance = 3;
    public float playerSpeed;
    void Update()
    {
        transform.Rotate(Vector3.forward, vitesseRotation * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.DOMoveX(playerDistance, playerSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("ça touche");
            vitesseRotation = 0;
        }
    }


}
