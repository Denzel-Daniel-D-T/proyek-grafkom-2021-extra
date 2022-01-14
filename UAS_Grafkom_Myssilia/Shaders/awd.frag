#version 330

uniform vec3 viewPos;
uniform int isBlinn;
uniform int dll_length;
uniform int pll_length;
uniform int fll_length;

in vec3 Normal;
in vec3 FragPos;

out vec4 outputColor;

struct Material {
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct DirectionLight {
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight {
	vec3 position;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};

struct FlashLight {
	vec3 position;
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float cutoff;
	float outerCutoff;

	float constant;
	float linear;
	float quadratic;
};

//vec3 ambient = material.ambient * light.ambient * vec3(texture(material.diffuse, TexCoords));
//vec3 diffuse = /*material.diffuse*/ vec3(texture(material.diffuse, TexCoords)) * light.diffuse * diff;
//vec3 specular = /*material.specular*/ vec3(texture(material.diffuse, TexCoords)) * light.specular * spec;

#define NR_DIRECTION_LIGHTS 3
#define NR_POINT_LIGHTS 24
#define NR_FLASH_LIGHTS 12

uniform Material material;

uniform DirectionLight directionLights[NR_DIRECTION_LIGHTS];
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform FlashLight flashLights[NR_FLASH_LIGHTS];

vec3 CalcDirectionLight(DirectionLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcFlashLight(FlashLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

uniform samplerCube skybox;

void main() {
	//properties
	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 result = vec3(0.0);
	
	//Direction Lights
	for (int i = 0; i < dll_length; i++)
		result += CalcDirectionLight(directionLights[i], norm, FragPos, viewDir);

	//Point Lights
	for (int i = 0; i < pll_length; i++)
		result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);

	//Flash Lights
	for (int i = 0; i < fll_length; i++)
		result += CalcFlashLight(flashLights[i], norm, FragPos, viewDir);

	//Final
	vec3 R = reflect(-viewDir, norm);
	outputColor = vec4(texture(skybox, R).rgb * result, 1.0);
}

vec3 CalcDirectionLight(DirectionLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {
	vec3 lightDir = -light.direction;

	//diffuse
	float diff = max(dot(normal, lightDir), 0.0);

	//specular
	float spec = 0.0;
	if (isBlinn == 1) {
		vec3 halfwayDir = normalize(lightDir + viewDir);
		spec = pow(max(dot(normal, halfwayDir), 0.0), 96);
	}
	else {
		vec3 reflectDir = reflect(-lightDir, normal);
		spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	}

	//declare
	vec3 ambient = material.ambient * light.ambient;
	vec3 diffuse = material.diffuse * light.diffuse * diff;
	vec3 specular = material.specular * light.specular * spec;
	
	//final
	return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {
	vec3 lightDir = normalize(light.position - fragPos);

	//diffuse
	float diff = max(dot(normal, lightDir), 0.0);

	//specular
	float spec = 0.0;
	if (isBlinn == 1) {
		vec3 halfwayDir = normalize(lightDir + viewDir);
		spec = pow(max(dot(normal, halfwayDir), 0.0), 96);
	}
	else {
		vec3 reflectDir = reflect(-lightDir, normal);
		spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	}

	//declare
	vec3 ambient = material.ambient * light.ambient;
	vec3 diffuse = material.diffuse * light.diffuse * diff;
	vec3 specular = material.specular * light.specular * spec;

	//attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
	float attenuation2 = 1.0 / (1.0 + 0.7 * distance + 1.8 * (distance * distance));

	ambient *= attenuation2;
	diffuse *= attenuation;
	specular *= attenuation;
	
	//final
	return (ambient + diffuse + specular);
}

vec3 CalcFlashLight(FlashLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {
	vec3 lightDir = normalize(light.position - fragPos);
	float theta = dot(lightDir, -light.direction);
	float epsilon = light.cutoff - light.outerCutoff;
	float intensity = clamp((theta - light.outerCutoff) / epsilon, 0.0, 1.0);

	float spec = 0.0;

	//diffuse
	float diff = max(dot(normal, lightDir), 0.0);

	//specular
	if (isBlinn == 1) {
		vec3 halfwayDir = normalize(lightDir + viewDir);
		spec = pow(max(dot(normal, halfwayDir), 0.0), 96);
	}
	else {
		vec3 reflectDir = reflect(-lightDir, normal);
		spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	}

	//declare
	vec3 ambient = material.ambient * light.ambient;
	vec3 diffuse = material.diffuse * light.diffuse * diff;
	vec3 specular = material.specular * light.specular * spec;

	diffuse *= intensity;
	specular *= intensity;
	
	//attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
	float attenuation2 = 1.0 / (1.0 + 0.007 * distance + 0.0002 * (distance * distance));

	ambient *= attenuation2;
	diffuse *= attenuation;
	specular *= attenuation;
	
	//final
	return (ambient + diffuse + specular);
}