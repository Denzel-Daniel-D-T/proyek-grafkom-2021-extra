using System;
using System.Collections.Generic;
using System.IO;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class Asset3d
	{
		private readonly string path = "F:/ifs4/Grafkom/UAS_Grafkom_Myssilia/UAS_Grafkom_Myssilia/UAS_Grafkom_Myssilia/";

		private string objectType;

		private List<uint> indices = new List<uint>();
		private List<Vector3> vertices = new List<Vector3>();
		private List<Vector3> normals = new List<Vector3>();
		private List<Vector2> textures = new List<Vector2>();
		private List<Vector3> combinedData = new List<Vector3>();

		private int _vertexBufferObject;
		private int _vertexArrayObject;
		private int _elementBufferObject;

		private Shader _shader;

		private Matrix4 model = Matrix4.Identity;
		private Matrix4 normalMat;

		private Vector3 ambient;
		private Vector3 diffuse;
		private Vector3 specular;

		public List<Vector3> _euler = new List<Vector3>();
		public Vector3 rotationCenter = Vector3.Zero;
		public Vector3 objectCenter = Vector3.Zero;
		public Vector3 objectDimension = Vector3.Zero;

		public int groupId, rotationId;

        public Asset3d(int groupId, int rotationId, Vector3 ambient, Vector3 diffuse, Vector3 specular)
		{
			_euler.Add(Vector3.UnitX);
			_euler.Add(Vector3.UnitY);
			_euler.Add(Vector3.UnitZ);

			this.groupId = groupId;
			this.rotationId = rotationId;
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
        }

		public Asset3d(Asset3d asset3D, int groupId, int rotationId)
		{
			objectType = asset3D.objectType;
			_euler = new List<Vector3>(asset3D._euler);
			indices = new List<uint>(asset3D.indices);
			vertices = new List<Vector3>(asset3D.vertices);
			normals = new List<Vector3>(asset3D.normals);
			model = new Matrix4(asset3D.model.Row0, asset3D.model.Row1, asset3D.model.Row2, asset3D.model.Row3);
			ambient = new Vector3(asset3D.ambient);
			diffuse = new Vector3(asset3D.diffuse);
			specular = new Vector3(asset3D.specular);
			rotationCenter = new Vector3(asset3D.rotationCenter);
			objectCenter = new Vector3(asset3D.objectCenter);
			objectDimension = new Vector3(asset3D.objectDimension);
			this.groupId = groupId;
			this.rotationId = rotationId;
		}

		public void load(string vertName, string fragName)
		{
			if (normals.Count == 0)
            {
				load_Vertex(vertName, fragName);
            }
			else
            {
				load_Vertex_Normals(vertName, fragName);
            }
		}

		public void load_Vertex(string vertName, string fragName)
        {
			_vertexBufferObject = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

			_vertexArrayObject = GL.GenVertexArray();

			GL.BindVertexArray(_vertexArrayObject);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			if (indices.Count != 0)
			{
				_elementBufferObject = GL.GenBuffer();
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
				GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
			}

			_shader = new Shader(path + "Shaders/" + vertName + ".vert", path + "Shaders/" + fragName + ".frag");
			_shader.Use();
		}

		public void load_Vertex_Normals(string vertName, string fragName)
        {
			var tempList = new List<Vector3>();
			for (int i = 0; i < vertices.Count; i++)
			{
				tempList.Add(vertices[i]);
				tempList.Add(normals[i]);
			}
			combinedData = new List<Vector3>(tempList);

			_vertexArrayObject = GL.GenVertexArray();
			_vertexBufferObject = GL.GenBuffer();

			GL.BindVertexArray(_vertexArrayObject);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, combinedData.Count * Vector3.SizeInBytes, combinedData.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

            if (indices.Count != 0 && objectType != "cuboid")
			{
				_elementBufferObject = GL.GenBuffer();
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
				GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
			}

			_shader = new Shader(path + "Shaders/" + vertName + ".vert", path + "Shaders/" + fragName + ".frag");
		}

		public void load_Cubemap()
		{
			_vertexArrayObject = GL.GenVertexArray();
			_vertexBufferObject = GL.GenBuffer();

			GL.BindVertexArray(_vertexArrayObject);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			_shader = new Shader(path + "Shaders/cubemap.vert", path + "Shaders/cubemap.frag");
		}

		public void render(int line, Matrix4 camera_view, Matrix4 camera_projection, List<DirLight> dirLightList, List<PointLight> pointLightList, List<FlashLight> flashLightList, Vector3 cameraPosition, int isBlinn)
		{
			GL.BindVertexArray(_vertexArrayObject);

			normalMat = Matrix4.Transpose(Matrix4.Invert(model));

			_shader.Use();

			_shader.SetMatrix4("model", model);
			_shader.SetMatrix4("normalMat", normalMat);
			_shader.SetMatrix4("view", camera_view);
			_shader.SetMatrix4("projection", camera_projection);

			_shader.SetVector3("viewPos", cameraPosition);

            _shader.SetInt("isBlinn", isBlinn);
			_shader.SetInt("dll_length", dirLightList.Count);
			_shader.SetInt("pll_length", pointLightList.Count);
			_shader.SetInt("fll_length", flashLightList.Count);

			_shader.SetVector3("material.ambient", ambient);
            _shader.SetVector3("material.diffuse", diffuse);
            _shader.SetVector3("material.specular", specular);

            for (int i = 0; i < dirLightList.Count; i++)
			{
				_shader.SetVector3($"directionLights[{i}].direction", dirLightList[i].direction);

				_shader.SetVector3($"directionLights[{i}].ambient", dirLightList[i].ambient);
				_shader.SetVector3($"directionLights[{i}].diffuse", dirLightList[i].diffuse);
				_shader.SetVector3($"directionLights[{i}].specular", dirLightList[i].specular);
			}

			for (int i = 0; i < pointLightList.Count; i++)
            {
				_shader.SetVector3($"pointLights[{i}].position", pointLightList[i].objectCenter);

				_shader.SetVector3($"pointLights[{i}].ambient", pointLightList[i].ambient);
				_shader.SetVector3($"pointLights[{i}].diffuse", pointLightList[i].diffuse);
				_shader.SetVector3($"pointLights[{i}].specular", pointLightList[i].specular);

				_shader.SetFloat($"pointLights[{i}].constant", pointLightList[i].constant);
				_shader.SetFloat($"pointLights[{i}].linear", pointLightList[i].linear);
				_shader.SetFloat($"pointLights[{i}].quadratic", pointLightList[i].quadratic);
			}

			for (int i = 0; i < flashLightList.Count; i++)
            {
				_shader.SetVector3($"flashLights[{i}].position", flashLightList[i].objectCenter);
				_shader.SetVector3($"flashLights[{i}].direction", flashLightList[i].direction);

				_shader.SetVector3($"flashLights[{i}].ambient", flashLightList[i].ambient);
				_shader.SetVector3($"flashLights[{i}].diffuse", flashLightList[i].diffuse);
				_shader.SetVector3($"flashLights[{i}].specular", flashLightList[i].specular);

				_shader.SetFloat($"flashLights[{i}].cutoff", flashLightList[i].cutoff);
				_shader.SetFloat($"flashLights[{i}].outerCutoff", flashLightList[i].outerCutoff);

				_shader.SetFloat($"flashLights[{i}].constant", flashLightList[i].constant);
				_shader.SetFloat($"flashLights[{i}].linear", flashLightList[i].linear);
				_shader.SetFloat($"flashLights[{i}].quadratic", flashLightList[i].quadratic);
			}

			if (indices.Count == 0)
			{
				if (line == 1)
				{
					GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
				}
				else if (line == -1)
				{
					GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);
				}
			}
			else
			{
				if (line == 1)
				{
					GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
				}
				else if (line == -1)
				{
					GL.DrawElements(PrimitiveType.LineStrip, indices.Count, DrawElementsType.UnsignedInt, 0);
				}
			}
		}

		public void renderCubemap(Matrix4 camera_view, Matrix4 camera_projection)
		{
			GL.BindVertexArray(_vertexArrayObject);

			_shader.Use();

			_shader.SetMatrix4("view", new Matrix4(new Matrix3(camera_view)));
			_shader.SetMatrix4("projection", camera_projection);

			GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count);
		}

		#region createShapes
		public void createCuboid(float x_, float y_, float z_, float lenX, float lenY, float lenZ, bool generateTexCoords)
		{
			objectType = "cuboid";

			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = lenX;
			objectDimension.Y = lenY;
			objectDimension.Z = lenZ;

			var tempVertices = new List<Vector3>();
			Vector3 temp_vector;

			//Titik 1
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 2
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 3
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 4
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 5
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 6
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 7
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 8
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			var tempIndices = new List<int>
			{
				//Back
				1, 2, 0,
				2, 1, 3,
				
				//Top
				5, 0, 4,
				0, 5, 1,

				//Right
				5, 3, 1,
				3, 5, 7,

				//Left
				0, 6, 4,
				6, 0, 2,

				//Front
				4, 7, 5,
				7, 4, 6,

				//Bottom
				3, 6, 2,
				6, 3, 7
			};

			for (int i = 0; i < tempIndices.Count; i++)
			{
				vertices.Add(tempVertices[tempIndices[i]]);
			}

			for (int i = 0; i < 6; i++)
			{
				normals.Add(-Vector3.UnitZ);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(Vector3.UnitY);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(Vector3.UnitX);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(-Vector3.UnitX);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(Vector3.UnitZ);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(-Vector3.UnitY);
			}

			if (generateTexCoords)
			{
				for (int i = 0; i < 6; i++)
				{
					textures.Add(new Vector2(1, 1));
					textures.Add(new Vector2(0, 0));
					textures.Add(new Vector2(0, 1));
					textures.Add(new Vector2(0, 0));
					textures.Add(new Vector2(1, 1));
					textures.Add(new Vector2(1, 0));
				}
			}
		}

		public void createRhombicPrism(float x_, float y_, float z_, float lenX, float lenY, float lenZ)
		{
			objectType = "cuboid";

			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = lenX;
			objectDimension.Y = lenY;
			objectDimension.Z = lenZ;

			var tempVertices = new List<Vector3>();
			Vector3 temp_vector;

			//Titik 1
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 2
			temp_vector.X = x_;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 3
			temp_vector.X = x_;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 4
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 5
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 6
			temp_vector.X = x_;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 7
			temp_vector.X = x_;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 8
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			var tempIndices = new List<int>
			{
				//Back
				2, 1, 0,
				1, 2, 3,
				
				//Top
				0, 5, 1,
				5, 0, 4,

				//Right
				5, 3, 1,
				3, 5, 7,

				//Left
				2, 4, 0,
				4, 2, 6,

				//Front
				6, 5, 4,
				5, 6, 7,

				//Bottom
				6, 3, 2,
				3, 6 ,7
			};

			var _tempIndices = tempIndices.ToArray();

			for (int i = 0; i < tempIndices.Count; i++)
			{
				vertices.Add(tempVertices[_tempIndices[i]]);
			}

			var normalUp = getRotationResult(Vector3.Zero, Vector3.UnitZ, -MathF.Atan(lenX / lenY), -Vector3.UnitX);
			var normalRight = getRotationResult(Vector3.Zero, Vector3.UnitZ, MathF.Atan(lenX / lenY), Vector3.UnitX);
			var normalLeft = getRotationResult(Vector3.Zero, Vector3.UnitZ, MathF.Atan(lenX / lenY), -Vector3.UnitX);
			var normalBottom = getRotationResult(Vector3.Zero, Vector3.UnitZ, -MathF.Atan(lenX / lenY), Vector3.UnitX);

			for (int i = 0; i < 6; i++)
			{
				normals.Add(-Vector3.UnitZ);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(normalUp);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(normalRight);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(normalLeft);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(Vector3.UnitZ);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(normalBottom);
			}
		}

		public void createTrapezoidPrism(float x_, float y_, float z_, float lenX, float lenY, float lenZ, float angleMinX, float anglePosX)
		{
			objectType = "cuboid";

			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = lenX;
			objectDimension.Y = lenY;
			objectDimension.Z = lenZ;

			var tempVertices = new List<Vector3>();
			Vector3 temp_vector;

			//Titik 1
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 2
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 3
			temp_vector.X = x_ - (lenX + (float)Math.Tan(angleMinX) * lenY * 2) / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 4
			temp_vector.X = x_ + (lenX + (float)Math.Tan(anglePosX) * lenY * 2) / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 5
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 6
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 7
			temp_vector.X = x_ - (lenX + (float)Math.Tan(angleMinX) * lenY * 2) / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

			//Titik 8
			temp_vector.X = x_ + (lenX + (float)Math.Tan(anglePosX) * lenY * 2) / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			tempVertices.Add(temp_vector);

            var tempIndices = new List<int>
            {
				//Back
				2, 1, 0,
                1, 2, 3,
				
				//Top
				0, 5, 1,
                5, 0, 4,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				2, 4, 0,
                4, 2, 6,

				//Front
				6, 5, 4,
                5, 6, 7,

				//Bottom
				6, 3, 2,
                3, 6, 7
            };

			for (int i = 0; i < tempIndices.Count; i++)
            {
				vertices.Add(tempVertices[tempIndices[i]]);
            }

			var normalRight = getRotationResult(Vector3.Zero, Vector3.UnitZ, anglePosX, Vector3.UnitX);
			var normalLeft = getRotationResult(Vector3.Zero, Vector3.UnitZ, -angleMinX, -Vector3.UnitX);

			for (int i = 0; i < 6; i++)
            {
				normals.Add(-Vector3.UnitZ);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(Vector3.UnitY);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(normalRight);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(normalLeft);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(Vector3.UnitZ);
			}
			for (int i = 0; i < 6; i++)
			{
				normals.Add(-Vector3.UnitY);
			}
		}

		public void createEllipsoid(float x_, float y_, float z_, float radX, float radY, float radZ, int sectorCount, int stackCount)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float stackStep = pi / stackCount;
			float sectorAngle, stackAngle, x, y, z;

			for (int i = 0; i <= stackCount; ++i)
			{
				stackAngle = pi / 2 - i * stackStep;
				x = radX * (float)Math.Cos(stackAngle);
				y = radY * (float)Math.Sin(stackAngle);
				z = radZ * (float)Math.Cos(stackAngle);

				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + x * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + y;
					temp_vector.Z = z_ + z * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);

					var temp_normal = new Vector3(temp_vector - objectCenter);
					normals.Add(temp_normal);
				}
			}

			uint k1, k2;
			for (int i = 0; i < stackCount; ++i)
			{
				k1 = (uint)(i * (sectorCount + 1));
				k2 = (uint)(k1 + sectorCount + 1);

				for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					if (i != 0)
					{
						indices.Add(k1);
						indices.Add(k1 + 1);
						indices.Add(k2);
					}

					if (i != stackCount - 1)
					{
						indices.Add(k2);
						indices.Add(k1 + 1);
						indices.Add(k2 + 1);
					}
				}
			}

			for (int i = 0; i < normals.Count; i++)
			{
				normals[i] = Vector3.Normalize(normals[i]);
			}
		}

		public void createFractionedEllipsoid(float x_, float y_, float z_, float radX, float radY, float radZ, int sectorCount, int stackCount, float fraction, float startPoint = 0)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = radX * 2;
			objectDimension.Y = radY * 2;
			objectDimension.Z = radZ * 2;

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float stackStep = pi / stackCount;
			float sectorAngle, stackAngle, x, y, z;
			int fractionedStack = (int)(stackCount * fraction);
			int fractionedStart = (int)(stackCount * startPoint);

			for (int i = fractionedStart; i <= fractionedStack; ++i)
			{
				stackAngle = pi / 2 - i * stackStep;
				x = radX * (float)Math.Cos(stackAngle);
				y = radY * (float)Math.Sin(stackAngle);
				z = radZ * (float)Math.Cos(stackAngle);

				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + x * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + y;
					temp_vector.Z = z_ + z * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);

					var temp_normal = new Vector3(temp_vector - objectCenter);
					normals.Add(temp_normal);
				}
			}

			uint k1, k2;
			for (int i = 0; i < fractionedStack - fractionedStart; ++i)
			{
				k1 = (uint)(i * (sectorCount + 1));
				k2 = (uint)(k1 + sectorCount + 1);

				for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					if (fractionedStart == 0)
					{
						if (i != 0)
						{
							indices.Add(k1);
							indices.Add(k1 + 1);
							indices.Add(k2);
						}
					}
					else
					{
						indices.Add(k1);
						indices.Add(k1 + 1);
						indices.Add(k2);
					}

					if (i != stackCount - 1)
					{
						indices.Add(k1 + 1);
						indices.Add(k2 + 1);
						indices.Add(k2);
					}
				}
			}

			for (int i = 0; i < normals.Count; i++)
			{
				normals[i] = Vector3.Normalize(normals[i]);
			}
		}

		public void createEllipticCone(float x_, float y_, float z_, float radX, float height, float radZ, int sectorCount, int stackCount)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = radX;
			objectDimension.Y = height;
			objectDimension.Z = radZ;

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float sectorAngle, x, y, z;
			float normalAngle = MathF.Atan(radX / height);
			float radius = 0;
			if (radX == radZ)
			{
				radius = radX;
			}

			for (int i = 0; i <= sectorCount; ++i)
			{
				vertices.Add(new Vector3(x_, y_ - height / 2, z_));
				normals.Add(-Vector3.UnitY);
			}

			for (int j = 0; j <= sectorCount; ++j)
			{
				sectorAngle = j * sectorStep;

				temp_vector.X = x_ + radX * (float)Math.Cos(sectorAngle);
				temp_vector.Y = y_ - height / 2;
				temp_vector.Z = z_ + radZ * (float)Math.Sin(sectorAngle);

				vertices.Add(temp_vector);
				normals.Add(-Vector3.UnitY);
			}

			for (int i = 0; i <= stackCount; ++i)
			{
				x = radX * (stackCount - i) / stackCount;
				y = height * i / stackCount - height / 2;
				z = radZ * (stackCount - i) / stackCount;

				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + x * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + y;
					temp_vector.Z = z_ + z * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);

					if (radX != radZ)
					{
						radius = radX * radZ / MathF.Sqrt(MathF.Pow(radX, 2) * MathF.Pow(MathF.Sin(sectorAngle), 2) + MathF.Pow(radZ, 2) * MathF.Pow(MathF.Cos(sectorAngle), 2));
					}
					normals.Add(new Vector3(MathF.Cos(sectorAngle) * height / radius, 1, MathF.Sin(sectorAngle) * height / radius));
				}
			}

			uint k1, k2;
			int newStackCount = stackCount + 2;
			for (int i = 0; i < newStackCount; ++i)
			{
				k1 = (uint)(i * (sectorCount + 1));
				k2 = (uint)(k1 + sectorCount + 1);

				for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					if (i != 1)
					{
						if (i != 0)
						{
							indices.Add(k1);
							indices.Add(k2);
							indices.Add(k1 + 1);
						}
						if (i != newStackCount - 1)
						{
							indices.Add(k1 + 1);
							indices.Add(k2);
							indices.Add(k2 + 1);
						}
					}
				}
			}

			for (int i = 0; i < normals.Count; i++)
            {
				normals[i] = Vector3.Normalize(normals[i]);
            }
		}

		public void createSinusoidCapsule(float x_, float y_, float z_, float radX, float radY, float radZ, int sectorCount, int stackCount)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float stackStep = pi / stackCount;
			float sectorAngle, stackAngle, x, y, z;
			float radius = 0;
			if (radX == radZ)
            {
				radius = radX;
            }

			for (int i = 0; i <= stackCount; ++i)
			{
				stackAngle = pi / 2 - i * stackStep;
				x = radX * (float)Math.Cos(stackAngle);
				y = 2 * radY * i / stackCount - radY;
				z = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + x * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + y;
					temp_vector.Z = z_ + z * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
					if (radX != radZ)
					{
						radius = radX * radZ / MathF.Sqrt(MathF.Pow(radX, 2) * MathF.Pow(MathF.Sin(sectorAngle), 2) + MathF.Pow(radZ, 2) * MathF.Pow(MathF.Cos(sectorAngle), 2));
					}
                    var temp_normal = new Vector3(MathF.Cos(sectorAngle) * radY * 2 / pi, radius * MathF.Sin(y / (radY * 2) * pi), MathF.Sin(sectorAngle) * radY * 2 / pi);
                    normals.Add(temp_normal);
                }
			}

			uint k1, k2;
			for (int i = 0; i < stackCount; ++i)
			{
				k1 = (uint)(i * (sectorCount + 1));
				k2 = (uint)(k1 + sectorCount + 1);

				for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					if (i != 0)
					{
						indices.Add(k1);
						indices.Add(k2);
						indices.Add(k1 + 1);
					}

					if (i != stackCount - 1)
					{
						indices.Add(k1 + 1);
						indices.Add(k2);
						indices.Add(k2 + 1);
					}
				}
			}

			for (int i = 0; i < normals.Count; i++)
			{
				normals[i] = Vector3.Normalize(normals[i]);
			}
		}

		public void createCylinder(float x_, float y_, float z_, float radX, float height, float radZ, int sectorCount, int stackCount)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = radX * 2;
			objectDimension.Y = height;
			objectDimension.Z = radZ * 2;

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float sectorAngle, x, y, z;

			for (int i = 0; i <= sectorCount; ++i)
			{
				vertices.Add(new Vector3(x_, y_ - height / 2, z_));
				normals.Add(-Vector3.UnitY);
			}

			for (int i = 0; i <= sectorCount; ++i)
			{
				sectorAngle = i * sectorStep;

				temp_vector.X = x_ + radX * (float)Math.Cos(sectorAngle);
				temp_vector.Y = y_ - height / 2;
				temp_vector.Z = z_ + radZ * (float)Math.Sin(sectorAngle);

				vertices.Add(temp_vector);
				normals.Add(-Vector3.UnitY);
			}

			for (int i = 0; i <= stackCount; ++i)
			{
				x = radX;
				y = height * i / stackCount - height / 2;
				z = radZ;

				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + x * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + y;
					temp_vector.Z = z_ + z * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);
					var temp_normal = new Vector3(temp_vector.X - x_, 0, temp_vector.Z - z_);
					normals.Add(temp_normal);
				}
			}

			for (int i = 0; i <= sectorCount; ++i)
			{
				sectorAngle = i * sectorStep;

				temp_vector.X = x_ + radX * (float)Math.Cos(sectorAngle);
				temp_vector.Y = y_ + height / 2;
				temp_vector.Z = z_ + radZ * (float)Math.Sin(sectorAngle);

				vertices.Add(temp_vector);
				normals.Add(Vector3.UnitY);
			}

			for (int j = 0; j <= sectorCount; ++j)
			{
				vertices.Add(new Vector3(x_, y_ + height / 2, z_));
				normals.Add(Vector3.UnitY);
			}

			uint k1, k2;
			int newStackCount = stackCount + 2 + 2;
			for (int i = 0; i < newStackCount; ++i)
			{
				k1 = (uint)(i * (sectorCount + 1));
				k2 = (uint)(k1 + sectorCount + 1);

				for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					if (i != 1 && i != newStackCount - 2)
					{
						if (i != 0)
						{
							indices.Add(k1);
							indices.Add(k2);
							indices.Add(k1 + 1);
						}

						if (i != newStackCount - 1)
						{
							indices.Add(k1 + 1);
							indices.Add(k2);
							indices.Add(k2 + 1);
						}
					}
				}
			}

			for (int i = 0; i < normals.Count; i++)
			{
				normals[i] = Vector3.Normalize(normals[i]);
			}
		}

		public void createVariedCylinder(float x_, float y_, float z_, float height, float topRad, float botRad, int sectorCount, int stackCount, bool isTopFilled, bool isBottomFilled)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float sectorAngle, x, y, z;
			float slope = height / (botRad - topRad);
			int newStackCount = stackCount;

			if (isBottomFilled)
			{
				for (int j = 0; j <= sectorCount; ++j)
				{
					vertices.Add(new Vector3(x_, y_ - height / 2, z_));
					normals.Add(-Vector3.UnitY);
				}

				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + botRad * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ - height / 2;
					temp_vector.Z = z_ + botRad * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);
					normals.Add(-Vector3.UnitY);
				}

				newStackCount += 2;
			}

			for (int i = 0; i <= stackCount; ++i)
			{
				x = z = botRad + i * (topRad - botRad) / stackCount;
				y = height * i / stackCount - height / 2;

				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + x * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + y;
					temp_vector.Z = z_ + z * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);
					var temp_normal = new Vector3(MathF.Cos(sectorAngle) * MathF.Abs(slope), slope / MathF.Abs(slope), MathF.Sin(sectorAngle) * MathF.Abs(slope));
					normals.Add(temp_normal);
				}
			}

			if (isTopFilled)
			{
				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + topRad * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + height / 2;
					temp_vector.Z = z_ + topRad * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);
					normals.Add(Vector3.UnitY);
				}

				for (int j = 0; j <= sectorCount; ++j)
				{
					vertices.Add(new Vector3(x_, y_ + height / 2, z_));
					normals.Add(Vector3.UnitY);
				}

				newStackCount += 2;
			}

			uint k1, k2;
			for (int i = 0; i < newStackCount; ++i)
			{
				k1 = (uint)(i * (sectorCount + 1));
				k2 = (uint)(k1 + sectorCount + 1);

				for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					if (isBottomFilled)
					{
						if (i != 0 && i != 1)
						{
							indices.Add(k1);
							indices.Add(k2);
							indices.Add(k1 + 1);
						}
					}
					else
					{
						indices.Add(k1);
						indices.Add(k2);
						indices.Add(k1 + 1);
					}

					if (isTopFilled)
					{
						if (i != newStackCount - 1 && i != newStackCount - 2)
						{
							indices.Add(k1 + 1);
							indices.Add(k2);
							indices.Add(k2 + 1);
						}
					}
					else
					{
						indices.Add(k1 + 1);
						indices.Add(k2);
						indices.Add(k2 + 1);
					}
				}
			}

			for (int i = 0; i < normals.Count; i++)
			{
				normals[i] = Vector3.Normalize(normals[i]);
			}
		}

		public void createEllipticParaboloid(float x_, float y_, float z_, float radX, float height, float radZ, int sectorCount, int stackCount, bool invertNormals)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = radX * 2;
			objectDimension.Y = height;
			objectDimension.Z = radZ * 2;

			height = MathF.Sqrt(height * 2);

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float stackStep = height * 2 / (2 * stackCount);
			float sectorAngle, stackAngle, x, y, z;

			for (int i = 0; i <= stackCount; ++i)
			{
				stackAngle = height - i * stackStep;
				x = radX * stackAngle;
				y = stackAngle * stackAngle;
				z = radZ * stackAngle;

				for (int j = 0; j <= sectorCount; ++j)
				{
					sectorAngle = j * sectorStep;

					temp_vector.X = x_ + x * (float)Math.Cos(sectorAngle);
					temp_vector.Y = y_ + y;
					temp_vector.Z = z_ + z * (float)Math.Sin(sectorAngle);

					vertices.Add(temp_vector);

                    var temp_normal = new Vector3(MathF.Cos(sectorAngle) * x * 2, -1, MathF.Sin(sectorAngle) * z * 2);
                    if (invertNormals)
                    {
                        temp_normal *= -1;
                    }
                    normals.Add(temp_normal);
                }
			}

			uint k1, k2;
			for (int i = 0; i < stackCount; ++i)
			{
				k1 = (uint)(i * (sectorCount + 1));
				k2 = (uint)(k1 + sectorCount + 1);

				for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
				{
					indices.Add(k1);
					indices.Add(k2);
					indices.Add(k1 + 1);

					if (i != stackCount - 1)
					{
						indices.Add(k1 + 1);
						indices.Add(k2);
						indices.Add(k2 + 1);
					}
				}
			}

			for (int i = 0; i < normals.Count; i++)
			{
				normals[i] = Vector3.Normalize(normals[i]);
			}
		}

		public void createCircle(float x_, float y_, float z_, float radX, float radZ, float sectorCount)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = radX * 2;
			objectDimension.Y = 0;
			objectDimension.Z = radZ * 2;

			float pi = (float)Math.PI;
			Vector3 temp_vector;
			float sectorStep = 2 * pi / sectorCount;
			float sectorAngle;

			for (int j = 0; j <= sectorCount; ++j)
			{
				vertices.Add(new Vector3(x_, y_, z_));
				normals.Add(Vector3.UnitY);
			}

			for (int j = 0; j <= sectorCount; ++j)
			{
				sectorAngle = j * sectorStep;

				temp_vector.X = x_ + radX * (float)Math.Cos(sectorAngle);
				temp_vector.Y = y_;
				temp_vector.Z = z_ + radZ * (float)Math.Sin(sectorAngle);

				vertices.Add(temp_vector);
				normals.Add(Vector3.UnitY);
			}

			uint k1, k2;
			k1 = 0;
			k2 = (uint)(sectorCount + 1);

			for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
			{
				indices.Add(k1);
				indices.Add(k2 + 1);
				indices.Add(k2);
			}
		}

		public void createOctahedron(float x_, float y_, float z_, float lenX, float lenY, float lenZ)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = lenX;
			objectDimension.Y = lenY;
			objectDimension.Z = lenZ;

			Vector3 temp_vector;
			List<Vector3> temp_vertices = new List<Vector3>();

			//Front
			temp_vector.X = x_;
			temp_vector.Y = y_;
			temp_vector.Z = z_ + lenZ / 2.0f;
			temp_vertices.Add(temp_vector);

			//Right
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_;
			temp_vertices.Add(temp_vector);

			//Top
			temp_vector.X = x_;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_;
			temp_vertices.Add(temp_vector);

			//Back
			temp_vector.X = x_;
			temp_vector.Y = y_;
			temp_vector.Z = z_ - lenZ / 2.0f;
			temp_vertices.Add(temp_vector);

			//Left
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_;
			temp_vertices.Add(temp_vector);

			//Bottom
			temp_vector.X = x_;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_;
			temp_vertices.Add(temp_vector);

			var temp_indices = new List<int>
			{
				//top
				0, 1, 2,

				1, 3, 2,

				3, 4, 2,

				4, 0, 2,

				//bottom
				0, 4, 5,

				4, 3, 5,

				3, 1, 5,

				1, 0, 5
			};

			foreach (int i in temp_indices)
            {
				vertices.Add(temp_vertices[i]);
            }

			for (int i = 0; i < 3; i++)
            {
				normals.Add(Vector3.Normalize(new Vector3(1, 1, 1)));
			}

			for (int i = 0; i < 3; i++)
			{
				normals.Add(Vector3.Normalize(new Vector3(1, 1, -1)));
			}

			for (int i = 0; i < 3; i++)
			{
				normals.Add(Vector3.Normalize(new Vector3(-1, 1, -1)));
			}

			for (int i = 0; i < 3; i++)
			{
				normals.Add(Vector3.Normalize(new Vector3(-1, 1, 1)));
			}

			for (int i = 0; i < 3; i++)
			{
				normals.Add(Vector3.Normalize(new Vector3(-1, -1, 1)));
			}

			for (int i = 0; i < 3; i++)
			{
				normals.Add(Vector3.Normalize(new Vector3(-1, -1, -1)));
			}

			for (int i = 0; i < 3; i++)
			{
				normals.Add(Vector3.Normalize(new Vector3(1, -1, -1)));
			}

			for (int i = 0; i < 3; i++)
			{
				normals.Add(Vector3.Normalize(new Vector3(1, -1, 1)));
			}
		}
		#endregion

		public void rotate(Vector3 pivot, Vector3 vector, float angle)
		{
            angle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle)))		, (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle))	, (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle))	, 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)), (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle)))		, (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle))	, 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle))	, (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)))		, 0),
                Vector4.UnitW
                );

            model *= Matrix4.CreateTranslation(-pivot);
            model *= arbRotationMatrix;
            model *= Matrix4.CreateTranslation(pivot);

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, angle, _euler[i], true));
            }

            rotationCenter = getRotationResult(pivot, vector, angle, rotationCenter);
            objectCenter = getRotationResult(pivot, vector, angle, objectCenter);
        }

		public Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
		{
			Vector3 temp, newPosition;

			if (isEuler)
			{
				temp = point;
			}
			else
			{
				temp = point - pivot;
			}

			newPosition.X =
				temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
				temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
				temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));

			newPosition.Y =
				temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
				temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
				temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));

			newPosition.Z =
				temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
				temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
				temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

			if (isEuler)
			{
				temp = newPosition;
			}
			else
			{
				temp = newPosition + pivot;
			}
			return temp;
		}

		public void resetEuler()
		{
			_euler[0] = Vector3.UnitX;
			_euler[1] = Vector3.UnitY;
			_euler[2] = Vector3.UnitZ;
		}

		public void translate(float x, float y, float z)
		{
			model *= Matrix4.CreateTranslation(x, y, z);
			objectCenter.X += x;
			objectCenter.Y += y;
			objectCenter.Z += z;

			rotationCenter.X += x;
			rotationCenter.Y += y;
			rotationCenter.Z += z;
		}

		public void mirrorX(float x)
		{
			objectCenter.X = 2 * x - objectCenter.X;

			rotationCenter.X = 2 * x - rotationCenter.X;

			model *= Matrix4.CreateTranslation(-x, 0, 0);

			model *= new Matrix4(
				-1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1
				);

			model *= Matrix4.CreateTranslation(x, 0, 0);
		}

		public void mirrorY(float y)
		{
			objectCenter.Y = 2 * y - objectCenter.Y;

			rotationCenter.Y = 2 * y - rotationCenter.Y;

			model *= Matrix4.CreateTranslation(0, -y, 0);

			model *= new Matrix4(
				1, 0, 0, 0,
				0, -1, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1
				);

			model *= Matrix4.CreateTranslation(0, y, 0);
		}

		public void scaleLocal(float scaleX, float scaleY, float scaleZ)
		{
            model *= Matrix4.CreateTranslation(-objectCenter);

            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);

            model *= Matrix4.CreateTranslation(objectCenter);

            objectDimension.X *= scaleX;
            objectDimension.Y *= scaleY;
            objectDimension.Z *= scaleZ;
		}

		public void scale(Vector3 centerPoint, float scaleX, float scaleY, float scaleZ)
		{
			model *= Matrix4.CreateTranslation(-centerPoint);

			model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);

			model *= Matrix4.CreateTranslation(centerPoint);

			var tempX = centerPoint.X - objectCenter.X;
            var tempY = centerPoint.Y - objectCenter.Y;
            var tempZ = centerPoint.Z - objectCenter.Z;

            objectCenter.X = objectCenter.X + tempX * (1 - scaleX);
            objectCenter.Y = objectCenter.Y + tempY * (1 - scaleY);
            objectCenter.Z = objectCenter.Z + tempZ * (1 - scaleZ);

            objectDimension.X *= scaleX;
            objectDimension.Y *= scaleY;
            objectDimension.Z *= scaleZ;
        }
	}
}
