#version 440

in vec2 pass_textcoord; 
in vec3 pass_Normal;
in vec3 toLightVector;
in float visibility;


uniform sampler2D TextureSampler;
uniform vec3 LightColor;
uniform vec3 SkyColor;

out vec4 outputColor;

void main()
{

	vec3 unitNormal=normalize(pass_Normal);
	vec3 unitLightVector=normalize(toLightVector);

    vec3 color = vec3(texture(TextureSampler, pass_textcoord));

    // ambient
	vec3 ambient=LightColor*color;

    // diffuse
    float nDot1=dot(unitNormal,unitLightVector);
	float brightness=max(nDot1,0.0f);
	vec3 diffuse=brightness*LightColor*color;

   float ambient_strength=0.2f;
   float diffuse_strength=0.8f;
   vec3 final_color = ambient_strength*ambient+diffuse_strength*diffuse;
   outputColor = vec4(final_color, 1.0);
   outputColor=mix(vec4(SkyColor,1.0),outputColor,visibility);
}