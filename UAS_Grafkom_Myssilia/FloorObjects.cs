using System;
using System.Collections.Generic;
using LearnOpenTK.Common;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class FloorObjects : ShapesCollection
	{
		private float constantTimePassed = 0;

		public override Vector3 ShapeCenter => objectList[0].objectCenter;

		public FloorObjects(Camera camera) : base(camera)
        {

		}

		public override void initObjects()
		{
			var tempColor = Vector3.One;
			var floor = new Asset3d(1, 0, tempColor, tempColor, tempColor);
			floor.createCuboid(0, -41.086f, 0, 550, 2, 550, false);
			objectList.Add(floor);

			var pedestalSpaceship = new Asset3d(1, 1, tempColor, tempColor, tempColor);
			pedestalSpaceship.createCuboid(-8, -40.086f, 12, 10, 10, 7.45f, false);
			objectList.Add(pedestalSpaceship);

			var pedestalUFO = new Asset3d(1, 2, tempColor, tempColor, tempColor);
			pedestalUFO.createCylinder(8, -40.086f, 12, 5, 10, 5, 72, 24);
			objectList.Add(pedestalUFO);
		}

		public override void load()
		{
			foreach (Asset3d i in objectList)
			{
				i.load("shader", "awd");
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

		public void seq1(float delta, float duration)
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
				animationStage++;
			}

			foreach (Asset3d i in objectList)
            {
				switch (i.rotationId)
                {
					case 1:
						i.translate(0, -delta, 0);
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
			if (expired)
			{
				expired = false;
			}

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
					case 2:
						i.translate(0, -delta, 0);
						break;
				}
			}

			if (!expired)
			{
				timePassed += delta;
			}
		}
	}
}
