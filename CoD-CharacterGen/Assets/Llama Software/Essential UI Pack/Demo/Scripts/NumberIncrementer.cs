using TMPro;
using UnityEngine;

namespace LlamaSoftware.UI.Demo
{
    public class NumberIncrementer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI Text = null;
        [SerializeField]
        private string Prefix = string.Empty;

        private int Number = 0;
        private const string format = "{0}{1}";
        public void IncrementText()
        {
            Number++;
            Text.SetText(string.Format(format, Prefix, Number));
        }
    }
}
