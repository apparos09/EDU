using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // A resource in the knowledge stage. A knowledge statement is matched to it.
    public class KnowledgeResource : KnowledgeElement
    {
        // Doesn't show up unless the object below is editable in the inspector.
        // [Header("Resource")]

        // The natural resource for this knowledge resource.
        // This should be set using SetResource, but then it wouldn't be editable in the inspector.
        public NaturalResources.naturalResource resource = NaturalResources.naturalResource.unknown;

        // The statement that is attached to this resource.
        public KnowledgeStatement attachedStatement;

        // The description for this resource. This is just the name of the resource.
        public TMP_Text resourceText;

        // The text that gets changed to show what statmenet this resource is matched with.
        public TMP_Text matchText;

        // If 'true', the resource colour is used instead of the button's base color.
        private bool useResourceColor = false;

        // The mix amount for the resource normal colour. The resource colour is lerped with the color white...
        // To set the button normal color.
        // The higher the value, the stronger the resource color is in the new color.
        private float resourceNormalColorMix = 0.333F;

        // The mix amount for the resource selected color. The button's normal color is lerped with the color black...
        // To set the button's selected color.
        // The higher the value, the stronger the button's normal color is in the new color.
        private float resourceSelectedColorMix = 0.75F;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // TODO: maybe put a "-" instead?
            // Clear out the match text.
            matchText.text = string.Empty;

            // Calls SetResource() to set the text.
            // Maybe comment this out when not doing tests.
            SetResource(resource);
        }

        // Called when the resource has been selected.
        public override void Select()
        {
            // If something is already selected, make sure the button image is its normal color.
            if (KnowledgeManager.Instance.selectedResource != null)
                KnowledgeManager.Instance.selectedResource.SetButtonToNormalColor();

            // Set as the selected resource.
            KnowledgeManager.Instance.selectedResource = this;

            // Sets the button image color to the selected color.
            SetButtonToSelectedColor();

            // Sets the selected knowledge element text.
            KnowledgeUI.Instance.SetSelectedKnowledgeElementText(this);
        }

        // Returns 'true' if this button is set to use the resource color.
        public bool UseResourceColor
        {
            get { return useResourceColor; }
        }

        // Gets the resource normal color mix amount.
        public float ResourceNormalColorMix
        {
            get { return resourceNormalColorMix; }
        }

        // Gets the resource selected color mix amount.
        public float ResourceSelectedColorMix
        {
            get { return resourceSelectedColorMix; }
        }

        // Sets the resource and the display text.
        public void SetResource(NaturalResources.naturalResource resource)
        {
            this.resource = resource;
            resourceText.text = NaturalResources.GetNaturalResourceName(resource);

            // If the resource color should be used.
            if(useResourceColor && buttonImage != null)
            {
                // Gets the resource color.
                Color resColor = NaturalResources.GetNaturalResourceColor(resource);

                // Mixes the resource color with white for the normal color.
                Color newNormalColor = Color.Lerp(Color.white, resColor, resourceNormalColorMix);
                
                // Mixes the new normal color with black for the selected clor.
                Color newSelectedColor = Color.Lerp(Color.black, newNormalColor, resourceSelectedColorMix);

                // Sets the button imag'es color, the normal and selected colors.
                buttonImage.color = newNormalColor;
                buttonNormalColor = newNormalColor;
                buttonSelectedColor = newSelectedColor;

                // Sets hte button to its normal color.
                SetButtonToNormalColor();
            }
        }

        // Gets the resource text translated.
        public string GetResourceTextTranslated()
        {
            return NaturalResources.GetNaturalResourceName(resource);
        }

        // Gets the resource name.
        public string GetResourceName()
        {
            return NaturalResources.GetNaturalResourceName(resource);
        }

        // Gets the resource name key.
        public string GetResourceNameKey()
        {
            return NaturalResources.GetNaturalResourceNameKey(resource);
        }

        // Checks if the resource is attached to a statement.
        public bool IsAttachedToStatement()
        {
            return attachedStatement != null;
        }

        // Returns true if the attached element is the same as the provided element.
        public bool IsAttachedToStatement(KnowledgeStatement statement)
        {
            return attachedStatement == statement;
        }

        // Returns 'true' if the attached statement has this resource attached.
        public bool AttachedStatementHasThisResource()
        {
            // Checks if there is a statement.
            if(IsAttachedToStatement())
            {
                // Returns true if the attached statement has this resource.
                return attachedStatement.attachedResource == this;
            }
            else
            {
                return false;
            }
        }

        // Attaches to the following statement.
        public void AttachToStatement(KnowledgeStatement newAttachment)
        {
            // If a statement is attached, detach it.
            if(attachedStatement != null)
            {
                DetachStatement();
            }

            // Set the new attachments.
            attachedStatement = newAttachment;
            attachedStatement.attachedResource = this;

            // Calls when the attachement has been changed.
            // If a statement was detached earlier this is technically called twice, but this should be fine.
            OnAttachmentChange();
        }

        // Detaches the statement.
        public void DetachStatement()
        {
            // If there's already an attachment, remove it.
            if (IsAttachedToStatement())
            {
                // Removes the attachments.
                // Do NOT change the matchText for the attachedStatement, as that should never change once properly set.
                // If the attached statement has this resource, set it to null. This is to account for mismatches.
                if(AttachedStatementHasThisResource())
                {
                    attachedStatement.attachedResource = null;
                }

                attachedStatement = null;
            }

            // Calls when the attachement has been changed.
            OnAttachmentChange();
        }

        // Called when the attachment has changed.
        public void OnAttachmentChange()
        {
            // If there is an attachment, change the text to match it.
            if (IsAttachedToStatement())
            {
                matchText.text = attachedStatement.matchText.text;
            }
            // If there isn't a statement, remove the text.
            else
            {
                matchText.text = "";
            }
        }

        // Returns 'true' if the attachment has the same resource.
        // If nothing is attached, returns false.
        // This does NOT call OnAttachmentMatchedCorrectly(). That happens in the verify function in the knowledge manager.
        public bool IsAttachmentMatchedCorrectly()
        {
            // If a statement is attached.
            if (attachedStatement != null)
            {
                // If there is a statement, check that the resources match.
                if(attachedStatement.Statement != null)
                {
                    return resource == attachedStatement.Statement.resource;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Read the resource text. Only does so if text to speech is enabled.
        public override void SpeakText()
        {
            // If TTS is enabled, read the button.
            if(IsTextToSpeechEnabled())
            {
                // Gets the key.
                string key = GetResourceNameKey();

                // Key exists, so speak the text.
                if(key != "")
                {
                    SystemManager.Instance.SpeakText(key);
                }
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // If attached to a statement...
            if(IsAttachedToStatement())
            {
                // If the knowledge statement isn't attached to this resource, it means it's attached to something else.
                // If so, this resource is wrongly attached to it, and said attachment should be removed.
                if(!attachedStatement.IsAttachedToResource(this))
                {
                    DetachStatement();
                }
            }
        }

    }
}