#version 440

in vec3 PositionAttribute;
in vec3 NormalAttribute;
in vec2 TextureCoordAttribute;

uniform mat4 ModelMatrixUniform;
uniform mat4 ViewMatrixUniform;
uniform mat4 ProjectionMatrixUniform;

out vec3 pass_Normal;
out vec3 toCameraVector;
out vec3 pass_Position;
out vec2 pass_textcoord;

void main()
{
	vec4 worldPosition=ModelMatrixUniform*vec4(PositionAttribute,1.0f);
	vec4 positionRelativeToCam=ViewMatrixUniform*worldPosition;
	gl_Position=ProjectionMatrixUniform*positionRelativeToCam;

	pass_Normal = mat3(transpose(inverse(ModelMatrixUniform))) * NormalAttribute;
	pass_Position=worldPosition.xyz;
	pass_textcoord=TextureCoordAttribute;

	toCameraVector=(inverse(ViewMatrixUniform)*vec4(0.0,0.0,0.0,1.0)).xyz-worldPosition.xyz;
};
