#version 440
in vec3 Position;
in vec2 TextCoord;
in vec3 Normal;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

out vec2 pass_textcoord;
out vec3 pass_Normal;


void main()
{
	mat4 matrix=ProjectionMatrix*ViewMatrix*ModelMatrix;
	gl_Position=matrix*vec4(Position,1.0f);
	pass_textcoord=TextCoord;
	pass_Normal = normalize(mat3(ModelMatrix)*Normal);

};
