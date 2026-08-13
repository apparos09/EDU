using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // A statement for the knowledge stage. It gets matched to a knowledge resource.
    public class KnowledgeStatement : KnowledgeElement
    {
        // Doesn't show up unless the object below is editable in the inspector.
        // [Header("Statement")]

        // The statement that this KnowledgeStatment is using from the list.
        private KnowledgeStatementList.Statement statement;

        // The resource attached to this statement.
        public KnowledgeResource attachedResource;

        // The text for the displayed statement.
        public TMP_Text statementText;

        // The match text, which is changed to show what the knowledge statement is attached to.
        public TMP_Text matchText;

        // If 'true', the identification text is used, which reveals the answer.
        private bool useIdText = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // NOTE: should probably be commented out when not running tests.
            // Generate the test statement and set it to this object.
            if(statement == null)
            {
                SetStatement(KnowledgeStatementList.GenerateTestStatement());
            }
        }

        // If 'true', the id text is shown instead of the statement text.
        public bool UseIdText
        {
            get { return useIdText; }
        }

        // Called when the statement has been selected.
        public override void Select()
        {
            // If something is already selected, make sure the button image is its normal color.
            if (KnowledgeManager.Instance.selectedStatement != null)
                KnowledgeManager.Instance.selectedStatement.SetButtonToNormalColor();

            // Set as the selected statement.
            KnowledgeManager.Instance.selectedStatement = this;

            // Sets the button image color to the selected color.
            SetButtonToSelectedColor();

            // Sets the selected knowledge element text.
            KnowledgeUI.Instance.SetSelectedKnowledgeElementText(this);
        }

        // The statement.
        public KnowledgeStatementList.Statement Statement
        {
            get
            {
                return GetStatement();
            }

            set
            {
                SetStatement(value);
            }
        }

        // Gets the statement.
        public KnowledgeStatementList.Statement GetStatement()
        {
            return statement;
        }

        // Sets the statement.
        public void SetStatement(KnowledgeStatementList.Statement statement)
        {
            this.statement = statement;
            statementText.text = useIdText ? statement.GenerateIdText() : statement.GetStatementTextTranslated();
        }

        // Gets the translated statement text.
        // If translation isn't possible, the default statement text is returned.
        public string GetStatementTextTranslated()
        {
            // Checks if the statement exists.
            if (statement != null)
                return statement.GetStatementTextTranslated();
            else
                return statement.text;
        }

        // Gets the statement key. Returns "" if the statement isn't set.
        public string GetStatementKey()
        {
            // Checks if the statement exists.
            if (statement != null)
                return statement.key;
            else
                return "";
        }

        // Checks if the statement is attached to a resource.
        public bool IsAttachedToResource()
        {
            return attachedResource != null;
        }

        // Returns true if the attached element is the same as the provided element.
        public bool IsAttachedToResource(KnowledgeResource resource)
        {
            return attachedResource == resource;
        }

        // Returns 'true' if the attached statement has this resource attached.
        public bool AttachedResourceHasThisStatement()
        {
            // Checks if there is a resource.
            if (IsAttachedToResource())
            {
                // Returns true if the attached resource has this statement.
                return attachedResource.attachedStatement == this;
            }
            else
            {
                return false;
            }
        }

        // Attaches to the following resource.
        public void AttachToResource(KnowledgeResource newResource)
        {
            // If a resource is attached, detach it.
            if (IsAttachedToResource())
            {
                DetachResource();
            }

            // Set the new attachments.
            attachedResource = newResource;
            attachedResource.attachedStatement = this;

            // Calls when the attachement has been changed.
            // If a statement was detached earlier this is technically called twice, but this should be fine.
            OnAttachmentChange();
        }

        // Detaches the resource.
        public void DetachResource()
        {
            // If there's already an attachment, remove it.
            if (IsAttachedToResource())
            {
                // Clears the matchText for attachedResource and removes the attachments.
                // If the attached resource has this statement, clear the match text and set it to null.
                // This is to account for mismatches.
                if (AttachedResourceHasThisStatement())
                {

                    attachedResource.matchText.text = "";
                    attachedResource.attachedStatement = null;
                }


                attachedResource = null;
            }

            // Calls when the attachement has been changed.
            OnAttachmentChange();
        }

        // Called when the attachment has changed.
        public void OnAttachmentChange()
        {
            // If there is an attachment, change the text to match it.
            if (attachedResource != null)
            {
                // Changes the attached resource's text to match the statement's text.
                attachedResource.matchText.text = matchText.text;
            }

        }

        // Returns 'true' if the attachment has the same resource.
        // If nothing is attached, returns false.
        // This does NOT call OnAttachmentMatchedCorrectly(). That happens in the verify function in the knowledge manager.
        public bool IsAttachmentMatchedCorrectly()
        {
            // If a resource is attached.
            if(attachedResource != null)
            {
                return statement.resource == attachedResource.resource;
            }
            else
            {
                return false;
            }
        }

        // Read the statement text. Only does so if text to speech is enabled.
        public override void SpeakText()
        {
            // If TTS is enabled, read the button.
            if (IsTextToSpeechEnabled() && statement != null)
            {
                // Gets the key.
                string key = statement.key;

                // Key exists, so speak the text for it.
                if (key != "")
                {
                    LanguageManager.Instance.SpeakText(key);
                }
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If attached to a statement...
            if (IsAttachedToResource())
            {
                // If the knowledge resource isn't attached to this statement, it means it's attached to something else.
                // If so, this statement is wrongly attached to it, and said attachment should be removed.
                if (!attachedResource.IsAttachedToStatement(this))
                {
                    DetachResource();
                }
            }
        }
    }
}