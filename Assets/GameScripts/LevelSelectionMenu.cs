using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenu : MonoBehaviour
{
    public void Level1()
  {
    SceneManager.LoadSceneAsync("Level1");
  }

  public void Home()
  {
    SceneManager.LoadSceneAsync("Main Menu");
  }

}
