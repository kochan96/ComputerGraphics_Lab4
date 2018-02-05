#version 440

in vec3 Position;
in vec3 Normal;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
uniform vec3 LightPosition;

void main()
{
	gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*vec4(Position,1.0f);
}
