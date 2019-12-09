using LlamaSoftware.UI.Utility;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LlamaSoftware.UI
{
    /// <summary>
    /// Component that supports swapping between options with left/right arrows. Comes fully 
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Selectable), typeof(EventTrigger))]
    public class MultiValueToggle : MonoBehaviour
    {
        #region Editor Controls
        /// <summary>
        /// Do not modify this directly, use the provided methods to interact with it. Public only for inspector.
        /// List of values to allow user to select from
        /// </summary>
        public List<string> Values;
        /// <summary>
        /// The initial index to select (0 = first item)
        /// </summary>
        public int StartIndex;
        /// <summary>
        /// UnityEvent to call when value changes
        /// </summary>
        public UnityEvent ChangeCallback;
        /// <summary>
        /// Delay between processing left/right arrow key presses. Too low a value will increase the risk of accidental "double taps".
        /// Too high a value will make the component appear unresponsive
        /// </summary>
        public float KeyboardDelay = .2f;
        /// <summary>
        /// Get only. The currently selected index.
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return _CurrentIndex;
            }
        }
        /// <summary>
        /// If enabled, allows decrementing at index 0 to wrap to the highest index and incrementing at highest index to wrap to index 0
        /// </summary>
        public bool AllowWrapping;
        #endregion

        #region MultIValueToggle internal variables
        private int _CurrentIndex;
        private Animator Animator;
        private TextMeshProUGUI DisplayText;
        private Selectable Selectable;
        private EventTrigger EventTrigger;
        private ScrollRect ParentScrollView;
        private float LastKeypressTime;

        // Generate strings specific for MVT
        private const string FADE_OUT_LEFT = UIAnimationConstants.FADE_OUT + UIAnimationConstants.LEFT;
        private const string FADE_IN_LEFT = UIAnimationConstants.FADE_IN + UIAnimationConstants.LEFT;

        private const string FADE_OUT_RIGHT = UIAnimationConstants.FADE_OUT + UIAnimationConstants.RIGHT;
        private const string FADE_IN_RIGHT = UIAnimationConstants.FADE_IN + UIAnimationConstants.RIGHT;

        private const string CLICKED_LEFT = UIAnimationConstants.CLICKED + UIAnimationConstants.LEFT;
        private const string CLICKED_RIGHT = UIAnimationConstants.CLICKED + UIAnimationConstants.RIGHT;
        #endregion

        #region Public Controls - these can be used by your scripts, or via the editor
        /// <summary>
        /// Decrements the current index value. Call MultiValueToggle.GetCurrentValue() in your ChangeCallback, or after calling this to get the updated result.
        /// </summary>
        public void Decrement()
        {
            if (CanDecrement())
            {
                _CurrentIndex -= 1;

                Animator.ResetTrigger(CLICKED_LEFT);
                Animator.SetTrigger(CLICKED_LEFT);

                if (_CurrentIndex < 0)
                {
                    _CurrentIndex = Values.Count - 1;
                }

                UpdateValue();
                // When performing a click, we lose focus from EventSystem, so reselect us
                EventSystem.current.SetSelectedGameObject(this.gameObject);
            }
            else
            {
                Debug.LogWarning(name + " is trying to decrement value but already is at minimum value. Check what is calling Decrement in this case!");
            }
        }

        /// <summary>
        /// Whether or not this MultiValueToggle can be decremented
        /// </summary>
        /// <returns>Whether or not this MultiValueToggle can be decremented</returns>
        public bool CanDecrement()
        {
            if (!AllowWrapping)
            {
                return _CurrentIndex > 0;
            }

            return true;
        }

        /// <summary>
        /// Increments the current index value. Call MultiValueToggle.GetCurrentValue() in your ChangeCallback, or after calling this to get the updated result.
        /// </summary>
        public void Increment()
        {
            if (CanIncrement())
            {
                Animator.ResetTrigger(CLICKED_RIGHT);
                Animator.SetTrigger(CLICKED_RIGHT);

                _CurrentIndex += 1;

                if (_CurrentIndex >= Values.Count)
                {
                    _CurrentIndex = 0;
                }

                UpdateValue();
                // When performing a click, we lose focus from EventSystem, so reselect us
                EventSystem.current.SetSelectedGameObject(this.gameObject);
            }
            else
            {
                Debug.LogWarning(name + " is trying to increment value but already is at the maximum value. Check what is calling Increment in this case!");
            }
        }

        /// <summary>
        /// Whether or not this MultiValueToggle can be incremented
        /// </summary>
        /// <returns>Whether or not this MultiValueToggle can be incremented</returns>
        public bool CanIncrement()
        {
            if (!AllowWrapping)
            {
                return _CurrentIndex < Values.Count - 1;
            }

            return true;
        }

        /// <summary>
        /// Returns the Value at the current index
        /// </summary>
        /// <returns>Current Value</returns>
        public string GetCurrentValue()
        {
            return Values[_CurrentIndex];
        }

        /// <summary>
        /// Returns a copy of the list of Values
        /// </summary>
        /// <returns>List of Values that can be chosen from</returns>
        public List<string> GetAllValues()
        {
            return new List<string>(Values);
        }

        /// <summary>
        /// Sets the current index to the specified index
        /// </summary>
        /// <param name="index">Index to set.</param>
        public void SetCurrentIndex(int index)
        {
            _CurrentIndex = index;
            if (ChangeCallback != null)
            {
                ChangeCallback.Invoke();
            }
            UpdateText();
        }

        /// <summary>
        /// Sets the current index to the specified value
        /// </summary>
        /// <param name="value">Value to set. Must be within the set of available Values</param>
        public void SetValue(string value)
        {
            if (Values.Contains(value))
            {
                _CurrentIndex = Values.IndexOf(value);
                if (ChangeCallback != null)
                {
                    ChangeCallback.Invoke();
                }
                UpdateText();
            }
        }

        /// <summary>
        /// Adds a specific value to the list
        /// </summary>
        /// <param name="value">Value to add</param>
        public void AddValue(string value)
        {
            Values.Add(value);
        }

        /// <summary>
        /// Removes a specific value from the list
        /// </summary>
        /// <param name="value">Value to remove</param>
        public void RemoveValue(string value)
        {
            Values.Remove(value);
        }

        /// <summary>
        /// Adds the specified values to the list of available values
        /// </summary>
        /// <param name="values">Values to add</param>
        public void AddValues(IEnumerable<string> values)
        {
            Values.AddRange(values);
        }

        /// <summary>
        /// Removes the specified values from the list of available values
        /// </summary>
        /// <param name="values">Values to remove</param>
        public void RemoveValues(IEnumerable<string> values)
        {
            Values.RemoveAll(value => values.GetEnumerator().Current.Contains(value));
        }

        /// <summary>
        /// Update the inner text of the MultiValueToggle based on the current index
        /// </summary>
        public void UpdateText()
        {
            if (_CurrentIndex >= 0 && _CurrentIndex < Values.Count)
            {
                DisplayText.text = Values[_CurrentIndex];
                UpdateValue();
            }
            else
            {
                Debug.LogWarning("MultiValueToggle trying to update text to index out of range. Current Index: " + _CurrentIndex.ToString() + ", but Values only has: " + Values.Count.ToString());
            }
        }

        /// <summary>
        /// Disables the MultiValueToggle
        /// </summary>
        public void Disable()
        {
            Selectable.interactable = false;
            EventTrigger.enabled = false;
            Animator.SetTrigger(UIAnimationConstants.DISABLED);
        }

        /// <summary>
        /// Enables the MultiValueToggle
        /// </summary>
        public void Enable()
        {
            Selectable.interactable = true;
            EventTrigger.enabled = true;
            Animator.SetTrigger(UIAnimationConstants.ENABLED);
        }

        /// <summary>
        /// Intended for EventTrigger use only
        /// </summary>
        /// <param name="pointerEvent">Pointer Event Data from the EventTrigger</param>
        public void UpdateParentScrollView(BaseEventData pointerEvent)
        {
            if (ParentScrollView != null)
            {
                ParentScrollView.OnScroll((PointerEventData)pointerEvent);
            }
        }
        #endregion

        #region MultiValueToggle Internals
        private void Awake()
        {
            _CurrentIndex = StartIndex;
            Animator = GetComponent<Animator>();
            DisplayText = GetComponentInChildren<TextMeshProUGUI>();
            Selectable = GetComponent<Selectable>();
            EventTrigger = GetComponent<EventTrigger>();
            ParentScrollView = GetComponentInParent<ScrollRect>();
            UpdateText();
        }

        private void UpdateValue()
        {
            DisplayText.SetText(Values[_CurrentIndex]);
            if (ChangeCallback != null)
            {
                ChangeCallback.Invoke();
            }

            if (!AllowWrapping)
            {
                UpdateArrowVisibilities();
            }
        }

        private void UpdateArrowVisibilities()
        {
            if (_CurrentIndex == 0)
            {
                Animator.SetTrigger(FADE_OUT_LEFT);
            }
            else if (_CurrentIndex == Values.Count - 1)
            {
                Animator.SetTrigger(FADE_OUT_RIGHT);
            }

            if (_CurrentIndex == 1)
            {
                Animator.SetTrigger(FADE_IN_LEFT);
            }
            if (_CurrentIndex == Values.Count - 2)
            {
                Animator.SetTrigger(FADE_IN_RIGHT);
            }
        }

        private void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == this.gameObject)
            {
                if (Input.GetKey(KeyCode.RightArrow) && Time.time > LastKeypressTime + KeyboardDelay && CanIncrement())
                {
                    Increment();
                    LastKeypressTime = Time.time;
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && Time.time > LastKeypressTime + KeyboardDelay && CanDecrement())
                {
                    Decrement();
                    LastKeypressTime = Time.time;
                }
            }
        }
        #endregion
    }
}