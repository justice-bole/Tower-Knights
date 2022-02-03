using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void RevealGameOverScreen() => gameObject.SetActive(true);
    public void RevealVictoryScreen() => gameObject.SetActive(true);
    public void LoadScene(int sceneIndex) => SceneManager.LoadSceneAsync(sceneIndex);
}
