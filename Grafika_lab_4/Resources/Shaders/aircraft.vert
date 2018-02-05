#version 440
in vec3 Position;
in vec3 Normal;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
uniform vec3 LightPosition;

out vec3 pass_Normal;
out vec3 toLightVector;
out vec3 toCameraVector;
out float visibility;

const float density=0.008;
const float gradient=1.5;

void main()
{
	vec4 worldPosition=ModelMatrix*vec4(Position,1.0f);
	vec4 positionRelativeToCam=ViewMatrix*worldPosition;
	gl_Position=ProjectionMatrix*positionRelativeToCam;

	pass_Normal = mat3(ModelMatrix)*Normal;
	toLightVector=LightPosition-vec3(worldPosition);
	toCameraVector=(inverse(ViewMatrix)*vec4(0.0,0.0,0.0,1.0)).xyz-worldPosition.xyz;

	float distance=length(positionRelativeToCam.xyz);
	visibility=exp(-pow((distance*density),gradient));
	visibility=clamp(visibility,0.0,1.0);
};
