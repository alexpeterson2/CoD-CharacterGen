using UnityEngine;
using UnityEngine.UI;

namespace LlamaSoftware.UI
{
    /// <summary>
    /// Sets the value of a Slider.
    /// Bind the <see cref="SetValue(string)">SetValue</see> method to the OnEndEdit event of a TMP_InputField.
    /// Do NOT bind it to the OnChange event of a TMP_InputField - this will result in incorrect behavior when using a Slider OnChange to set the value of an InputField
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class SliderValueSetter : MonoBehaviour
    {
        private Slider Slider;

        private void Awake()
        {
            Slider = GetComponent<Slider>();
        }

        /// <summary>
        /// Bind to the OnEndEdit event of a TMP_InputField. 
        /// Will set the Slider value to the value passed in.
        /// </summary>
        /// <param name="text">new Slider value</param>
        public void SetValue(string text)
        {
            Slider.value = float.Parse(text);
        }
    }
}
