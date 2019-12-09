using TMPro;
using UnityEngine;

namespace LlamaSoftware.UI
{
    /// <summary>
    /// Sets the value of this TMP_InputField to the value received from a Slider.
    /// Attach this to a TMP_InputField, bind the OnChange event of a Slider to call SetValue.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldSliderValueDisplay : MonoBehaviour
    {
        private TMP_InputField InputField;
        /// <summary>
        /// Number of decimals to display in the InputField
        /// </summary>
        public int Decimals = 2;

        private const string NumberFormat = "N{0}";

        private void Awake()
        {
            InputField = GetComponent<TMP_InputField>();
        }

        /// <summary>
        /// Bind this to the OnChange event of a Slider. 
        /// Sets the value of the InputField to the received value.
        /// </summary>
        /// <param name="value">Value to display</param>
        public void SetValue(float value)
        {
            InputField.text = value.ToString(string.Format(NumberFormat, Decimals));
        }
    }
}
