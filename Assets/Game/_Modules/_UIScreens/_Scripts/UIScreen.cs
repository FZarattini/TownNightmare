using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScreen : MonoBehaviour
{
    [SerializeField] string stageName;
    [SerializeField] string mainMenuName;

    public void ResetStage()
    {
        SceneManager.LoadScene(stageName);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }
}
