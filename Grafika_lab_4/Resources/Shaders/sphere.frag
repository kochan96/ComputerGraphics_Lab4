#version 440

in vec2 pass_textcoord;
in vec3 pass_Normal;
in vec3 toLightVector[4];
in float visibility;
in vec3 pass_Color;
in vec3 toCameraVector;

uniform vec3 LightColor[4];
uniform bool HasTexture;
uniform sampler2D TextureSampler;

out vec4 outputColor;


void main()
{
	vec3 unitNormal=normalize(pass_Normal);
	
	vec3 unitCameraVector=normalize(toCameraVector);

	vec3 color;
	if(HasTexture)
	{
		color = vec3(texture(TextureSampler, pass_textcoord));
	}else
	{
		color = pass_Color;
	}
	
	vec3 totalDiffuse=vec3(0.0)
	vec3 totalSpecular=vec3(0.0);
	vec3 totalAmbient=vec3(0.0);
	for(int i=0;i<4;i++){
		//lightninhg
		vec3 unitLightVector=normalize(toLightVector[i]);
		// ambient
		totalAmbient*=LightColor[i];

		// diffuse
		float nDot1=dot(unitNormal,unitLightVector);
		float brightness=max(nDot1,0.0f);
		totalDiffuse+=brightness*LightColor[i];

		//specular
		vec3 lightDirection=-unitLightVector;
		vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
		float specularFactor=dot(reflectedLightDirection,unitCameraVector);
		specularFactor=max(specularFactor,0.0f);
		float dampedFactor=pow(specularFactor,20);
		totalSpecular+=dampedFactor*LightColor[i];
	}

   float ambient_strength=0.2f;
   float diffuse_strength=0.6f;
   float specular_strength=0.2f;

   vec3 final_color = ambient_strength*ambient*color+diffuse_strength*diffuse*color+specular_strength*finalSpecular*color;
   outputColor = vec4(final_color, 1.0);

}
