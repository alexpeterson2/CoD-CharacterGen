using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LlamaSoftware.UI.Utility
{
    /// <summary>
    /// The Controller for all of your pages. Use only 1 per Canvas.
    /// All Page Management should be done via a single instance of MenuController.
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(Animator))]
    public class MenuController : MonoBehaviour
    {
        private Canvas RootCanvas;
        private Animator CanvasAnimator;
        /// <summary>
        /// The Page to push on start.
        /// </summary>
        public Page InitialPage;
        private bool IsVisible;
        /// <summary>
        /// The first GameObject you would like the current <seealso cref="EventSystem">EventSystem</seealso> to select
        /// </summary>
        public GameObject FirstFocusItem;
        /// <summary>
        /// If enabled, will process Tab and Shift+Tab as navigation events similar to what you expect from an HTML page, or forms in general.
        /// </summary>
        public bool EnableTabSelection = true;
        /// <summary>
        /// If enabled, will support pressing ESC key to pop pages, and fade out the Canvas (if CanvasGroup is attached) when all pages are popped
        /// </summary>
        public bool EnableEscapeToGoBack = true;
        /// <summary>
        /// If true, will make the Canvas fade out when all pages have been popped. Pressing Escape again will make the Canvas fade back in.
        /// </summary>
        public bool FadeOutCanvasWhenNoPagesRemaining = true;
        /// <summary>
        /// When enabled, will force the Menu Controller to retain 1 page in the stack at all times. This overrides the previous property <see cref="FadeOutCanvasWhenNoPagesRemaining">Fade Out Canvas When No Pages Remaining</see>. 
        /// This means if both are enabled, the Canvas will never fade out, and OnHide events will never be called.
        /// </summary>
        public bool ForceKeepOnePage = false;
        private GameObject CurrentSelectedObject;
        private Selectable CurrentSelectable;
        private Selectable NextSelectable;

        /// <summary>
        /// UnityEvent to call when the Canvas becomes inactive. Requires <see cref="FadeOutCanvasWhenNoPagesRemaining">Fade Out Canvas When No Pages Remaining</see> to be enabled.
        /// </summary>
        public UnityEvent OnHide;
        /// <summary>
        /// UnityEvent to call when the Canvas becomes active. Will be invoked every time the Page Stack goes from 0 to 1 in size (including on Awake if <see cref="InitialPage">Initial Page</see> is set). 
        /// </summary>
        public UnityEvent OnShow;

        private Stack<Page> PageStack = new Stack<Page>();

        #region avoid gc alloc
        private string NO_PAGES_ON_STACK_WARNING = "Trying to pop page when there are none on the stack!";
        #endregion

        private void Awake()
        {
            RootCanvas = GetComponent<Canvas>();
            CanvasAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            IsVisible = true;

            if (FirstFocusItem != null)
            {
                EventSystem.current.SetSelectedGameObject(FirstFocusItem);
            }

            if (InitialPage != null)
            {
                PushPage(InitialPage);
            }
        }

        private void Update()
        {
            if (RootCanvas.enabled && RootCanvas.gameObject.activeInHierarchy)
            {
                if (EnableEscapeToGoBack && Input.GetKeyUp(KeyCode.Escape))
                {
                    if (PageStack.Count != 0)
                    {
                        PopPage();
                    }
                    else
                    {
                        PushPage(InitialPage);
                    }
                }
                else if (EnableTabSelection && Input.GetKeyUp(KeyCode.Tab))
                {
                    HandleTabSelect();
                }
            }
            else
            {
                if (EnableEscapeToGoBack && Input.GetKeyUp(KeyCode.Escape))
                {
                    if (PageStack.Count != 0)
                    {
                        PopPage();
                    }
                }
            }
        }

        /// <summary>
        /// Add a new Page on top of the Page stack and plays appropriate animations.
        /// </summary>
        /// <param name="page">The Page to add</param>
        public void PushPage(Page page)
        {
            page.Enter(true);

            if (PageStack.Count == 0)
            {
                if (FadeOutCanvasWhenNoPagesRemaining)
                {
                    FadeIn();
                }
            }
            else
            {
                Page currentPage = PageStack.Peek();
                DisableSelectablesOnPage(currentPage);

                if (currentPage.ExitOnNewPagePush)
                {
                    currentPage.Exit(false);
                }
            }

            PageStack.Push(page);
            EnableSelectablesOnPage(page);
        }

        private void DisableSelectablesOnPage(Page page)
        {
            foreach (Selectable selectable in page.GetComponentsInChildren<Selectable>())
            {
                selectable.enabled = false;
            }
        }

        private void EnableSelectablesOnPage(Page page)
        {
            int i = 0;
            foreach (Selectable selectable in page.GetComponentsInChildren<Selectable>())
            {
                selectable.enabled = true;
                if (i == 0 && !EventSystem.current.alreadySelecting)
                {
                    EventSystem.current.SetSelectedGameObject(selectable.gameObject);
                }

                i++;
            }
        }

        /// <summary>
        /// Remove the top Page from the stack and plays appropriate animations
        /// </summary>
        public void PopPage()
        {
            if (PageStack.Count == 1 && ForceKeepOnePage)
            {
                return;
            }
            else if (PageStack.Count > 0)
            {
                Page page = PageStack.Pop();
                page.Exit(true);
                DisableSelectablesOnPage(page);

                if (PageStack.Count == 0)
                {
                    if (FadeOutCanvasWhenNoPagesRemaining)
                    {
                        FadeOut();
                    }
                }
                else
                {
                    Page newCurrentPage = PageStack.Peek();
                    EnableSelectablesOnPage(newCurrentPage);
                    if (newCurrentPage.ExitOnNewPagePush)
                    {
                        newCurrentPage.Enter(false);
                    }
                }
            }
            else
            {
                Debug.LogWarning(NO_PAGES_ON_STACK_WARNING);
            }
        }

        /// <summary>
        /// Remove all Pages from the stack and fades out the Canvas
        /// </summary>
        public void PopAllPages()
        {
            for (int i = 0; i < PageStack.Count; i++)
            {
                PopPage();
            }
        }

        private void HandleTabSelect()
        {
            CurrentSelectedObject = EventSystem.current.currentSelectedGameObject;
            CurrentSelectable = CurrentSelectedObject != null ? CurrentSelectedObject.GetComponent<Selectable>() : null;
            if (CurrentSelectedObject != null && CurrentSelectedObject != null)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (CurrentSelectable.navigation.mode == Navigation.Mode.Automatic || CurrentSelectable.navigation.mode == Navigation.Mode.Horizontal)
                    {
                        NextSelectable = CurrentSelectable.FindSelectableOnLeft();
                        if (NextSelectable == null)
                        {
                            NextSelectable = CurrentSelectable.FindSelectableOnUp();
                        }
                    }
                    else
                    {
                        NextSelectable = CurrentSelectable.FindSelectableOnUp();
                        if (NextSelectable == null)
                        {
                            NextSelectable = CurrentSelectable.FindSelectableOnLeft();
                        }
                    }
                }
                else
                {
                    if (CurrentSelectable.navigation.mode == Navigation.Mode.Automatic || CurrentSelectable.navigation.mode == Navigation.Mode.Horizontal)
                    {
                        NextSelectable = CurrentSelectable.FindSelectableOnRight();
                        if (NextSelectable == null)
                        {
                            NextSelectable = CurrentSelectable.FindSelectableOnDown();
                        }
                    }
                    else
                    {
                        NextSelectable = CurrentSelectable.FindSelectableOnDown();
                        if (NextSelectable == null)
                        {
                            NextSelectable = CurrentSelectable.FindSelectableOnRight();
                        }
                    }
                }

                if (NextSelectable != null)
                {
                    NextSelectable.Select();
                    NextSelectable = null;
                }
                else
                {
                    SelectFirstSelectable();
                }
            }
            else
            {
                SelectFirstSelectable();
            }
        }

        private void SelectFirstSelectable()
        {
            CurrentSelectable = Selectable.allSelectables[0];
            if (CurrentSelectable != null)
            {
                CurrentSelectable.Select();
            }
        }

        /// <summary>
        /// Fade in the root Canvas. Will call any UnityEvents bound to OnShow.
        /// Calls <see cref="UIAnimationConstants">UIAnimationConstants.FADE_IN</see> on the connected Animator.
        /// </summary>
        public void FadeIn()
        {
            if (!IsVisible)
            {
                CanvasAnimator.SetTrigger(UIAnimationConstants.FADE_IN);
                IsVisible = true;
                if (OnShow != null)
                {
                    OnShow.Invoke();
                }
            }
        }

        /// <summary>
        /// Fade out the root Canvas. Will call any UnityEvents bound to OnHide.
        /// Calls <see cref="UIAnimationConstants">UIAnimationConstants.FADE_OUT</see> on the connected Animator.
        /// </summary>
        public void FadeOut()
        {
            if (IsVisible)
            {
                CanvasAnimator.SetTrigger(UIAnimationConstants.FADE_OUT);
                IsVisible = false;

                if (OnHide != null)
                {
                    OnHide.Invoke();
                }
            }
        }
    }
}
