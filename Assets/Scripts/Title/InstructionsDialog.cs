using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The instructions dialog.
    public class InstructionsDialog : MonoBehaviour
    {
        // An element for the instructions dialog.
        protected struct Element
        {
            // NAME
            // The name of the element.
            public string name;

            // The key for the name.
            public string nameKey;

            // Gets set to 'true' if the name has been translated.
            public bool nameTranslated;

            // Saves the name translated if true.
            public bool saveNameTranslated;

            // DESCRIPTION
            // The description of the element.
            public string description;
            
            // The key for the description.
            public string descriptionKey;

            // Gets set to 'true' if the description has been translated.
            public bool descTranslated;

            // Saves the description if it has been translated.
            public bool saveDescTranslated;

            // The diagram sprite.
            public Sprite diagramSprite;

            // Returns the name translated.
            public string GetNameTranslated()
            {
                // Result to be returned.
                string result;

                // If the name should be saved translated and it has been translated, use the name.
                if(saveNameTranslated && nameTranslated)
                {
                    result = name;
                }
                else
                {
                    // Checks if the LOL Manager exists for translation.
                    if (LanguageManager.IsInstantiatedAndIsLanguageLoaderInitialized() && nameKey != "")
                    {
                        result = LanguageManager.GetLanguageTextStatic(nameKey);

                        // If the translated name should be saved.
                        if(saveNameTranslated)
                        {
                            // Overwrites the name variable and marks that it has been translated.
                            name = result;
                            nameTranslated = true;
                        }
                    }
                    else
                    {
                        // Set to default.
                        result = name;
                    }
                }

                return result;
            }

            // Returns the description translated
            public string GetDescriptionTranslated()
            {
                // Result to be returned.
                string result;

                // If the description should be saved translated and it has been translated, use the description.
                if (saveDescTranslated && descTranslated)
                {
                    result = description;
                }
                else
                {
                    // Checks if the LOL Manager exists for translation.
                    if (LanguageManager.IsInstantiatedAndIsLanguageLoaderInitialized() && descriptionKey != "")
                    {
                        result = LanguageManager.GetLanguageTextStatic(descriptionKey);

                        // If the translated description should be saved.
                        if (saveDescTranslated)
                        {
                            // Overwrites the description variable and marks that it has been translated.
                            description = result;
                            descTranslated = true;
                        }
                    }
                    else
                    {
                        // Set to default.
                        result = description;
                    }
                }

                return result;
            }
        }

        // The title UI.
        public TitleUI titleUI;

        // The elements for the instructions dialog.
        private List<Element> elements = new List<Element>();

        // The index of the current element.
        private int elementIndex = -1;

        [Header("Text")]

        // The controls text.
        public TMP_Text descText;

        // The element name text.
        public TMP_Text elementNameText;

        // The element description text.
        public TMP_Text elementDescText;

        [Header("Images, Sprites")]

        // The diagram image.
        public Image diagramImage;

        // The sprites for the world diagram.
        public Sprite worldSprite;

        // The sprite for the action diagram.
        public Sprite actionSprite;

        // The sprite for the knowledge diagram.
        public Sprite knowledgeSprite;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the title UI instance.
            if (titleUI == null)
                titleUI = TitleUI.Instance;

            // Element Object
            Element element;

            // World
            element = new Element();
            element.name = "World";
            element.nameKey = "kwd_world";
            element.nameTranslated = false;
            element.saveNameTranslated = true;

            element.description = "In the world section, you select action stages and knowledge stages. Once all stages are complete, the game is finished.";
            element.descriptionKey = "dlg_instructions_emt_world";
            element.descTranslated = false;
            element.saveDescTranslated = true;

            element.diagramSprite = worldSprite;
            
            elements.Add(element);

            // Action
            element = new Element();
            element.name = "Action";
            element.nameKey = "kwd_action";
            element.nameTranslated = false;
            element.saveNameTranslated = true;

            element.description = "In an action stage, you place generator units to generate energy and place defense units to defend your base from enemies.";
            element.descriptionKey = "dlg_instructions_emt_action";
            element.descTranslated = false;
            element.saveDescTranslated = true;

            element.diagramSprite = actionSprite;

            elements.Add(element);

            // Knowledge
            element = new Element();
            element.name = "Knowledge";
            element.nameKey = "kwd_knowledge";
            element.nameTranslated = false;
            element.saveNameTranslated = true;

            element.description = "In a knowledge stage, you match statements with the resources they apply to.";
            element.descriptionKey = "dlg_instructions_emt_knowledge";
            element.descTranslated = false;
            element.saveDescTranslated = true;

            element.diagramSprite = knowledgeSprite;

            elements.Add(element);

            // Sets to the first element.
            // Doesn't speak text since the instructions description might be being read.
            SetElement(0, false);
        }

        // Sets the element and tells it to speak the text.
        public void SetElement(int index)
        {
            SetElement(index, true);
        }

        // Sets the element.
        // speakText: tries to use TTS if true. Won't use it if TTS is disabled or unavailable.
        public void SetElement(int index, bool speakText)
        {
            // Sets the index.
            elementIndex = index;
            elementIndex = Mathf.Clamp(elementIndex, 0, elements.Count - 1);

            // Sets the name, description, and diagram.
            elementNameText.text = elements[elementIndex].GetNameTranslated();
            elementDescText.text = elements[elementIndex].GetDescriptionTranslated();
            diagramImage.sprite = elements[elementIndex].diagramSprite;

            // Tries to speak the text.
            if(speakText)
            {
                SpeakText(elements[elementIndex].descriptionKey);
            }
        }

        // Goes to the previous element.
        public void PreviousElement()
        {
            // Element index minus 1.
            int index = elementIndex - 1;

            // Wrap around.
            if (index < 0)
                index = elements.Count - 1;

            // Set
            SetElement(index);
        }

        // Goes to the next element.
        public void NextElement()
        {
            // Element index plus 1.
            int index = elementIndex + 1;

            // Wrap around.
            if (index >= elements.Count)
                index = 0;

            // Set
            SetElement(index);
        }

        // Speaks the text.
        public void SpeakText(string key)
        {
            // Checks if the instances exist: LOL SDK, Text-to-Speech, and GameSettings.
            if (LanguageManager.IsInstantiatedAndIsLanguageLoaderInitialized() && TextToSpeech.Instantiated && GameSettings.Instantiated)
            {
                // Gets the instances.
                GameSettings gameSettings = GameSettings.Instance;
                LanguageManager lolManager = LanguageManager.Instance;

                // Checks if TTS should be used.
                if (gameSettings.UseTextToSpeech)
                {
                    // Grabs the LOL Manager to trigger text-to-speech.
                    lolManager.textToSpeech.SpeakText(key);
                }
            }
        }
    }
}