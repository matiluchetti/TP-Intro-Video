using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {
  public GameObject LoadingScreen;
  public Slider Slider;

  public void LoadScene(int sceneId) {
    StartCoroutine(LoadSceneAsync(sceneId));
  }

  IEnumerator LoadSceneAsync(int sceneId) {
    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

    if (LoadingScreen != null) {
      LoadingScreen.SetActive(true);
    }

    while (!operation.isDone) {
      float progress = Mathf.Clamp01(operation.progress / .9f);
      Slider.value = progress;
      yield return null;
    }
  }
}
