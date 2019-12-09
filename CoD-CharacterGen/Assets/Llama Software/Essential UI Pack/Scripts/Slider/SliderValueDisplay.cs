using TMPro;
using UnityEngine;

namespace LlamaSoftware.UI
{
    /// <summary>
    /// Displays the value of a Slider on a TextMeshProUGUI component.
    /// Bind <see cref="SliderValueDisplay.SetText(float)">SetText</see> to the OnChange event of a Slider.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SliderValueDisplay : MonoBehaviour
    {
        /// <summary>
        /// Number of decimals to display
        /// </summary>
        public int Decimals = 0;

        private TextMeshProUGUI Text;
        private const string DisplayFormat = "N{0}";

        private void Awake()
        {
            Text = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Bind to the OnChange event of a Slider. 
        /// Will set the TextMeshProUGUI text to the value passed in.
        /// </summary>
        /// <param name="value">Value to display</param>
        public void SetText(float value)
        {
            Text.SetText(value.ToString(string.Format(DisplayFormat, Decimals)));
        }
    }
}
