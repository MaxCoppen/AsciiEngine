using Ascii.Rendering;
using Ascii.Types;
using libtcod;
using System;
using System.Threading.Tasks;

namespace Ascii.Engine
{
    public static class AsciiEngine
    {
        // Engine statistics.
        private static int frameRate;
        private static float time;

        // Engine responsibilities.
        public delegate void BasicEventHandler();
        public delegate void UpdateEventHandler(float deltaTime);
        public delegate void InputEventHandler(AsciiInput input);

        public static event BasicEventHandler OnStart;
        public static event UpdateEventHandler OnUpdate;
        public static event InputEventHandler OnKeyPressed;
        public static event BasicEventHandler OnRender;
        public static event BasicEventHandler OnClose;

        /// <summary>
        /// Initializes the game engine.
        /// </summary>
        public static void Initialize(int frameRate)
        {
            // Set the engine settings.
            AsciiEngine.frameRate = frameRate;

            // Set the sys settings.
            SystemAPI.FPS = frameRate;

            // Start the engine.
            EngineCycle();

            // Start input cycle.
            InputCycle();
        }

        /// <summary>
        /// The engine life cycle.
        /// </summary>
        private static async void EngineCycle()
        {
            // Invoke the start event.
            OnStart?.Invoke();

            while (AsciiRenderer.IsWindowOpen())
            {
                // Calculate the delta time.
                float deltaTime = SystemAPI.ElapsedSeconds - time;

                // Update the time.
                time = SystemAPI.ElapsedSeconds;

                // Invoke the update event.
                OnUpdate?.Invoke(deltaTime);

                // Invoke the render event.
                OnRender?.Invoke();

                // Render the buffer.
                AsciiRenderer.Render();

                // Wait for the frame to end.
                await Task.Delay(1000 / frameRate);
            }

            // Invoke the close event.
            OnClose?.Invoke();
        }

        /// <summary>
        /// The input cycle.
        /// </summary>
        private static void InputCycle()
        {
            while (AsciiRenderer.IsWindowOpen())
            {
                // Wait until key is pressed.
                KeyPress press = Keyboard.WaitForKeyPress(true);

                // Convert byte to keycode.
                AsciiInput input = UserInput.FormatInput(press);

                // Invoke the key pressed event.
                OnKeyPressed?.Invoke(input);
            }
        }
    }
}
