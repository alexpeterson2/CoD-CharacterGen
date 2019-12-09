using LlamaSoftware.UI.Utility;
using UnityEngine;

namespace LlamaSoftware.UI
{
    /// <summary>
    /// Simple Popover. Manages Visibility and FadeIn/Out of the Popover
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(CanvasGroup))]
    public class Popover : MonoBehaviour
    {
        private Animator Animator;
        private bool IsVisible;

        private void Start()
        {
            IsVisible = false;

            Animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Shows the popover. Will do nothing if already showing.
        /// </summary>
        public void Show()
        {
            if (!IsVisible)
            {
                Animator.SetTrigger(UIAnimationConstants.FADE_IN);
                IsVisible = true;
            }
        }

        /// <summary>
        /// Hides the popover. Will do nothing if already hiding
        /// </summary>
        public void Hide()
        {
            if (IsVisible)
            {
                Animator.SetTrigger(UIAnimationConstants.FADE_OUT);
                IsVisible = false;
            }
        }

        /// <summary>
        /// Inverts the state of the popover. If already showing, will hide. If hidden, will show. Useful for single binding onClick in the inspector
        /// </summary>
        public void Toggle()
        {
            if (IsVisible)
            {
                Hide();
            }
            else
            {
                Show(); 
            }
        }
    }
}
