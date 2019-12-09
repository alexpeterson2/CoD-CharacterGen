using System.Collections;
using TMPro;
using UnityEngine;

namespace LlamaSoftware.UI.Utility
{
    /// <summary>
    /// Utility class to update a given TextMeshProUGUI component with the given progress.
    /// </summary>
    public class TextUpdatingProgressBar : SimpleProgressBar
    {
        /// <summary>
        /// Text to update
        /// </summary>
        public TextMeshProUGUI Text;
        /// <summary>
        /// Suffix to append to the provided percent
        /// </summary>
        public string Suffix = "%";
        /// <summary>
        /// Prefix to prepend to the provided percent
        /// </summary>
        public string Prefix = "";
        /// <summary>
        /// If percent should be displayed as 0.85 or 85
        /// </summary>
        public bool ShowAsDecimal = false;
        /// <summary>
        /// Maximum number of decimals to display
        /// </summary>
        public int DecimalsToShow = 1;

        private string DisplayFormat = "{0}{1}{2}";
        private string DecimalDisplayFormat = "N{0}";

        /// <summary>
        /// Animate the progress bar from current fill to the provided Progress. Also updates the text.
        /// </summary>
        /// <param name="Progress">0-1 value representing the percent to fill the progress bar</param>
        public override void SetProgress(float Progress)
        {
            base.SetProgress(Progress);

            SetText();
        }

        /// <summary>
        /// Immediately set the progress bar to the provided Progress (no animation). Also updates the text.
        /// </summary>
        /// <param name="Progress">0-1 value representing the percent to fill the progress bar</param>
        public override void SetProgressImmediate(float Progress)
        {
            base.SetProgressImmediate(Progress);

            SetText();
        }

        /// <summary>
        /// Coroutine to animate progress and update text.
        /// Do not call directly. Called in SimpleProgressBar.
        /// </summary>
        /// <returns>Coroutine</returns>
        protected override IEnumerator AnimateProgress()
        {
            Wait = new WaitForSeconds(1f / AnimationFPS);
            EndTime = Time.time + AnimationTime;

            while (Time.time < EndTime)
            {
                ProgressBarImage.fillAmount = Mathf.Lerp(startingProgress, targetProgress, 1 - ((EndTime - Time.time) / AnimationTime));
                SetText();
                yield return Wait;
            }

            ProgressBarImage.fillAmount = targetProgress;
            SetText();
            if (OnCompleted != null)
            {
                OnCompleted.Invoke();
            }
        }

        private void SetText()
        {
            Text.SetText(string.Format(DisplayFormat, Prefix, ShowAsDecimal ? ProgressBarImage.fillAmount.ToString(string.Format(DecimalDisplayFormat, DecimalsToShow)) : (ProgressBarImage.fillAmount * 100).ToString(string.Format(DecimalDisplayFormat, DecimalsToShow)), Suffix));
        }
    }
}
