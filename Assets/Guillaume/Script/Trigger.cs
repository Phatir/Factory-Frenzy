using UnityEngine;
using UnityEngine.SceneManagement;


public class Trigger : MonoBehaviour
{

    [SerializeField] private PlayerMoovement player;

    private void OnTriggerStay(Collider other)
    {

        if (Input.GetKeyDown(KeyCode.E) && other.tag == "Player")
        {
            Debug.Log("SceneLancement)");
            SceneManager.LoadScene("Paul");
            
        }



    }




}
