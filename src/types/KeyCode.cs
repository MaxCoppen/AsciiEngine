using libtcod;
using System;

namespace Ascii.Types
{
    public class UserInput
    {
        /// <summary>
        /// Format the key press as ascii input.
        /// </summary>
        public static AsciiInput FormatInput(KeyPress pressEvent)
        {
            AsciiInput input = new AsciiInput(pressEvent);

            return input;
        }
    }

    public struct AsciiInput
    {
        // The key
        public readonly string Key;

        // Action keys
        public readonly bool Alt;
        public readonly bool Shift;
        public readonly bool Ctrl;

        public AsciiInput(KeyPress key)
        {
            // Set action keys.
            Alt = key.Alt;
            Shift = key.Shift;
            Ctrl = key.Control;

            // Set the Key string.
            Key = key.KeyCode switch
            {
                // If the key is unknown:
                KeyCode.None => "None",

                // If the key is a number:
                KeyCode.Zero => "0",
                KeyCode.One => "1",
                KeyCode.Two => "2",
                KeyCode.Three => "3",
                KeyCode.Four => "4",
                KeyCode.Five => "5",
                KeyCode.Six => "6",
                KeyCode.Seven => "7",
                KeyCode.Eight => "8",
                KeyCode.Nine => "9",

                // If the key is a character:
                KeyCode.Char => string.Empty + Convert.ToChar(key.Character),

                // Default:
                _ => key.KeyCode.ToString(),
            };
        }
    }
}
