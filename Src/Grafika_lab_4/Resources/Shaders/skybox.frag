#version 440
in vec3 pass_textcoord;

uniform samplerCube CubeMap;

out vec4 outputColor;

void main()
{
	vec3 color=vec3(texture(CubeMap,pass_textcoord));
	outputColor=vec4(color,1.0f);
}