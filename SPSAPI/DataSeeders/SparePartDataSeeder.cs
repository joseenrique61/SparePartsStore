using SPSModels.Models;
using SPSAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace SPSAPI.DataSeeders
{
	public class SparePartDataSeeder : ISparePartDataSeeder
	{
		private readonly IServiceProvider _serviceProvider;

		public SparePartDataSeeder(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task Initialize()
		{
			using var context = new ApplicationDBContext(_serviceProvider.GetRequiredService<DbContextOptions<ApplicationDBContext>>());
			try
			{
				if (context.SparePart.Any())
				{
					return;
				}

				// Code for spare part seeding (not yet defined default elements)
				List<SparePart>? spareParts = context.SparePart.ToList();
				spareParts ??= [];
				if (spareParts.Count != 0)
				{
					return;
				}

				spareParts =
				[
					new SparePart
					{
						Name="Pastillas",
						Description="No son medicinales jaja",
						Stock=200,
						Image="pastillas_freno.jpg",
						Price=150.0,
						CategoryId=1,
					},
					new SparePart
					{
						Name="Balancines",
						Description="Lorem ipsum dolor sit amet",
						Stock=200,
						Image="balancines_motor.jpg",
						Price=250.0,
						CategoryId=1,
					},
					new SparePart
					{
						Name="Bomba de aceite",
						Description="Lorem ipsum dolor sit amet",
						Stock=500,
						Image="bomba_aceite_motor.jpg",
						Price=200.0,
						CategoryId=1,
					},
					new SparePart
					{
						Name="Cigueñal",
						Description="Lorem ipsum dolor sit amet",
						Stock=350,
						Image="ciguenal_motor.jpg",
						Price=50.0,
						CategoryId=1,
					},
					new SparePart
					{
						Name="Bomba de agua",
						Description="Lorem ipsum dolor sit amet",
						Stock=500,
						Image="bomba_agua_refrigeracion.jpg",
						Price=30.0,
						CategoryId=9,
					},
					new SparePart
					{
						Name="Árbol de levas",
						Description="Lorem ipsum dolor sit amet",
						Stock=125,
						Image="arbol_levas_motor.jpg",
						Price=40.0,
						CategoryId=1,
					},
					new SparePart
					{
						Name="Discos de freno",
						Description="Lorem ipsum dolor sit amet",
						Stock=500,
						Image="disco_freno.jpg",
						Price=15.0,
						CategoryId=1,
					},
					new SparePart
					{
						Name="Zapatas",
						Description="Lorem ipsum dolor sit amet",
						Stock=60,
						Image="zapatas_freno.jpg",
						Price=25.0,
						CategoryId=1,
					},
					new SparePart
					{
						Name="Termostato",
						Description="Lorem ipsum dolor sit amet",
						Stock=20,
						Image="termostato_refrigeracion.jpg",
						Price=150.0,
						CategoryId=9,
					},
					new SparePart
					{
						Name="Radiador",
						Description="Lorem ipsum dolor sit amet",
						Stock=100,
						Image="radiador_refrigeracion.jpg",
						Price=100.0,
						CategoryId=9,
					}
				];

				foreach (SparePart sparePart in spareParts)
				{
					context.SparePart.Add(sparePart);
				}
				await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}
}
