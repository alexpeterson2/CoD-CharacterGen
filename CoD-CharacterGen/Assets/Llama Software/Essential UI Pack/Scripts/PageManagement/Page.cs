#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LlamaSoftware.UI.Utility
{
    [RequireComponent(typeof(Animator), typeof(AudioSource))]
    public class Page : MonoBehaviour
    {
        #region Required Components
        private AudioSource audioSource;
        private Animator animator;
        #endregion

        #region Configuration
        /// <summary>
        /// If true, MenuController will call Exit when a new page is pushed above this, and Enter when this page is on top of the stack again
        /// </summary>
        public bool ExitOnNewPagePush = false;
        /// <summary>
        /// The AudioClip to play when page is coming in. Will not play when entering due to ExitOnNewPagePush.
        /// </summary>
        public AudioClip EntryClip;
        /// <summary>
        /// The AudioClip to play when page is exiting. Will not play when exiting due to ExitOnNewPagePush.
        /// </summary>
        public AudioClip ExitClip;
        /// <summary>
        /// The Animation mode to use to have the page "Enter"
        /// </summary>
        public UIAnimationConstants.EntryMode EntryMode = UIAnimationConstants.EntryMode.SLIDE;
        /// <summary>
        /// The Direction the page should "Enter"
        /// </summary>
        public UIAnimationConstants.Direction EntryDirection = UIAnimationConstants.Direction.LEFT;
        /// <summary>
        /// The Animation mode to use to have the page "Exit"
        /// </summary>
        public UIAnimationConstants.EntryMode ExitMode = UIAnimationConstants.EntryMode.SLIDE;
        /// <summary>
        /// The Direction the page should "Exit"
        /// </summary>
        public UIAnimationConstants.Direction ExitDirection = UIAnimationConstants.Direction.LEFT;
        /// <summary>
        /// Action to call before pushing a new page (before Enter is called). You should do any required data setup here.
        /// </summary>
        public UnityEvent PrePushAction;
        /// <summary>
        /// Action to call after page is visible (after Enter animation has completed). You should do things like layout adjustments and selecting objects here.
        /// </summary>
        public UnityEvent PostPushAction;
        /// <summary>
        /// Action to call before the page is removed from the screen (before Exit is called). Do not reset the state of the page here because the user can still see this.
        /// </summary>
        public UnityEvent PrePopAction;
        /// <summary>
        /// Action to call after the page is no longer visible (after Exit animation has completed). You can reset the state of the page here.
        /// </summary>
        public UnityEvent PostPopAction;
        #endregion

        #region pre-allocate all animation trigger strings
        private const string SLIDE_IN_LEFT = UIAnimationConstants.SLIDE_IN + UIAnimationConstants.LEFT;
        private const string SLIDE_IN_TOP = UIAnimationConstants.SLIDE_IN + UIAnimationConstants.TOP;
        private const string SLIDE_IN_BOTTOM = UIAnimationConstants.SLIDE_IN + UIAnimationConstants.BOTTOM;
        private const string SLIDE_IN_RIGHT = UIAnimationConstants.SLIDE_IN + UIAnimationConstants.RIGHT;

        private const string SLIDE_OUT_LEFT = UIAnimationConstants.SLIDE_OUT + UIAnimationConstants.LEFT;
        private const string SLIDE_OUT_TOP = UIAnimationConstants.SLIDE_OUT + UIAnimationConstants.TOP;
        private const string SLIDE_OUT_BOTTOM = UIAnimationConstants.SLIDE_OUT + UIAnimationConstants.BOTTOM;
        private const string SLIDE_OUT_RIGHT = UIAnimationConstants.SLIDE_OUT + UIAnimationConstants.RIGHT;
        #endregion
#if UNITY_EDITOR
        /// <summary>
        /// Used to allow creating pages from the menu
        /// </summary>
        [MenuItem("GameObject/UI/Page", priority = 0)]
        public static void CreateNewPage()
        {
            GameObject newPage = new GameObject("Page");
            Undo.RegisterCreatedObjectUndo(newPage, "Create Page");

            newPage.AddComponent<Image>();
            newPage.AddComponent<Page>();

            // If user isn't right clicking an item in the Canvas to add the page to
            if (Selection.activeTransform == null || Selection.activeGameObject.GetComponentInParent<Canvas>() == null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                // and there's no Canvas
                if (canvas == null)
                {
                    // Create one and use that
                    GameObject newCanvas = new GameObject("Canvas");
                    newPage.transform.SetParent(newCanvas.transform, false);
                }
                else
                {
                    // Otherwise, set as a child of this Canvas
                    newPage.transform.SetParent(canvas.transform, false);
                }
            }
            else
            {
                newPage.transform.SetParent(Selection.activeTransform, false);
            }

            RectTransform rectTransform = newPage.GetComponent<RectTransform>();
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;

            RuntimeAnimatorController animatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Llama Software/Essential UI Pack/Animations/Pages/Simple/Page");
            if (animatorController != null)
            {
                newPage.GetComponent<Animator>().runtimeAnimatorController = animatorController;
            }

            Selection.activeGameObject = newPage.gameObject;
        }
#endif

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();

            //For each page, we want to disable their selectables so tab navigation cannot go off screen.
            foreach (Selectable selectable in GetComponentsInChildren<Selectable>())
            {
                selectable.enabled = false;
            }
        }

        protected virtual void Start()
        {
            //Configure properly the audio source
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0;
        }

        #region Public Controls (MenuController will use these)

        /// <summary>
        /// Animates the page in. Calls PrePushAction, then plays animation, then calls PostPushAction
        /// </summary>
        /// <param name="PlayAudio">If the audio clip should be played</param>
        public void Enter(bool PlayAudio)
        {
            if (PrePushAction != null)
            {
                PrePushAction.Invoke();
            }

            switch (EntryMode)
            {
                case UIAnimationConstants.EntryMode.SLIDE:
                    SlideIn(PlayAudio);
                    break;
                case UIAnimationConstants.EntryMode.ZOOM:
                    ZoomIn(PlayAudio);
                    break;
                case UIAnimationConstants.EntryMode.FADE:
                    FadeIn(PlayAudio);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Animates the page out. Calls PrePopAction, then plays animation, then calls PostPopAction
        /// </summary>
        public void Exit(bool PlayAudio)
        {
            if (PrePopAction != null)
            {
                PrePopAction.Invoke();
            }

            switch (ExitMode)
            {
                case UIAnimationConstants.EntryMode.SLIDE:
                    SlideOut(PlayAudio);
                    break;
                case UIAnimationConstants.EntryMode.ZOOM:
                    ZoomOut(PlayAudio);
                    break;
                case UIAnimationConstants.EntryMode.FADE:
                    FadeOut(PlayAudio);
                    break;
            }
        }

        /// <summary>
        /// Calls the PostPushAction if bound. Intended to be called by an AnimationEvent
        /// </summary>
        public void CallPostPushAction()
        {
            if (PostPushAction != null)
            {
                PostPushAction.Invoke();
            }
        }

        /// <summary>
        /// Calls the PostPopAction if bound. Intended to be called by an AnimationEvent
        /// </summary>
        public void CallPostPopAction()
        {
            if (PostPopAction != null)
            {
                PostPopAction.Invoke();
            }
        }

        public void SetExitOnNewPagePush(bool ExitOnNewPagePush)
        {
            this.ExitOnNewPagePush = ExitOnNewPagePush;
        }
        #endregion

        private void SlideIn(bool PlayAudio)
        {
            switch (EntryDirection)
            {
                case UIAnimationConstants.Direction.TOP:
                    animator.SetTrigger(SLIDE_IN_TOP);
                    break;
                case UIAnimationConstants.Direction.RIGHT:
                    animator.SetTrigger(SLIDE_IN_RIGHT);
                    break;
                case UIAnimationConstants.Direction.BOTTOM:
                    animator.SetTrigger(SLIDE_IN_BOTTOM);
                    break;
                case UIAnimationConstants.Direction.LEFT:
                    animator.SetTrigger(SLIDE_IN_LEFT);
                    break;
            }

            PlayEntryClip(PlayAudio);
        }

        private void SlideOut(bool PlayAudio)
        {
            switch (ExitDirection)
            {
                case UIAnimationConstants.Direction.TOP:
                    animator.SetTrigger(SLIDE_OUT_TOP);
                    break;
                case UIAnimationConstants.Direction.RIGHT:
                    animator.SetTrigger(SLIDE_OUT_RIGHT);
                    break;
                case UIAnimationConstants.Direction.BOTTOM:
                    animator.SetTrigger(SLIDE_OUT_BOTTOM);
                    break;
                case UIAnimationConstants.Direction.LEFT:
                    animator.SetTrigger(SLIDE_OUT_LEFT);
                    break;
            }

            PlayExitClip(PlayAudio);
        }

        private void ZoomIn(bool PlayAudio)
        {
            animator.SetTrigger(UIAnimationConstants.ZOOM_IN);
            PlayEntryClip(PlayAudio);
        }

        private void ZoomOut(bool PlayAudio)
        {
            animator.SetTrigger(UIAnimationConstants.ZOOM_OUT);
            PlayExitClip(PlayAudio);
        }

        private void FadeIn(bool PlayAudio)
        {
            animator.SetTrigger(UIAnimationConstants.FADE_IN);
            PlayEntryClip(PlayAudio);
        }

        private void FadeOut(bool PlayAudio)
        {
            animator.SetTrigger(UIAnimationConstants.FADE_OUT);
            PlayExitClip(PlayAudio);
        }

        private void PlayEntryClip(bool PlayAudio)
        {
            if (PlayAudio)
            {
                audioSource.PlayOneShot(EntryClip);
            }
        }

        private void PlayExitClip(bool PlayAudio)
        {
            if (PlayAudio)
            {
                audioSource.PlayOneShot(ExitClip);
            }
        }
    }
}
