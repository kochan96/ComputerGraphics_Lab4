#version 440

in vec2 pass_textcoord;
in vec3 pass_Normal;
in vec3 toLightVector;
in float visibility;
in vec3 pass_Color;
in vec3 toCameraVector;

uniform vec3 LightColor;
uniform bool HasTexture;
uniform sampler2D TextureSampler;

out vec4 outputColor;


void main()
{
	vec3 unitNormal=normalize(pass_Normal);
	vec3 unitLightVector=normalize(toLightVector);
	vec3 unitCameraVector=normalize(toCameraVector);
	vec3 color;
	if(HasTexture)
	{
		color = vec3(texture(TextureSampler, pass_textcoord));
	}else
	{
		color = pass_Color;
	}
	

	//ambient
	vec3 ambient=LightColor*color;

	//diffuse
	float nDot1=dot(unitNormal,unitLightVector);
	float brightness=max(nDot1,0.0);
	vec3 diffuse=brightness*LightColor*color;

	//specular
	vec3 lightDirection=-unitLightVector;
	vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
	float specularFactor=dot(reflectedLightDirection,unitCameraVector);
	specularFactor=max(specularFactor,0.0f);
	float dampedFactor=pow(specularFactor,50);
	vec3 finalSpecular=dampedFactor*LightColor*color;


	float ambient_strength=0.2f;
	float diffuse_strength=0.2f;
	float specular_strength=0.6f;

	vec3 finalColor=ambient_strength*ambient+diffuse_strength*diffuse+specular_strength*finalSpecular;
	outputColor=vec4(finalColor,1.0f);
}
