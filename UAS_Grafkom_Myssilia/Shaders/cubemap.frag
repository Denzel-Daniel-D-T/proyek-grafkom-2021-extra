#version 330

uniform vec3 objectColor;

out vec4 outputColor;

in vec3 TexCoords;

uniform samplerCube cubemap;

void main() {
	outputColor = texture(cubemap, TexCoords);
}
