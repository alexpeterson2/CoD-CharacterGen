
namespace LlamaSoftware.UI.Utility
{
    /// <summary>
    /// Collection of all constants used in Animations
    /// </summary>
    public class UIAnimationConstants
    {
        public const string FADE_IN = "FadeIn";
        public const string FADE_OUT = "FadeOut";

        public const string SLIDE_IN = "SlideIn";
        public const string SLIDE_OUT = "SlideOut";

        public const string ZOOM_IN = "ZoomIn";
        public const string ZOOM_OUT = "ZoomOut";

        public const string TOP = "Top";
        public const string RIGHT = "Right";
        public const string BOTTOM = "Bottom";
        public const string LEFT = "Left";

        public const string DISABLED = "Disabled";
        public const string ENABLED = "Enabled";

        public const string SELECTED = "Selected";
        public const string DESELECTED = "Deselected";

        public const string CLICKED = "Clicked";

        /// <summary>
        /// Page Entry and Exit Modes. You can define any custom animation and bind it to one of these modes in your RuntimeAnimatorController. 
        /// By Default the Page Controller has this all set up.
        /// </summary>
        public enum EntryMode
        {
            /// <summary>
            /// Page will not animate in or out. Useful if you want a static first page that never exits or enters
            /// </summary>
            DO_NOTHING,
            /// <summary>
            /// Slide in and out from a <see cref="Direction">Direction</see>.
            /// </summary>
            SLIDE,
            /// <summary>
            /// Zooms in and out from the middle of the screen (no <see cref="Direction">Direction</see>)
            /// </summary>
            ZOOM,
            /// <summary>
            /// Fades in and out
            /// </summary>
            FADE
        }

        /// <summary>
        /// Used to specify which direction a <see cref="EntryMode.SLIDE">SLIDE</see> animation should occur.
        /// </summary>
        public enum Direction
        {
            TOP,
            RIGHT,
            BOTTOM,
            LEFT
        }
    }
}
