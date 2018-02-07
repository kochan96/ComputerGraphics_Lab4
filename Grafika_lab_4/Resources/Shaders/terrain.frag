#version 440

in vec2 pass_textcoord; 
in vec3 pass_Normal;
in vec3 toLightVector;
in vec3 toCameraVector;


uniform sampler2D TextureSampler;
uniform vec3 LightColor;
uniform vec3 SkyColor;

out vec4 outputColor;

void main()
{

	vec3 unitNormal=normalize(pass_Normal);
	vec3 unitLightVector=normalize(toLightVector);
	vec3 unitCameraVector=normalize(toCameraVector);

    vec3 color = vec3(texture(TextureSampler, pass_textcoord));

    // ambient
	vec3 ambient=LightColor*color;

    // diffuse
    float nDot1=dot(unitNormal,unitLightVector);
	float brightness=max(nDot1,0.0f);
	vec3 diffuse=brightness*LightColor*color;

	//specular
	vec3 lightDirection=-unitLightVector;
	vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
	float specularFactor=dot(reflectedLightDirection,unitCameraVector);
	specularFactor=max(specularFactor,0.0f);
	float dampedFactor=pow(specularFactor,20);
	vec3 finalSpecular=dampedFactor*LightColor*color;

   float ambient_strength=0.2f;
   float diffuse_strength=0.6f;
   float specular_strength=0.2f;

   vec3 final_color = ambient_strength*ambient+diffuse_strength*diffuse+specular_strength*finalSpecular;
   outputColor = vec4(final_color, 1.0);
}