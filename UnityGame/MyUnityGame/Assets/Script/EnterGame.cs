using UnityEngine;

public class EnterGame : MonoBehaviour 

{

    public Canvas GameMenu;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            GameMenu.enabled = true;
        }
    }

   
}
