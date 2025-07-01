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
            if (legacyText || textMeshPro)
            {
                return;
            }
            
            legacyText = GetComponent<Text>();
            textMeshPro = GetComponent<TextMeshProUGUI>();

            if (!legacyText && !textMeshPro)
            {
                throw new Exception("Must have at least one text element");
            }
        }

        public void SetText(object text)
        {
            CacheTextElement();
            
            if (legacyText)
            {
                legacyText.text = text.ToString();
            }

            if (textMeshPro)
            {
                textMeshPro.text = text.ToString();
            }
        }
        
        public string GetText()
        {
            CacheTextElement();
            
            if (legacyText)
            {
                return legacyText.text;
            }

            if (textMeshPro)
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
                
                if (legacyText)
                {
                    return legacyText.color;
                }
                
                if (textMeshPro)
                {
                    return textMeshPro.color;
                }

                throw new InvalidOperationException("Cant get color if no text component");
            }
            set
            {
                CacheTextElement();
                
                if (legacyText)
                {
                    legacyText.color = value;
                }
                
                if (textMeshPro)
                {
                    textMeshPro.color = value;
                }
            }
        }
    }
}