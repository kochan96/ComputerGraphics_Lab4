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
in vec3 gouroudColor;

uniform sampler2D TextureSamplerUniform;

uniform vec3 AmbientColorUniform;
uniform vec3 DiffuseColorUniform;
uniform vec3 SpecularColorUniform;
uniform float SpecularExponentUniform;

uniform Light LightsUniform[5];

uniform bool PhongLightningUniform;
uniform bool PhongShadingUniform;
uniform bool HasTextureUniform;
uniform bool DiscardUniform;

out vec4 outputColor;

void Phong()
{
	vec4 color; 
	if(HasTextureUniform==true)
	{
		color=texture(TextureSamplerUniform,pass_textcoord);
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

		if(LightsUniform[i].Color==vec3(0.0))
		{
			continue;
		}

		

		vec3 toLightVector=LightsUniform[i].Position - pass_Position;

		//attenuation
		float distance=length(toLightVector);
		float attenuationFactor=LightsUniform[i].Attenuation.x+(LightsUniform[i].Attenuation.y*distance)+(LightsUniform[i].Attenuation.z*distance*distance);
		
		vec3 unitLightVector=normalize(toLightVector);
		bool inCone=false;
		if(LightsUniform[i].LightType == 1 && degrees(acos(dot(-unitLightVector, LightsUniform[i].Direction))) < LightsUniform[i].ConeAngle)
		{
			inCone = true;
		}
		
		if(LightsUniform[i].LightType==2)
		{
			toLightVector=-LightsUniform[i].Direction;
			unitLightVector=normalize(toLightVector);
		}
		
		//ambient
		totalAmbient+=LightsUniform[i].AmbientIntensity*LightsUniform[i].Color;
		if(LightsUniform[i].LightType!=1 || inCone){
			//diffuse

			float nDot1=dot(unitNormal,unitLightVector);
			float brightness=max(nDot1,0.2);
			totalDiffuse+=(LightsUniform[i].DiffuseIntensity*brightness*LightsUniform[i].Color)/attenuationFactor;
		
			//specular
			if(PhongLightningUniform)
			{
				vec3 lightDirection=-unitLightVector;
				vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
				float specularFactor=dot(reflectedLightDirection,unitCameraVector);
				specularFactor=max(specularFactor,0.2f);
				float dampedFactor=pow(specularFactor,SpecularExponentUniform);
				totalSpecular+=(LightsUniform[i].SpecularIntensity*dampedFactor*LightsUniform[i].Color)/attenuationFactor;
			}
		}
	}

	if(color.a<0.5 && DiscardUniform)
	{
		discard;
	}

	outputColor=color*vec4((totalAmbient*AmbientColorUniform+totalDiffuse*DiffuseColorUniform+totalSpecular*SpecularColorUniform),1.0);
};

void Gouraud()
{
	vec4 color; 
	if(HasTextureUniform==true)
	{
		color=texture(TextureSamplerUniform,pass_textcoord);
	}
	else
	{
		color=vec4(1.0,1.0,1.0,1.0);
	}

	if(color.a < 0.5)
	{
		discard;
	}

	outputColor=color*vec4(gouroudColor, 1.0);
};

void main()
{
	if(PhongShadingUniform)
	{
		Phong();
	}
	else
	{
		Gouraud();
	}
};

