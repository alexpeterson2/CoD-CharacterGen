using LlamaSoftware.UI.Utility;
using System.Collections;
using UnityEngine;

namespace LlamaSoftware.UI.Demo
{
    public class ProgressBarRandomAnimator : MonoBehaviour
    {
        [SerializeField]
        [Range(0.5f, 3f)]
        private float MinDuration = 3f;
        [SerializeField]
        [Range(0.75f, 5f)]
        private float MaxDuration = 5f;
        [SerializeField]
        private SimpleProgressBar ProgressBar = null;

        private void Start()
        {
            StartCoroutine(AnimateProgressBar());
        }

        private IEnumerator AnimateProgressBar()
        {
            // Pick how long it should take to fill up
            float Duration = Random.Range(MinDuration, MaxDuration);
            WaitForSeconds Wait = new WaitForSeconds(Duration + 0.5f);

            // Set how long it should take to complete the animation
            ProgressBar.AnimationTime = Duration;

            while (true)
            {
                // Make it animate 
                ProgressBar.SetProgress(1);

                // Wait for animation to complete
                yield return Wait;

                // Reset progress
                ProgressBar.SetProgressImmediate(0);

            }
        }
    }
}
