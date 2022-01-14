using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
    class DirLight : Light
    {
        public Vector3 direction;

        public DirLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3 direction) : base(ambient, diffuse, specular)
        {
            this.direction = Vector3.Normalize(direction);
        }
    }
}
