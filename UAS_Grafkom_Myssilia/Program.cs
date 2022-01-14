using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace UAS_Grafkom_Myssilia
{
	class Program
	{
		static void Main(string[] args)
		{
			var ourWindow = new NativeWindowSettings()
			{
				Size = new Vector2i(1280, 720),
				Title = "UAS Grafkom - Andreas, Denzel & Wilson"
			};

			using (var window = new Window(GameWindowSettings.Default, ourWindow))
			{
				window.Run();
			}
		}
	}
}
