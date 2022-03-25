using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    #region
    public GameObject loadingPanel, loadingBar_All;
    public Text txt_Loading;
    public RectTransform loadingBar;
    public GameObject txt_pressAnykey;
    AsyncOperation asyncLoad;
    float displayProgress = 0;
    #endregion

    public void SwitchScene(int scene)
    {
        StartCoroutine(LoadAsyncScene(scene));
        //SceneManager.LoadScene(scene);
    }
    IEnumerator LoadAsyncScene(int scene)
    {
        loadingPanel.SetActive(true);
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress < 0.9f)
            {
                displayProgress = asyncLoad.progress;
            }
            else
            {
                displayProgress = 1;
            }
            txt_Loading.text = (int)(displayProgress * 100) + "%";
            loadingBar.localPosition = new Vector3((-992 + 992 * (displayProgress / 1)), 0, 0);
            if (asyncLoad.progress >= 0.9f)
            {
                loadingBar_All.SetActive(false);
                txt_pressAnykey.SetActive(true);
                if (Input.anyKeyDown)
                    asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
