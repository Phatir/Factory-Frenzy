using DG.Tweening;
using System.Collections;
using System.Threading;
using UnityEngine;

public class MiniGameScript2 : MonoBehaviour
{
    public float jumpForce;
    public Rigidbody rb;
    public float distanceSol = 0.1f; // Distance pour vérifier le sol
    public LayerMask solLayer;
    [Space(10)]
    [Header("CAR")]
    public Transform car;
    public float carSpeed;

    private float delai;

    private void Start()
    {
        delai = Random.Range(2, 5);
        print(delai);
        StartCoroutine(LancementVoiture(delai));

    }

    IEnumerator LancementVoiture(float temps)
    {
        yield return new WaitForSeconds(temps);
        car.DOMoveX(-15, 2);

    }
    // Update is called once per frame
    void Update()
    {
        print(isGrounded());
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Sauter();
        }
    }

    void Sauter()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceSol + 0.1f, solLayer);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Time.timeScale = 0f;
        }
    }

}
