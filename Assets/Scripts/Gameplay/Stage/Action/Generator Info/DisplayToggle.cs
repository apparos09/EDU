using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RM_EDU
{
    // The display toggle. These toggles are only used to show if a setting is on or off.
    public class DisplayToggle : MonoBehaviour
    {
        // The action UI.
        public ActionUI actionUI;

        // The toggle.
        public Toggle toggle;

        // Start is called before the first frame update
        void Start()
        {
            // Finds the action UI if it's not set.
            if (actionUI == null)
                actionUI = ActionUI.Instance;

            // If the toggle isn't set, set it.
            if (toggle == null)
                toggle = GetComponent<Toggle>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}