#version 440

struct Light {
 vec3 Position;
 vec3 Color;

 float DiffuseIntensity;
 float AmbientIntensity;
 float SpecularIntensity;

 int LightType;
 vec3 Direction;
 float ConeAngle;
 vec3 Attenuation;
};

in vec2 pass_textcoord; 
in vec3 pass_Normal;
in vec3 pass_Position;
in vec3 toCameraVector;

uniform sampler2D TextureSamplerUniform;
uniform Light LightsUniform[5];
uniform bool PhongLightningUniform;

out vec4 outputColor;

void main()
{

	vec3 unitNormal=normalize(pass_Normal);
	
	vec3 unitCameraVector=normalize(toCameraVector);

    vec3 color = vec3(texture(TextureSamplerUniform, pass_textcoord));

	vec3 totalDiffuse=vec3(0.0);
	vec3 totalSpecular=vec3(0.0);
	vec3 totalAmbient=vec3(0.0);
	for(int i=0;i<5;i++){

		if(LightsUniform[i].Color==vec3(0.0))
		{
			continue;
		}

		vec3 toLightVector=LightsUniform[i].Position - pass_Position;
		//attenuation
		float distance=length(toLightVector);
		float attenuationFactor=LightsUniform[i].Attenuation.x+(LightsUniform[i].Attenuation.y*distance)+(LightsUniform[i].Attenuation.z*distance*distance);
		vec3 unitLightVector=normalize(toLightVector);

		 bool inCone = false;
		if(LightsUniform[i].LightType == 1 && degrees(acos(dot(-unitLightVector, LightsUniform[i].Direction))) < LightsUniform[i].ConeAngle)
		{
			inCone = true;
		}

		if(LightsUniform[i].LightType == 2){
			unitLightVector = normalize(-LightsUniform[i].Direction);
		}

		//ambient
			totalAmbient+=LightsUniform[i].AmbientIntensity*LightsUniform[i].Color;
		if(LightsUniform[i].LightType!=1 || inCone){
			
			//diffuse
			float nDot1=dot(unitNormal,unitLightVector);
			float brightness=max(nDot1,0.0);
			totalDiffuse+=(LightsUniform[i].DiffuseIntensity*brightness*LightsUniform[i].Color)/attenuationFactor;
		
			//specular
			if(PhongLightningUniform)
			{
				vec3 lightDirection=-unitLightVector;
				vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
				float specularFactor=dot(reflectedLightDirection,unitCameraVector);
				specularFactor=max(specularFactor,0.0f);
				float dampedFactor=pow(specularFactor,20);
				totalSpecular+=(LightsUniform[i].SpecularIntensity*dampedFactor*LightsUniform[i].Color)/attenuationFactor;
			}
		}
	}
	
   vec3 final_color =totalAmbient*color+totalDiffuse*color+totalSpecular*color;
   outputColor = vec4(final_color, 1.0);
}