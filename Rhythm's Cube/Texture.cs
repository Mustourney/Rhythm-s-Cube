using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

class Texture
{
    private int handle = GL.GenTexture();

    public bool is_there_error = false;

    public Texture(string path)
    {
        if (Path.Exists(path) == false)
        {
            Console.WriteLine($"The texture path doesn't exists! {path}");
            is_there_error = true;
            return;
        }

        StbImage.stbi_set_flip_vertically_on_load(1);

        ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

        float[] vertices =
        {
            //Position          Texture coordinates
            0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        Shader shader = new("Shaders/texture.vert", "Shaders/texture.frag");
        int tex_coord_location = GL.GetAttribLocation(shader.handle, "aTexCoord");
        GL.EnableVertexAttribArray(tex_coord_location);
        GL.VertexAttribPointer(tex_coord_location, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        shader.Set_Int("texture0", 1);
    }

    public void Use(TextureUnit texture_unit)
    {
        GL.ActiveTexture(texture_unit);
        GL.BindTexture(TextureTarget.Texture2D, handle);
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(handle);

            disposedValue = true;
        }
    }

    ~Texture()
    {
        if (disposedValue == false)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}