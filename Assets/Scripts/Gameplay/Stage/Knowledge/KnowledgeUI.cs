using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The UI for the knowledge stage.
    public class KnowledgeUI : StageUI
    {
        // The singleton instance.
        private static KnowledgeUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Knowledge")]

        // Manager
        public KnowledgeManager knowledgeManager;

        [Header("Knowledge/Statements, Resources")]
        // The section text
        // This is no longer referenced since statements and resources are all on one screen.
        public TMP_Text sectionText;

        // The parent object of all statements.
        public GameObject statementsParent;

        // The statements parent rect transform.
        public RectTransform statementsParentRect;

        // The statements in the knowledge stage.
        public List<KnowledgeStatement> statements = new List<KnowledgeStatement>();

        // The parent object for all resources.
        public GameObject resourcesParent;

        // The resources parent rect transform.
        public RectTransform resourcesParentRect;

        // The resources in the knowledge stage.
        public List<KnowledgeResource> resources = new List<KnowledgeResource>();

        [Header("Knowledge/Matches")]

        // The matches text.
        public TMP_Text matchesNumberText;

        [Header("Knowledge/Selected")]

        // The text for the selected elemeent type.
        [Tooltip("The selection type text.")]
        public TMP_Text selectedTitleText;

        // The selected knowledgement element text object.
        [Tooltip("Selected knowledge element text object.")]
        public TMP_Text selectedKEText;

        // The butto used to verify the statements.
        public Button verifyButton;

        [Header("Knowledge/Buttons, Dialogs")]

        // The info log button.
        public Button infoLogButton;

        // The info log dialog.
        public InfoLog infoLogDialog;

        // The knowledge stage end dialog.
        public KnowledgeStageEndDialog stageEndDialog;

        // Constructor
        private KnowledgeUI()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }

            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Set the knowledge manager.
            if(knowledgeManager == null)
            {
                knowledgeManager = KnowledgeManager.Instance;
            }

            // If the statements parent rect is equal to null and the statements parent is set...
            // Grab the rect transform from the statements parent.
            if(statementsParentRect == null && statementsParent != null)
            {
                statementsParentRect = statementsParent.GetComponent<RectTransform>();
            }

            // If the statements list is empty, fill the list automatically.
            if (statements.Count == 0)
            {
                statements = new List<KnowledgeStatement>(FindObjectsOfType<KnowledgeStatement>(true));
            }

            // If the resources parent rect is null and the resources parent is set...
            // Grab the rect transform from the resources parent.
            if (resourcesParentRect == null && resourcesParent != null)
            {
                resourcesParentRect = resourcesParent.GetComponent<RectTransform>();
            }

            // If the resources list is empty, fill the list automatically.
            if (resources.Count == 0)
            {
                resources = new List<KnowledgeResource>(FindObjectsOfType<KnowledgeResource>(true));
            }

            // Clears the knowledge element text.
            ClearKnowledgeElementText();

            // The finish button is now in a dialog, so it's interactability doesn't need to be changed.
            // The finish button only becomes interactable once all elements have been matched correctly.
            // finishButton.interactable = false;
        }

        // Gets the instance.
        public static KnowledgeUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<KnowledgeUI>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Knowledge UI (singleton)");
                        instance = go.AddComponent<KnowledgeUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // STATEMENTS ACTIVE, RESOURCES ACTIVE
        // Gets the number of active statements.
        public int GetStatementsActiveCount()
        {
            // The number to return.
            int num = 0;

            // Checks what statements are active.
            foreach(KnowledgeStatement statement in statements)
            {
                // The statement exists.
                if(statement != null)
                {
                    // The statement object is active.
                    if(statement.gameObject.activeSelf)
                    {
                        num++;
                    }
                }
            }

            return num;
        }

        // Gets the number of statements that're matched correctly.
        public int GetStatementsMatchedCorrectlyCount()
        {
            // The number to return.
            int num = 0;

            // Checks what statements are active.
            foreach (KnowledgeStatement statement in statements)
            {
                // The statement exists.
                if (statement != null)
                {
                    // The statement object is active and its button isn't interactable.
                    // When a statement has been matched correctly, it's button isn't interactable.
                    if (statement.gameObject.activeSelf && !statement.button.interactable)
                    {
                        num++;
                    }
                }
            }

            return num;
        }

        // Gets the number of active resources.
        public int GetResourcesActiveCount()
        {
            // The number to return.
            int num = 0;

            // Checks what statements are active.
            foreach (KnowledgeResource resource in resources)
            {
                // The resource exists.
                if (resource != null)
                {
                    // The resource object is active.
                    if (resource.gameObject.activeSelf)
                    {
                        num++;
                    }
                }
            }

            return num;
        }

        // Gets the number of resources that're matched correctly.
        public int GetResourcesMatchedCorrectlyCount()
        {
            // The number to return.
            int num = 0;

            // Checks what resources are active.
            foreach (KnowledgeResource resource in resources)
            {
                // The resource exists.
                if (resource != null)
                {
                    // The resource object is active and its button isn't interactable.
                    // When a resource has been matched correctly, it's button isn't interactable.
                    if (resource.gameObject.activeSelf && !resource.button.interactable)
                    {
                        num++;
                    }
                }
            }

            return num;
        }

        // SELECTED ELEMENT
        // Sets the selected knowledge element text.
        public void SetSelectedKnowledgeElementText(KnowledgeStatement statement)
        {
            // Commented out since this doesn't specify the selection type anymore.
            // // Selection Type Text.
            // selectedTypeText.text =
            //     KnowledgeManager.Instance.GetSelectedStringTranslated() + ": " + 
            //     KnowledgeManager.Instance.GetStatementsStringTranslated();

            // Knowledge Element Text.
            selectedKEText.text = statement.GetStatementTextTranslated();
        }

        // Sets the selected knowledge element text.
        public void SetSelectedKnowledgeElementText(KnowledgeResource resource)
        {
            // Commented out since this doesn't specify the selection type anymore.
            // // Selection Type Text.
            // selectedTypeText.text =
            //    KnowledgeManager.Instance.GetSelectedStringTranslated() + ": " + 
            //    KnowledgeManager.Instance.GetResourcesStringTranslated();

            // Knowledge Element Text.
            selectedKEText.text = resource.GetResourceTextTranslated();
        }

        // Clears the knowledge element text.
        public void ClearKnowledgeElementText()
        {
            // Commented out since this doesn't specify the selection type anymore.
            // // Selection Type Text.
            // selectedTypeText.text = 
            //     KnowledgeManager.Instance.GetSelectedStringTranslated() + ": " +
            //     KnowledgeManager.Instance.GetNoneStringTranslated();

            // Selected Element Text.
            selectedKEText.text = "-";
        }

        // VERIFY
        // Verifies the matches.
        public void VerifyMatches()
        {
            // Calls function to verify matches.
            knowledgeManager.VerifyMatches();

            // If the info log button is off, make it interactable since a verification attempt...
            // Has been made.
            if (knowledgeManager.ChangeInfoLogButtonInteractable && !infoLogButton.interactable)
                infoLogButton.interactable = true;

            // Updates the matches display.
            UpdateMatchesDisplay();
        }

        // Clears all unverified matches.
        public void ClearUnverifiedMatches()
        {
            knowledgeManager.ClearUnverifiedMatches();
        }

        // Updates the display of the matches the player has.
        public void UpdateMatchesDisplay()
        {
            int correctCount = GetStatementsMatchedCorrectlyCount();
            int totalCount = GetStatementsActiveCount();

            // The string that will be displayed.
            string displayStr = correctCount.ToString() + "/" + totalCount.ToString();

            // Updates the matches number text if it isn't set already.
            if (matchesNumberText.text != displayStr)
            {
                matchesNumberText.text = displayStr;
            }
        }

        // DIALOGS //
        // Generates a list of dialogs.
        public override List<GameObject> GenerateDialogList()
        {
            // Gets the base list.
            List<GameObject> dialogList = base.GenerateDialogList();

            // Adds the rest of the dialogs.
            dialogList.Add(infoLogDialog.gameObject);
            dialogList.Add(stageEndDialog.gameObject);

            return dialogList;
        }

        // Returns 'true' if the info log is open.
        public bool IsInfoLogDialogOpen()
        {
            return infoLogDialog.gameObject.activeSelf;
        }

        // Opens the info log dialog.
        public void OpenInfoLogDialog(bool closeOtherDialogs)
        {
            OpenDialog(infoLogDialog.gameObject, closeOtherDialogs);

            // Randomize statements.
            // NOTE: as of build V04, the statements are no longer randomzied when the info log is opened.
            // knowledgeManager.RandomizeStatements();

            // Makes the info log button non-interactable.
            if(knowledgeManager.ChangeInfoLogButtonInteractable)
                infoLogButton.interactable = false;
        }

        // Closes the info log dialog.
        public void CloseInfoLogDialog()
        {
            CloseDialog(infoLogDialog.gameObject);
        }

        // Opens the stage end dialog.
        public void OpenStageEndDialog()
        {
            OpenDialog(stageEndDialog.gameObject, true);
            stageEndDialog.UpdateStageEndStats();
        }

        // Closes the stage end dialog.
        public void CloseStageEndDialog()
        {
            CloseDialog(stageEndDialog.gameObject);
        }

        // Resets the stage.
        // This function shouldn't be used because the verified correct matches shouldn't be undone...
        // Once the matches have been done correctly.
        public override void ResetStage()
        {
            knowledgeManager.ResetStage();
            CloseAllDialogs();
        }

        // Finishes the stage. This should only be called once all statements have been matched correctly.
        public override void FinishStage()
        {
            knowledgeManager.FinishStage();
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected virtual void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }

    }
}