using System.Collections;
using UnityEngine;

public class MiniGameScript1 : MonoBehaviour
{

    public Rigidbody barre;
    private float delai;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hand" && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espace pressé !");
            barre.isKinematic = !barre.isKinematic;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hand" && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espace pressé !");
            barre.isKinematic = !barre.isKinematic;
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


