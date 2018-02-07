# version 440

in vec3 Position;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

out vec3 pass_textcoord;

void main()
{
	gl_Position=ProjectionMatrix*ViewMatrix*vec4(Position,1.0f);
	pass_textcoord=Position;

}
