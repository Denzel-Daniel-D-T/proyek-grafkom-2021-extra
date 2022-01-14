using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
    abstract class PhysicalLight : Light
    {
        public float constant;
        public float linear;
        public float quadratic;

        public PhysicalLight(Vector3 ambient, Vector3 diffuse, Vector3 specular, float constant, float linear, float quadratic) : base(ambient, diffuse, specular)
        {
            this.constant = constant;
            this.linear = linear;
            this.quadratic = quadratic;
        }

        public PhysicalLight() : base()
        {

        }
    }
}
