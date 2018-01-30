#version 440

in vec2 pass_textcoord; 
in vec3 pass_Normal;
in vec3 toLightVector;

out vec4 outputColor;

uniform sampler2D TextureSampler;
uniform vec3 LightColor;

void main()
{
	 
     vec4 color = texture(TextureSampler, pass_textcoord);
	 float cosNL=pass_Normal.x*toLightVector.x+pass_Normal.y*toLightVector.y+pass_Normal.z*toLightVector.z;
	 float R=LightColor.x*color.x*cosNL;
	 float G=LightColor.y*color.y*cosNL;
	 float B=LightColor.z*color.z*cosNL;

	 outputColor=vec4(R,G,B,1.0f);
};