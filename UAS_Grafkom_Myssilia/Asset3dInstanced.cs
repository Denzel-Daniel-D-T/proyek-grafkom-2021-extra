using System;
using System.Collections.Generic;
using System.IO;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class Asset3dInstanced
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

        private List<Matrix4> models = new List<Matrix4>();
		private List<Matrix4> modelNormals = new List<Matrix4>();
		private int _instanceBO1, _instanceBO2, _instanceBO3, _instanceBO4, _instanceBO5, _instanceBO6, _instanceBO7, _instanceBO8;

		public Vector3 Ambient { set => ambient = value; }
        public Vector3 Diffuse { set => diffuse = value; }
        public Vector3 Specular { set => specular = value; }
        public List<Matrix4> Models { set => models = value; }
        public List<Matrix4> ModelNormals { set => modelNormals = value; }

        public Asset3dInstanced(int groupId, int rotationId, Vector3 ambient, Vector3 diffuse, Vector3 specular)
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

		public Asset3dInstanced(Asset3dInstanced asset3D, int groupId, int rotationId)
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
			var row0 = new List<Vector4>();
			var row1 = new List<Vector4>();
			var row2 = new List<Vector4>();
			var row3 = new List<Vector4>();

			var row4 = new List<Vector4>();
			var row5 = new List<Vector4>();
			var row6 = new List<Vector4>();
			var row7 = new List<Vector4>();

			foreach (Matrix4 i in models)
			{
				row0.Add(i.Row0);
				row1.Add(i.Row1);
				row2.Add(i.Row2);
				row3.Add(i.Row3);
			}

			foreach (Matrix4 i in modelNormals)
			{
				row4.Add(i.Row0);
				row5.Add(i.Row1);
				row6.Add(i.Row2);
				row7.Add(i.Row3);
			}

			var tempList = new List<Vector3>();
			for (int i = 0; i < vertices.Count; i++)
			{
				tempList.Add(vertices[i]);
				tempList.Add(normals[i]);
			}
			combinedData = new List<Vector3>(tempList);

			_vertexArrayObject = GL.GenVertexArray();
			_vertexBufferObject = GL.GenBuffer();

			_instanceBO1 = GL.GenBuffer();
			_instanceBO2 = GL.GenBuffer();
			_instanceBO3 = GL.GenBuffer();
			_instanceBO4 = GL.GenBuffer();

			_instanceBO5 = GL.GenBuffer();
			_instanceBO6 = GL.GenBuffer();
			_instanceBO7 = GL.GenBuffer();
			_instanceBO8 = GL.GenBuffer();

			GL.BindVertexArray(_vertexArrayObject);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, combinedData.Count * Vector3.SizeInBytes, combinedData.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO1);
			GL.BufferData(BufferTarget.ArrayBuffer, row0.Count * Vector4.SizeInBytes, row0.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(2, 1);
			GL.EnableVertexAttribArray(2);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO2);
			GL.BufferData(BufferTarget.ArrayBuffer, row1.Count * Vector4.SizeInBytes, row1.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(3, 1);
			GL.EnableVertexAttribArray(3);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO3);
			GL.BufferData(BufferTarget.ArrayBuffer, row2.Count * Vector4.SizeInBytes, row2.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(4, 1);
			GL.EnableVertexAttribArray(4);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO4);
			GL.BufferData(BufferTarget.ArrayBuffer, row3.Count * Vector4.SizeInBytes, row3.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(5, 1);
			GL.EnableVertexAttribArray(5);

			//==============

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO5);
			GL.BufferData(BufferTarget.ArrayBuffer, row4.Count * Vector4.SizeInBytes, row0.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(6, 1);
			GL.EnableVertexAttribArray(6);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO6);
			GL.BufferData(BufferTarget.ArrayBuffer, row5.Count * Vector4.SizeInBytes, row1.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(7, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(7, 1);
			GL.EnableVertexAttribArray(7);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO7);
			GL.BufferData(BufferTarget.ArrayBuffer, row6.Count * Vector4.SizeInBytes, row2.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(8, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(8, 1);
			GL.EnableVertexAttribArray(8);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO8);
			GL.BufferData(BufferTarget.ArrayBuffer, row7.Count * Vector4.SizeInBytes, row3.ToArray(), BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(9, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribDivisor(9, 1);
			GL.EnableVertexAttribArray(9);

			if (indices.Count != 0 && objectType != "cuboid")
			{
				_elementBufferObject = GL.GenBuffer();
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
				GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
			}

			_shader = new Shader(path + "Shaders/" + vertName + ".vert", path + "Shaders/" + fragName + ".frag");
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
					GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, vertices.Count, models.Count);
				}
				else if (line == -1)
				{
					GL.DrawArraysInstanced(PrimitiveType.LineStrip, 0, vertices.Count, models.Count);
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

		#endregion

		public void rotateOrigin(Vector3 pivot, Vector3 vector, float angle)
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

		public void rotateInstanced(Vector3 vector, float angle)
		{
			angle = MathHelper.DegreesToRadians(angle);

			var arbRotationMatrix = new Matrix4
				(
				new Vector4((float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle)), 0),
				new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)), (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle)), 0),
				new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)), (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle))), 0),
				Vector4.UnitW
				);

			for (int i = 0; i < models.Count; i++)
			{
				models[i] *= arbRotationMatrix;
				modelNormals[i] = Matrix4.Transpose(Matrix4.Invert(models[i]));
			}

			var row0 = new List<Vector4>();
			var row1 = new List<Vector4>();
			var row2 = new List<Vector4>();
			var row3 = new List<Vector4>();

			var row4 = new List<Vector4>();
			var row5 = new List<Vector4>();
			var row6 = new List<Vector4>();
			var row7 = new List<Vector4>();

			foreach (Matrix4 i in models)
			{
				row0.Add(i.Row0);
				row1.Add(i.Row1);
				row2.Add(i.Row2);
				row3.Add(i.Row3);
			}

			foreach (Matrix4 i in modelNormals)
			{
				row4.Add(i.Row0);
				row5.Add(i.Row1);
				row6.Add(i.Row2);
				row7.Add(i.Row3);
			}

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO1);
			GL.BufferData(BufferTarget.ArrayBuffer, row0.Count * Vector4.SizeInBytes, row0.ToArray(), BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO2);
			GL.BufferData(BufferTarget.ArrayBuffer, row1.Count * Vector4.SizeInBytes, row1.ToArray(), BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO3);
			GL.BufferData(BufferTarget.ArrayBuffer, row2.Count * Vector4.SizeInBytes, row2.ToArray(), BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO4);
			GL.BufferData(BufferTarget.ArrayBuffer, row3.Count * Vector4.SizeInBytes, row3.ToArray(), BufferUsageHint.StaticDraw);

			//==============

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO5);
			GL.BufferData(BufferTarget.ArrayBuffer, row4.Count * Vector4.SizeInBytes, row0.ToArray(), BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO6);
			GL.BufferData(BufferTarget.ArrayBuffer, row5.Count * Vector4.SizeInBytes, row1.ToArray(), BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO7);
			GL.BufferData(BufferTarget.ArrayBuffer, row6.Count * Vector4.SizeInBytes, row2.ToArray(), BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _instanceBO8);
			GL.BufferData(BufferTarget.ArrayBuffer, row7.Count * Vector4.SizeInBytes, row3.ToArray(), BufferUsageHint.StaticDraw);
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
