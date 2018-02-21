#version 440

in vec3 Position;
in vec3 Normal;
in vec2 TextCoord;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

out vec3 pass_Normal;
out vec3 toCameraVector;
out vec3 pass_Position;
out vec2 pass_textcoord;


void main()
{
	vec4 worldPosition=ModelMatrix*vec4(Position,1.0f);
	vec4 positionRelativeToCam=ViewMatrix*worldPosition;
	gl_Position=ProjectionMatrix*positionRelativeToCam;

	pass_Normal = mat3(transpose(inverse(ModelMatrix))) * Normal;
	pass_Position=worldPosition.xyz;
	pass_textcoord=TextCoord;

	toCameraVector=(inverse(ViewMatrix)*vec4(0.0,0.0,0.0,1.0)).xyz-worldPosition.xyz;
};
