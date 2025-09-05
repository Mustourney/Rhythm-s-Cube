#version 460 core

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
    FragColor = mix(texture(texture1, TexCoord), texture(texture2, TexCoord), 0.2);
}