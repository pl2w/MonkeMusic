using BepInEx.Configuration;

namespace MonkeMusic.Behaviours.Loaders
{
    public static class MusicConfig
    {
        public static EInput bindedInput;
        public static float heldSensitivity;

        public static void Init(ConfigFile config)
        {
            bindedInput = config.Bind("Input", "Bind", EInput.LeftGrip, "What button opens the music player?").Value;
            heldSensitivity = config.Bind("Input", "Opening Sensitivity", 0.5f, "How sensitive opening is.").Value;
        }
    }
}
