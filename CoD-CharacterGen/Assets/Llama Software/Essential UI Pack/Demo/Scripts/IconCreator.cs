using System.Collections.Generic;
using UnityEngine;

namespace LlamaSoftware.UI.Demo
{
    public class IconCreator : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> Icons = new List<Sprite>();
        [SerializeField]
        private List<Border> Borders = new List<Border>();
        [SerializeField]
        private IconDemo IconPrefab = null;
        [SerializeField]
        private RectTransform ContentPanel = null;

        private void Start()
        {
            int i = 0;
            Icons.ForEach(icon =>
            {
                IconDemo Icon = GameObject.Instantiate(IconPrefab);
                Icon.transform.SetParent(ContentPanel, false);

                Icon.Icon.sprite = icon;
                Icon.Border.sprite = Borders[i].BorderSprite;
                Icon.Border.color = Borders[i].Color;

                i++;
                if (i >= Borders.Count)
                {
                    i = 0;
                }
            });
        }

        [System.Serializable]
        public class Border
        {
            public Sprite BorderSprite;
            public Color Color = Color.white;
        }
    }
}
