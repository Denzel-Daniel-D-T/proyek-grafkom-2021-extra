using LearnOpenTK.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace UAS_Grafkom_Myssilia
{
    class LightCollection
    {
        private Vector3 globalRotationCenter = Vector3.Zero;
        private List<Vector3> globalEuler = new List<Vector3>();
        private List<DirLight> directionLightList = new List<DirLight>();
        private List<PointLight> pointLightList = new List<PointLight>();
        private List<FlashLight> flashLightList = new List<FlashLight>();
        private Camera camera;

        private bool expired = false;

        private float timePassed = 0;
        private float constantTimePassed = 0;
        private float scale = 1;
        private float linearSpeed = 5;

        public int renderSetting = 1;
        public int animationStage = 0;

        public LightCollection(Camera camera)
        {
            this.camera = camera;
            globalEuler.Add(Vector3.UnitX);
            globalEuler.Add(Vector3.UnitY);
            globalEuler.Add(Vector3.UnitZ);
        }

        public void initObjects()
        {
            //Attenuations
            var lightOne = new List<float>();
            lightOne.Add(1);
            lightOne.Add(0.14f);
            lightOne.Add(0.07f);

            var lightTwo = new List<float>();
            lightTwo.Add(1);
            lightTwo.Add(0.09f);
            lightTwo.Add(0.032f);

            var lightThree = new List<float>();
            lightThree.Add(1);
            lightThree.Add(0.07f);
            lightThree.Add(0.017f);

            var lightFour = new List<float>();
            lightFour.Add(1);
            lightFour.Add(0.007f);
            lightFour.Add(0.0002f);

            var lightFive = new List<float>();
            lightFive.Add(1);
            lightFive.Add(0.0035f);
            lightFive.Add(0.00005f);

            //Dir Lights
            //0
            var starColor = new Vector3(0.1f, 0.1f, 0.225f);
            var dir1 = new DirLight(starColor * 0.2f, starColor * 0.5f, starColor * 0.5f, new Vector3(-2, -4, -2));
            directionLightList.Add(dir1);

            //Point Lights
            //0
            var oct1 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightTwo[0], lightTwo[1], lightTwo[2], 1, 1);
            oct1.createOctahedron(0, -24, 16, scale * 2, scale * 2, scale * 2);
            pointLightList.Add(oct1);

            //1
            var oct2 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightTwo[0], lightTwo[1], lightTwo[2], 1, 2);
            oct2.createOctahedron(-32, -22, 16, scale * 2.5f, scale * 2.5f, scale * 2.5f);
            pointLightList.Add(oct2);

            //2
            var oct3 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightTwo[0], lightTwo[1], lightTwo[2], 1, 3);
            oct3.createOctahedron(-12, -25, 28, scale * 1.25f, scale * 1.25f, scale * 1.25f);
            pointLightList.Add(oct3);

            //3
            var oct4 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightTwo[0], lightTwo[1], lightTwo[2], 1, 4);
            oct4.createOctahedron(24, -23, 24, scale * 3.25f, scale * 3.25f, scale * 3.25f);
            pointLightList.Add(oct4);

            // ========================================

            //4 5 6
            var angle = 240f;
            for (int i = 0; i < 3; i++)
            {
                var octRevolve = new PointLight(Vector3.One, Vector3.One, Vector3.One, lightThree[0], lightThree[1], lightThree[2], 1, 5);
                octRevolve.createOctahedron(MathF.Cos(MathHelper.DegreesToRadians(angle * i + 40)) * 130, 20 * i + 10, MathF.Sin(MathHelper.DegreesToRadians(angle * i + 30)) * 130, scale * 10, scale * 10, scale * 10);
                pointLightList.Add(octRevolve);
            }

            //7 8 9
            angle = 95f;
            for (int i = 0; i < 4; i++)
            {
                var octRevolve = new PointLight(Vector3.One, Vector3.One, Vector3.One, lightThree[0], lightThree[1], lightThree[2], 1, 5);
                octRevolve.createOctahedron(MathF.Cos(MathHelper.DegreesToRadians(angle * i + 10)) * 130, 35 * i - 22, MathF.Sin(MathHelper.DegreesToRadians(angle * i + 10)) * 130, scale * 10, scale * 10, scale * 10);
                pointLightList.Add(octRevolve);
            }

            // ========== Spaceship's Pedestal ==========

            //10
            var cube1 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightOne[0], lightOne[1], lightOne[2], 1, 6);
            cube1.createCuboid(-8 + 5 - 0.25f, -40.086f + 5 + 0.25f, 12 + 3.72f - 0.25f, 0.5f, 0.5f, 0.5f);
            pointLightList.Add(cube1);

            //11
            var cube2 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightOne[0], lightOne[1], lightOne[2], 1, 6);
            cube2.createCuboid(-8 - 5 + 0.25f, -40.086f + 5 + 0.25f, 12 + 3.72f - 0.25f, 0.5f, 0.5f, 0.5f);
            pointLightList.Add(cube2);

            //12
            var cube3 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightOne[0], lightOne[1], lightOne[2], 1, 6);
            cube3.createCuboid(-8 + 5 - 0.25f, -40.086f + 5 + 0.25f, 12 - 3.72f + 0.25f, 0.5f, 0.5f, 0.5f);
            pointLightList.Add(cube3);

            //13
            var cube4 = new PointLight(Vector3.One * 0.2f, Vector3.One, Vector3.One * 0.5f, lightOne[0], lightOne[1], lightOne[2], 1, 6);
            cube4.createCuboid(-8 - 5 + 0.25f, -40.086f + 5 + 0.25f, 12 - 3.72f + 0.25f, 0.5f, 0.5f, 0.5f);
            pointLightList.Add(cube4);

            // =========================================

            //Flash Lights
            //0
            var aa = Vector3.One;
            var fl1 = new FlashLight(aa * 0.2f, aa, aa * 0.5f, lightFour[0], lightFour[1], lightFour[2], new Vector3(0, 0, 0), 12.5f, 17.5f, 1, 1);
            fl1.createTetrahedron(0, -40.086f + (MathF.Sqrt(3 / 8.0f) / 3.0f * scale * 2), 8, scale * 2);
            flashLightList.Add(fl1);

            //1
            var bb = Vector3.One;
            var fl2 = new FlashLight(bb * 0.2f, bb, bb * 0.5f, lightFour[0], lightFour[1], lightFour[2], new Vector3(-100, -20, 0), 22.5f, 25.5f, 1, 3);
            fl2.createTetrahedron(-80, -39, 0, scale * 5);
            flashLightList.Add(fl2);

            //2
            var cc = Vector3.One;
            var fl3 = new FlashLight(cc * 0.2f, cc, cc * 0.5f, lightFour[0], lightFour[1], lightFour[2], new Vector3(100, -20, 0), 22.5f, 25.5f, 1, 4);
            fl3.createTetrahedron(80, -39, 0, scale * 5);
            flashLightList.Add(fl3);

            //3
            var dd = Vector3.UnitX;
            var fl4 = new FlashLight(dd * 0.2f, dd, dd * 0.5f, lightFour[0], lightFour[1], lightFour[2], new Vector3(0, -20, -100), 12.5f, 17.5f, 1, 1);
            fl4.createTetrahedron(-20, -39, -80, scale * 5);
            flashLightList.Add(fl4);

            //4
            var ee = Vector3.UnitY;
            var fl5 = new FlashLight(ee * 0.2f, ee, ee * 0.5f, lightFour[0], lightFour[1], lightFour[2], new Vector3(0, -20, -100), 12.5f, 17.5f, 1, 5);
            fl5.createTetrahedron(0, -39, -80, scale * 5);
            flashLightList.Add(fl5);

            //5
            var ff = Vector3.UnitZ;
            var fl6 = new FlashLight(ff * 0.2f, ff, ff * 0.5f, lightFour[0], lightFour[1], lightFour[2], new Vector3(0, -20, -100), 12.5f, 17.5f, 1, 2);
            fl6.createTetrahedron(20, -39, -80, scale * 5);
            flashLightList.Add(fl6);

            // ============= UFO's Pedestal =============

            //6 7 8
            for (int i = 0; i < 3; i++)
            {
                var gg = Vector3.One;
                var fl789 = new FlashLight(gg * 0.2f, gg, gg * 0.5f, lightTwo[0], lightTwo[1], lightTwo[2], new Vector3(8, -32.711f, 12), 32.5f, 45.5f, 1, 6);
                fl789.createTetrahedron(8 + MathF.Cos(MathHelper.DegreesToRadians(120 * i + 30)) * 4.45f, -35.086f + (MathF.Sqrt(3 / 8.0f) / 3.0f * scale), 12 + MathF.Sin(MathHelper.DegreesToRadians(120 * i + 30)) * 4.45f, scale);
                flashLightList.Add(fl789);
            }
        }

        public void load()
        {
            foreach (PointLight i in pointLightList)
            {
                i.load();
            }

            foreach (FlashLight i in flashLightList)
            {
                i.load();
            }
        }

        public void render()
        {
            foreach (PointLight i in pointLightList)
            {
                i.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            }

            foreach (FlashLight i in flashLightList)
            {
                i.render(renderSetting, camera.GetViewMatrix(), camera.GetProjectionMatrix());
            }
        }

        public List<DirLight> DirLightList
        {
            get
            {
                return directionLightList;
            }
        }

        public List<PointLight> PointLightList
        {
            get
            {
                return pointLightList;
            }
        }

        public List<FlashLight> FlashLightList
        {
            get
            {
                return flashLightList;
            }
        }

        public void constantOn(float delta)
        {
            if (animationStage != 0)
            {
                var j = -1;
                var k = 0;
                foreach (PointLight i in pointLightList)
                {
                    var coord = i.objectCenter - new Vector3(0, i.objectCenter.Y, 0);
                    var radius = MathF.Sqrt(coord.X * coord.X + coord.Y * coord.Y + coord.Z * coord.Z);

                    switch (i.rotationId)
                    {
                        case 1:
                            i.rotate(globalRotationCenter, globalEuler[1], delta * MathHelper.RadiansToDegrees(linearSpeed / radius));
                            i.rotate(i.rotationCenter, i._euler[1], MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 90)) * delta * 90);
                            i.rotate(i.rotationCenter, i._euler[0], delta * 90);
                            i.translate(0, MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 90)) * delta * 4, 0);
                            break;
                        case 2:
                            i.rotate(globalRotationCenter, globalEuler[1], delta * MathHelper.RadiansToDegrees(linearSpeed / radius));
                            i.rotate(i.rotationCenter, i._euler[1], MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 60)) * delta * 90);
                            i.rotate(i.rotationCenter, i._euler[0], delta * 90);
                            i.translate(0, MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 90 + 30)) * delta * 4, 0);
                            break;
                        case 3:
                            i.rotate(globalRotationCenter, globalEuler[1], delta * MathHelper.RadiansToDegrees(linearSpeed / radius));
                            i.rotate(i.rotationCenter, i._euler[1], MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 120)) * delta * 90);
                            i.rotate(i.rotationCenter, i._euler[0], delta * 90);
                            i.translate(0, MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 90 + 45)) * delta * 4, 0);
                            break;
                        case 4:
                            i.rotate(globalRotationCenter, globalEuler[1], delta * MathHelper.RadiansToDegrees(linearSpeed / radius));
                            i.rotate(i.rotationCenter, i._euler[1], MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 120)) * delta * 90);
                            i.rotate(i.rotationCenter, i._euler[0], delta * 90);
                            i.translate(0, MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 90 + 90)) * delta * 4, 0);
                            break;
                        case 5:
                            i.translate(0, MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 90 / 8.0f - k)) * delta * 2 * j, 0);
                            j *= -1;
                            k += 45;
                            break;
                    }
                }

                foreach (FlashLight i in flashLightList)
                {
                    switch (i.rotationId)
                    {
                        case 1:
                            i.moveDestination(new Vector3(MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 45)) / 2.0f, 0, 0));
                            i.rotate(i.objectCenter, i._euler[1], 30 * delta);
                            break;
                        case 2:
                            i.moveDestination(new Vector3(-MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 45)) / 2.0f, 0, 0));
                            i.rotate(i.objectCenter, i._euler[1], 37.5f * delta);
                            break;
                        case 3:
                            i.moveDestination(new Vector3(0, 0, MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 45)) / 2.0f));
                            i.rotate(i.objectCenter, i._euler[1], 45 * delta);
                            break;
                        case 4:
                            i.moveDestination(new Vector3(0, 0, -MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 45)) / 2.0f));
                            i.rotate(i.objectCenter, i._euler[1], 52.5f * delta);
                            break;
                        case 5:
                            i.moveDestination(new Vector3(-MathF.Cos(MathHelper.DegreesToRadians(constantTimePassed * 45)) / 2.0f, MathF.Sin(MathHelper.DegreesToRadians(constantTimePassed * 45)) / 2.0f, 0));
                            i.rotate(i.objectCenter, i._euler[1], 60 * delta);
                            break;
                    }
                }
                constantTimePassed += delta;
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

            foreach (PointLight i in pointLightList)
            {
                switch (i.rotationId)
                {
                    case 6:
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
            expired = false;

            if (duration - timePassed < delta)
            {
                delta = duration - timePassed;
                timePassed = 0;
                expired = true;
                animationStage++;
            }

            foreach (FlashLight i in flashLightList)
            {
                switch (i.rotationId)
                {
                    case 6:
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
