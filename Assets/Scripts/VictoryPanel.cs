using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanel : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject gameOverlay;
    [SerializeField] GameObject vPanel;

    public void Victory()
    {
        player.SetActive(false);
        gameOverlay.SetActive(false);
        vPanel.SetActive(true);
    }
        public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("GameScene");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Victory();
        }
    }
}
