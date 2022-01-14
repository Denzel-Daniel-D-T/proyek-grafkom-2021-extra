using System;
using System.Collections.Generic;
using LearnOpenTK.Common;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class UFO : ShapesCollection
	{
		public override Vector3 ShapeCenter => objectList[3].objectCenter;

		private List<Vector3> tempPivots = new List<Vector3>();

		private float legDistance = 0;
		private float scale = 0.5f;
		private float legDistanceLimiter = 2.0f;
		private float acceleration = 1.0f;
		private float tempScale = 3.0f;

		public int extendState = -1;

		public UFO(Camera camera) : base(camera)
        {

		}

		public override void initObjects()
		{
			var tempColor = new List<Vector3>();
			tempColor.Add(Vector3.Zero);
			tempColor.Add(new Vector3(0.7f, 0.8f, 1));
			tempColor.Add(new Vector3(0, 0.2f, 0.4f));
			tempColor.Add(new Vector3(0.3f, 0, 0.4f));
			tempColor.Add(new Vector3(0.5f, 0, 0.1f));
			tempColor.Add(new Vector3(0.4f, 0.2f, 0));
			tempColor.Add(new Vector3(1, 0.5f, 1));
			tempColor.Add(new Vector3(0, 0.7f, 1));
			tempColor.Add(new Vector3(0, 0.2f, 0.4f));

			//0
			Asset3d dome = new Asset3d(1, 0, tempColor[1], tempColor[1], tempColor[1]);
			dome.createFractionedEllipsoid(0, 0.5f * scale, 0, 4 * scale, 4 * scale, 4 * scale, 72, 24, 0.5f);
			objectList.Add(dome);

			//1
			Asset3d circlee = new Asset3d(6, 0, tempColor[6], tempColor[6], tempColor[6]);
			circlee.createCircle(0, scale / 2.0f, 0, 5 * scale, 5 * scale, 72);
			objectList.Add(circlee);

			//2
			Asset3d bodyTop1 = new Asset3d(2, 0, tempColor[2], tempColor[2], tempColor[2]);
			bodyTop1.createVariedCylinder(0, 3 * scale / 8.0f, 0, scale / 4, 5 * scale, 5.75f * scale, 72, 24, false, false);
			objectList.Add(bodyTop1);

			//3
			Asset3d bodyTop2 = new Asset3d(3, 0, tempColor[3], tempColor[3], tempColor[3]);
			bodyTop2.createVariedCylinder(0, 0, 0, scale / 2, 5.75f * scale, 7.25f * scale, 72, 24, false, false);
			objectList.Add(bodyTop2);

			//4
			Asset3d bodyTop3 = new Asset3d(2, 0, tempColor[2], tempColor[2], tempColor[2]);
			bodyTop3.createVariedCylinder(0, -3 * scale / 8.0f, 0, scale / 4, 7.25f * scale, 8 * scale, 72, 24, false, true);
			objectList.Add(bodyTop3);

			//5
			Asset3d bodyBottom = new Asset3d(3, 0, tempColor[3], tempColor[3], tempColor[3]);
			bodyBottom.createVariedCylinder(0, -1.25f * scale, 0, 1.5f * scale, 6 * scale, 3 * scale, 72, 24, false, true);
			objectList.Add(bodyBottom);

			float j = 12;

			//6 - 17
			for (int i = 0; i < j; i++)
			{
				float angle = 360 / j;

				Asset3d upperLights = new Asset3d(4, 1, tempColor[4], tempColor[4], tempColor[4]);

				upperLights.createEllipsoid(6.5f * scale, 0, 0, 0.4f * scale, 0.4f * scale, 0.4f * scale, 36, 12);
				upperLights.rotate(globalRotationCenter, globalEuler[1], -angle * i);

				objectList.Add(upperLights);
			}

			j = 16;
			//18 - 33
			for (int i = 0; i < j; i++)
			{
				float angle = 360 / j;

				Asset3d lowerLights = new Asset3d(7, 2, tempColor[7], tempColor[7], tempColor[7]);

				lowerLights.createEllipsoid(7 * scale, -0.5f * scale, 0, 0.2f * scale, 0.2f * scale, 0.2f * scale, 36, 12);
				lowerLights.rotate(globalRotationCenter, globalEuler[1], -angle * i);

				objectList.Add(lowerLights);
			}

			j = 3;

			//34 - 39
			for (int i = 0; i < j; i++)
			{
				float angle = 360 / j;

				Asset3d legBar = new Asset3d(8, 3, tempColor[8], tempColor[8], tempColor[8]);

				legBar.createCuboid(0, 3 * scale, 0, 0.4f * scale, 4 * scale, 0.4f * scale, false);
				legBar.rotate(legBar.rotationCenter, globalEuler[2], 45);
				legBar.translate(5.5f * scale, -5.5f * scale, 0);

				Asset3d legBottom = new Asset3d(5, 4, tempColor[5], tempColor[5], tempColor[5]);

				legBottom.createEllipticCone(legBar.objectCenter.X, legBar.objectCenter.Y, 0, scale, 1.5f * scale, scale, 36, 12);
				legBottom.translate(1.5f * scale, -1.5f * scale, 0);

				legBar.rotate(globalRotationCenter, globalEuler[1], -angle * i);
				legBottom.rotate(globalRotationCenter, globalEuler[1], -angle * i);

				objectList.Add(legBar);
				objectList.Add(legBottom);
			}

			//40
			Asset3d nullCenter = new Asset3d(1, 0, tempColor[1], tempColor[1], tempColor[1]);
            nullCenter.objectCenter = new Vector3(bodyTop2.objectCenter);
            nullCenter.objectCenter.Y += dome.objectDimension.Y * 0.375f;
            objectList.Add(nullCenter);

			foreach (Asset3d i in objectList)
			{
				i.translate(8, -32.711f, 12);
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

		public void moveStraightGlobal(float delta, Vector3 velocityVector)
		{
			foreach (Asset3d i in objectList)
			{
				i.translate(velocityVector.X * delta, velocityVector.Y * delta, velocityVector.Z * delta);
			}
		}

		public void moveStraight(float delta, int eulerId, float velocity)
		{
			foreach (Asset3d i in objectList)
			{
				i.translate(objectList[3]._euler[eulerId].X * velocity * delta, objectList[3]._euler[eulerId].Y * velocity * delta, objectList[3]._euler[eulerId].Z * velocity * delta);
			}
		}

		public void rotateAroundPoint(float delta, Vector3 pivot, int eulerId, float angularVelocity, bool isGlobal = false)
		{
			foreach (Asset3d i in objectList)
			{
				if (isGlobal)
				{
					i.rotate(pivot, globalEuler[eulerId], delta * angularVelocity);
				}
				else
				{
					i.rotate(pivot, objectList[3]._euler[eulerId], delta * angularVelocity);
				}
			}
		}

		public void rotateAroundObjectCenter(float delta, Asset3d selectedObject, int eulerId, float angularVelocity)
		{
			foreach (Asset3d i in objectList)
			{
				i.rotate(selectedObject.objectCenter, selectedObject._euler[eulerId], delta * angularVelocity);
			}
		}

		public void rotateTopLamp(float delta, float angularVelocity)
        {
			foreach (Asset3d i in objectList)
            {
				if (i.rotationId == 1)
                {
					i.rotate(objectList[3].objectCenter, objectList[3]._euler[1], delta * angularVelocity);
                }
            }
        }

		public void rotateBottomLamp(float delta, float angularVelocity)
		{
			foreach (Asset3d i in objectList)
			{
				if (i.rotationId == 2)
				{
					i.rotate(objectList[3].objectCenter, objectList[3]._euler[1], delta * angularVelocity);
				}
			}
		}

		public void moveLeg(float delta)
        {
			switch (extendState)
            {
				case 1:
					moveLegIn(delta);
					break;
				case -1:
					moveLegOut(delta);
					break;
			}
        }

		public void moveLegIn(float delta)
		{
			var legCount = 0;

			if (legDistance < legDistanceLimiter)
			{
				for (int i = 0; i < objectList.Count; i++)
				{
					if (objectList[i].rotationId == 3 || objectList[i].rotationId == 4)
					{
						var tempPoint = new Vector3(objectList[3].objectCenter.X, objectList[40].objectCenter.Y, objectList[3].objectCenter.Z);
						var tempVelocity = Vector3.Normalize(tempPoint - objectList[i + 4 - objectList[i].rotationId].objectCenter);
						

						objectList[i].translate(delta * tempVelocity.X * scale * 3.0f, delta * tempVelocity.Y * scale * 3.0f, delta * tempVelocity.Z * scale * 3.0f);
						legCount++;
						if (legCount == 6)
						{
							legDistance += delta;
						}
					}
				}
			}

			if (legDistance > legDistanceLimiter)
			{
				for (int i = 0; i < objectList.Count; i++)
				{
					if (objectList[i].rotationId == 3 || objectList[i].rotationId == 4)
					{
						var tempPoint = new Vector3(objectList[3].objectCenter.X, objectList[40].objectCenter.Y, objectList[3].objectCenter.Z);
						var tempVelocity = Vector3.Normalize(tempPoint - objectList[i + 4 - objectList[i].rotationId].objectCenter);

						objectList[i].translate((legDistanceLimiter - legDistance) * tempVelocity.X * scale * 3.0f, (legDistanceLimiter - legDistance) * tempVelocity.Y * scale * 3.0f, (legDistanceLimiter - legDistance) * tempVelocity.Z * scale * 3.0f);
						legCount++;
						if (legCount == 12)
						{
							legDistance = legDistanceLimiter;
						}
					}
				}
			}
		}

		public void moveLegOut(float delta)
		{
			var legCount = 0;

			if (legDistance > 0)
			{
				for (int i = 0; i < objectList.Count; i++)
				{
					if (objectList[i].rotationId == 3 || objectList[i].rotationId == 4)
					{
						var tempPoint = new Vector3(objectList[3].objectCenter.X, objectList[40].objectCenter.Y, objectList[3].objectCenter.Z);
						var tempVelocity = Vector3.Normalize(tempPoint - objectList[i + 4 - objectList[i].rotationId].objectCenter);

						objectList[i].translate(-delta * tempVelocity.X * scale * 3.0f, -delta * tempVelocity.Y * scale * 3.0f, -delta * tempVelocity.Z * scale * 3.0f);
						legCount++;
						if (legCount == 6)
						{
							legDistance -= delta;
						}
					}
				}
			}

			if (legDistance < 0)
			{
				for (int i = 0; i < objectList.Count; i++)
				{
					if (objectList[i].rotationId == 3 || objectList[i].rotationId == 4)
					{
						var tempPoint = new Vector3(objectList[3].objectCenter.X, objectList[40].objectCenter.Y, objectList[3].objectCenter.Z);
						var tempVelocity = Vector3.Normalize(tempPoint - objectList[i + 4 - objectList[i].rotationId].objectCenter);

						objectList[i].translate(-legDistance * tempVelocity.X * scale * 3.0f, -legDistance * tempVelocity.Y * scale * 3.0f, -legDistance * tempVelocity.Z * scale * 3.0f);
						legCount++;
						if (legCount == 12)
						{
							legDistance = 0;
						}
					}
				}
			}
		}

		public void scaleGlobal(Vector3 centerPosition, Vector3 scaleVector)
		{
			foreach (Asset3d i in objectList)
            {
				i.scale(centerPosition, scaleVector.X, scaleVector.Y, scaleVector.Z);
			}
			scale *= scaleVector.X;
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
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			moveStraightGlobal(delta, new Vector3(0, MathF.Sin(MathHelper.DegreesToRadians(timePassed * 45.0f)) * 4.0f, 0));

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void constantOn(float delta)
        {
			

			moveLeg(delta);

			if (animationStage != 0)
            {
				rotateTopLamp(delta, 90.0f / 4.0f * acceleration);
				rotateBottomLamp(delta, -90.0f / 4.0f * acceleration);
			}
		}

		public void seq1(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			if (tempFirstRun)
            {
				extendState *= -1;
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

			var x_ = -MathF.Sin(MathHelper.DegreesToRadians(timePassed * 45)) * 4;
			var z_ = 1.5f;

			moveStraightGlobal(delta, new Vector3(x_, 0, z_));

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

			switch ((int)(timePassed * 3) % 4)
			{
				case 0:
					moveStraightGlobal(delta, new Vector3(0, 40, 0));
					break;
				case 1:
					moveStraightGlobal(delta, new Vector3(-60, 0, 0));
					break;
				case 2:
					moveStraightGlobal(delta, new Vector3(0, -40, 0));
					break;
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

			var x_ = 1.5f;
			var z_ = -MathF.Sin(MathHelper.DegreesToRadians(timePassed * 45)) * 4;

			moveStraightGlobal(delta, new Vector3(x_, 0, z_));

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

			if (tempFirstRun)
            {
				var tempPivot = new Vector3(objectList[3].objectCenter.X + Vector3.UnitX.X * 8, objectList[3].objectCenter.Y, objectList[3].objectCenter.Z);
				tempPivots.Add(tempPivot);
				tempFirstRun = false;
            }

			if (!expired)
			{
				switch ((int)timePassed % 2)
				{
					case 0:
						rotateAroundPoint(delta, tempPivots[0], 1, 270.0f, true);
						moveStraightGlobal(delta, new Vector3(0, 10, 0));
						break;
					case 1:
						moveStraightGlobal(delta, new Vector3(0, -10, 0));
						break;
				}
				timePassed += delta;
			}
			else
            {
				extendState *= -1;
				tempFirstRun = true;
				tempPivots.Clear();
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

			moveStraightGlobal(delta, new Vector3(-0.05f, 0, -0.025f));

			if (!expired)
			{
				timePassed += delta;
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

			if (tempFirstRun)
            {
				extendState *= -1;
				tempFirstRun = false;
            }

			moveStraightGlobal(delta, new Vector3(0, 0.5f, 0));
			if (timePassed < duration / 4.0f)
			{
				scaleGlobal(objectList[3].objectCenter, new Vector3(1.0025f, 1.0025f, 1.0025f));
			}

			if (!expired)
			{
				acceleration += 0.2f * delta;
				timePassed += delta;
			}
			else
            {
				tempFirstRun = true;
            }
		}

		public void seq8(float delta, float duration, int mode)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			rotateAroundPoint(delta, new Vector3(0, 0, -9), 1, 270.0f / 4.0f * mode, true);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq9(float delta, float duration, int mode)
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
				var tempPivot1 = new Vector3(objectList[3].objectCenter.X + 1 * tempScale, objectList[3].objectCenter.Y, objectList[3].objectCenter.Z + 2 * tempScale);
				var tempPivot2 = new Vector3(objectList[3].objectCenter.X + 3 * tempScale, objectList[3].objectCenter.Y, objectList[3].objectCenter.Z + 6 * tempScale);
				tempPivots.Add(tempPivot1);
				tempPivots.Add(tempPivot2);
				tempFirstRun = false;
            }

			switch ((int)(timePassed * 2) % 2)
            {
				case 0:
					if (mode == 1)
					{
						rotateAroundPoint(delta, tempPivots[0], 1, 360.0f * mode, true);
					}
                    else
					{
						rotateAroundPoint(delta, tempPivots[1], 1, -360.0f * mode, true);
					}
					break;
				case 1:
					if (mode == 1)
					{
						rotateAroundPoint(delta, tempPivots[1], 1, -360.0f * mode, true);
					}
					else
					{
						rotateAroundPoint(delta, tempPivots[0], 1, 360.0f * mode, true);
					}
					break;
            }


			if (!expired)
			{
				timePassed += delta;
			}
			else
            {
				if (mode == -1)
                {
					tempFirstRun = true;
					tempPivots.Clear();
				}
            }
		}

		public void seq10(float delta, float duration, Vector3 destination, int mode)
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
				moveStraightGlobal(1, new Vector3(destination - objectList[3].objectCenter));
				tempFirstRun = false;
            }

			if (!expired)
			{
				moveStraightGlobal(delta, new Vector3(50 * mode, 0, 0));
				timePassed += delta;
			}
			else
            {
				tempFirstRun = true;
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

			if (tempFirstRun)
			{
				var tempPivot1 = new Vector3(objectList[3].objectCenter);
				tempPivot1.Z += 8;
				tempPivots.Add(tempPivot1);
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivots[0], 1, 1 / duration * 360.0f * mode);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
				tempPivots.Clear();
			}
		}

		public void seq12(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			moveStraightGlobal(delta, new Vector3(0, 15, 0));

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

			moveStraightGlobal(delta, new Vector3(-10, 0, 0));

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq14(float delta, float duration, Vector3 destination, int mode)
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
				moveStraightGlobal(1, new Vector3(destination - objectList[3].objectCenter));
				tempFirstRun = false;
			}

			if (!expired)
			{
				moveStraightGlobal(delta, new Vector3(30 * mode, -30, 0));
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq15(float delta, float duration, Vector3 destination)
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
				moveStraightGlobal(1, new Vector3(destination - objectList[3].objectCenter));
				tempFirstRun = false;
			}

			if (!expired)
			{
				moveStraightGlobal(delta, new Vector3(0, 30, 0));
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq16(float delta, float duration, int mode)
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
				var tempPivot1 = new Vector3(objectList[3].objectCenter);
				tempPivot1.Z -= 16;
				tempPivots.Add(tempPivot1);
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivots[0], 1, 1 / duration * 360.0f * mode);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
				tempPivots.Clear();
			}
		}

		public void seq17(float delta, float duration, int mode)
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
				if (mode == 1)
                {
					acceleration = 1.5f;
					var tempPivot1 = new Vector3(objectList[3].objectCenter);
					tempPivot1.Z -= 16;
					tempPivots.Add(tempPivot1);
				}
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivots[0], 1, 1 / duration * 180.0f * mode);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				if (mode == -1)
                {
					acceleration = 1;
					extendState *= -1;
					tempPivots.Clear();
				}
				tempFirstRun = true;
			}
		}
	}
}
