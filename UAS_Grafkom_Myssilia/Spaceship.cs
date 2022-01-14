using System;
using System.Collections.Generic;
using LearnOpenTK.Common;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class Spaceship : ShapesCollection
	{
		public override Vector3 ShapeCenter => objectList[0].objectCenter;

		private Vector3 tempPivot = Vector3.Zero;

		private float legRotated = 0;
		private float rollRotated = 0;
		private float scale = 2.0f;
		private float acceleration = 1;

		private const float rotateLimiter = 0.25f;
		private const float legAngleLimiter = 0.25f;

		public Spaceship(Camera camera) : base(camera)
        {

		}

		public override void initObjects()
		{
			var tempColor = new List<Vector3>();
			tempColor.Add(Vector3.Zero);
			tempColor.Add(new Vector3(0.7f, 0.7f, 0.8f));
			tempColor.Add(new Vector3(1, 1, 1));
			tempColor.Add(new Vector3(1, 0.6f, 0));
			tempColor.Add(new Vector3(1, 0, 0));
			tempColor.Add(new Vector3(0.1f, 0.1f, 0.1f));
			tempColor.Add(new Vector3(0, 0.5f, 0));
			tempColor.Add(new Vector3(0.3f, 0, 0.4f));

			//0
			Asset3d cylinder1 = new Asset3d(1, 0, tempColor[1], tempColor[1], tempColor[1]);
			cylinder1.createCylinder(0, 0, 0, 0.75f * scale, 2 * scale, 0.75f * scale, 72, 24);
			cylinder1.rotate(cylinder1.objectCenter, cylinder1._euler[0], 90);
			objectList.Add(cylinder1);

			//1
			Asset3d setBolaFront = new Asset3d(2, 0, tempColor[2], tempColor[2], tempColor[2]);
			setBolaFront.createFractionedEllipsoid(0, 0, scale, 0.75f * scale, 0.75f * scale, 0.75f * scale, 72, 24, 0.5f, 0.225f);
			setBolaFront.rotate(setBolaFront.objectCenter, setBolaFront._euler[0], 90);
			objectList.Add(setBolaFront);

			//2
			Asset3d setBolaFrontKaca = new Asset3d(3, 0, tempColor[3], tempColor[3], tempColor[3]);
			setBolaFrontKaca.createFractionedEllipsoid(0, 0, scale, 0.75f * scale, 0.75f * scale, 0.75f * scale, 72, 24, 0.225f);
			setBolaFrontKaca.rotate(setBolaFrontKaca.objectCenter, setBolaFrontKaca._euler[0], 90);
			objectList.Add(setBolaFrontKaca);

			//3
			Asset3d setBolaBack = new Asset3d(2, 0, tempColor[2], tempColor[2], tempColor[2]);
			setBolaBack.createFractionedEllipsoid(0, 0, -scale, 0.75f * scale, 0.75f * scale, 0.75f * scale, 72, 24, 0.5f);
			setBolaBack.rotate(setBolaBack.objectCenter, setBolaBack._euler[0], -90);
			objectList.Add(setBolaBack);

			//4
			Asset3d silinder = new Asset3d(5, 3, tempColor[5], tempColor[5], tempColor[5]);
			silinder.createCylinder(0, -0.734f * scale, 0, 0.35f * scale, 0.15f * scale, 0.35f * scale, 72, 24);
			silinder.rotate(silinder.objectCenter, silinder._euler[2], -180);
			objectList.Add(silinder);

			for (int i = 0; i < 4; i++)
			{
				float posX, posZ;

				switch (i)
				{
					case 0:
						posX = posZ = 1;
						break;
					case 1:
						posX = -1;
						posZ = 1;
						break;
					case 2:
						posX = posZ = -1;
						break;
					case 3:
						posX = 1;
						posZ = -1;
						break;
					default:
						posX = posZ = 1;
						break;
				}

				//5 7 9 11
				Asset3d balokk = new Asset3d(7, 1, tempColor[7], tempColor[7], tempColor[7]);

				//6 8 10 12
				Asset3d kerucutt = new Asset3d(4, 1, tempColor[4], tempColor[4], tempColor[4]);

				balokk.createCuboid(1.1f * scale * posX, 0, 0.75f * scale * posZ, 1.6f * scale, 0.2f * scale, 0.2f * scale, false);
				balokk.rotate(globalRotationCenter, globalEuler[2], -135 * posX);
				objectList.Add(balokk);

				kerucutt.createEllipticCone(1.5f * scale * posX, silinder.objectCenter.Y - silinder.objectDimension.Y / 2.0f + 0.275f * scale - 0.734f * scale, 0.75f * scale * posZ, 0.55f * scale, 0.55f * scale, 0.55f * scale, 72, 24);
				objectList.Add(kerucutt);
			}

			for (int i = 0; i < 2; i++)
			{
				//13 18
				Asset3d setBolaFrontSide1 = new Asset3d(2, 0, tempColor[2], tempColor[2], tempColor[2]);
				setBolaFrontSide1.createFractionedEllipsoid(1.75f * scale, 0, 0.5f * scale, 0.75f * scale, 0.75f * scale, 0.75f * scale, 72, 24, 0.5f, 0.2f);
				setBolaFrontSide1.rotate(setBolaFrontSide1.objectCenter, setBolaFrontSide1._euler[0], 90);
				setBolaFrontSide1.rotate(cylinder1.objectCenter, globalEuler[2], 180 * i);
				objectList.Add(setBolaFrontSide1);

				//14 19
				Asset3d setBolaFrontSide2 = new Asset3d(3, 0, tempColor[3], tempColor[3], tempColor[3]);
				setBolaFrontSide2.createFractionedEllipsoid(1.75f * scale, 0, 0.5f * scale, 0.75f * scale, 0.75f * scale, 0.75f * scale, 72, 24, 0.2f);
				setBolaFrontSide2.rotate(setBolaFrontSide2.objectCenter, setBolaFrontSide2._euler[0], 90);
				setBolaFrontSide2.rotate(cylinder1.objectCenter, globalEuler[2], 180 * i);
				objectList.Add(setBolaFrontSide2);

				//15 20
				Asset3d cylinder1Side = new Asset3d(1, 0, tempColor[1], tempColor[1], tempColor[1]);
				cylinder1Side.createCylinder(1.75f * scale, 0, 0, 0.75f * scale, scale, 0.75f * scale, 72, 24);
				cylinder1Side.rotate(cylinder1Side.objectCenter, cylinder1Side._euler[0], 90);
				cylinder1Side.rotate(cylinder1.objectCenter, globalEuler[2], 180 * i);
				objectList.Add(cylinder1Side);

				//16 21
				Asset3d setBolaBackSide = new Asset3d(2, 0, tempColor[2], tempColor[2], tempColor[2]);
				setBolaBackSide.createFractionedEllipsoid(1.75f * scale, 0, -0.5f * scale, 0.75f * scale, 0.75f * scale, 0.75f * scale, 72, 24, 0.5f);
				setBolaBackSide.rotate(setBolaBackSide.objectCenter, setBolaBackSide._euler[0], -90);
				setBolaBackSide.rotate(cylinder1.objectCenter, globalEuler[2], 180 * i);
				objectList.Add(setBolaBackSide);

				//17 22
				Asset3d corridorSide = new Asset3d(6, 0, tempColor[6], tempColor[6], tempColor[6]);
				corridorSide.createCuboid(0.875f * scale, 0, 0, 0.75f * scale, 0.75f * scale, 0.75f * scale, false);
				corridorSide.rotate(cylinder1.objectCenter, globalEuler[2], 180 * i);
				objectList.Add(corridorSide);
			}

			//23
			Asset3d weaponBox = new Asset3d(5, 0, tempColor[5], tempColor[5], tempColor[5]);
			weaponBox.createCuboid(0, 0.9f * scale, 0, 0.5f * scale, 0.5f * scale, 0.5f * scale, false);
			objectList.Add(weaponBox);

			//24
			Asset3d weaponBarrel = new Asset3d(3, 0, tempColor[3], tempColor[3], tempColor[3]);
			weaponBarrel.createCylinder(0, 0.95f * scale, 0.5f * scale, 0.1f * scale, scale, 0.1f * scale, 36, 24);
			weaponBarrel.rotate(weaponBarrel.objectCenter, globalEuler[0], 90);
			objectList.Add(weaponBarrel);

			for (int i = 0; i < 6; i++)
			{
				//25 26 27 28 29 30
				Asset3d weaponNozzle = new Asset3d(4, 2, tempColor[4], tempColor[4], tempColor[4]);
				weaponNozzle.createCylinder(0, 1.005f * scale, 0.75f * scale, 0.025f * scale, scale, 0.025f * scale, 36, 24);
				weaponNozzle.rotate(weaponNozzle.objectCenter, globalEuler[0], 90);
				weaponNozzle.rotate(weaponBarrel.objectCenter, globalEuler[2], 60 * i);
				objectList.Add(weaponNozzle);
			}

			for (int i = 0; i < 4; i++)
			{
				//31 32 33 34
				Asset3d elevatorStrut = new Asset3d(7, 3, tempColor[7], tempColor[7], tempColor[7]);
				elevatorStrut.createCylinder(0.2f * scale, -0.3f * scale, 0.2f * scale, 0.02f * scale, 0.75f * scale, 0.02f * scale, 36, 24);
				elevatorStrut.rotate(silinder.objectCenter, globalEuler[1], i * 90);
				objectList.Add(elevatorStrut);
			}

			//35
			Asset3d boosterLeft = new Asset3d(4, 0, tempColor[4], tempColor[4], tempColor[4]);
			boosterLeft.createEllipticParaboloid(1.75f * scale, 0, -1.25f * scale, 0.75f, 0.75f, 0.75f, 72, 24, true);
			boosterLeft.rotate(boosterLeft.objectCenter, globalEuler[0], -90);
			boosterLeft.scaleLocal(1, 1, 0.5f);
			objectList.Add(boosterLeft);

			//36
			Asset3d boosterLeftInner = new Asset3d(4, 0, tempColor[4], tempColor[4], tempColor[4]);
			boosterLeftInner.createEllipticParaboloid(1.75f * scale, 0, -1.25f * scale, 0.75f, 0.75f, 0.75f, 72, 24, false);
			boosterLeftInner.rotate(boosterLeft.objectCenter, globalEuler[0], -90);
			boosterLeftInner.translate(0, 0, boosterLeft.objectDimension.Y);
			objectList.Add(boosterLeftInner);

			//37
			Asset3d boosterMiddle = new Asset3d(4, 0, tempColor[4], tempColor[4], tempColor[4]);
			boosterMiddle.createEllipticParaboloid(0, 0, -1.75f * scale, 0.75f, 0.75f, 0.75f, 72, 24, true);
			boosterMiddle.rotate(boosterMiddle.objectCenter, globalEuler[0], -90);
			boosterMiddle.scaleLocal(1, 1, 0.5f);
			objectList.Add(boosterMiddle);

			//38
			Asset3d boosterMiddleInner = new Asset3d(4, 0, tempColor[4], tempColor[4], tempColor[4]);
			boosterMiddleInner.createEllipticParaboloid(0, 0, -1.75f * scale, 0.75f, 0.75f, 0.75f, 72, 24, false);
			boosterMiddleInner.rotate(boosterMiddle.objectCenter, globalEuler[0], -90);
			boosterMiddleInner.translate(0, 0, boosterMiddle.objectDimension.Y);
			objectList.Add(boosterMiddleInner);

			//39
			Asset3d boosterRight = new Asset3d(4, 0, tempColor[4], tempColor[4], tempColor[4]);
			boosterRight.createEllipticParaboloid(-1.75f * scale, 0, -1.25f * scale, 0.75f, 0.75f, 0.75f, 72, 24, true);
			boosterRight.rotate(boosterRight.objectCenter, globalEuler[0], -90);
			boosterRight.scaleLocal(1, 1, 0.5f);
			objectList.Add(boosterRight);

			//40
			Asset3d boosterRightInner = new Asset3d(4, 0, tempColor[4], tempColor[4], tempColor[4]);
			boosterRightInner.createEllipticParaboloid(-1.75f * scale, 0, -1.25f * scale, 0.75f, 0.75f, 0.75f, 72, 24, false);
			boosterRightInner.rotate(boosterRight.objectCenter, globalEuler[0], -90);
			boosterRightInner.translate(0, 0, boosterRight.objectDimension.Y);
			objectList.Add(boosterRightInner);

			foreach (Asset3d i in objectList)
			{
				i.translate(-8, -32, 12);
				i.resetEuler();
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
				i.translate(objectList[0]._euler[eulerId].X * velocity * delta, objectList[0]._euler[eulerId].Y * velocity * delta, objectList[0]._euler[eulerId].Z * velocity * delta);
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
					i.rotate(pivot, objectList[0]._euler[eulerId], delta * angularVelocity);
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

		public void toggleElevator(float delta, float openLength, bool open)
		{
			foreach (Asset3d i in objectList)
			{
				if (i.rotationId == 3)
				{
					if (open)
					{
						i.translate(-i._euler[1].X * delta / openLength * 0.7313f * scale, -i._euler[1].Y * delta / openLength * 0.7313f * scale, -i._euler[1].Z * delta / openLength * 0.7313f * scale);
					}
					else
					{
						i.translate(i._euler[1].X * delta / openLength * 0.7313f * scale, i._euler[1].Y * delta / openLength * 0.7313f * scale, i._euler[1].Z * delta / openLength * 0.7313f * scale);
					}
				}
			}
		}

		public void activateWeapon(float delta, float angularVelocity)
        {
			foreach (Asset3d i in objectList)
            {
				if (i.rotationId == 2)
                {
					i.rotate(objectList[24].objectCenter, i._euler[2], angularVelocity * delta);
                }
            }
        }

		public void rotateLegsBack(float delta, float angularVelocity)
		{
			var legCount = 0;

			if (legRotated < legAngleLimiter)
			{
				foreach (Asset3d i in objectList)
				{
					if (i.rotationId == 1)
					{
						i.rotate(objectList[0].objectCenter, i._euler[0], delta * angularVelocity);
						legCount++;
						if (legCount == 8)
						{
							legRotated += delta;
						}
					}
				}
			}

			if (legRotated > legAngleLimiter)
			{
				foreach (Asset3d i in objectList)
				{
					if (i.rotationId == 1)
					{
						i.rotate(objectList[0].objectCenter, globalEuler[0], (legAngleLimiter - legRotated) * angularVelocity);
						legCount++;
						if (legCount == 16)
						{
							legRotated = legAngleLimiter;
						}
					}
				}
			}
		}

		public void rotateLegsFront(float delta, float angularVelocity)
		{
			var legCount = 0;

			if (legRotated > 0)
			{
				foreach (Asset3d i in objectList)
				{
					if (i.rotationId == 1)
					{
						i.rotate(objectList[0].objectCenter, i._euler[0], -delta * angularVelocity);
						legCount++;
						if (legCount == 8)
						{
							legRotated -= delta;
						}
					}
				}
			}

			if (legRotated < 0)
			{
				foreach (Asset3d i in objectList)
				{
					if (i.rotationId == 1)
					{
						i.rotate(objectList[0].objectCenter, globalEuler[0], -legRotated * angularVelocity);
						legCount++;
						if (legCount == 16)
						{
							legRotated = 0;
						}
					}
				}
			}
		}

		public void rollStart(float delta, float angularVelocity)
        {
			if (rollRotated < rotateLimiter)
			{
				foreach (Asset3d i in objectList)
				{
					i.rotate(objectList[0].objectCenter, objectList[0]._euler[2], delta * angularVelocity);
				}
				rollRotated += delta;
			}

			if (rollRotated > rotateLimiter)
            {
				foreach (Asset3d i in objectList)
				{
					i.rotate(objectList[0].objectCenter, objectList[0]._euler[2], (rotateLimiter - rollRotated) * angularVelocity);
				}
				rollRotated = rotateLimiter;
			}
        }

		public void rollEnd(float delta, float angularVelocity)
		{
			if (rollRotated > 0)
			{
				foreach (Asset3d i in objectList)
				{
					i.rotate(objectList[0].objectCenter, objectList[0]._euler[2], -delta * angularVelocity);
				}
				rollRotated -= delta;
			}

			if (rollRotated < 0)
			{
				foreach (Asset3d i in objectList)
				{
					i.rotate(objectList[0].objectCenter, objectList[0]._euler[2], -rollRotated * angularVelocity);
				}
				rollRotated = 0;
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

		public void seq1(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			switch ((int)timePassed / 8 % 2)
			{
				case 0:
					toggleElevator(delta, 8, true);
					break;
				case 1:
					toggleElevator(delta, 8, false);
					break;
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

			moveStraight(delta, 2, 2);
			rotateLegsBack(delta, 90.0f / 2.0f);

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

			if (tempFirstRun)
			{
				tempPivot = new Vector3(objectList[0].objectCenter.X + Vector3.UnitX.X * -16 * objectList[0]._euler[0].X, objectList[0].objectCenter.Y, objectList[0].objectCenter.Z);
				tempFirstRun = false;
			}

			rollStart(delta, 90.0f / 2.0f);
			rotateAroundPoint(delta, tempPivot, 1, -90.0f / 8.0f, true);
			activateWeapon(delta, -90.0f * 4);

			if (!expired)
			{
				timePassed += delta;
			}
			else
            {
				tempFirstRun = true;
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

			switch ((int)(timePassed * 3) % 5)
            {
				case 0:
					acceleration += 0.6f;
					break;
				case 1:
				case 2:
					acceleration += 0.9f;
					break;
				case 3:
					break;
			}

			rotateAroundPoint(delta, tempPivot, 1, -90.0f / 8.0f * acceleration, true);

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

			moveStraight(delta, 2, 2);
			rollEnd(delta, 90.0f / 2.0f);

			if (!expired)
			{
				timePassed += delta;
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

			if (tempFirstRun)
			{
				tempPivot = new Vector3(objectList[0].objectCenter.X + Vector3.UnitX.X * 32 * objectList[0]._euler[0].X, objectList[0].objectCenter.Y, objectList[0].objectCenter.Z + Vector3.UnitZ.Z * 32 * objectList[0]._euler[0].Z);
				tempFirstRun = false;
			}

			rollStart(delta, -90.0f / 2.0f);
			rotateAroundPoint(delta, tempPivot, 1, 90.0f * 2, true);
			activateWeapon(delta, -90.0f * 4);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
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
				tempPivot = new Vector3(objectList[0].objectCenter.X + Vector3.UnitX.X * -16 * objectList[0]._euler[0].X, objectList[0].objectCenter.Y, objectList[0].objectCenter.Z + Vector3.UnitZ.Z * -16 * objectList[0]._euler[0].Z);
				rollRotated = 0;
				tempFirstRun = false;
			}

			rollStart(delta, 90.0f);
			rotateAroundPoint(delta, tempPivot, 1, -90.0f * 2, true);
			activateWeapon(delta, -90.0f * 4);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
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
				var radd = 64.0f;
				tempPivot = new Vector3(objectList[0].objectCenter.X + Vector3.UnitX.X * radd * objectList[0]._euler[0].X, objectList[0].objectCenter.Y, objectList[0].objectCenter.Z + Vector3.UnitZ.Z * radd * objectList[0]._euler[0].Z);
				tempFirstRun = false;
			}

			rollEnd(delta, 90.0f / 2.0f);
			rotateAroundPoint(delta, tempPivot, 1, 3.75f * 90.0f / 64.0f, true);

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
				tempPivot = new Vector3(objectList[0].objectCenter);
				tempPivot.Y += 16;
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivot, 0, -360.0f / 32.0f);

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

			rotateAroundPoint(delta, tempPivot, 0, -360.0f / 16.0f);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq11(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			rotateAroundPoint(delta, tempPivot, 0, 3.0f * -360.0f / 32.0f);

			if (!expired)
			{
				timePassed += delta;
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

			rotateAroundPoint(delta, tempPivot, 0, 3.0f * -360.0f / 16.0f);

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
				var radd = 64.0f;
				tempPivot = new Vector3(objectList[0].objectCenter.X + Vector3.UnitX.X * radd * objectList[0]._euler[0].X, objectList[0].objectCenter.Y, objectList[0].objectCenter.Z + Vector3.UnitZ.Z * radd * objectList[0]._euler[0].Z);
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivot, 1, 360.0f / 32.0f, true);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq14(float delta, float duration, float radius)
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
				tempPivot = new Vector3(objectList[0].objectCenter.X + Vector3.UnitX.X * radius * objectList[0]._euler[0].X, objectList[0].objectCenter.Y, objectList[0].objectCenter.Z + Vector3.UnitZ.Z * radius * objectList[0]._euler[0].Z);
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivot, 1, radius / MathF.Abs(radius) * 360.0f / duration, true);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq15(float delta, float duration, float radius)
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
				tempPivot = new Vector3(objectList[0].objectCenter);
				tempPivot.Y += radius;
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivot, 0, radius / MathF.Abs(radius) * -360.0f / duration);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq16(float delta, float duration, float radius, int mode)
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
				tempPivot = new Vector3(objectList[0].objectCenter.X + Vector3.UnitX.X * radius * objectList[0]._euler[0].X, objectList[0].objectCenter.Y, objectList[0].objectCenter.Z + Vector3.UnitZ.Z * radius * objectList[0]._euler[0].Z);
				tempFirstRun = false;
			}

			rotateAroundPoint(delta, tempPivot, 1, radius / MathF.Abs(radius) * mode * 90.0f / duration, true);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				tempFirstRun = true;
			}
		}

		public void seq17(float delta, float duration, bool reverseScale = false, int count = 0)
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
					i.scale(objectList[0].objectCenter, 1.25f, 1.25f, 1.25f);
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
				if (reverseScale)
				{
					foreach (Asset3d i in objectList)
					{
						i.scale(objectList[0].objectCenter, 1 / MathF.Pow(1.25f, count), 1 / MathF.Pow(1.25f, count), 1 / MathF.Pow(1.25f, count));
					}
				}
			}
		}

		public void seq19(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			moveStraight(delta, 2, 10);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				moveStraight(duration, 2, -10);
			}
		}

		public void seq20(float delta, float duration, float dist)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			moveStraight(delta, 1, dist);

			if (!expired)
			{
				timePassed += delta;
			}
			else
			{
				moveStraight(duration, 1, -dist);
			}
		}

		public void seq21(float delta, float duration, float dist)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			moveStraight(delta, 2, dist / duration);

			if (!expired)
			{
				timePassed += delta;
			}
		}

		public void seq22(float delta, float duration)
		{
			expired = false;

			

			if (duration - timePassed < delta)
			{
				delta = duration - timePassed;
				timePassed = 0;
				expired = true;
				animationStage++;
			}

			rotateLegsFront(delta, 90.0f / 2.0f);
			switch ((int)timePassed / 8 % 2)
			{
				case 0:
					toggleElevator(delta, 8, true);
					break;
				case 1:
					toggleElevator(delta, 8, false);
					break;
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}
	}
}
