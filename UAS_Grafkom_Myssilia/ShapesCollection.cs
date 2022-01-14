using LearnOpenTK.Common;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace UAS_Grafkom_Myssilia
{
    abstract class ShapesCollection
    {
		protected Vector3 globalRotationCenter = Vector3.Zero;
		protected List<Vector3> globalEuler = new List<Vector3>();
		protected List<Asset3d> objectList = new List<Asset3d>();
		protected Camera camera;

		protected bool tempFirstRun = true;
		protected bool expired = true;

		protected float timePassed = 0;

		public int renderSetting = 1;
		public int animationStage = 0;
		public int isBlinn = 1;

		public abstract Vector3 ShapeCenter
        {
			get;
        }

		public ShapesCollection(Camera camera)
		{
			this.camera = camera;
			globalEuler.Add(Vector3.UnitX);
			globalEuler.Add(Vector3.UnitY);
			globalEuler.Add(Vector3.UnitZ);
		}

		public abstract void initObjects();
		public abstract void load();
		public abstract void render(List<DirLight> dirLightList, List<PointLight> pointLightList, List<FlashLight> flashLightList);
	}
}
