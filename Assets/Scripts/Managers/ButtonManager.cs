using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    // loads the next scene in the queue (e.g. the customisation screen or the game)
    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)  // if scene is that of the main menu
        {
            StartCoroutine(WaitForLerp());
        }
        else
        {
            AdvanceScene();
        }
    }

    // calls to the LerpBetween script on the main camera, and does not load 
    // the next scene until this script has completed it's operation
    private IEnumerator WaitForLerp()
    {
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<LerpBetween>().Begin();

        yield return new WaitUntil(() => !camera.GetComponent<LerpBetween>().IsLerping);
        AdvanceScene();
    }

    // loads the next scene
    private void AdvanceScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // exits the program
    public void QuitGame()
    {
        Debug.Log("User has quit the game!");
        Application.Quit();
    }

    // loads the Main Menu scene
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
