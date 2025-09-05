using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

class Scene
{
    public void Call_On_Update_Frame(FrameEventArgs args, KeyboardState key_input, out bool close_the_window)
    {
        current_scene.On_Update_Frame(args, key_input);
        close_the_window = current_scene.close_the_window;
    }

    public void Call_On_Load(out bool close_the_window)
    {
        current_scene.On_Load();
        close_the_window = current_scene.close_the_window;
    }

    public void Call_On_Render_Frame(FrameEventArgs args)
    {
        current_scene.On_Render_Frame(args);
    }

    public void Call_On_Framebuffer_Resize(FramebufferResizeEventArgs e)
    {
        current_scene.On_Framebuffer_Resize(e);
    }
    public void Call_On_Unload()
    {
        current_scene.On_Unload();
    }

    private static Menu menu = new Menu();
    private Menu current_scene = menu;

    public void Load_and_Unload(string scene_name, out bool success)
    {
        success = true;

        if (scene_name == "Menu")
        {
            current_scene.On_Unload();
            current_scene = menu;
            current_scene.On_Load();
        }
        else
        {
            success = false;
        }
    }
}