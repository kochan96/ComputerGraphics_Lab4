#version 440

in vec3 pass_Normal;
in vec3 toLightVector;
in vec3 toCameraVector;

uniform vec3 LightColor;
uniform vec3 AmbientColor;
uniform vec3 DiffuseColor;
uniform vec3 SpecularColor;
uniform float SpecularExponent;

out vec4 outputColor;

void main()
{

	vec3 unitNormal=normalize(pass_Normal);
	vec3 unitLightVector=normalize(toLightVector);
	vec3 unitCameraVector=normalize(toCameraVector);

	//ambient
	vec3 ambient=LightColor * AmbientColor;

	//diffuse
	float nDot1=dot(unitNormal,unitLightVector);
	float brightness=max(nDot1,0.0);
	vec3 diffuse=brightness*LightColor*DiffuseColor;

	//specular
	vec3 lightDirection=-unitLightVector;
	vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
	float specularFactor=dot(reflectedLightDirection,unitCameraVector);
	specularFactor=max(specularFactor,0.0f);
	float dampedFactor=pow(specularFactor,SpecularExponent);
	vec3 finalSpecular=dampedFactor*LightColor*SpecularColor;


	float ambient_strength=0.05f;
	float diffuse_strength=0.9f;
	float specular_strength=0.05f;

	vec3 finalColor=ambient_strength*ambient+diffuse_strength*diffuse+specular_strength*finalSpecular;
	outputColor=vec4(finalColor,1.0f);
}