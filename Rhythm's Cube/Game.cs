using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;

namespace Rhythms_Cube;

public struct Game_Window
{
    public Game_Window(Vector2i size, string title, int max_fps)
    {
        Size = size;
        Title = title;
        Max_FPS = max_fps;
    }

    public Vector2i Size { get; }
    public string Title { get; }
    public int Max_FPS { get; }

    public override string ToString() =>
        $"Window: title = '{Title}', size = '{Size.X.ToString()}, {Size.Y.ToString()}', max_fps = '{Max_FPS}'";
}

class Game : GameWindow
{
    Scene current_scene = new Scene();

    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { ClientSize = (width, height), Title = title })
    {
        Game_Window game_window = new(new Vector2i(width, height), Title, 30);
        Console.WriteLine(game_window.ToString());
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        current_scene.Call_On_Load(out bool close_the_window);
        if (close_the_window)
        {
            Close();
        }
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        current_scene.Call_On_Update_Frame(args, KeyboardState, out bool close_the_window);
        if (close_the_window)
        {
            Close();
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        current_scene.Call_On_Render_Frame(args);

        SwapBuffers();
    }

    [STAThread]
    static public void Main()
    {
        using (Game game = new(800, 600, "Rhythm's Cube"))
        {
            game.Run();
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        current_scene.Call_On_Framebuffer_Resize(e);
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        current_scene.Call_On_Unload();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        Console.WriteLine("See you next time!");
    }
}