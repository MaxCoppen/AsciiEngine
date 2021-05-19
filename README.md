# AsciiEngine
A C# Game Engine using libtcod and SDL2.

```csharp
// Initialize the renderer.
AsciiRenderer.Initialize(80, 50, "Ascii Game");

// Ascii engine events :
GameManager gameManager = new GameManager();
AsciiEngine.OnStart += gameManager.Start;
AsciiEngine.OnUpdate += gameManager.Update;
AsciiEngine.OnKeyPressed += gameManager.KeyPressed;
AsciiEngine.OnRender += gameManager.Render;

// Initialize the engine.
AsciiEngine.Initialize(60);
```
