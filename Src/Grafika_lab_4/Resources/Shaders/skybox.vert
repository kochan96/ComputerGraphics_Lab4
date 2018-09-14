# version 440

in vec3 PositionAttribute;

uniform mat4 ViewMatrixUniform;
uniform mat4 ProjectionMatrixUniform;

out vec3 pass_textcoord;

void main()
{
	gl_Position=ProjectionMatrixUniform*ViewMatrixUniform*vec4(PositionAttribute,1.0f);
	pass_textcoord=PositionAttribute;
}
