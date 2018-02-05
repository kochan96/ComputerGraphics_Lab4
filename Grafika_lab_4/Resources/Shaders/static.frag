#version 440

in vec2 pass_textcoord; 
in vec3 pass_Normal;
in vec3 toLightVector;
in float visibility;


uniform sampler2D TextureSampler;
uniform vec3 LightColor;
uniform vec3 SkyColor;

out vec4 outputColor;


void main()
{
	outputColor=vec4(0.5f,0.5f,0.0f,1.0f);
	
}
