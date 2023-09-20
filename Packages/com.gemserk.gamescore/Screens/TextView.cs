using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens
{
    public interface ITextView
    {
        void SetText(object text);

        string GetText();

        string text
        {
            get;
            set;
        }
        
        Color color
        {
            get;
            set;
        }
    }
    
    public class TextView : MonoBehaviour, ITextView
    {
        private Text legacyText;
        private TextMeshProUGUI textMeshPro;

        private void CacheTextElement()
        {
            if (legacyText != null || textMeshPro != null)
            {
                return;
            }
            
            legacyText = GetComponent<Text>();
            textMeshPro = GetComponent<TextMeshProUGUI>();

            if (legacyText == null && textMeshPro == null)
            {
                throw new Exception("Must have at least one text element");
            }
        }

        public void SetText(object text)
        {
            CacheTextElement();
            
            if (legacyText != null)
            {
                legacyText.text = text.ToString();
            }

            if (textMeshPro != null)
            {
                textMeshPro.text = text.ToString();
            }
        }
        
        public string GetText()
        {
            CacheTextElement();
            
            if (legacyText != null)
            {
                return legacyText.text;
            }

            if (textMeshPro != null)
            {
                return textMeshPro.text;
            }

            throw new Exception("Must have at least one text element");
        }

        public string text
        {
            get => GetText();
            set => SetText(value);
        }

        public Color color
        {
            get
            {
                CacheTextElement();
                
                if (legacyText != null)
                {
                    return legacyText.color;
                }
                
                if (textMeshPro != null)
                {
                    return textMeshPro.color;
                }

                throw new InvalidOperationException("Cant get color if no text component");
            }
            set
            {
                CacheTextElement();
                
                if (legacyText != null)
                {
                    legacyText.color = value;
                }
                
                if (textMeshPro != null)
                {
                    textMeshPro.color = value;
                }
            }
        }
    }
}