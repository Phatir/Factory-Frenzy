using UnityEngine;
using UnityEngine.SceneManagement;



public class Trigger : MonoBehaviour
{

    [SerializeField] private PlayerMoovement player;
    [SerializeField] GameObject CanvasObject;

    private void OnTriggerEnter(Collider other)
    {
        CanvasObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player")
        {

            Debug.Log("SceneLancement)");
            SceneManager.LoadScene("PaulScene");
            CanvasObject.SetActive(false);
        }



    }

    private void OnTriggerStay(Collider other)
    {
        CanvasObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player")
        {

            Debug.Log("SceneLancement)");
            SceneManager.LoadScene("PaulScene");
            CanvasObject.SetActive(false);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        CanvasObject.SetActive(false);
    }



}




