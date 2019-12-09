using LlamaSoftware.UI.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LlamaSoftware.UI.Demo
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField]
        private string SceneName = string.Empty;
        [SerializeField]
        private SimpleProgressBar ProgressBar = null;

        private AsyncOperation LoadLevelOperation = null;

        public void LoadScene()
        {
            if (LoadLevelOperation != null)
            {
                LoadLevelOperation.allowSceneActivation = false;
            }

            LoadLevelOperation = SceneManager.LoadSceneAsync(SceneName);
            LoadLevelOperation.allowSceneActivation = true;
        }

        private void Update()
        {
            if (LoadLevelOperation != null)
            {
                ProgressBar.SetProgressImmediate(LoadLevelOperation.progress);
            }
        }
    }
}

