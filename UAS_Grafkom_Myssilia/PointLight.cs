using System;
using System.Collections.Generic;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class PointLight : PhysicalLight
	{
		private readonly string path = "F:/ifs4/Grafkom/UAS_Grafkom_Myssilia/UAS_Grafkom_Myssilia/UAS_Grafkom_Myssilia/Shaders/";

		public List<Vector3> vertices = new List<Vector3>();
		private List<uint> indices = new List<uint>();

		private int _vertexBufferObject;
		private int _vertexArrayObject;
		private int _elementBufferObject;

		private Shader _shader;

		private Matrix4 model = Matrix4.Identity;

		public List<Vector3> _euler = new List<Vector3>();
		public Vector3 rotationCenter = Vector3.Zero;
		public Vector3 objectCenter = Vector3.Zero;
		public Vector3 objectDimension = Vector3.Zero;

		public int groupId, rotationId;

		public PointLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic, int groupId, int rotationId) : base(ambient, diffuse, specular, constant, linear, quadratic)
		{
			_euler.Add(Vector3.UnitX);
			_euler.Add(Vector3.UnitY);
			_euler.Add(Vector3.UnitZ);
			this.groupId = groupId;
			this.rotationId = rotationId;
		}

		public PointLight(PointLight pointLight, int groupId, int rotationId) : base()
		{
			_euler = new List<Vector3>(pointLight._euler);
			vertices = new List<Vector3>(pointLight.vertices);
			indices = new List<uint>(pointLight.indices);
			rotationCenter = new Vector3(pointLight.rotationCenter);
			objectCenter = new Vector3(pointLight.objectCenter);
			objectDimension = new Vector3(pointLight.objectDimension);
			ambient = new Vector3(pointLight.ambient);
			diffuse = new Vector3(pointLight.diffuse);
			specular = new Vector3(pointLight.specular);
			constant = pointLight.constant;
			linear = pointLight.linear;
			quadratic = pointLight.quadratic;
			this.groupId = groupId;
			this.rotationId = rotationId;
		}

		public void load()
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

			_shader = new Shader(path + "shader.vert", path + "shader.frag");
			_shader.Use();
		}

		public void render(int line, Matrix4 camera_view, Matrix4 camera_projection)
		{
			GL.BindVertexArray(_vertexArrayObject);

			_shader.Use();

			_shader.SetMatrix4("model", model);
			_shader.SetMatrix4("view", camera_view);
			_shader.SetMatrix4("projection", camera_projection);

			_shader.SetVector3("objectColor", diffuse);

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

		public void createCuboid(float x_, float y_, float z_, float lenX, float lenY, float lenZ)
		{
			rotationCenter.X = objectCenter.X = x_;
			rotationCenter.Y = objectCenter.Y = y_;
			rotationCenter.Z = objectCenter.Z = z_;

			objectDimension.X = lenX;
			objectDimension.Y = lenY;
			objectDimension.Z = lenZ;

			Vector3 temp_vector;

			//Titik 1
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Titik 2
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Titik 3
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Titik 4
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ - lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Titik 5
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Titik 6
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Titik 7
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Titik 8
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_ + lenZ / 2.0f;
			vertices.Add(temp_vector);

			indices = new List<uint>
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

			//Front
			temp_vector.X = x_;
			temp_vector.Y = y_;
			temp_vector.Z = z_ + lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Right
			temp_vector.X = x_ + lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_;
			vertices.Add(temp_vector);

			//Top
			temp_vector.X = x_;
			temp_vector.Y = y_ + lenY / 2.0f;
			temp_vector.Z = z_;
			vertices.Add(temp_vector);

			//Back
			temp_vector.X = x_;
			temp_vector.Y = y_;
			temp_vector.Z = z_ - lenZ / 2.0f;
			vertices.Add(temp_vector);

			//Left
			temp_vector.X = x_ - lenX / 2.0f;
			temp_vector.Y = y_;
			temp_vector.Z = z_;
			vertices.Add(temp_vector);

			//Bottom
			temp_vector.X = x_;
			temp_vector.Y = y_ - lenY / 2.0f;
			temp_vector.Z = z_;
			vertices.Add(temp_vector);

			indices = new List<uint>
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
		}

		public void rotate(Vector3 pivot, Vector3 vector, float angle)
		{
			angle = MathHelper.DegreesToRadians(angle);

			var arbRotationMatrix = new Matrix4
				(
				new Vector4((float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)), 0),
				new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)), (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)), 0),
				new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle)), (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle))), 0),
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
