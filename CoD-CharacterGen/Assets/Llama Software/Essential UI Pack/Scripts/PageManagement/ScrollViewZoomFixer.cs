using UnityEngine;
using UnityEngine.UI;

namespace LlamaSoftware.UI.Utility
{
    /// <summary>
    /// Utility class to fix issues with ScrollRects when adjusting scale with ZOOM animation specifically. 
    /// Optionally will always scroll to top of a ScrollRect when a <see cref="Page">Page</see> enters.
    /// </summary>
    public class ScrollViewZoomFixer : MonoBehaviour
    {
        /// <summary>
        /// If you would like to force scrolling to the top or not
        /// </summary>
        public bool ForceScrollToStart;
        /// <summary>
        /// The ScrollRect to operate on
        /// </summary>
        public ScrollRect ScrollView;

        private bool vertical;
        private bool horizontal;

        private void Start()
        {
            vertical = ScrollView.vertical;
            horizontal = ScrollView.horizontal;
        }

        /// <summary>
        /// Bind to PostPushPage to fix ScrollView issues with ZOOM animation
        /// </summary>
        public void PostPushPage()
        {
            ScrollView.horizontal = horizontal;
            ScrollView.vertical = vertical;
            if (ForceScrollToStart)
            {
                ScrollView.horizontalNormalizedPosition = 1;
                ScrollView.verticalNormalizedPosition = 1;
            }
        }

        /// <summary>
        /// Bind to PrePopPage to fix ScrollView issues with ZOOM animation
        /// </summary>
        public void PrePopPage()
        {
            ScrollView.horizontal = false;
            ScrollView.vertical = false;
        }
    }
}
