using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
    abstract class Light
    {
        public Vector3 ambient = Vector3.Zero;
        public Vector3 diffuse = Vector3.Zero;
        public Vector3 specular = Vector3.Zero;

        public Light(Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
        }

        public Light()
        {

        }
    }
}
