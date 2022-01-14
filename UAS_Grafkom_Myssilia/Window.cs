using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using LearnOpenTK.Common;
using System.Collections.Generic;

namespace UAS_Grafkom_Myssilia
{
	class Window : GameWindow
	{
		private readonly string path = "F:/ifs4/Grafkom/UAS_Grafkom_Myssilia/UAS_Grafkom_Myssilia/UAS_Grafkom_Myssilia/";
		private readonly string imageFormat = "png";

		private bool firstMove = true;

		private int renderSetting = 1;

		private float cameraStage = 0;
		private float cameraSpeed = 12.0f;
		private float sensitivity = 0.2f;
		private List<DirLight> dirLightList;
		private List<PointLight> pointLightList;
		private List<FlashLight> flashLightList;

		private bool started = false;
		private bool mouseEnabled = true;
		private int isBlinn = 1;

		private const float BPM = 145.5f;

		private Camera camera;

		private Vector2 lastPos;

		private MyssiliaCrest myssiliaCrest;
		private UFO ufo;
		private Spaceship spaceship;
		private FloorObjects floorObjects;
		private LightCollection lights;

		private Asset3d Cubemap;

		private Texture cubemap;

		private Asset3dInstanced instancedCubes, instancedPillars;

		#region Framebuffer
		private int _frameBufferObject;
		private int _fboTexture;
		#endregion

		#region Renderbuffer
		private int _renderBufferObject;
		#endregion

		#region ScreenTexture
		private int screenVAO, screenVBO;
		private float[] screenCoords = new float[]
		{
			-1.0f, 1.0f, 0.0f, 1.0f,
			-1.0f, -1.0f, 0.0f, 0.0f,
			1.0f, -1.0f, 1.0f, 0.0f,

			-1.0f, 1.0f, 0.0f, 1.0f,
			1.0f, -1.0f, 1.0f, 0.0f,
			1.0f, 1.0f, 1.0f, 1.0f,
		};
		private Shader _screenShader;
        #endregion

        #region Camera Vars
        private bool expired = true;
		private float timePassed = 0;
		private List<Vector3> tempVectors = new List<Vector3>();
		#endregion

		public Window(GameWindowSettings gWS, NativeWindowSettings nWS) : base(gWS, nWS)
		{

		}

		protected override void OnLoad()
		{
			base.OnLoad();

			//User Manual
			Console.WriteLine("Controls:\nDefault WASD Controls\nSpace - Move Upwards\nShift - Move Downwards\nCtrl (Hold) - Increase Speed\n` - Switch Render Mode (Solid or Wireframe)\nB - Switch Between Phong and Blinn-Phong Shading\nEsc - Exit Program\n\nF - Start Animation (Locks camera into animation, see below)\nG - Turn off preset camera movement");

			camera = new Camera(new Vector3(0, -30, 32), Size.X / (float)Size.Y);

			Cubemap = new Asset3d(0, 0, Vector3.One, Vector3.One, Vector3.One);
			Cubemap.createCuboid(0, 0, 0, 1, 1, 1, false);
			Cubemap.load_Cubemap();

			myssiliaCrest = new MyssiliaCrest(camera);
			ufo = new UFO(camera);
			spaceship = new Spaceship(camera);
			floorObjects = new FloorObjects(camera);
			lights = new LightCollection(camera);

			GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

			var cubemapPaths = new List<string>();
			cubemapPaths.Add(path + "cubemap/right." + imageFormat);
			cubemapPaths.Add(path + "cubemap/left." + imageFormat);
			cubemapPaths.Add(path + "cubemap/top." + imageFormat);
			cubemapPaths.Add(path + "cubemap/bottom." + imageFormat);
			cubemapPaths.Add(path + "cubemap/front." + imageFormat);
			cubemapPaths.Add(path + "cubemap/back." + imageFormat);
			cubemap = Texture.LoadFromFile(cubemapPaths);

			myssiliaCrest.initObjects();
			myssiliaCrest.load();

			ufo.initObjects();
			ufo.load();

			spaceship.initObjects();
			spaceship.load();

			floorObjects.initObjects();
			floorObjects.load();

			lights.initObjects();
			lights.load();

			instancedCubes = new Asset3dInstanced(0, 0, Vector3.One, Vector3.One, Vector3.One);
			instancedCubes.createCuboid(0, 0, 0, 1, 1, 1, false);

			var Models = new List<Matrix4>();
			var ModelNormals = new List<Matrix4>();
			var random = new Random();

			for (int i = 0; i < 100; i++)
			{
				Matrix4 modl = Matrix4.Identity;
				var angle = (float)random.NextDouble() * 360;
				var moved = (float)random.NextDouble() * 50 + 100;
				modl *= Matrix4.CreateScale(random.Next(30, 100) / 10.0f);
				modl *= Matrix4.CreateTranslation(new Vector3(MathF.Cos(MathHelper.DegreesToRadians(angle)) * moved, random.Next(30) - 15, MathF.Sin(MathHelper.DegreesToRadians(angle)) * moved));
				modl = Matrix4.Transpose(modl);
				Models.Add(modl);
				ModelNormals.Add(Matrix4.Transpose(Matrix4.Invert(modl)));
			}

			instancedCubes.Models = new List<Matrix4>(Models);
			instancedCubes.ModelNormals = new List<Matrix4>(ModelNormals);

			instancedCubes.load("instancedShader", "cobe");

			instancedPillars = new Asset3dInstanced(0, 0, Vector3.One, Vector3.One, Vector3.One);
			instancedPillars.createCuboid(0, 0, 0, 1, 1, 1, false);

			Models.Clear();
			ModelNormals.Clear();

			for (int i = 0; i < 500; i++)
            {
                Matrix4 modl = Matrix4.Identity;
                var angle = (float)random.NextDouble() * 360;
                var moved = (float)random.NextDouble() * 100 + 150;
                modl *= Matrix4.CreateScale(random.Next(30, 100) / 5.0f, (moved - 145) * 1.5f + ((float)random.NextDouble() * 10) - 5, random.Next(30, 100) / 5.0f);
                modl *= Matrix4.CreateTranslation(new Vector3(MathF.Cos(MathHelper.DegreesToRadians(angle)) * moved, -40, MathF.Sin(MathHelper.DegreesToRadians(angle)) * moved));
                modl = Matrix4.Transpose(modl);
                Models.Add(modl);
                ModelNormals.Add(Matrix4.Transpose(Matrix4.Invert(modl)));
            }

			instancedPillars.Models = new List<Matrix4>(Models);
			instancedPillars.ModelNormals = new List<Matrix4>(ModelNormals);

			instancedPillars.load("instancedShader", "awd");

			GL.DepthFunc(DepthFunction.Lequal);

			CursorGrabbed = true;

			//Framebuffer
			_frameBufferObject = GL.GenFramebuffer();

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBufferObject);

			//FBOTexture
			_fboTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, _fboTexture);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Size.X, Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, (IntPtr)null);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			//==================

			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _fboTexture, 0);

			_renderBufferObject = GL.GenRenderbuffer();

			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _renderBufferObject);

			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, Size.X, Size.Y);

			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, _renderBufferObject);

			if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == FramebufferErrorCode.FramebufferComplete)
			{
				Console.WriteLine("Framebuffer Complete");
			}

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			//==============

			screenVAO = GL.GenVertexArray();
			screenVBO = GL.GenBuffer();

			GL.BindVertexArray(screenVAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, screenVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, screenCoords.Length * sizeof(float), screenCoords, BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
			GL.EnableVertexAttribArray(1);

			_screenShader = new Shader(path + "Shaders/fboTexture.vert", path + "Shaders/fboTexture.frag");
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			float time = (float)args.Time;
			time *= BPM / 60.0f;

			dirLightList = lights.DirLightList;
            pointLightList = lights.PointLightList;
			flashLightList = lights.FlashLightList;

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, _frameBufferObject);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.Enable(EnableCap.DepthTest);

			myssiliaCrest.render(dirLightList, pointLightList, flashLightList);
			ufo.render(dirLightList, pointLightList, flashLightList);
			spaceship.render(dirLightList, pointLightList, flashLightList);
			floorObjects.render(dirLightList, pointLightList, flashLightList);
			lights.render();

			cubemap.Use(TextureUnit.Texture0);
			Cubemap.renderCubemap(camera.GetViewMatrix(), camera.GetProjectionMatrix());

			instancedCubes.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix(), dirLightList, pointLightList, flashLightList, camera.Position, isBlinn);

			instancedPillars.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix(), dirLightList, pointLightList, flashLightList, camera.Position, isBlinn);

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.Disable(EnableCap.DepthTest);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.BindVertexArray(screenVAO);
			_screenShader.Use();
			GL.BindTexture(TextureTarget.Texture2D, _fboTexture);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

			instancedCubes.rotateOrigin(instancedCubes.objectCenter, instancedCubes._euler[1], time * 90 / 32.0f);
			instancedCubes.rotateInstanced(instancedCubes._euler[1], time * -90 / 32.0f);

			switch (myssiliaCrest.animationStage)
			{
				case 1:
					myssiliaCrest.pause(time, 128.0f);
					break;
				case 2:
					myssiliaCrest.seq1(time, 16.0f, 0);
					break;
				case 3:
					myssiliaCrest.seq1(time, 2.0f / 3.0f, 1);
					break;
				case 4:
					myssiliaCrest.seq1(time, 46.0f / 3.0f, 0);
					break;
				case 5:
					myssiliaCrest.seq2(time, 24.0f);
					break;
				case 6:
					myssiliaCrest.seq3(time, 8.0f);
					break;
				//8 beats
				case 7:
					myssiliaCrest.seq4(time, 2.0f);
					break;
				case 8:
					myssiliaCrest.seq5(time, 5.0f / 3.0f);
					break;
				case 9:
					myssiliaCrest.seq6(time, 4.0f / 3.0f);
					break;
				case 10:
					myssiliaCrest.seq7(time, 1.0f);
					break;
				case 11:
					myssiliaCrest.pause(time, 2.0f / 3.0f);
					break;
				case 12:
					myssiliaCrest.seq8(time, 1.0f / 6.0f);
					break;
				case 13:
					myssiliaCrest.seq8(time, 1.0f / 6.0f);
					break;
				case 14:
					myssiliaCrest.seq8(time, 1.0f);
					break;
				case 15:
					myssiliaCrest.seq9(time, 0);
					break;
				//8 beats
				case 16:
					myssiliaCrest.seq10(time, 11.0f / 3.0f);
					break;
				case 17:
					myssiliaCrest.seq11(time, 1.0f, 1);
					break;
				case 18:
					myssiliaCrest.pause(time, 1.0f);
					break;
				case 19:
					myssiliaCrest.seq11(time, 1.0f, -1);
					break;
				case 20:
					myssiliaCrest.pause(time, 1.0f / 3.0f);
					break;
				case 21:
					myssiliaCrest.seq11(time, 1.0f, 1);
					break;
				//8 beats
				case 22:
					myssiliaCrest.seq12(time, 1.0f / 3.0f, new Vector3(30, 0, 0));
					break;
				case 23:
					myssiliaCrest.seq12(time, 1.0f / 3.0f, new Vector3(-60, 0, 0));
					break;
				case 24:
					myssiliaCrest.seq12(time, 2.0f / 3.0f, new Vector3(0, 0, -30));
					break;
				case 25:
					myssiliaCrest.seq12(time, 2.0f / 3.0f, new Vector3(30, 0, 0));
					break;
				case 26:
					myssiliaCrest.seq13(time, 2.0f);
					break;
				case 27:
					myssiliaCrest.seq14(time, 2.0f);
					break;
				case 28:
					myssiliaCrest.seq15(time, 2.0f);
					break;
				//8 beats
				case 29:
					myssiliaCrest.seq16(time, 2.0f);
					break;
				case 30:
					myssiliaCrest.seq17(time, 5.0f / 3.0f);
					break;
				case 31:
					myssiliaCrest.seq11(time, 4.0f / 3.0f, -1);
					break;
				case 32:
					myssiliaCrest.pause(time, 2.0f / 3.0f);
					break;
				case 33:
					myssiliaCrest.seq11(time, 1.0f, 1);
					break;
				case 34:
					myssiliaCrest.pause(time, 1.0f / 3.0f);
					break;
				case 35:
					myssiliaCrest.seq11(time, 1.0f, -1);
					break;
				//32 beats
				case 36:
					myssiliaCrest.idle(time, 32.0f);
					break;
				//8 beats
				case 37:
					myssiliaCrest.seq11(time, 2.0f, 1);
					break;
				case 38:
					myssiliaCrest.seq16(time, 2.0f);
					break;
				case 39:
					myssiliaCrest.seq15(time, 2.0f);
					break;
				case 40:
					myssiliaCrest.seq11(time, 2.0f, -1);
					break;
				//8 beats
				case 41:
					myssiliaCrest.seq16(time, 8.0f);
					break;
				//8 beats
				case 42:
					myssiliaCrest.seq11(time, 1.0f, 1);
					break;
				case 43:
					myssiliaCrest.seq19(time, 1.0f / 3.0f, new Vector3(-30, 0, 0));
					break;
				case 44:
					myssiliaCrest.seq19(time, 1.0f / 3.0f, new Vector3(60, 0, 0));
					break;
				case 45:
					myssiliaCrest.seq19(time, 1.0f / 3.0f, new Vector3(0, 0, -60));
					break;
				case 46:
					myssiliaCrest.seq18(time, 2.0f);
					break;
				case 47:
					myssiliaCrest.seq20(time, 2.0f / 3.0f, 1, 1);
					break;
				case 48:
					myssiliaCrest.seq20(time, 2.0f / 3.0f, 1, -1);
					break;
				case 49:
					myssiliaCrest.seq20(time, 2.0f / 3.0f, -1, 1);
					break;
				case 50:
					myssiliaCrest.seq11(time, 1.0f, 1);
					break;
				case 51:
					myssiliaCrest.seq11(time, 1.0f, -1);
					break;
				//8 beats
				case 52:
					myssiliaCrest.seq16(time, 2.0f);
					break;
				case 53:
					myssiliaCrest.seq21(time, 5.0f / 3.0f);
					break;
				case 54:
					myssiliaCrest.seq11(time, 4.0f / 3.0f, 1);
					break;
				case 55:
					myssiliaCrest.pause(time, 2.0f / 3.0f);
					break;
				case 56:
					myssiliaCrest.seq11(time, 1.0f, -1);
					break;
				case 57:
					myssiliaCrest.seq22(time, 4.0f / 3.0f, 1);
					break;
				//128 beats
				case 58:
					myssiliaCrest.idle(time, 128.0f);
					break;
			}

			switch (spaceship.animationStage)
			{
				case 1:
					spaceship.seq1(time, 64.0f);
					break;
				case 2:
					spaceship.seq2(time, 8.0f);
					break;
				case 3:
					spaceship.seq3(time, 39.0f / 6.0f);
					break;
				case 4:
					spaceship.seq4(time, 9.0f / 6.0f);
					break;
				case 5:
					spaceship.seq5(time, 12.0f);
					break;
				case 6:
					spaceship.seq6(time, 2.0f);
					break;
				case 7:
					spaceship.seq7(time, 2.0f);
					break;
				case 8:
					spaceship.seq8(time, 64.0f);
					break;
				case 9:
					spaceship.seq9(time, 8.0f);
					break;
				case 10:
					spaceship.seq10(time, 8.0f);
					break;
				case 11:
					spaceship.seq11(time, 8.0f);
					break;
				case 12:
					spaceship.seq12(time, 8.0f);
					break;
				case 13:
					spaceship.seq13(time, 32.0f);
					break;
				//8 beats
				case 14:
					spaceship.seq14(time, 1.0f, 16);
					break;
				case 15:
					spaceship.seq14(time, 1.0f, -12);
					break;
				case 16:
					spaceship.seq15(time, 5.0f / 3.0f, 16);
					break;
				case 17:
					spaceship.seq16(time, 5.0f / 6.0f, -8, 1);
					break;
				case 18:
					spaceship.seq16(time, 1.0f / 2.0f, -8, -1);
					break;
				case 19:
					spaceship.seq16(time, 1.0f / 2.0f, 8, -1);
					break;
				case 20:
					spaceship.seq16(time, 1.0f / 2.0f, 8, 1);
					break;
				case 21:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 22:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 23:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 24:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 25:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 26:
					spaceship.seq17(time, 1.0f / 3.0f, true, 6);
					break;
				//8 beats
				case 27:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 28:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 29:
					spaceship.seq17(time, 1.0f / 3.0f, true, 3);
					break;
				case 30:
					spaceship.seq14(time, 1.0f, -32);
					break;
				case 31:
					spaceship.seq19(time, 5.0f / 3.0f);
					break;
				case 32:
					spaceship.seq14(time, 4.0f / 3.0f, 8);
					break;
				case 33:
					spaceship.pause(time, 2.0f / 3.0f);
					break;
				case 34:
					spaceship.seq14(time, 1.0f, -10);
					break;
				case 35:
					spaceship.pause(time, 1.0f / 3.0f);
					break;
				case 36:
					spaceship.seq14(time, 1.0f, 12);
					break;
				//8 beats
				case 37:
					spaceship.seq20(time, 1.0f, 20);
					break;
				case 38:
					spaceship.seq20(time, 1.0f, 20);
					break;
				case 39:
					spaceship.seq20(time, 1.0f / 3.0f, 80);
					break;
				case 40:
					spaceship.seq20(time, 1.0f / 3.0f, 70);
					break;
				case 41:
					spaceship.seq20(time, 1.0f / 3.0f, 60);
					break;
				case 42:
					spaceship.seq20(time, 1.0f / 3.0f, 50);
					break;
				case 43:
					spaceship.seq20(time, 1.0f / 3.0f, 40);
					break;
				case 44:
					spaceship.seq20(time, 1.0f / 3.0f, 30);
					break;
				case 45:
					spaceship.seq14(time, 2.0f, 30);
					break;
				case 46:
					spaceship.seq14(time, 1.0f, 12);
					break;
				case 47:
					spaceship.seq14(time, 1.0f, -12);
					break;
				//8 beats
				case 48:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 49:
					spaceship.seq17(time, 1.0f / 3.0f);
					break;
				case 50:
					spaceship.seq17(time, 1.0f / 3.0f, true, 3);
					break;
				case 51:
					spaceship.seq14(time, 1.0f, -12);
					break;
				case 52:
					spaceship.seq14(time, 5.0f / 3.0f, 8);
					break;
				case 53:
					spaceship.seq14(time, 4.0f / 3.0f, -10);
					break;
				case 54:
					spaceship.pause(time, 2.0f / 3.0f);
					break;
				case 55:
					spaceship.seq14(time, 1.0f, 12);
					break;
				case 56:
					spaceship.pause(time, 1.0f / 3.0f);
					break;
				case 57:
					spaceship.seq14(time, 1.0f, -14);
					break;
				//
				case 58:
					spaceship.seq21(time, 32.0f, 5);
					break;
				case 59:
					spaceship.seq15(time, 64.0f, 16);
					break;
				case 60:
					spaceship.seq22(time, 64.0f);
					break;
			}

			ufo.constantOn(time);
			switch (ufo.animationStage)
			{
				case 1:
					ufo.pause(time, 94.0f);
					break;
				case 2:
					ufo.seq1(time, 2.0f);
					break;
				case 3:
					ufo.seq2(time, 15.0f);
					break;
				case 4:
					ufo.seq3(time, 1.0f);
					break;
				case 5:
					ufo.seq4(time, 14.0f);
					break;
				case 6:
					ufo.seq5(time, 2.0f);
					break;
				case 7:
					ufo.seq6(time, 32.0f);
					break;
				case 8:
					ufo.seq7(time, 24.0f);
					break;
				case 9:
					ufo.seq8(time, 4.0f, 1);
					break;
				case 10:
					ufo.seq8(time, 4.0f, -1);
					break;
				//8 beats
				case 11:
					ufo.seq9(time, 1.0f, 1);
					break;
				case 12:
					ufo.seq9(time, 1.0f, -1);
					break;
				case 13:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(10, -32, -20), 1);
					break;
				case 14:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(10, -32, -12), -1);
					break;
				case 15:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(10, -32, -4), 1);
					break;
				case 16:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(10, -32, 4), -1);
					break;
				case 17:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(10, -32, 12), 1);
					break;
				case 18:
					ufo.seq10(time, 0, new Vector3(10, -32, 20), 1);
					break;
				case 19:
					ufo.seq11(time, 4.0f / 3.0f, 1);
					break;
				case 20:
					ufo.seq11(time, 1.0f, -1);
					break;
				case 21:
					ufo.seq12(time, 1.0f);
					break;
				case 22:
					ufo.seq13(time, 1.0f);
					break;
				//8 beats
				case 23:
					ufo.seq14(time, 1.0f / 3.0f, new Vector3(-4, -12, 20), 1);
					break;
				case 24:
					ufo.seq14(time, 1.0f / 3.0f, new Vector3(-4, -16, 20), -1);
					break;
				case 25:
					ufo.seq14(time, 1.0f / 3.0f, new Vector3(-4, -20, 20), 1);
					break;
				case 26:
					ufo.seq14(time, 1.0f, new Vector3(-4, -24, 20), -1);
					break;
				case 27:
					ufo.seq15(time, 1.0f / 3.0f, new Vector3(-4, -22, 20));
					break;
				case 28:
					ufo.seq15(time, 1.0f / 3.0f, new Vector3(2, -22, 20));
					break;
				case 29:
					ufo.seq15(time, 1.0f / 3.0f, new Vector3(8, -22, 20));
					break;
				case 30:
					ufo.seq15(time, 1.0f / 3.0f, new Vector3(14, -22, 20));
					break;
				case 31:
					ufo.seq15(time, 1.0f / 3.0f, new Vector3(20, -22, 20));
					break;
				case 32:
					ufo.seq15(time, 0, new Vector3(14, -32, 20));
					break;
				case 33:
					ufo.seq16(time, 4.0f / 3.0f, 1);
					break;
				case 34:
					ufo.pause(time, 2.0f / 3.0f);
					break;
				case 35:
					ufo.seq11(time, 1.0f, 1);
					break;
				case 36:
					ufo.pause(time, 1.0f / 3.0f);
					break;
				case 37:
					ufo.seq11(time, 1.0f, -1);
					break;
				//8 beats
				case 38:
					ufo.seq8(time, 2.0f, 1);
					break;
				case 39:
					ufo.seq8(time, 2.0f, -1);
					break;
				case 40:
					ufo.seq11(time, 1.0f, -1);
					break;
				case 41:
					ufo.seq11(time, 1.0f, 1);
					break;
				case 42:
					ufo.seq12(time, 1.0f);
					break;
				case 43:
					ufo.seq13(time, 1.0f);
					break;
				//8 beats
				case 44:
					ufo.seq15(time, 0, new Vector3(14, -32, 20));
					break;
				case 45:
					ufo.seq11(time, 1.0f, -1);
					break;
				case 46:
					ufo.seq11(time, 1.0f, 1);
					break;
				case 47:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(14, -32, 20), 1);
					break;
				case 48:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(14, -32, 12), -1);
					break;
				case 49:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(14, -32, 4), 1);
					break;
				case 50:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(14, -32, -4), -1);
					break;
				case 51:
					ufo.seq10(time, 1.0f / 3.0f, new Vector3(14, -32, -12), 1);
					break;
				case 52:
					ufo.seq15(time, 0, new Vector3(14, -32, 20));
					break;
				case 53:
					ufo.seq16(time, 4.0f / 3.0f, 1);
					break;
				case 54:
					ufo.pause(time, 2.0f / 3.0f);
					break;
				case 55:
					ufo.seq11(time, 1.0f, 1);
					break;
				case 56:
					ufo.pause(time, 1.0f / 3.0f);
					break;
				case 57:
					ufo.seq11(time, 1.0f, -1);
					break;
				case 58:
					ufo.idle(time, 64.0f);
					break;
				case 59:
					ufo.seq17(time, 32.0f, 1);
					break;
				case 60:
					ufo.seq17(time, 32.0f, -1);
					break;
				case 61:
					ufo.pause(time, 64.0f);
					break;
			}

			switch(floorObjects.animationStage)
            {
				case 1:
					floorObjects.pause(time, 64.0f);
					break;
				case 2:
					floorObjects.seq1(time, 4.0f);
					break;
				case 3:
					floorObjects.pause(time, 28.0f);
					break;
				case 4:
					floorObjects.seq2(time, 4.0f);
					break;
			}

			lights.constantOn(time);
			switch (lights.animationStage)
			{
				case 1:
					lights.pause(time, 64.0f);
					break;
				case 2:
					lights.seq1(time, 4.0f);
					break;
				case 3:
					lights.pause(time, 28.0f);
					break;
				case 4:
					lights.seq2(time, 4.0f);
					break;
			}

			switch (cameraStage)
			{
				case 1:
					cameraSeq1(time, 32.0f);
					break;
				case 2:
					cameraSeq2(time, 8.0f);
					break;
                case 3:
                    cameraSeq3(time, 8.0f);
                    break;
                case 4:
                    cameraSeq4(time, 14.0f);
                    break;
				case 5:
					cameraSeq5(time, 2.0f);
					break;
				case 6:
					cameraSeq6(time, 8.0f);
					break;
				case 7:
					cameraSeq7(time, 8.0f);
					break;
				case 8:
					cameraSeq8(time, 12.0f);
					break;
				case 9:
					cameraSeq9(time, 3.0f);
					break;
				case 10:
					cameraSeq10(time, 1.0f);
					break;
				case 11:
					cameraSeq11(time, 15.0f);
					break;
				case 12:
					cameraSeq12(time, 1.0f);
					break;
				case 13:
					cameraSeq13(time, 14.0f);
					break;
				case 14:
					cameraSeq14(time, 2.0f);
					break;
				case 15:
					cameraSeq15(time, 32.0f);
					break;
				case 16:
					cameraSeq16(time, 24.0f);
					break;
				case 17:
					cameraSeq17(time, 6.0f);
					break;
				case 18:
					cameraSeq18(time, 2.0f);
					break;
				//8 beats
				case 19:
					cameraSeq19(time, 2.0f);
					break;
				case 20:
					cameraSeq20(time, 5.0f / 3.0f);
					break;
				case 21:
					cameraSeq21(time, 7.0f / 3.0f);
					break;
				case 22:
					cameraSeq22(time, 2.0f);
					break;
				//8 beats
				case 23:
					cameraSeq23(time, 2.0f);
					break;
				case 24:
					cameraSeq24(time, 4.0f);
					break;
				case 25:
					cameraSeq25(time, 2.0f);
					break;
				//8 beats
				case 26:
					cameraSeq26(time, 2.0f);
					break;
				case 27:
					cameraSeq27(time, 2.0f);
					break;
				case 28:
					cameraPause(time, 2.0f);
					break;
				case 29:
					cameraSeq29(time, 2.0f);
					break;
				//8 beats
				case 30:
					cameraSeq30(time, 11.0f / 3.0f, 180);
					break;
				case 31:
					cameraSeq30(time, 4.0f / 3.0f, 60);
					break;
				case 32:
					cameraPause(time, 2.0f / 3.0f);
					break;
				case 33:
					cameraSeq30(time, 1.0f, 60);
					break;
				case 34:
					cameraPause(time, 1.0f / 3.0f);
					break;
				case 35:
					cameraSeq30(time, 1.0f, 60);
					break;
				//16 beats
				case 36:
					cameraSeq36(time, 2.0f);
					break;
				case 37:
					if (expired)
					{
						tempVectors.Clear();
						tempVectors.Add(new Vector3(-13, -32, 16));
					}
					cameraSeq37(time, 14.0f);
					break;
				//16 beats
				case 38:
					cameraSeq38(time, 2.0f, 1);
					break;
				case 39:
					cameraSeq37(time, 2.0f);
					break;
				case 40:
					cameraSeq38(time, 2.0f, -1);
					break;
				case 41:
					cameraSeq37(time, 8.0f);
					break;
				case 42:
					cameraSeq42(time, 2.0f);
					break;
				//8 beats
				case 43:
                    cameraSeq43(time, 2.0f);
                    break;
				case 44:
					cameraSeq44(time, 12.0f);
					break;
				case 45:
					cameraSeq45(time, 2.0f);
					break;
				//8 beats
				case 46:
					cameraPause(time, 2.0f);
					break;
				case 47:
					cameraSeq47(time, 6.0f);
					break;
				case 48:
					cameraPause(time, 6.0f);
					break;
				case 49:
					cameraSeq49(time, 2.0f);
					break;
			}

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

			float time = (float)args.Time;

			if (!IsFocused)
			{
				return;
			}

			var input = KeyboardState;

			if (input.IsKeyDown(Keys.Escape))
			{
				Close();
			}

			if (input.IsKeyPressed(Keys.F))
			{
				if (!started)
				{
					mouseEnabled = false;
					cameraStage = 1;
                    myssiliaCrest.animationStage = 1;
                    ufo.animationStage = 1;
                    spaceship.animationStage = 1;
					floorObjects.animationStage = 1;
                    lights.animationStage = 1;
					started = true;
				}
			}

			if (input.IsKeyDown(Keys.W))
			{
				camera.Position += Vector3.Normalize(Vector3.Cross(camera.Up, camera.Right)) * cameraSpeed * time;
			}

			if (input.IsKeyDown(Keys.S))
			{
				camera.Position -= Vector3.Normalize(Vector3.Cross(camera.Up, camera.Right)) * cameraSpeed * time;
			}

			if (input.IsKeyDown(Keys.A))
			{
				camera.Position -= camera.Right * cameraSpeed * time;
			}

			if (input.IsKeyDown(Keys.D))
			{
				camera.Position += camera.Right * cameraSpeed * time;
			}

			if (input.IsKeyDown(Keys.Space))
			{
				camera.Position += camera.Up * cameraSpeed * time;
			}

			if (input.IsKeyDown(Keys.LeftShift))
			{
				camera.Position -= camera.Up * cameraSpeed * time;
			}

			if (input.IsKeyPressed(Keys.LeftControl))
			{
				cameraSpeed += 5;
				camera.Fov += 10;
			}

			if (input.IsKeyReleased(Keys.LeftControl))
			{
				cameraSpeed -= 5;
				camera.Fov -= 10;
			}

			if (input.IsKeyPressed(Keys.GraveAccent))
			{
				renderSetting *= -1;
				myssiliaCrest.renderSetting = renderSetting;
				ufo.renderSetting = renderSetting;
				spaceship.renderSetting = renderSetting;
				floorObjects.renderSetting = renderSetting;
				lights.renderSetting = renderSetting;
			}

			if (input.IsKeyPressed(Keys.B))
			{
				isBlinn *= -1;
				myssiliaCrest.isBlinn = isBlinn;
				spaceship.isBlinn = isBlinn;
				ufo.isBlinn = isBlinn;
				floorObjects.isBlinn = isBlinn;
				if (isBlinn == -1)
				{
					Console.WriteLine("\nShader: Phong");
				}
				else
				{
					Console.WriteLine("\nShader: Blinn-Phong");
				}
			}

			if (input.IsKeyPressed(Keys.G))
			{
				if (!mouseEnabled)
				{
					mouseEnabled = true;
					cameraStage = 0;
				}
			}

			var mouse = MouseState;
			if (mouseEnabled)
			{
				if (firstMove)
				{
					lastPos = new Vector2(mouse.X, mouse.Y);
					firstMove = false;
				}
				else
				{
					var deltaX = mouse.X - lastPos.X;
					var deltaY = mouse.Y - lastPos.Y;
					lastPos = new Vector2(mouse.X, mouse.Y);

					camera.Yaw += deltaX * sensitivity;
					camera.Pitch -= deltaY * sensitivity;
				}
			}
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(0, 0, Size.X, Size.Y);

			camera.AspectRatio = Size.X / (float)Size.Y;

			GL.BindTexture(TextureTarget.Texture2D, _fboTexture);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Size.X, Size.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, (IntPtr)null);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _renderBufferObject);

			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, Size.X, Size.Y);

			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, _renderBufferObject);
		}

		public Matrix4 generateArbRotationMatrix(Vector3 axis, float angle)
		{
			angle = MathHelper.DegreesToRadians(angle);

			var arbRotationMatrix = new Matrix4(
				(float)Math.Cos(angle) + (float)Math.Pow(axis.X, 2) * (1 - (float)Math.Cos(angle))	, axis.X * axis.Y * (1 - (float)Math.Cos(angle)) - axis.Z * (float)Math.Sin(angle)	, axis.X * axis.Z * (1 - (float)Math.Cos(angle)) + axis.Y * (float)Math.Sin(angle)	, 0,
				axis.Y * axis.X * (1 - (float)Math.Cos(angle)) + axis.Z * (float)Math.Sin(angle)	, (float)Math.Cos(angle) + (float)Math.Pow(axis.Y, 2) * (1 - (float)Math.Cos(angle)), axis.Y * axis.Z * (1 - (float)Math.Cos(angle)) - axis.X * (float)Math.Sin(angle)	, 0, 
				axis.Z * axis.X * (1 - (float)Math.Cos(angle)) - axis.Y * (float)Math.Sin(angle)	, axis.Z * axis.Y * (1 - (float)Math.Cos(angle)) + axis.X * (float)Math.Sin(angle)	, (float)Math.Cos(angle) + (float)Math.Pow(axis.Z, 2) * (1 - (float)Math.Cos(angle)), 0,
				0, 0, 0, 1
				);

			return arbRotationMatrix;
		}

		public void rotateCamera(Vector3 axis, Vector3 camRotationCenter, Vector3 lookAt, float rotationSpeed)
		{
			camera.Position -= camRotationCenter;
			if (axis == Vector3.UnitY)
			{
				camera.Yaw += rotationSpeed;
			}
			camera.Position = Vector3.Transform(camera.Position, generateArbRotationMatrix(axis, rotationSpeed).ExtractRotation());
			camera.Position += camRotationCenter;
			camera._front = Vector3.Normalize(lookAt - camera.Position);
		}

		#region Camera Movements
		public void cameraPause(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq1(float delta, float duration)
		{
			if (expired)
            {
				camera.Position = new Vector3(0, -30, 32);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			var camFront = new Vector3(0, -30, 15);
			rotateCamera(Vector3.UnitX, camFront, camFront, -45 / 64.0f * delta);

            if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq2(float delta, float duration)
		{
			if (expired)
            {
				camera.Position = spaceship.ShapeCenter + new Vector3(0, -1, 9);

				var camFrontTemp = spaceship.ShapeCenter + Vector3.UnitY;
				rotateCamera(Vector3.UnitY, camFrontTemp, camFrontTemp, -30);

				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			var camFront = spaceship.ShapeCenter + Vector3.UnitY;
			rotateCamera(Vector3.UnitY, camFront, camFront, 360 / duration * delta);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq3(float delta, float duration)
		{
			if (expired)
			{
				camera.Position = ufo.ShapeCenter + new Vector3(0, -1, 9);

				var camFrontTemp = ufo.ShapeCenter + Vector3.UnitY;
				rotateCamera(Vector3.UnitY, camFrontTemp, camFrontTemp, -30);

				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			var camFront = ufo.ShapeCenter + Vector3.UnitY;
			rotateCamera(Vector3.UnitY, camFront, camFront, 360 / duration * delta);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq4(float delta, float duration)
		{
			if (expired)
			{
				camera.Position = myssiliaCrest.ShapeCenter + new Vector3(0, 0, 15);
				tempVectors.Add(myssiliaCrest.ShapeCenter);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitX, new Vector3(0, 0, 45), tempVectors[0], 90 / duration * delta);
			tempVectors[0] -= Vector3.UnitY * delta;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq5(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Add(spaceship.ShapeCenter - tempVectors[0]);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			tempVectors[0] += tempVectors[1] / duration * delta;
			camera._front = Vector3.Normalize(tempVectors[0] - camera.Position);
			camera.Position -= Vector3.UnitZ * delta * 12;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq6(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Add(camera.Position - spaceship.ShapeCenter);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position = spaceship.ShapeCenter + tempVectors[2];
			camera._front = Vector3.Normalize(spaceship.ShapeCenter - camera.Position);
			tempVectors[2] -= Vector3.UnitX * delta;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq7(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(camera.Position - Vector3.UnitZ * 8);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], spaceship.ShapeCenter, 90 * delta / duration);
			camera.Position += Vector3.UnitY * delta;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq8(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera._front = Vector3.Normalize(spaceship.ShapeCenter - camera.Position);
			camera.Position += Vector3.UnitY * delta * 1.5f;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq9(float delta, float duration)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera._front = Vector3.Normalize(spaceship.ShapeCenter - camera.Position);
			camera.Position += new Vector3(1.25f, -0.75f, 0) * delta * 8;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq10(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Add(spaceship.ShapeCenter);
				tempVectors.Add((ufo.ShapeCenter - tempVectors[0]) * delta / duration);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera._front = Vector3.Normalize(tempVectors[0] - camera.Position);
			camera.Position += new Vector3(1.25f, -0.75f, 0) * delta * 8;
			tempVectors[0] += tempVectors[1];

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq11(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera._front = Vector3.Normalize(ufo.ShapeCenter - camera.Position);
			camera.Position += new Vector3(-1, 0.1f, 0) * delta;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq12(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Add(camera.Position - Vector3.UnitX * 20);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], ufo.ShapeCenter, 90 * delta / duration);
            camera.Position += Vector3.UnitY * delta * 12;

            if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq13(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += Vector3.UnitY * delta;
			camera._front = Vector3.Normalize(ufo.ShapeCenter - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq14(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(ufo.ShapeCenter);
				tempVectors.Add(myssiliaCrest.ShapeCenter - Vector3.UnitY * 16 - tempVectors[0]);
				tempVectors.Add(myssiliaCrest.ShapeCenter + new Vector3(0, -30, 32) - camera.Position);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			tempVectors[0] += tempVectors[1] * delta / duration;
			camera.Position += tempVectors[2] * delta / duration;
			camera._front = Vector3.Normalize(tempVectors[0] - camera.Position);

            if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq15(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(myssiliaCrest.ShapeCenter - Vector3.UnitY * 16);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitX, tempVectors[0], tempVectors[0], 22.5f * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq16(float delta, float duration)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], tempVectors[0], 720 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq17(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(myssiliaCrest.ShapeCenter - Vector3.UnitY * 16);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], tempVectors[0], -360 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq18(float delta, float duration)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += new Vector3(-2, 1, 0) * delta;
			tempVectors[0] += Vector3.UnitY * delta;
			camera._front = Vector3.Normalize(tempVectors[0] - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq19(float delta, float duration)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitX, tempVectors[0], tempVectors[0], 180 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq20(float delta, float duration)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], tempVectors[0], 180 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq21(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Add(new Vector3(1, 3, -1));
				tempVectors[1] = Vector3.Normalize(tempVectors[1]);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(tempVectors[1], tempVectors[0], tempVectors[0], -360 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq22(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(-Vector3.UnitY * 16);
				tempVectors.Add(tempVectors[0] + Vector3.UnitZ * 32 - camera.Position);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += tempVectors[1] * delta / duration;
			camera._front = Vector3.Normalize(tempVectors[0] - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq23(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(-Vector3.UnitY * 16);
				tempVectors.Add(new Vector3(1, 4, 1));
				tempVectors[1] = Vector3.Normalize(tempVectors[1]);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(tempVectors[1], tempVectors[0], tempVectors[0], 360 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq24(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(-Vector3.UnitY * 16);
				tempVectors.Add(new Vector3(4, 0, -1));
				tempVectors[1] = Vector3.Normalize(tempVectors[1]);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(tempVectors[1], tempVectors[0], tempVectors[0], 180 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq25(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(-Vector3.UnitY * 16);
				tempVectors.Add(new Vector3(180 - MathHelper.RadiansToDegrees(MathF.Atan(camera.Position.X / camera.Position.Z))));
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], tempVectors[0], -tempVectors[1].X * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq26(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(camera.Position - (myssiliaCrest.ShapeCenter - Vector3.UnitY * 16));
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position = myssiliaCrest.ShapeCenter - Vector3.UnitY * 16 + tempVectors[0];
			camera._front = Vector3.Normalize(myssiliaCrest.ShapeCenter - Vector3.UnitY * 16 - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq27(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(new Vector3(0, -16, 32) - camera.Position);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += tempVectors[0] * delta / duration;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq29(float delta, float duration)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera._front = Vector3.Normalize(myssiliaCrest.ShapeCenter - Vector3.UnitY * 16 - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq30(float delta, float duration, float angle)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, Vector3.Zero, -Vector3.UnitY * 16, angle * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq36(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(-Vector3.UnitY * 16);
				tempVectors.Add(new Vector3(-13, -32, 16) - tempVectors[0]);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			tempVectors[0] += tempVectors[1] * delta / duration;
			camera._front = Vector3.Normalize(tempVectors[0] - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq37(float delta, float duration)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], tempVectors[0], 21.1764f * delta);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq38(float delta, float duration, int direction)
		{
			if (expired)
			{
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, tempVectors[0], tempVectors[0], 21.1764f * delta);
			tempVectors[0] += Vector3.UnitY * 10 * delta * direction;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq42(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(new Vector3(0, -16, 32) - camera.Position);
				tempVectors.Add(new Vector3(-13, -32, 16));
				tempVectors.Add(-Vector3.UnitY * 16 - tempVectors[1]);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += tempVectors[0] * delta / duration;
			camera._front = Vector3.Normalize(tempVectors[1] - camera.Position);
			tempVectors[1] += tempVectors[2] * delta / duration;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq43(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(new Vector3(0, 45, 200) - camera.Position);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += tempVectors[0] * delta / duration;
			camera._front = Vector3.Normalize(-Vector3.UnitY * 16 - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}
		
		public void cameraSeq44(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, Vector3.Zero, -Vector3.UnitY * 16, 360 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq45(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(new Vector3(0, -16, 32) - camera.Position);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += tempVectors[0] * delta / duration;
			camera._front = Vector3.Normalize(-Vector3.UnitY * 16 - camera.Position);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq47(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			rotateCamera(Vector3.UnitY, Vector3.Zero, -Vector3.UnitY * 16, 360 * delta / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void cameraSeq49(float delta, float duration)
		{
			if (expired)
			{
				tempVectors.Clear();
				tempVectors.Add(new Vector3(0, -24, 40) - camera.Position);
				tempVectors.Add(new Vector3(-Vector3.UnitY * 16));
				tempVectors.Add(-Vector3.UnitY * 12 - tempVectors[1]);
				expired = false;
			}

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				cameraStage++;
			}

			camera.Position += tempVectors[0] * delta / duration;
			camera._front = Vector3.Normalize(tempVectors[1] - camera.Position);
			tempVectors[1] += tempVectors[2] * delta / duration;

			if (!expired)
			{
				timePassed += delta;
			}
		}

		#endregion
	}
}
