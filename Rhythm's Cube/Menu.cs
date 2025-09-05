using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

class Menu : Scene
{
    private List<Shader> active_shaders = new();
    private List<Texture> active_textures = new();
    public bool Close_the_window = false;

    private int vertex_array_object;
    private int vertex_buffer_object;

    public void On_Load()
    {
        GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);

        float[] vertices = [
            -1f, -1, 0.0f, //Bottom-left vertex
            0f, -1f, 0.0f, //Bottom-right vertex
            -0.5f,  0f, 0.0f  //Top vertex
        ];

        vertex_buffer_object = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_object);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        vertex_array_object = GL.GenVertexArray();
        GL.BindVertexArray(vertex_array_object);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

        // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        Shader my_first_shader = new("Shaders/shader.vert", "Shaders/shader.frag");

        if (my_first_shader.is_there_error)
        {
            Close_the_window = true;
            return;
        }

        active_shaders.Add(my_first_shader);
        my_first_shader.Use();

        Texture texture = new("Textures/texture.png");
        if (texture.is_there_error)
        {
            Close_the_window = true;
            return;
        }

        active_textures.Add(texture);
    }

    public void On_Update_Frame(FrameEventArgs args, KeyboardState key_input)
    {
        if (key_input.IsKeyPressed(Keys.Escape))
        {
            Close_the_window = true;
        }
    }

    public void On_Render_Frame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        foreach (Texture texture in active_textures)
        {
            texture.Use(TextureUnit.Texture0);
        }

        foreach (Shader shader in active_shaders)
        {
            shader.Use();
        }

        GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, 0);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    public void On_Framebuffer_Resize(FramebufferResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    public void On_Unload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        GL.DeleteBuffer(vertex_buffer_object);
        GL.DeleteVertexArray(vertex_array_object);

        foreach (Shader shader in active_shaders)
        {
            GL.DeleteProgram(shader.handle);
        }
    }
}