#version 440

struct Light {
 vec3 Position;
 vec3 Color;
 
 float AmbientIntensity;
 float DiffuseIntensity;
 float SpecularIntensity;

 int LightType;
 vec3 Direction;
 float ConeAngle;
 vec3 Attenuation;
};

in vec3 pass_Normal;
in vec3 pass_Position;
in vec2 pass_textcoord;
in vec3 toCameraVector;

uniform sampler2D TextureSampler;
uniform vec3 AmbientColor;
uniform vec3 DiffuseColor;
uniform vec3 SpecularColor;
uniform float SpecularExponent;
uniform Light Lights[5];
uniform bool PhongLightning;
uniform bool HasTexture;

out vec4 outputColor;

void main()
{
	vec4 color; 
	if(HasTexture==true)
	{
		color=texture(TextureSampler,pass_textcoord);
	}
	else
	{
		color=vec4(1.0,1.0,1.0,1.0);
	}

	vec3 unitNormal=normalize(pass_Normal);
	
	vec3 unitCameraVector=normalize(toCameraVector);


	vec3 totalDiffuse=vec3(0.0);
	vec3 totalSpecular=vec3(0.0);
	vec3 totalAmbient=vec3(0.0);
	for(int i=0;i<5;i++){

		if(Lights[i].Color==vec3(0.0))
		{
			continue;
		}
		vec3 toLightVector=Lights[i].Position - pass_Position;
		vec3 unitLightVector=normalize(toLightVector);
		bool inCone=false;
		if(Lights[i].LightType == 1 && degrees(acos(dot(-unitLightVector, Lights[i].Direction))) < Lights[i].ConeAngle)
		{
			inCone = true;
		}
		
		if(Lights[i].LightType==2)
		{
			toLightVector=-Lights[i].Direction;
			unitLightVector=normalize(toLightVector);
		}
		
		//attenuation
		float distance=length(toLightVector);
		float attenuationFactor=Lights[i].Attenuation.x+(Lights[i].Attenuation.y*distance)+(Lights[i].Attenuation.z*distance*distance);
		
		//ambient
		totalAmbient+=Lights[i].AmbientIntensity*Lights[i].Color;
		if(Lights[i].LightType!=1 || inCone){
			//diffuse

			float nDot1=dot(unitNormal,unitLightVector);
			float brightness=max(nDot1,0.2);
			totalDiffuse+=(Lights[i].DiffuseIntensity*brightness*Lights[i].Color)/attenuationFactor;
		
			//specular
			if(PhongLightning)
			{
				vec3 lightDirection=-unitLightVector;
				vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
				float specularFactor=dot(reflectedLightDirection,unitCameraVector);
				specularFactor=max(specularFactor,0.2f);
				float dampedFactor=pow(specularFactor,SpecularExponent);
				totalSpecular+=(Lights[i].SpecularIntensity*dampedFactor*Lights[i].Color)/attenuationFactor;
			}
		}
	}

	if(color.a<0.5)
	{
		discard;
	}

	outputColor=color*vec4((totalAmbient*AmbientColor+totalDiffuse*DiffuseColor+totalSpecular*SpecularColor),1.0);
}