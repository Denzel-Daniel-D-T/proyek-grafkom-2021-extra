using System;
using System.Collections.Generic;
using LearnOpenTK.Common;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class MyssiliaCrest : ShapesCollection
	{
		public override Vector3 ShapeCenter => objectList[0].rotationCenter;

		public MyssiliaCrest(Camera camera) : base(camera)
        {

		}

		public override void initObjects()
		{
			var tempColor = new Vector3(0, 1, 1);
			var nullCenter = new Asset3d(0, 0, tempColor, tempColor, Vector3.One);
			nullCenter.objectCenter = globalRotationCenter;
			nullCenter.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenter);

			var nullCenterId1 = new Asset3d(0, 1, tempColor, tempColor, Vector3.One);
			nullCenterId1.objectCenter = globalRotationCenter;
			nullCenterId1.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId1);

			var nullCenterId2 = new Asset3d(0, 2, tempColor, tempColor, Vector3.One);
			nullCenterId2.objectCenter = globalRotationCenter;
			nullCenterId2.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId2);

			var nullCenterId3_4_5 = new Asset3d(0, 3, tempColor, tempColor, Vector3.One);
			nullCenterId3_4_5.objectCenter = globalRotationCenter;
			nullCenterId3_4_5.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId3_4_5);

			var nullCenterId6 = new Asset3d(0, 6, tempColor, tempColor, Vector3.One);
			nullCenterId6.objectCenter = globalRotationCenter;
			nullCenterId6.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId6);

			var nullCenterId7 = new Asset3d(0, 7, tempColor, tempColor, Vector3.One);
			nullCenterId7.objectCenter = globalRotationCenter;
			nullCenterId7.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId7);

			var nullCenterId8 = new Asset3d(0, 8, tempColor, tempColor, Vector3.One);
			nullCenterId8.objectCenter = globalRotationCenter;
			nullCenterId8.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId8);

			var nullCenterId9 = new Asset3d(0, 9, tempColor, tempColor, Vector3.One);
			nullCenterId9.objectCenter = globalRotationCenter;
			nullCenterId9.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId9);

			var nullCenterId10 = new Asset3d(0, 10, tempColor, tempColor, Vector3.One);
			nullCenterId10.objectCenter = globalRotationCenter;
			nullCenterId10.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId10);

			var nullCenterId11 = new Asset3d(0, 11, tempColor, tempColor, Vector3.One);
			nullCenterId11.objectCenter = globalRotationCenter;
			nullCenterId11.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId11);

			var nullCenterId12 = new Asset3d(0, 12, tempColor, tempColor, Vector3.One);
			nullCenterId12.objectCenter = globalRotationCenter;
			nullCenterId12.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId12);

			var nullCenterId13 = new Asset3d(0, 13, tempColor, tempColor, Vector3.One);
			nullCenterId13.objectCenter = globalRotationCenter;
			nullCenterId13.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId13);

			var nullCenterId14 = new Asset3d(0, 14, tempColor, tempColor, Vector3.One);
			nullCenterId14.objectCenter = globalRotationCenter;
			nullCenterId14.rotationCenter = globalRotationCenter;
			objectList.Add(nullCenterId14);

			{

				var c1 = new Asset3d(1, 1, tempColor, tempColor, Vector3.One);
				c1.createTrapezoidPrism(0, 0, 0, 6.75f, 0.5f, 0.5f, MathHelper.DegreesToRadians(30.0f), MathHelper.DegreesToRadians(60.0f));
				c1.translate(-(c1.objectDimension.X / 2) - (MathF.Tan(MathHelper.DegreesToRadians(60.0f)) * c1.objectDimension.Y), (c1.objectDimension.Y / 2), 0);
				c1.translate(c1.objectDimension.X * 1.8f, -(MathF.Tan(MathHelper.DegreesToRadians(30.0f)) * c1.objectDimension.X * 1.8f), 0);
				objectList.Add(c1);

				var c2 = new Asset3d(1, 1, tempColor, tempColor, Vector3.One);
				c2.createTrapezoidPrism(0, 0, 0, 3f, 0.5f, 0.5f, MathHelper.DegreesToRadians(60.0f), MathHelper.DegreesToRadians(-30.0f));
				c2.rotationCenter = new Vector3(-(c2.objectDimension.X / 2) - (MathF.Tan(MathHelper.DegreesToRadians(60.0f)) * c2.objectDimension.Y), c2.objectCenter.Y - (c2.objectDimension.Y / 2), 0);
				c2.rotate(c2.rotationCenter, c2._euler[2], 120);
				c2.translate((c1.objectDimension.X / 2) + (c2.objectDimension.X / 2) + (MathF.Tan(MathHelper.DegreesToRadians(60.0f)) * c2.objectDimension.Y * 2) - (c1.objectDimension.X / 2) - (MathF.Tan(MathHelper.DegreesToRadians(60.0f)) * c1.objectDimension.Y), (c1.objectDimension.Y / 2), 0);
				c2.translate(c1.objectDimension.X * 1.8f, -(MathF.Tan(MathHelper.DegreesToRadians(30.0f)) * c1.objectDimension.X * 1.8f), 0);
				objectList.Add(c2);

				var c3 = new Asset3d(c1, 2, 2);
				c3.rotationCenter = c2.rotationCenter;
				c3.translate(2 * MathF.Tan(MathHelper.DegreesToRadians(60.0f)) * c3.objectDimension.Y, -2 * c3.objectDimension.Y, 0);
				var rotatePoint1 = c3.objectCenter.X + c3.objectDimension.X / 2 + MathF.Tan(MathHelper.DegreesToRadians(60.0f)) * c3.objectDimension.Y;
				c3.mirrorX(rotatePoint1);
				c3.rotate(c3.rotationCenter, c3._euler[2], 120);
				objectList.Add(c3);

				var c4 = new Asset3d(c2, 2, 2);
				c4.rotationCenter = c2.rotationCenter;
				c4.translate(2 * MathF.Tan(MathHelper.DegreesToRadians(60.0f)) * c3.objectDimension.Y, -2 * c3.objectDimension.Y, 0);
				c4.mirrorX(rotatePoint1);
				c4.rotate(c4.rotationCenter, c4._euler[2], 120);
				objectList.Add(c4);

				for (int i = 1; i <= 2; i++)
				{
					var c5 = new Asset3d(c1, 1, 1);
					c5.rotate(globalRotationCenter, globalEuler[2], -120 * i);
					objectList.Add(c5);
					var c6 = new Asset3d(c2, 1, 1);
					c6.rotate(globalRotationCenter, globalEuler[2], -120 * i);
					objectList.Add(c6);

					var c7 = new Asset3d(c3, 2, 2);
					c7.rotate(globalRotationCenter, globalEuler[2], -120 * i);
					objectList.Add(c7);
					var c8 = new Asset3d(c4, 2, 2);
					c8.rotate(globalRotationCenter, globalEuler[2], -120 * i);
					objectList.Add(c8);
				}
			}

			{
				var scale = 2.5f;
				var s1 = new Asset3d(3, 3, tempColor, tempColor, Vector3.One);
				s1.createRhombicPrism(0, 0, 0, 0.125f * scale, 0.125f * scale * MathF.Tan(MathHelper.DegreesToRadians(60.0f)), 0.5f);
				s1.translate(0, s1.objectDimension.Y / 2, 0);
				s1.rotate(globalRotationCenter, globalEuler[2], 15);
				objectList.Add(s1);

				for (int i = 1; i <= 5; i++)
				{
					var si = new Asset3d(s1, 3, 3);
					si.rotate(globalRotationCenter, globalEuler[2], -60 * i);
					objectList.Add(si);
				}
			}

			{
				var scale = 2.5f;
				var os1 = new Asset3d(4, 4, tempColor, tempColor, Vector3.One);
				var dwa = 0.625f * scale * MathF.Sin(MathHelper.DegreesToRadians(50.0f));
				var os1_X = dwa / MathF.Tan(MathHelper.DegreesToRadians(20.0f)) - dwa / MathF.Tan(MathHelper.DegreesToRadians(50.0f));
				os1.createTrapezoidPrism(0, 0, 0, os1_X, MathF.Cos(MathHelper.DegreesToRadians(40.0f)) * 0.375f * scale, 0.5f, MathHelper.DegreesToRadians(70.0f), MathHelper.DegreesToRadians(-40.0f));

				var os2 = new Asset3d(os1, 4, 4);
				os2.rotationCenter = new Vector3(-MathF.Tan(MathHelper.DegreesToRadians(70.0f)) * os2.objectDimension.Y - os2.objectDimension.X / 2, os2.objectCenter.Y - os2.objectDimension.Y / 2, 0);
				os2.mirrorX(-MathF.Tan(MathHelper.DegreesToRadians(70.0f)) * os2.objectDimension.Y - os2.objectDimension.X / 2);
				os2.rotate(os2.rotationCenter, os2._euler[2], -140);

				os1.rotationCenter = os2.rotationCenter;

				os2.translate(-os1.rotationCenter.X, -os1.rotationCenter.Y, 0);
				os1.translate(-os1.rotationCenter.X, -os1.rotationCenter.Y, 0);

				var os_s1 = new Asset3d(5, 4, tempColor, tempColor, Vector3.One);
				os_s1.createTrapezoidPrism(0, 2, 0, 0, MathF.Cos(MathHelper.DegreesToRadians(40.0f)) * 0.5f * scale, 0.5f, MathHelper.DegreesToRadians(70.0f), MathHelper.DegreesToRadians(-40.0f));

				var os_s2 = new Asset3d(os_s1, 5, 4);
				os_s2.rotationCenter = new Vector3(-MathF.Tan(MathHelper.DegreesToRadians(70.0f)) * os_s2.objectDimension.Y - os_s2.objectDimension.X / 2, os_s2.objectCenter.Y - os_s2.objectDimension.Y / 2, 0);
				os_s2.mirrorX(-MathF.Tan(MathHelper.DegreesToRadians(70.0f)) * os_s2.objectDimension.Y - os_s2.objectDimension.X / 2);
				os_s2.rotate(os_s2.rotationCenter, os_s2._euler[2], -140);

				os_s1.rotationCenter = os_s2.rotationCenter;

				os_s2.translate(-os_s1.rotationCenter.X, -os_s1.rotationCenter.Y, 0);
				os_s1.translate(-os_s1.rotationCenter.X, -os_s1.rotationCenter.Y, 0);
				var awd = 0.5f * scale * MathF.Cos(MathHelper.DegreesToRadians(40.0f));
				var awd2 = awd / MathF.Tan(MathHelper.DegreesToRadians(20.0f));
				os_s1.translate(awd2, awd, 0);
				os_s2.translate(awd2, awd, 0);

				var tempList1 = new List<Asset3d>();
				var tempList2 = new List<Asset3d>();
				tempList1.Add(os1);
				tempList1.Add(os2);
				tempList2.Add(os_s1);
				tempList2.Add(os_s2);

				var translateLength = (os1.objectDimension.X + os1.objectDimension.Y * MathF.Tan(MathHelper.DegreesToRadians(70.0f)) + os1.objectDimension.Y * MathF.Tan(MathHelper.DegreesToRadians(-40.0f)));
				var testtt = translateLength + MathF.Cos(MathHelper.DegreesToRadians(50.0f)) * scale;
				var ddd = testtt / MathF.Cos(MathHelper.DegreesToRadians(20.0f));

				foreach (Asset3d i in tempList1)
				{
					i.rotate(globalRotationCenter, globalEuler[2], -110);
					i.translate(0, ddd + 0.5f * scale, 0);
				}

				foreach (Asset3d i in tempList2)
				{
					i.rotate(globalRotationCenter, globalEuler[2], -110);
					i.translate(0, ddd + 0.5f * scale, 0);
				}

				for (int i = 0; i < 6; i++)
				{
					foreach (Asset3d j in tempList1)
					{
						var os_i = new Asset3d(j, 4, 4);
						os_i.rotate(globalRotationCenter, globalEuler[2], 60 * i - 15);
						objectList.Add(os_i);
					}

					foreach (Asset3d j in tempList2)
					{
						var os_i = new Asset3d(j, 5, 4);
						os_i.rotate(globalRotationCenter, globalEuler[2], 60 * i - 15);
						objectList.Add(os_i);
					}
				}
			}

			{
				var scale = 2.5f;
				var cn1 = new Asset3d(6, 5, tempColor, tempColor, Vector3.One);
				cn1.createEllipticCone(0, (MathF.Cos(MathHelper.DegreesToRadians(60.0f)) + 0.125f) * scale + MathF.PI / 2, 0, 0.025f * MathF.PI, MathF.PI, 0.025f * MathF.PI, 72, 24);
				cn1.rotate(cn1.rotationCenter, globalEuler[0], 180);

				var capsule1 = new Asset3d(6, 5, tempColor, tempColor, Vector3.One);
				capsule1.createSinusoidCapsule(0, ((MathF.Cos(MathHelper.DegreesToRadians(60.0f))) + 0.125f + 1 + 0.25f) * scale, 0, 0.125f, 0.5f, 0.125f, 72, 24);

				var tempList = new List<Asset3d>();
				tempList.Add(cn1);
				tempList.Add(capsule1);

				for (int i = 0; i < 6; i++)
				{
					foreach (Asset3d j in tempList)
					{
						var flake = new Asset3d(j, 6, 5);
						flake.rotate(globalRotationCenter, globalEuler[2], 60 * i + 15);
						objectList.Add(flake);
					}
				}
			}

			{
				var scale = 3.0f;
				var rh1 = new Asset3d(7, 6, tempColor, tempColor, Vector3.One);
				rh1.createRhombicPrism(0, -2.75f * 0.5f * 7, 0, scale * 0.5f, scale * 0.5f * MathF.Sqrt(3), 0.5f);
				objectList.Add(rh1);

				var rh2 = new Asset3d(rh1, 7, 7);
				rh2.scaleLocal(4 / 3.0f, 4 / 3.0f, 1);
				rh2.translate((rh2.objectDimension.Y + rh2.objectDimension.X) / 2, -rh1.objectDimension.Y / 2, 0);
				objectList.Add(rh2);

				var rh3 = new Asset3d(rh2, 7, 8);
				rh3.translate(rh1.objectDimension.X + rh2.objectDimension.X / 2, -rh1.objectDimension.Y / 2, 0);
				objectList.Add(rh3);

				var rh4 = new Asset3d(rh2, 7, 7);
				rh4.mirrorX(0);
				objectList.Add(rh4);

				var rh5 = new Asset3d(rh3, 7, 8);
				rh5.mirrorX(0);
				objectList.Add(rh5);

				var rh_en1 = new Asset3d(8, 9, tempColor, tempColor, Vector3.One);
				rh_en1.createTrapezoidPrism(0, 0, 0, MathF.Sqrt(MathF.Pow(rh1.objectDimension.X, 2) + MathF.Pow(rh1.objectDimension.Y, 2)), MathF.Cos(MathHelper.DegreesToRadians(30.0f)) * rh2.objectDimension.X / 4, 0.5f, MathHelper.DegreesToRadians(-30.0f), MathHelper.DegreesToRadians(-30.0f));
				rh_en1.rotationCenter = new Vector3(rh_en1.objectCenter.X + rh_en1.objectDimension.X / 2, rh_en1.objectCenter.Y + rh_en1.objectDimension.Y / 2, 0);
				rh_en1.translate(0, -rh_en1.objectDimension.Y / 2, 0);
				rh_en1.objectCenter = rh_en1.rotationCenter;
				rh_en1.rotate(rh_en1.rotationCenter, rh_en1._euler[2], -60);

				var rh_en2 = new Asset3d(rh_en1, 8, 9);
				rh_en2.mirrorY(0);

				rh_en1.translate((rh3.objectCenter.X + rh3.objectDimension.X) - rh_en1.objectCenter.X, rh3.objectCenter.Y - rh_en1.objectCenter.Y, 0);
				rh_en2.translate((rh3.objectCenter.X + rh3.objectDimension.X) - rh_en2.objectCenter.X, rh3.objectCenter.Y - rh_en2.objectCenter.Y, 0);

				var rh_en3 = new Asset3d(rh_en1, 8, 9);
				rh_en3.mirrorX(0);

				var rh_en4 = new Asset3d(rh_en2, 8, 9);
				rh_en4.mirrorX(0);

				objectList.Add(rh_en1);
				objectList.Add(rh_en2);
				objectList.Add(rh_en3);
				objectList.Add(rh_en4);

				var rh6 = new Asset3d(rh2, 9, 10);
				rh6.scaleLocal(1.2f, 1.2f, 1);
				rh6.translate(-rh6.objectCenter.X, -(rh1.objectDimension.Y + rh6.objectDimension.Y * 1.5f), 0);
				objectList.Add(rh6);

				var rh_en5 = new Asset3d(10, 11, tempColor, tempColor, Vector3.One);
				var rh_en5_Y = (rh6.objectDimension.X * rh6.objectDimension.Y / 2) / (MathF.Sqrt(MathF.Pow(rh6.objectDimension.X, 2) + MathF.Pow(rh6.objectDimension.Y, 2)) / 2);
				rh_en5.createTrapezoidPrism(0, 0, 0, rh6.objectDimension.X, rh_en5_Y / 3, 0.5f, MathHelper.DegreesToRadians(60.0f), MathHelper.DegreesToRadians(-30.0f));
				rh_en5.translate(rh_en5.objectDimension.X / 2 + rh_en5.objectDimension.Y * MathF.Tan(MathHelper.DegreesToRadians(60.0f)), rh_en5.objectDimension.Y / 2, 0);
				rh_en5.rotate(globalRotationCenter, globalEuler[2], -120);
				rh_en5.translate(0, -(rh1.objectDimension.Y + rh6.objectDimension.Y * 1.5f - rh2.objectCenter.Y - 11 / 6.0f * rh6.objectDimension.Y), 0);

				var rh_en6 = new Asset3d(rh_en5, 10, 11);
				rh_en6.mirrorX(0);

				var rh_en7 = new Asset3d(rh_en5, 10, 11);
				rh_en7.mirrorY(rh6.objectCenter.Y);

				var rh_en8 = new Asset3d(rh_en6, 10, 11);
				rh_en8.mirrorY(rh6.objectCenter.Y);

				objectList.Add(rh_en5);
				objectList.Add(rh_en6);
				objectList.Add(rh_en7);
				objectList.Add(rh_en8);

				var rh_en9 = new Asset3d(11, 12, tempColor, tempColor, Vector3.One);
				rh_en9.createTrapezoidPrism(0, 0, 0, rh6.objectDimension.X * 2 + MathF.Tan(MathHelper.DegreesToRadians(30.0f)) * rh_en5_Y / 1.5f, rh_en5_Y / 3, 0.5f, MathHelper.DegreesToRadians(-30.0f), MathHelper.DegreesToRadians(-30.0f));
				rh_en9.translate(0, -rh_en9.objectDimension.Y / 2, 0);
				rh_en9.rotationCenter = new Vector3(rh_en9.objectDimension.X / 2, 0, 0);
				rh_en9.objectCenter = rh_en9.rotationCenter;
				rh_en9.rotate(rh_en9.rotationCenter, globalEuler[2], -60);
				rh_en9.translate(rh6.objectDimension.X * 1.5f + rh_en9.objectDimension.Y / MathF.Cos(MathHelper.DegreesToRadians(30.0f)) - rh_en9.objectCenter.X, rh6.objectCenter.Y, 0);

				var rh_en10 = new Asset3d(rh_en9, 11, 12);
				rh_en10.mirrorY(rh_en9.objectCenter.Y);

				var rh_en11 = new Asset3d(rh_en9, 11, 12);
				rh_en11.mirrorX(0);

				var rh_en12 = new Asset3d(rh_en10, 11, 12);
				rh_en12.mirrorX(0);

				var rh_en13 = new Asset3d(rh_en9, 12, 13);

				var rh_en14 = new Asset3d(rh_en10, 12, 13);

				var rh_en15 = new Asset3d(rh_en11, 12, 13);

				var rh_en16 = new Asset3d(rh_en12, 12, 13);

				rh_en13.translate(3 * rh6.objectDimension.X, 0, 0);
				rh_en14.translate(3 * rh6.objectDimension.X, 0, 0);

				rh_en15.translate(-3 * rh6.objectDimension.X, 0, 0);
				rh_en16.translate(-3 * rh6.objectDimension.X, 0, 0);

				var rh6_modX = rh6.objectDimension.X * 2 / 3.0f;
				rh_en9.translate(MathF.Cos(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, MathF.Sin(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, 0);
				rh_en10.translate(MathF.Cos(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, MathF.Sin(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, 0);

				rh_en11.translate(-MathF.Cos(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, MathF.Sin(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, 0);
				rh_en12.translate(-MathF.Cos(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, MathF.Sin(MathHelper.DegreesToRadians(60.0f)) * rh6_modX, 0);

				objectList.Add(rh_en9);
				objectList.Add(rh_en10);
				objectList.Add(rh_en11);
				objectList.Add(rh_en12);
				objectList.Add(rh_en13);
				objectList.Add(rh_en14);
				objectList.Add(rh_en15);
				objectList.Add(rh_en16);

				var rh7 = new Asset3d(rh1, 7, 6);
				rh7.mirrorY(rh6.objectCenter.Y);
				objectList.Add(rh7);

				var rh_en17 = new Asset3d(13, 14, tempColor, tempColor, Vector3.One);
				rh_en17.createTrapezoidPrism(0, 0, 0, rh6.objectDimension.X * 2, MathF.Cos(MathHelper.DegreesToRadians(30.0f)) * rh6.objectDimension.X / 4, 0.5f, MathHelper.DegreesToRadians(-60.0f), MathHelper.DegreesToRadians(-30.0f));
				rh_en17.translate(0, -rh_en17.objectDimension.Y / 2, 0);
				rh_en17.rotationCenter = new Vector3(rh_en17.objectDimension.X / 2, 0, 0);
				rh_en17.objectCenter = rh_en17.rotationCenter;
				rh_en17.rotate(rh_en17.rotationCenter, globalEuler[2], -60);
				rh_en17.translate(0, rh6.objectCenter.Y, 0);

				var rh_en18 = new Asset3d(rh_en17, 13, 14);
				rh_en18.mirrorY(rh6.objectCenter.Y);

				var rh_en19 = new Asset3d(rh_en17, 13, 14);
				rh_en19.mirrorX(0);

				var rh_en20 = new Asset3d(rh_en18, 13, 14);
				rh_en20.mirrorX(0);

				objectList.Add(rh_en17);
				objectList.Add(rh_en18);
				objectList.Add(rh_en19);
				objectList.Add(rh_en20);
			}
        }

		public override void load()
		{
			foreach (Asset3d i in objectList)
			{
				i.load("shader", "cobe");
			}
		}

		public override void render(List<DirLight> dirLightList, List<PointLight> pointLightList, List<FlashLight> flashLightList)
		{
			foreach (Asset3d i in objectList)
			{
				i.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix(), dirLightList, pointLightList, flashLightList, camera.Position, isBlinn);
			}
		}

		public void pause(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void idle(float delta, float duration)
        {
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], 45 * delta / 8.0f);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq1(float delta, float duration, int mode)
        {
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
            {
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 2:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						if (mode == 1)
						{
							i.rotate(objectList[0].rotationCenter, objectList[25]._euler[2], delta * -90.0f);
						}
						break;
					case 6:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
					case 7:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						break;
					case 8:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 8.0f);
						break;
					case 9:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 10:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						break;
					case 11:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 12:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 4.0f);
						break;
					case 13:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
					case 14:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 4.0f);
						break;
				}
            }

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq2(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta);
						break;
					case 2:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 2.0f);
						break;
					case 6:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta);
						break;
					case 7:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 2.0f);
						break;
					case 8:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 2.0f);
						break;
					case 9:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
					case 10:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 2.0f);
						break;
					case 11:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
					case 12:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta / 2.0f);
						break;
					case 13:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
					case 14:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta / 2.0f);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq3(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 2:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 90 * 3.0f / 4.0f * delta);
						break;
					case 6:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 7:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta);
						break;
					case 8:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta);
						break;
					case 9:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta);
						break;
					case 10:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta);
						break;
					case 11:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta);
						break;
					case 12:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], 45 * delta);
						break;
					case 13:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta);
						break;
					case 14:
						i.rotate(objectList[0].rotationCenter, globalEuler[1], -45 * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq4(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 180 * delta);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 2:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -180 * delta);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -180 * delta);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 7:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -180 * delta);
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 8:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 180 * delta);
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 12:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 180 * delta);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 13:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -180 * delta);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 14:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -180 * delta);
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -90 * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq5(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.translate(-20 * delta, 0, 0);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 2:
						i.translate(20 * delta, 0, 0);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.translate(20 * delta, 0, 0);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 12:
						i.rotate(new Vector3(24, 0, 0), globalEuler[1], 90 / duration * delta);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 13:
						i.rotate(new Vector3(-24, 0, 0), globalEuler[1], 90 / duration * delta);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
            {
				foreach (Asset3d i in objectList)
                {
					switch (i.rotationId)
					{
						case 1:
							i.translate(20 * duration, 0, 0);
							break;
						case 2:
							i.translate(-20 * duration, 0, 0);
							break;
						case 3:
						case 4:
						case 5:
							i.translate(-20 * duration, 0, 0);
							break;
						case 12:
							i.rotate(new Vector3(24, 0, 0), globalEuler[1], -90);
							break;
						case 13:
							i.rotate(new Vector3(-24, 0, 0), globalEuler[1], -90);
							break;
					}
                }
            }
		}

		public void seq6(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				i.translate(120 * delta, 0, 0);
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
            {
				foreach (Asset3d i in objectList)
                {
					i.translate(-120 * duration, 0, 0);
                }
            }
		}

		public void seq7(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[2], -120 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq8(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			if (tempFirstRun)
            {
				foreach (Asset3d i in objectList)
                {
					switch (i.rotationId)
                    {
						case 3:
						case 4:
						case 5:
							i.scale(objectList[3].objectCenter, 2, 2, 2);
							i.rotate(objectList[3].objectCenter, globalEuler[2], -45);
							break;
                    }
                }
				tempFirstRun = false;
            }

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;	
			}
		}

		public void seq9(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			if (tempFirstRun)
			{
				foreach (Asset3d i in objectList)
				{
					switch (i.rotationId)
					{
						case 3:
						case 4:
						case 5:
							i.scale(objectList[3].objectCenter, 0.125f, 0.125f, 0.125f);
							i.rotate(objectList[3].objectCenter, globalEuler[2], 135);
							break;
					}
				}
				tempFirstRun = false;
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq10(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq11(float delta, float duration, int mode)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 2:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 6:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 7:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 8:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 9:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 10:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 11:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 12:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 13:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 14:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				foreach (Asset3d i in objectList)
				{
					switch (i.rotationId)
					{
						case 1:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 2:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 3:
						case 4:
						case 5:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 6:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 7:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 8:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 9:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 10:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 11:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 12:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 13:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 14:
							i.translate(40 * duration * mode, 0, 0);
							break;
					}
				}
			}
		}

		public void seq12(float delta, float duration, Vector3 moveVector)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				i.translate(moveVector.X * delta, moveVector.Y * delta, moveVector.Z * delta);
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], 180 * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq13(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			if (tempFirstRun)
            {
				foreach (Asset3d i in objectList)
                {
					i.translate(-10, 0, 20);
                }
				tempFirstRun = false;
            }

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 2:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 7:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 8:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 12:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 13:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 14:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -90 * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
            {
				tempFirstRun = true;
            }
		}

		public void seq14(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq15(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				i.translate(100 * delta, 0, 0);
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 180 * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -180 * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], 180 * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
            {
				foreach (Asset3d i in objectList)
                {
					i.translate(-100 * duration, 0, 0);
                }
            }
		}

		public void seq16(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 2:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 7:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 8:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 12:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -90 * delta);
						break;
					case 13:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 90 * delta);
						break;
					case 14:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -90 * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq17(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				i.translate(-100 * delta, 0, 0);
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -720 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], 720 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				foreach (Asset3d i in objectList)
				{
					i.translate(100 * duration, 0, 0);
				}
			}
		}

		public void seq18(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			if (tempFirstRun)
			{
				foreach (Asset3d i in objectList)
				{
					i.translate(-10, 0, 20);
				}
				tempFirstRun = false;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 2:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 7:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 8:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 12:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], 360 / duration * delta);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 13:
						i.rotate(new Vector3(0, 0, 48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 14:
						i.rotate(new Vector3(0, 0, -48), globalEuler[1], -360 / duration * delta);
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq19(float delta, float duration, Vector3 moveVector)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				i.translate(moveVector.X * delta, moveVector.Y * delta, moveVector.Z * delta);
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq20(float delta, float duration, int modX, int modY)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.translate(-20 * delta * modX, 20 * delta * modY, 0);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 2:
						i.translate(20 * delta * modX, 20 * delta * modY, 0);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.translate(20 * delta * modX, 0, 0);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 12:
						i.rotate(new Vector3(24, 0, 0), globalEuler[1], -90 / duration * delta);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 13:
						i.rotate(new Vector3(-24, 0, 0), globalEuler[1], -90 / duration * delta);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				foreach (Asset3d i in objectList)
				{
					switch (i.rotationId)
					{
						case 1:
							i.translate(20 * duration * modX, -20 * duration * modY, 0);
							break;
						case 2:
							i.translate(-20 * duration * modX, -20 * duration * modY, 0);
							break;
						case 3:
						case 4:
						case 5:
							i.translate(-20 * duration * modX, 0, 0);
							break;
						case 12:
							i.rotate(new Vector3(24, 0, 0), globalEuler[1], 90);
							break;
						case 13:
							i.rotate(new Vector3(-24, 0, 0), globalEuler[1], 90);
							break;
					}
				}
			}
		}

		public void seq21(float delta, float duration)
		{
			expired = false;

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.rotate(objectList[1].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 2:
						i.rotate(objectList[2].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 6:
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 7:
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 8:
						i.rotate(objectList[6].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 9:
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 10:
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 11:
						i.rotate(objectList[9].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 12:
						i.rotate(objectList[10].rotationCenter, globalEuler[1], 360 / duration * delta);
						break;
					case 13:
						i.rotate(objectList[11].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 14:
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq22(float delta, float duration, int mode)
		{
			expired = false;

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			foreach (Asset3d i in objectList)
			{
				switch (i.rotationId)
				{
					case 1:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[1].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 2:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[2].rotationCenter, globalEuler[1], -360 / duration * delta);
						break;
					case 3:
					case 4:
					case 5:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[3].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 6:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[4].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 7:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[5].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 8:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[6].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 9:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[7].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 10:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[8].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 11:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[9].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 12:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[10].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
					case 13:
						i.translate(40 * delta * mode, 0, 0);
						i.rotate(objectList[11].rotationCenter, globalEuler[1], 180 / duration * delta);
						break;
					case 14:
						i.translate(-40 * delta * mode, 0, 0);
						i.rotate(objectList[12].rotationCenter, globalEuler[1], -180 / duration * delta);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				foreach (Asset3d i in objectList)
				{
					switch (i.rotationId)
					{
						case 1:
							i.translate(40 * duration * mode, 0, 0);
							i.rotate(objectList[1].rotationCenter, globalEuler[1], 180);
							break;
						case 2:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 3:
						case 4:
						case 5:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 6:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 7:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 8:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 9:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 10:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 11:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 12:
							i.translate(40 * duration * mode, 0, 0);
							break;
						case 13:
							i.translate(-40 * duration * mode, 0, 0);
							break;
						case 14:
							i.translate(40 * duration * mode, 0, 0);
							break;
					}
				}
			}
		}
	}
}
