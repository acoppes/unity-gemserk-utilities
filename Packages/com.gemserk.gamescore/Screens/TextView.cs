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

        string originalText
        {
            get;
        }
    }
    
    public class TextView : MonoBehaviour, ITextView
    {
        public enum TextCase
        {
            Normal,
            UpperCase,
            LowerCase
        }

        public TextCase textCase = TextCase.Normal;
        
        private Text legacyText;
        private TextMeshProUGUI textMeshPro;
        
        public string originalText { get; private set; }

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

        private string ProcessCase(string textString)
        {
            if (textCase == TextCase.LowerCase)
            {
                return textString.ToLowerInvariant();
            }

            if (textCase == TextCase.UpperCase)
            {
                return textString.ToUpperInvariant();
            }

            return textString;
        }

        public void SetText(object textString)
        {
            CacheTextElement();

            originalText = textString.ToString();
            
            if (legacyText)
            {
                legacyText.text = ProcessCase(originalText);
            }

            if (textMeshPro)
            {
                textMeshPro.text = ProcessCase(originalText);
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