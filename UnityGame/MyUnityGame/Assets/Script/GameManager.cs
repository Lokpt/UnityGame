using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public Canvas GameMenu;

    private bool isShowingMenu = false;

    public TMP_Text Title;
    public TMP_Text Message;

    public Button ContinueButton;
    public Button RestartButton;

    void Start()
    {
        GameMenu.enabled = isShowingMenu;
        Title.text = "Welcome to Unity!";
        Message.text = "Do you want continue?";
        ContinueButton.GetComponentInChildren<TMP_Text>().text = "Enter";
        RestartButton.GetComponentInChildren<TMP_Text>().text = "Restart";
    }
}