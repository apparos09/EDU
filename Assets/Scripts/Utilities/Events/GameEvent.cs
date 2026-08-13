using UnityEngine;

namespace util
{
    // This script is used to trigger events once certain conditions are met.
    public abstract class GameEvent : MonoBehaviour
    {
        // The name of the event.
        public string eventName = string.Empty;

        // The ID number of the event.
        public int eventNumber = 0;

        // The event ID.
        public string eventId = string.Empty;

        // A tag used to mark the event.
        public string eventTag = string.Empty;

        [Header("Cleared")]
        // Set this to 'true' to show that the event is cleared.
        [Tooltip("Shows that the event is completed.")]
        public bool cleared = false;

        // Shows that the init function was called.
        [Tooltip("Shows that the init function has been called.")]
        public bool calledInit = false;

        // Shows that the complete function was called.
        [Tooltip("Shows that the complete function has been called.")]
        public bool calledComplete = false;


        // Awake is called when the script instance is being loaded 
        protected virtual void Awake()
        {
            // ...
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            // Initializes the event.
            InitalizeEvent();
        }

        // Initializes the event.
        public virtual void InitalizeEvent()
        {
            calledInit = true;
        }

        // Updates an event, checking if the event has been cleared yet.
        public abstract void UpdateEvent();

        // Called when the event is completed.
        public virtual void OnEventComplete()
        {
            calledComplete = true;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // If the event hasn't been cleared yet, call UpdateEvent().
            if (cleared) // Cleared
            {
                // If the complete function hasn't been called yet, call it.
                if (!calledComplete)
                {
                    // Call the event complete.
                    OnEventComplete();
                }
            }
            else // Not cleared yet, so call update function.
            {
                UpdateEvent();
            }

        }

    }
}