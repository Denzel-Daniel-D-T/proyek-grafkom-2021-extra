#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in mat4 instancedModel;
layout (location = 6) in mat4 instancedNormalMat;

uniform mat4 model;
uniform mat4 normalMat;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos;

void main(void) {
	gl_Position = vec4(aPosition, 1.0) * instancedModel * model * view * projection;

	FragPos = vec3(vec4(aPosition, 1.0) * instancedModel * model);
	Normal = aNormal * mat3(instancedNormalMat) * mat3(normalMat);
}