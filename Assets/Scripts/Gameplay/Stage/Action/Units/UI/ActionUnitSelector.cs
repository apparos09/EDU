using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // Action unit button page.
    public class ActionUnitSelector : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // An array of prefabs.
        public ActionUnit[,] unitPrefabs = null;

        // The prefabs row index, which determines what prefabs to currently display.
        public int currUnitPrefabsRow = -1;

        [Header("Buttons")]

        // The buttons on the page.
        public List<ActionUnitButton> unitButtons = new List<ActionUnitButton>();

        // The default number of unit buttons that're used for the action unit selector.
        public const int UNIT_BUTTONS_COUNT_DEFAULT = 5;

        // The previous page (page up) button.
        public Button prevPageButton;

        // The next page (page down) button.
        public Button nextPageButton;

        // Start is called before the first frame update
        void Start()
        {
            // Gets the instance.
            if (actionUI == null)
                actionUI = ActionUI.Instance;
        }

        // Sets the action unit prefabs using the provided list.
        // The program assembles the list into an array.
        // The number of columns is based on the number of buttons.
        public void SetActionUnitPrefabs(List<ActionUnit> newPrefabs)
        {
            // Gets the row count.
            // Converts one of the values to a float so that decimal places are included...
            // Instead of automatically rounding down.
            // Any remaining spots in a page will be left empty.
            int rowCount = Mathf.CeilToInt((float)newPrefabs.Count / unitButtons.Count);

            // Gets the column count.
            int colCount = unitButtons.Count;

            // Creates the unit prefabs array.
            unitPrefabs = new ActionUnit[rowCount, colCount];

            // The index of the new prefabs list.
            int newPrefabsIndex = 0;

            // Fills in the unit prefabs 2D array with the prefabs from the list.
            for(int r = 0; r < unitPrefabs.GetLength(0); r++) // Rows
            {
                for(int c = 0; c < unitPrefabs.GetLength(1); c++) // Columns
                {
                    // Index validity check.
                    if(newPrefabsIndex >= 0 && newPrefabsIndex < newPrefabs.Count)
                    {
                        // Puts the value in the array and increases the index.
                        unitPrefabs[r, c] = newPrefabs[newPrefabsIndex];
                        newPrefabsIndex++;
                    }
                    else
                    {
                        // No prefab, so fill with null.
                        unitPrefabs[r, c] = null;
                    }
                }
            }

            // Go to the first row.
            SetRow(0);
        }

        // Gets the row count for the unit prefabs 2D array.
        // Returns -1 if the array is null.
        public int UnitPrefabsRowCount
        {
            get 
            {
                if (unitPrefabs != null)
                    return unitPrefabs.GetLength(0);
                else
                    return -1;
            }
        }

        // Gets the column count for the unit prefabs 2D array.
        // Returns -1 if the array is null.
        public int UnitPrefabsColumnCount
        {
            get 
            {
                if (unitPrefabs != null)
                    return unitPrefabs.GetLength(1);
                else
                    return -1;
            }
        }

        // Sets the row.
        public void SetRow(int newRow)
        {
            // If the unit prefabs list doesn't exist, set the row to -1 and return.
            if(unitPrefabs == null)
            {
                currUnitPrefabsRow = -1;
                return;
            }

            // Gets the prefab row index, clamping the value.
            currUnitPrefabsRow = Mathf.Clamp(newRow, 0, unitPrefabs.GetLength(0));

            // Refreshes the unit buttons.
            RefreshUnitButtons();
        }

        // Goes to the previous row (up arrow).
        public void PreviousRow()
        {
            // The new row.
            int newRow = currUnitPrefabsRow - 1;

            // If the new row is now negative, loop around to the end.
            if (newRow < 0)
                newRow = unitPrefabs.GetLength(0) - 1;

            // Sets the row.
            SetRow(newRow);
        }

        // Goes to the next row (down arrow).
        public void NextRow()
        {
            // The new row.
            int newRow = currUnitPrefabsRow + 1;

            // If row is now out of bounds, loop around to the start.
            if (newRow >= unitPrefabs.GetLength(0))
                newRow = 0;

            // Sets the row.
            SetRow(newRow);
        }

        // Refreshes the unit buttons.
        public void RefreshUnitButtons()
        {
            // Gets set to true if the refresh attempt was valid.
            bool valid = false;

            // The array exists.
            if(unitPrefabs != null)
            {
                // True if the row is valid.
                valid = currUnitPrefabsRow >= 0 && currUnitPrefabsRow < unitPrefabs.Length;

                // If row is valid, refresh the buttons.
                if(valid)
                {
                    // The row.
                    int r = currUnitPrefabsRow;

                    // The unit button index.
                    int unitButtonIndex = 0;

                    // Goes through all columns at the current row.
                    for (int c = 0; c < unitPrefabs.GetLength(1) && unitButtonIndex < unitButtons.Count; c++)
                    {
                        // Gets the unit button.
                        // If a button doens't have a prefab, turn the button off.
                        ActionUnitButton unitButton = unitButtons[unitButtonIndex];

                        // The unit button exists.
                        if(unitButton != null)
                        {
                            // If there's a prefab, set it to the unit button.
                            if (unitPrefabs[r, c] != null)
                            {
                                unitButton.ApplyUnitPrefabInfo(unitPrefabs[r, c]);
                            }
                            // No prefab, so clear the button and disable it.
                            else
                            {
                                unitButton.ClearUnitButton();
                            }
                        }

                        // Increase the unit button index.
                        unitButtonIndex++;
                    }
                }
            }

            // If the button refresh wasn't successful, clear the buttons.
            if(!valid)
            {
                // Goes through all the unit buttons, clearing each one.
                foreach (ActionUnitButton unitButton in unitButtons)
                {
                    unitButton.ClearUnitButton();
                }
            }

            // Checks if there are unit prefabs.
            if(unitPrefabs != null)
            {
                // The buttons loop around if they go over or under the page number.
                // As such, the buttons should be interactable as long as there's more than 1 page.
                if(unitPrefabs.GetLength(0) > 1)
                {
                    prevPageButton.interactable = true;
                    nextPageButton.interactable = true;
                }
                else
                {
                    prevPageButton.interactable = false;
                    nextPageButton.interactable = false;
                }
            }
            else
            // No unit prefabs, so make the buttons non-interactable.
            {
                prevPageButton.interactable = false;
                nextPageButton.interactable = false;
            }

            // Refreshes the highlights for the unit buttons.
            RefreshUnitButtonHighlights();
        }

        // Refreshes the interactable component of the unit buttons.
        public void RefreshUnitButtonsInteractable()
        {
            // Refreshes all the unit button interactables.
            foreach(ActionUnitButton unitButton in unitButtons)
            {
                unitButton.RefreshUnitButtonInteractable();
            }

            // Refresh the highlights for the unit buttons.
            RefreshUnitButtonHighlights();
        }

        // Refreshes the unit button highlights.
        public void RefreshUnitButtonHighlights()
        {
            // Gets the row and column count.
            int rowCount = UnitPrefabsRowCount;
            int colCount = UnitPrefabsColumnCount;

            // If the row count is 0, then there's no rows.
            if (rowCount <= 0)
                return;

            // Checks the number of columns.
            if(colCount > 0)
            {
                // Originally it would use this code even if there's one row, which didn't make sense.
                // If there's more than 1 row, check the highlights. If theres only 1 row...
                // The highlights should stay off, since there's no other row to reference.
                // if(rowCount > 0)
                if (rowCount > 1)
                {
                    // The previous and next row.
                    // Up arrow is previous, down arrow is next.
                    // Top highlight is previous, bottom highlight is next.
                    int row = currUnitPrefabsRow;
                    int prevRow = currUnitPrefabsRow - 1;
                    int nextRow = currUnitPrefabsRow + 1;

                    // Loop around to the end of the list if out f bounds.
                    if (prevRow < 0)
                        prevRow = rowCount - 1;

                    // Loop around to the start of the list if out of bounds.
                    if (nextRow >= rowCount)
                        nextRow = 0;

                    // Gets the player user.
                    ActionPlayerUser playerUser = ActionManager.Instance.playerUser;

                    // Goes through all the unit buttons.
                    for(int i = 0; i < unitButtons.Count; i++)
                    {
                        // The number of buttons should match with the column count of the unit prefabs array.
                        ActionUnit topUnit = unitPrefabs[prevRow, i];
                        ActionUnit bottomUnit = unitPrefabs[nextRow, i];

                        // Sees if the top unit can be created.
                        bool topUnitAvail;
                        bool bottomUnitAvail;

                        // If the top unit exists, check if it can be made. If it doesn't exist, set to false.
                        if (topUnit != null)
                            topUnitAvail = playerUser.HasEnergyToCreateActionUnit(topUnit);
                        else
                            topUnitAvail = false;

                        // If the bottom unit exists, check if it can be made. If it doesn't exist, set to false.
                        if(bottomUnit != null)
                            bottomUnitAvail = playerUser.HasEnergyToCreateActionUnit(bottomUnit);
                        else
                            bottomUnitAvail = false;
                        
                        // Turns the top hihglights on or off.
                        unitButtons[i].SetHighlightTopOn(topUnitAvail);
                        unitButtons[i].SetHighlightBottomOn(bottomUnitAvail);
                    }
                }
                else
                {
                    // Turn off all the highlights.
                    SetAllHighlightsOn(false);
                }
            }
            // If there are no columns, turn all the highlights off.
            else
            {
                SetAllHighlightsOn(false);
            }
        }

        // Sets all the highlights on if value is true.
        public void SetAllHighlightsOn(bool value)
        {
            // Turns off the highlights.
            foreach (ActionUnitButton unitButton in unitButtons)
            {
                // Unit button exists.
                if (unitButton != null)
                {
                    // Sets the highlights on the top and bottom.
                    unitButton.SetHighlightTopOn(value);
                    unitButton.SetHighlightBottomOn(value);
                }
            }
        }
    }
}