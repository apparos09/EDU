using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace RM_EDU
{
    // A page for a text box.
    public class EDU_Page : Page
    {
        // The language key for the page title.
        public string titleLanguageKey = string.Empty;

        // Set to 'true' if the title has been translated.
        protected bool titleTranslated = false;

        // The language key for the page text.
        public string textLanguageKey = string.Empty;

        // The speak key for the page.
        public string textSpeakKey = string.Empty;

        // Set to 'true' if the text has been translated.
        protected bool textTranslated = false;

        // The display sprite for the page.
        public Sprite displaySprite = null;

        // Adds a page.
        public EDU_Page() : base()
        {
            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Adds a page with text.
        public EDU_Page(string text) : base(text)
        {
            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Add a page with text and a display sprite.
        public EDU_Page(string text, Sprite displaySprite) : base(text)
        {
            // Set the sprite.
            this.displaySprite = displaySprite;

            // Add speak text callback.
            AddSpeakTextCallback();
        }

        // Adds a page with text and a language/speak key.
        public EDU_Page(string text, string textLanguageKey) : base(text)
        {
            // Sets the language key and translates the text.
            TranslateText(textLanguageKey, true);

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Adds a page with text and a language/speak key.
        public EDU_Page(string text, string textLanguageKey, Sprite displaySprite) : base(text)
        {
            // Sets the language key and translates the text.
            // Also sets the language key to the speak key.
            TranslateText(textLanguageKey, true);

            // Sets the display sprite.
            this.displaySprite = displaySprite;

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Adds a page with text, a langauge key, and a speak key.
        public EDU_Page(string text, string textLanguageKey, string textSpeakKey) : base(text)
        {
            // Sets the language key and translates the text.
            // Also sets the speak key, which may be different than the language key.
            SetTextLanguageKeyAndSpeakKey(textLanguageKey, textSpeakKey);

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Adds a page with text, a langauge key, and a speak key.
        public EDU_Page(string text, string textLanguageKey, string textSpeakKey, Sprite displaySprite) : base(text)
        {
            // Sets the language key and translates the text.
            // Also sets the speak key, which may be different than the language key.
            SetTextLanguageKeyAndSpeakKey(textLanguageKey, textSpeakKey);

            // Sets the display sprite.
            this.displaySprite = displaySprite;

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Sets the title, the title language key, the text, and the text language key.
        public EDU_Page(string title, string titleLanguageKey, string text, string textLanguageKey) : base(title, text)
        {
            // Sets the title and translates the text.
            TranslateTitle(titleLanguageKey);

            // Sets the text language key and translates the text.
            // Also sets the speak key to the language key.
            TranslateText(textLanguageKey, true);

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Sets the title, title language key, text, text language key, and text speak key.
        public EDU_Page(string title, string titleLanguageKey, string text, string textLanguageKey, string textSpeakKey) : base(title, text)
        {
            // Sets the title and translates the text.
            TranslateTitle(titleLanguageKey);

            // Sets the language key and translates the text.
            // Also sets the speak key, which may be different than the language key.
            SetTextLanguageKeyAndSpeakKey(textLanguageKey, textSpeakKey);

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Sets the title, title language key, text, text language key, and display sprite.
        public EDU_Page(string title, string titleLanguageKey, string text, string textLanguageKey, Sprite displaySprite) : base(title, text)
        {
            // Sets the title and translates the text.
            TranslateTitle(titleLanguageKey);

            // Sets the text language key and translates the text.
            // Also sets the speak key to the language key.
            TranslateText(textLanguageKey, true);

            // Sets the display sprite.
            this.displaySprite = displaySprite;

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Sets the title, title language key, text, text language key, text speak key, and display sprite.
        public EDU_Page(string title, string titleLanguageKey, string text, string textLanguageKey, string textSpeakKey, Sprite displaySprite) : base(title, text)
        {
            // Sets the title and translates the text.
            TranslateTitle(titleLanguageKey);

            // Sets the language key and translates the text.
            // Also sets the speak key, which may be different than the language key.
            SetTextLanguageKeyAndSpeakKey(textLanguageKey, textSpeakKey);

            // Sets the display sprite.
            this.displaySprite = displaySprite;

            // Adds the speak text callback.
            AddSpeakTextCallback();
        }

        // Returns 'true' if the title is translated.
        public bool IsTitleTranslated()
        {
            return titleTranslated;
        }

        // Returns 'true' if the text is translated.
        public bool IsTextTranslated()
        {
            return textTranslated;
        }

        // Translates the title and text for the page title and text.
        public void TranslateTitleAndText()
        {
            // If the LOL Manager is instantiated...
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                // Gets the new title and text.
                string newTitle = SystemManager.GetLanguageTextStatic(titleLanguageKey);
                string newText = SystemManager.GetLanguageTextStatic(textLanguageKey);

                // If the new text is not null, use it. If the new text is null, use an empty string.
                title = (newTitle != null) ? newTitle : string.Empty;
                text = (newText != null) ? newText : string.Empty;
            }
        }

        // Sets the title language text (translation).
        public void TranslateTitle()
        {
            // If the LOL Manager is instantiated...
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                // Gets the new title text.
                string newTitle = SystemManager.GetLanguageTextStatic(titleLanguageKey);

                // If the new text is not null, use it. If the new text is null, use an empty string.
                text = (newTitle != null) ? newTitle : string.Empty;

                // The title has been translated.
                titleTranslated = true;
            }
        }

        // Sets the title language key and translates the title.
        public void TranslateTitle(string newTitleLangKey)
        {
            titleLanguageKey = newTitleLangKey;
            TranslateTitle();
        }

        // Translates the page text using the language key.
        public void TranslateText()
        {
            // If the LOL Manager is instantiated...
            if (SystemManager.IsInstantiatedAndIsLanguageLoaderInitialized())
            {
                // Gets the new text.
                string newText = SystemManager.GetLanguageTextStatic(textLanguageKey);

                // If the new text is not null, use it. If the new text is null, use an empty string.
                text = (newText != null) ? newText : string.Empty;

                // The text has been translated.
                textTranslated = true;
            }
        }

        // Sets the text language key and translates it.
        public void TranslateText(string newTextLangKey)
        {
            textLanguageKey = newTextLangKey;
            TranslateText();
        }

        // Sets the text language key and translates it.
        // If setSpeakKey is true, the speak key is also set.
        public void TranslateText(string newTextLangKey, bool setTextSpeakKey)
        {
            // Sets the language key.
            TranslateText(newTextLangKey);

            // If this is also the speak key, set it as the speak key.
            if (setTextSpeakKey)
            {
                SetTextSpeakKey(newTextLangKey);
            }
        }

        // Sets the new text speak key.
        public void SetTextSpeakKey(string newTextSpeakKey)
        {
            textSpeakKey = newTextSpeakKey;
        }

        // Sets the text language text key and speak key at the same time.
        public void SetTextLanguageKeyAndSpeakKey(string newTextLangKey, string newTextSpeakKey)
        {
            TranslateText(newTextLangKey);
            SetTextSpeakKey(newTextSpeakKey);
        }

        // Speaks the title for the tutorial page.
        public void SpeakTitle()
        {
            // If the LOL SDK is initialized, TTS is available, TTS is on, and the title key is set.
            if (SystemManager.IsTextToSpeechUsableAndEnabled() && titleLanguageKey != string.Empty)
            {
                SystemManager.Instance.SpeakText(titleLanguageKey);
                // TextToSpeech.Instance.SpeakText(speakKey);
            }
        }

        // Speaks the text for the tutorial page.
        public void SpeakText()
        {
            // If the LOL SDK is initialized, TTS is available, TTS is on, and the text speak key is set.
            if (SystemManager.IsTextToSpeechUsableAndEnabled() && textSpeakKey != string.Empty)
            {
                SystemManager.Instance.SpeakText(textSpeakKey);
                // TextToSpeech.Instance.SpeakText(speakKey);
            }
        }

        // Adds the speak text callback.
        public void AddSpeakTextCallback()
        {
            OnPageOpenedAddCallback(SpeakText);
        }

        // Removes the speak text callback.
        public void RemoveSpeakTextCallback()
        {
            OnPageOpenedRemoveCallback(SpeakText);
        }


        // Returns 'true' if the page has a display sprite.
        public bool HasDisplaySprite()
        {
            return displaySprite != null;
        }
    }
}