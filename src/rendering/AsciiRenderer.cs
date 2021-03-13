using System;
using Ascii.Types;
using libtcod;

namespace Ascii.Rendering
{
    public static class AsciiRenderer
    {
        // Console window.
        public static RootConsoleWindow window;

        // Screen data.
        private static AsciiPixel[,] screenBuffer;
        private static AsciiColor white = AsciiColor.FromRGB(255, 255, 255);

        // Screen size.
        private static int screenWidth;
        private static int screenHeight;

        /// <summary>
        /// Initializes the renderer.
        /// </summary>
        public static void Initialize(int screenWidth, int screenHeight, string windowTitle, bool fullscreen = false)
        {
            AsciiRenderer.screenWidth = screenWidth;
            AsciiRenderer.screenHeight = screenHeight;

            window = RootConsoleWindow.Setup(screenWidth, screenHeight, windowTitle, fullscreen, ConsoleRender.SDL);
            window.Clear();
            screenBuffer = new AsciiPixel[screenWidth, screenHeight];

            for (int y = 0; y < screenHeight; y++)
            {
                for (int x = 0; x < screenWidth; x++)
                {
                    screenBuffer[x, y] = new AsciiPixel(char.MinValue, white);
                }
            }
        }

        /// <summary>
        /// Set a character in the screen buffer.
        /// </summary>
        public static void SetChar(int x, int y, char c)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");

            screenBuffer[x, y].SetChar(c);
        }

        /// <summary>
        /// Set a colored character in the screen buffer.
        /// </summary>
        public static void SetChar(int x, int y, char c, AsciiColor cc)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");

            screenBuffer[x, y].SetChar(c);
            screenBuffer[x, y].SetColor(cc);
        }

        /// <summary>
        /// Set a string in the screen buffer.
        /// </summary>
        public static void SetString(int x, int y, string s)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");
            if (IsOnScreen(x + s.Length, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");

            for (int c = 0; c < s.Length; c++)
            {
                screenBuffer[x + c, y].SetChar(s[c]);
            }
        }

        /// <summary>
        /// Set a colored string in the screen buffer.
        /// </summary>
        public static void SetString(int x, int y, string s, AsciiColor cc)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");
            if (IsOnScreen(x + s.Length, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");

            for (int c = 0; c < s.Length; c++)
            {
                screenBuffer[x + c, y].SetChar(s[c]);
                screenBuffer[x + c, y].SetColor(cc);
            }
        }

        /// <summary>
        /// Set a colored string in the screen buffer.
        /// </summary>
        public static void SetString(int x, int y, string s, AsciiColor cc, AsciiColor bcc)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");
            if (IsOnScreen(x + s.Length, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");

            for (int c = 0; c < s.Length; c++)
            {
                screenBuffer[x + c, y].SetChar(s[c]);
                screenBuffer[x + c, y].SetColor(cc);
                screenBuffer[x + c, y].SetBackgroundColor(bcc);
            }
        }

        /// <summary>
        /// Set a foreground color in the screen buffer.
        /// </summary>
        public static void SetForegroundColor(int x, int y, AsciiColor cc)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");

            screenBuffer[x, y].SetColor(cc);
        }

        /// <summary>
        /// Set a background color in the screen buffer.
        /// </summary>
        public static void SetBackgroundColor(int x, int y, AsciiColor cc)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at setting buffer element outside of the buffer size");

            screenBuffer[x, y].SetBackgroundColor(cc);
        }

        /// <summary>
        /// Get a character from the screen buffer.
        /// </summary>
        public static char GetChar(int x, int y)
        {
            if (IsOnScreen(x, y) == false)
                throw new Exception("Attempt at getting buffer element outside of the buffer size");

            return screenBuffer[x, y].character;
        }

        /// <summary>
        /// Set the background color.
        /// </summary>
        public static void SetBackground(AsciiColor cc)
        {
            for (int y = 0; y < screenBuffer.GetLength(1); y++)
            {
                for (int x = 0; x < screenBuffer.GetLength(0); x++)
                {
                    screenBuffer[x, y].SetBackgroundColor(cc);
                }
            }
        }

        /// <summary>
        /// Render the current screen buffer.
        /// </summary>
        public static void Render()
        {
            // Clear the window.
            //window.Clear();

            for (int y = 0; y < screenHeight; y++)
            {
                for (int x = 0; x < screenWidth; x++)
                {
                    // Skip this char if its not updated.
                    if (screenBuffer[x, y] == false)
                        continue;

                    // Update the char and set its status to updated.
                    Point p = new Point(x, y);
                    window.SetCharacter(p, screenBuffer[x, y].character);
                    window.SetForeground(p, screenBuffer[x, y].color);
                    window.SetBackground(p, screenBuffer[x, y].bgcolor);
                    screenBuffer[x, y].Update();
                }
            }
            
            // Flush the window.
            window.Flush();
        }

        /// <returns>If the window is open.</returns>
        public static bool IsWindowOpen()
        {
            return window.IsWindowClosed == false;
        }

        /// <summary>
        /// Check if a position is within the screen bounds.
        /// </summary>
        private static bool IsOnScreen(int x, int y)
        {
            return !(x < 0 || x >= screenWidth || y < 0 || y >= screenHeight);
        }
    }

    public class AsciiPixel
    {
        public char character
        { get; private set; }

        public AsciiColor color
        { get; private set; }

        public AsciiColor bgcolor
        { get; private set; }

        public bool updated
        { get; private set; }

        public AsciiPixel(char c, AsciiColor cc)
        {
            character = c;
            color = cc;
            bgcolor = AsciiColor.FromRGB(0, 0, 0);

            updated = true;
        }

        public static implicit operator bool(AsciiPixel p) => p.updated;

        public void SetChar(char c) { character = c; updated = true; }

        public void SetColor(AsciiColor cc) { color = cc; updated = true; }

        public void SetBackgroundColor(AsciiColor cc) { bgcolor = cc; updated = true; }

        public void Update() => updated = false;
    }

    public class AsciiLayer
    {
        /// Render layer.
    }
}
