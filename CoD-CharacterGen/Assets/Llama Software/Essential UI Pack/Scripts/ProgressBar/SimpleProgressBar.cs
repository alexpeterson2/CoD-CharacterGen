using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LlamaSoftware.UI.Utility
{
    /// <summary>
    /// A Progress Bar that will animate the Fill of any specified Image.
    /// </summary>
    public class SimpleProgressBar : MonoBehaviour
    {
        /// <summary>
        /// The Filled Image that represents the progress
        /// </summary>
        public Image ProgressBarImage= null;
        public Image.FillMethod FillMethod = Image.FillMethod.Horizontal;
        /// <summary>
        /// How many frames per second the progress bar should animate with
        /// </summary>
        public int AnimationFPS = 30;
        /// <summary>
        /// How long the animation should play in seconds
        /// </summary>
        public float AnimationTime = 0.5f;
        /// <summary>
        /// Action to call when progress bar completes animating
        /// </summary>
        public UnityEvent OnCompleted = null;
        /// <summary>
        /// Read-only convenience property. Will always be the same as <see cref="ProgressBarImage"/>.fillAmount
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        #region variable cache
        protected string OutOfRangeProgressMessage = "Progress {0} outside of the range of 0-1 is invalid. Unexpected results may occur.";
        protected int i;
        protected float startingProgress;
        protected float targetProgress;
        protected float EndTime;
        protected WaitForSeconds Wait;
        #endregion

        private void Awake()
        {
            ProgressBarImage.type = Image.Type.Filled;
            ProgressBarImage.fillMethod = FillMethod;
        }

        /// <summary>
        /// Resets the progress bar to 0 fill immediately
        /// </summary>
        public void Reset()
        {
            StopAllCoroutines();
            ProgressBarImage.fillAmount = 0;
            Progress = 0;
        }

        /// <summary>
        /// Stops the animation at the current progress
        /// </summary>
        public void StopAnimation()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Animate the progress bar from current fill to the provided Progress
        /// </summary>
        /// <param name="Progress">0-1 value representing the percent to fill the progress bar</param>
        public virtual void SetProgress(float Progress)
        {
            if (Progress > 1 || Progress < 0)
            {
                Debug.LogWarning(string.Format(OutOfRangeProgressMessage, Progress));
            }

            StopAllCoroutines();

            startingProgress = ProgressBarImage.fillAmount;
            targetProgress = Progress;

            StartCoroutine(AnimateProgress());
        }

        /// <summary>
        /// Immediately set the progress bar to the provided Progress (no animation)
        /// </summary>
        /// <param name="Progress">0-1 value representing the percent to fill the progress bar</param>
        public virtual void SetProgressImmediate(float Progress)
        {
            if (Progress > 1 || Progress < 0)
            {
                Debug.LogWarning(string.Format(OutOfRangeProgressMessage, Progress));
            }

            ProgressBarImage.fillAmount = Progress;
            this.Progress = Progress;
        }

        /// <summary>
        /// Animate the progress bar starting from empty to the provided Progress
        /// </summary>
        /// <param name="Progress">0-1 value representing the percent to fill the progress bar</param>
        public virtual void AnimateFromZero(float Progress)
        {
            ProgressBarImage.fillAmount = 0;
            this.Progress = 0;
            SetProgress(Progress);
        }

        /// <summary>
        /// Coroutine to animate the Progress. Should not be called directly
        /// </summary>
        /// <returns>Coroutine</returns>
        protected virtual IEnumerator AnimateProgress()
        {
            Wait = new WaitForSeconds(1f / AnimationFPS);
            EndTime = Time.time + AnimationTime;

            while (Time.time < EndTime)
            {
                ProgressBarImage.fillAmount = Mathf.Lerp(startingProgress, targetProgress, 1 - ((EndTime - Time.time) / AnimationTime));
                Progress = ProgressBarImage.fillAmount;

                yield return Wait;
            }

            ProgressBarImage.fillAmount = targetProgress;
            Progress = ProgressBarImage.fillAmount;

            if (OnCompleted != null)
            {
                OnCompleted.Invoke();
            }
        }
    }
}
