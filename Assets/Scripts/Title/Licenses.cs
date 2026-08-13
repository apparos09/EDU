using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The licenses dialog.
    public class Licenses : MonoBehaviour
    {
        // The title UI.
        public TitleUI titleUI;

        // The credits interface.
        public AudioCreditsInterface creditsInterface;

        // The BGM audio credits for the game.
        public AudioCredits bgmCredits;

        // The BGM credits page.
        public int bgmCreditsIndex = 0;

        // The SFX audio credits for the game.
        public AudioCredits sfxCredits;

        // The sound effects credit page.
        public int sfxCreditsIndex = 0;

        [Header("UI")]

        // BGM Button
        public Button bgmButton;

        // SFX Button
        public Button sfxButton;

        // Start is called before the first frame update
        void Start()
        {
            // Loads all the credits.
            LoadBackgroundMusicCredits();
            LoadSoundEffectCredits();

            // Sets this if it has not been set already.
            if (creditsInterface == null)
                creditsInterface = GetComponent<AudioCreditsInterface>();

            // Show the background credits.
            ShowBackgroundMusicCredits();
        }

        // Loads the BGM credits.
        private void LoadBackgroundMusicCredits()
        {
            AudioCredits.AudioCredit credit;

            // Template
            // credit = new AudioCredits.AudioCredit();
            // credit.title = "-";
            // credit.artist = "-";
            // credit.collection = "-";
            // credit.source = "-";
            // credit.link1 = "";
            // credit.link2 = "";
            // credit.copyright = "-";

            // Title
            credit = new AudioCredits.AudioCredit();
            credit.title = "Inspiration and Success";
            credit.artist = "Rafael Krux";
            credit.collection = "Motivational";
            credit.source = "Orchestralis.net";
            credit.link1 = "https://music.orchestralis.net/track/28566398";
            credit.link2 = "-";
            credit.copyright = 
                "'Inspiration and Success' by Rafael Krux (orchestralis.net)\n" + 
                "Creative Commons 4.0 License.\n" + 
                "https://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // World
            credit = new AudioCredits.AudioCredit();
            credit.title = "A View From Earth";
            credit.artist = "Jason Shaw";
            credit.collection = "Soundtrack";
            credit.source = "Audionautix.com";
            credit.link1 = "https://audionautix.com/free-music/soundtrack";
            credit.link2 = "-";
            credit.copyright = 
                "\"A View From Earth\"\n" + 
                "Creative Commons Music by Jason Shaw on Audionautix.com\n" + 
                "Creative Commons Attribution 4.0 International License\n" + 
                "https://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // Stage Preparation
            credit = new AudioCredits.AudioCredit();
            credit.title = "Cycles";
            credit.artist = "Jason Shaw";
            credit.collection = "Electronic";
            credit.source = "Audionautix.com";
            credit.link1 = "https://audionautix.com/free-music/electronic";
            credit.link2 = "-";
            credit.copyright =
                "\"Cycles\"\n" +
                "Creative Commons Music by Jason Shaw on Audionautix.com\n" +
                "Creative Commons Attribution 4.0 International License\n" +
                "https://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // Action Stage Theme 1
            credit = new AudioCredits.AudioCredit();
            credit.title = "Thought Bot";
            credit.artist = "Jason Shaw";
            credit.collection = "Electronic";
            credit.source = "Audionautix.com";
            credit.link1 = "https://audionautix.com/free-music/electronic";
            credit.link2 = "-";
            credit.copyright =
                "\"Thought Bot\"\n" +
                "Creative Commons Music by Jason Shaw on Audionautix.com\n" +
                "Creative Commons Attribution 4.0 International License\n" +
                "https://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // Action Stage Theme 2
            credit = new AudioCredits.AudioCredit();
            credit.title = "Code Blue";
            credit.artist = "Jason Shaw";
            credit.collection = "Jazz + Funk";
            credit.source = "Audionautix.com";
            credit.link1 = "https://audionautix.com/free-music/JAZZ+FUNK";
            credit.link2 = "-";
            credit.copyright =
                "\"Code Blue\"\n" +
                "Creative Commons Music by Jason Shaw on Audionautix.com\n" +
                "Creative Commons Attribution 4.0 International License\n" +
                "https://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // Knowledge Stage Theme 1
            credit = new AudioCredits.AudioCredit();
            credit.title = "Limit 70";
            credit.artist = "Kevin MacLeod";
            credit.collection = "Medium Electronic (Electronic and Rock)";
            credit.source = "Incompetech.com";
            credit.link1 = "https://incompetech.com/music/royalty-free/music.html";
            credit.link2 = "-";
            credit.copyright = 
                "\"Limit 70\" Kevin MacLeod (incompetech.com)\n" + 
                "Licensed under Creative Commons: By Attribution 4.0 License\n" + 
                "http://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // Knowledge Stage Theme 2
            credit = new AudioCredits.AudioCredit();
            credit.title = "Mesmerizing Galaxy";
            credit.artist = "Kevin MacLeod";
            credit.collection = "Oddities (Everything Else)";
            credit.source = "Incompetech.com";
            credit.link1 = "https://incompetech.com/music/royalty-free/music.html";
            credit.link2 = "-";
            credit.copyright = "\"Mesmerizing Galaxy\" Kevin MacLeod (incompetech.com)\n" + 
                "Licensed under Creative Commons: By Attribution 4.0 License\n" + 
                "http://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // Stage Results
            credit = new AudioCredits.AudioCredit();
            credit.title = "Andreas Theme";
            credit.artist = "Kevin MacLeod";
            credit.collection = "Noire (Film Scoring Moods)";
            credit.source = "Incompetech.com";
            credit.link1 = "https://incompetech.com/music/royalty-free/music.html";
            credit.link2 = "-";
            credit.copyright = 
                "\"Andreas Theme\" Kevin MacLeod (incompetech.com)\n" + 
                "Licensed under Creative Commons: By Attribution 4.0 License\n" + 
                "http://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);

            // Game Results
            credit = new AudioCredits.AudioCredit();
            credit.title = "Morning";
            credit.artist = "Kevin MacLeod";
            credit.collection = "Touching Moments (Lighter Faire)";
            credit.source = "Incompetech.com";
            credit.link1 = "https://incompetech.com/music/royalty-free/music.html";
            credit.link2 = "-";
            credit.copyright = "\"Morning\" Kevin MacLeod (incompetech.com)\n" + 
                "Licensed under Creative Commons: By Attribution 4.0 License\n" + 
                "http://creativecommons.org/licenses/by/4.0/";

            bgmCredits.audioCredits.Add(credit);
        }

        // Loads the SFX and JNG credits.
        private void LoadSoundEffectCredits()
        {
            AudioCredits.AudioCredit credit;

            // Loads sound effects and jingles.

            // Sound Effects
            // Kenney.nl Sound Packs
            // Digital Audio
            credit = new AudioCredits.AudioCredit();
            credit.title = "Digital Audio (Sound Pack)";
            credit.artist = "Kenney Vleugels";
            credit.collection = "Digital Audio";
            credit.source = "GameSounds.xyz, Kenney.nl";
            credit.link1 = "https://gamesounds.xyz/?dir=Kenney%27s%20Sound%20Pack/Digital%20Audio";
            credit.link2 = "https://www.kenney.nl/assets/digital-audio";
            credit.copyright = 
                "Digital Audio\n" + 
                "By Kenney Vleugels (Kenney.nl)\n" + 
                "License: Creative Commons Zero, CC0\n" + 
                "https://creativecommons.org/publicdomain/zero/1.0/";

            sfxCredits.audioCredits.Add(credit);

            // Impact Sounds
            credit = new AudioCredits.AudioCredit();
            credit.title = "Impact Sounds (Sound Pack)";
            credit.artist = "Kenney Vleugels";
            credit.collection = "Impact Sounds";
            credit.source = "GameSounds.xyz, Kenney.nl";
            credit.link1 = "https://gamesounds.xyz/?dir=Kenney%27s%20Sound%20Pack/Impact%20Sounds";
            credit.link2 = "https://www.kenney.nl/assets/impact-sounds";
            credit.copyright =
                "Impact Sounds\n" +
                "By Kenney Vleugels (Kenney.nl)\n" +
                "License: Creative Commons Zero, CC0\n" +
                "https://creativecommons.org/publicdomain/zero/1.0/";

            sfxCredits.audioCredits.Add(credit);

            // Retro Sounds
            // This is no longer available from Kenney.nl, so this was taken from GameSounds.xyz only.
            credit = new AudioCredits.AudioCredit();
            credit.title = "Retro Sounds (Sound Pack)";
            credit.artist = "Kenney Vleugels";
            credit.collection = "Retro Sounds";
            credit.source = "GameSounds.xyz";
            credit.link1 = "https://gamesounds.xyz/?dir=Kenney%27s%20Sound%20Pack/Retro%20Sounds%201";
            credit.link2 = "-";
            credit.copyright =
                "Retro Sounds\n" +
                "By Kenney Vleugels (Kenney.nl)\n" +
                "License: Creative Commons Zero, CC0\n" +
                "https://creativecommons.org/publicdomain/zero/1.0/";

            sfxCredits.audioCredits.Add(credit);

            // Sci-Fi Sounds
            credit = new AudioCredits.AudioCredit();
            credit.title = "Sci-Fi Sounds (Sound Pack)";
            credit.artist = "Kenney Vleugels";
            credit.collection = "Sci-Fi Sounds";
            credit.source = "GameSounds.xyz, Kenney.nl";
            credit.link1 = "https://gamesounds.xyz/?dir=Kenney%27s%20Sound%20Pack/Sci-Fi%20Sounds";
            credit.link2 = "https://kenney.nl/assets/sci-fi-sounds";
            credit.copyright =
                "Sci-Fi Sounds\n" +
                "By Kenney Vleugels (Kenney.nl)\n" +
                "License: Creative Commons Zero, CC0\n" +
                "https://creativecommons.org/publicdomain/zero/1.0/";

            sfxCredits.audioCredits.Add(credit);

            // UI Audio
            credit = new AudioCredits.AudioCredit();
            credit.title = "UI Audio (Sound Pack)";
            credit.artist = "Kenney Vleugels";
            credit.collection = "UI Audio";
            credit.source = "GameSounds.xyz, Kenney.nl";
            credit.link1 = "https://gamesounds.xyz/?dir=Kenney%27s%20Sound%20Pack/UI%20Audio";
            credit.link2 = "https://www.kenney.nl/assets/ui-audio";
            credit.copyright = 
                "UI Audio\n" + 
                "By Kenney Vleugels (Kenney.nl)\n" + 
                "License: Creative Commons Zero, CC0\n" + 
                "https://creativecommons.org/publicdomain/zero/1.0/";

            sfxCredits.audioCredits.Add(credit);

            // Other Sounds
            // Action - Action Defense Generator - Hydro - Flood
            credit = new AudioCredits.AudioCredit();
            credit.title = "Water Splash";
            credit.artist = "Mike Koenig";
            credit.collection = "-";
            credit.source = "SoundBible.com";
            credit.link1 = "https://soundbible.com/1460-Water-Splash.html";
            credit.link2 = "-";
            credit.copyright = 
                "\"Water Splash\"\n" + 
                "Mike Koenig\n" + 
                "Licensed under Creative Commons: By Attribution 3.0 License\n" + 
                "https://creativecommons.org/licenses/by/3.0/";

            sfxCredits.audioCredits.Add(credit);
        }

        // Enables all the credit buttons.
        public void EnableAllCreditButtons()
        {
            // Disables all the credit buttons.
            bgmButton.interactable = true;
            sfxButton.interactable = true;
        }

        // Disables all the credit buttons.
        public void DisableAllCreditButtons()
        {
            // Disables all the credit buttons.
            bgmButton.interactable = false;
            sfxButton.interactable = false;
        }

        // Shows the BGM credits.
        public void ShowBackgroundMusicCredits()
        {
            // Saves the current credit index, switches over, and then sets the new credit index.
            SaveCurrentCreditIndex();
            creditsInterface.audioCredits = bgmCredits;
            creditsInterface.SetCreditIndex(bgmCreditsIndex);

            // Change button settings.
            EnableAllCreditButtons();
            bgmButton.interactable = false;
        }

        // Shows the SFX credits.
        public void ShowSoundEffectCredits()
        {
            // Saves the old credits to know what button to enable.
            AudioCredits oldCredits = creditsInterface.audioCredits;

            // Saves the current credit index, switches over, and then sets the new credit index.
            SaveCurrentCreditIndex();
            creditsInterface.audioCredits = sfxCredits;
            creditsInterface.SetCreditIndex(sfxCreditsIndex);

            // Change button settings.
            EnableAllCreditButtons();
            sfxButton.interactable = false;
        }

        // Gets the current credits.
        public AudioCredits GetCurrentCredits()
        {
            return creditsInterface.audioCredits;
        }

        // Returns 'true' if the BGM credits are the current credits.
        public bool IsCurrentCreditsBgmCredits()
        {
            return GetCurrentCredits() == bgmCredits;
        }

        // Returns 'true' if the SFX credits are the current credits.
        public bool IsCurrentCreditsSfxCredits()
        {
            return GetCurrentCredits() == sfxCredits;
        }

        // Saves the current credit index of the applicable window.
        public void SaveCurrentCreditIndex()
        {
            // Only one of these should be active at a time.
            if (IsCurrentCreditsBgmCredits())
            {
                bgmCreditsIndex = creditsInterface.GetCurrentCreditIndex();
            }
            else if (IsCurrentCreditsSfxCredits())
            {
                sfxCreditsIndex = creditsInterface.GetCurrentCreditIndex();
            }
        }
    }
}