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

in vec3 PositionAttribute;
in vec3 NormalAttribute;
in vec2 TextureCoordAttribute;

uniform mat4 ModelMatrixUniform;
uniform mat4 ViewMatrixUniform;
uniform mat4 ProjectionMatrixUniform;

uniform vec3 AmbientColorUniform;
uniform vec3 DiffuseColorUniform;
uniform vec3 SpecularColorUniform;
uniform float SpecularExponentUniform;

uniform Light LightsUniform[5];

uniform bool PhongLightningUniform;
uniform bool PhongShadingUniform;

out vec3 pass_Normal;
out vec3 toCameraVector;
out vec3 pass_Position;
out vec2 pass_textcoord;
out vec3 gouroudColor;

void Phong()
{
	vec4 worldPosition=ModelMatrixUniform*vec4(PositionAttribute,1.0f);
	vec4 positionRelativeToCam=ViewMatrixUniform*worldPosition;
	gl_Position=ProjectionMatrixUniform*positionRelativeToCam;

	pass_Normal = mat3(transpose(inverse(ModelMatrixUniform))) * NormalAttribute;
	pass_Position=worldPosition.xyz;
	pass_textcoord=TextureCoordAttribute;

	toCameraVector=(inverse(ViewMatrixUniform)*vec4(0.0,0.0,0.0,1.0)).xyz-worldPosition.xyz;
};

void Gouraud()
{
	Phong();

	vec3 unitNormal=normalize(pass_Normal);
	
	vec3 unitCameraVector=normalize(toCameraVector);


	vec3 totalDiffuse=vec3(0.0);
	vec3 totalSpecular=vec3(0.0);
	vec3 totalAmbient=vec3(0.0);
	for(int i=0;i<5;i++)
	{

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
		if(LightsUniform[i].LightType!=1 || inCone)
		{
			//diffuse

			float nDot1=dot(unitNormal,unitLightVector);
			float brightness=max(nDot1,0.2);
			totalDiffuse+=(LightsUniform[i].DiffuseIntensity*brightness*LightsUniform[i].Color)/attenuationFactor;
		
			//specular
			vec3 lightDirection = -unitLightVector;
			float dampedFactor = 1;

			if(PhongLightningUniform)
			{
				vec3 reflectedLightDirection=reflect(lightDirection,unitNormal);
				float specularFactor=dot(reflectedLightDirection,unitCameraVector);
				dampedFactor=pow(specularFactor,SpecularExponentUniform);
			}
			else
			{
				// this is blinn phong
				vec3 halfDir = normalize(lightDirection + unitCameraVector);
				float specularFactor=dot(unitNormal,halfDir);
				dampedFactor=pow(specularFactor,SpecularExponentUniform);
			}

			totalSpecular+=(LightsUniform[i].SpecularIntensity*dampedFactor*LightsUniform[i].Color)/attenuationFactor;


			gouroudColor = totalAmbient * AmbientColorUniform+totalDiffuse * DiffuseColorUniform + totalSpecular * SpecularColorUniform;
		}
	}
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
