#version 440
in vec3 Position;
in vec2 TextCoord;
in vec3 Normal;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
uniform vec3 LightPosition;

out vec2 pass_textcoord;
out vec3 pass_Normal;
out vec3 toLightVector;



void main()
{
	vec4 worldPosition=ModelMatrix*vec4(Position,1.0f);
	gl_Position=ProjectionMatrix*ViewMatrix*worldPosition;
	pass_textcoord=TextCoord;
	pass_Normal = normalize(mat3(ModelMatrix)*Normal);
	toLightVector=normalize(LightPosition-vec3(worldPosition));

};
