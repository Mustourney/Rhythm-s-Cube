using OpenTK.Graphics.OpenGL4;

public class Shader
{
    public int handle;
    public bool is_there_error = false;

    public Shader(string vertex_path, string fragment_path, bool use_current_dir)
    {
        if (use_current_dir)
        {
            string project_dir = Directory.GetCurrentDirectory();
            vertex_path = $"{project_dir}/{vertex_path}";
            fragment_path = $"{project_dir}/{fragment_path}";
        }

        if (File.Exists(vertex_path) == false)
        {
            Console.WriteLine($"The vertex shader path doesn't exists! {vertex_path}");
            is_there_error = true;
            return;
        }

        if (File.Exists(fragment_path) == false)
        {
            Console.WriteLine($"The fragment shader path doesn't exists! {fragment_path}");
            is_there_error = true;
            return;
        }

        string vertex_shader_source = File.ReadAllText(vertex_path);
        string framgent_shader_source = File.ReadAllText(fragment_path);

        int vertex_shader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertex_shader, vertex_shader_source);

        int fragment_shader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragment_shader, framgent_shader_source);

        GL.CompileShader(vertex_shader);

        GL.GetShader(vertex_shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            is_there_error = true;
            string infoLog = GL.GetShaderInfoLog(vertex_shader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(fragment_shader);

        GL.GetShader(fragment_shader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            is_there_error = true;
            string infoLog = GL.GetShaderInfoLog(fragment_shader);
            Console.WriteLine(infoLog);
        }

        handle = GL.CreateProgram();

        GL.AttachShader(handle, vertex_shader);
        GL.AttachShader(handle, fragment_shader);

        GL.LinkProgram(handle);

        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            is_there_error = true;
            string infoLog = GL.GetProgramInfoLog(handle);
            Console.WriteLine(infoLog);
        }

        GL.DetachShader(handle, vertex_shader);
        GL.DetachShader(handle, fragment_shader);
        GL.DeleteShader(fragment_shader);
        GL.DeleteShader(vertex_shader);
    }

    public void Use()
    {
        GL.UseProgram(handle);
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

    ~Shader()
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