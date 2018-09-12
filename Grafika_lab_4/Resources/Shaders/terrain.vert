#version 440
in vec3 PositionAttribute;
in vec2 TextureCoordAttribute;
in vec3 NormalAttribute;

uniform mat4 ModelMatrixUniform;
uniform mat4 ViewMatrixUniform;
uniform mat4 ProjectionMatrixUniform;

out vec2 pass_textcoord;
out vec3 pass_Normal;
out vec3 pass_Position;
out vec3 toCameraVector;

void main()
{
	vec4 worldPosition=ModelMatrixUniform*vec4(PositionAttribute,1.0f);
	vec4 positionRelativeToCam=ViewMatrixUniform*worldPosition;
	gl_Position=ProjectionMatrixUniform*positionRelativeToCam;

	pass_textcoord=TextureCoordAttribute;
	pass_Normal = (ModelMatrixUniform*vec4(NormalAttribute,0.0)).xyz;
	pass_Position=worldPosition.xyz;
	toCameraVector=(inverse(ViewMatrixUniform)*vec4(0.0,0.0,0.0,1.0)).xyz-worldPosition.xyz;
};