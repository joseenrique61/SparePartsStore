using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SPSMobile.Data.ViewModels;
public class SparePartsViewModel
{
    public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>
    {
        "freno.jpeg",
        "motor.jpeg",
        "radiador.webp"
    };
    public ObservableCollection<SparePart> SpareParts { get; set; }/* = new ObservableCollection<SparePart>
    {
        new SparePart 
        {
            Id=1000,
            Name="Pastillas",
            Description="No son medicinales jaja",
            Stock=200,
            Image="pastillas_freno.jpg",
            Price=150.0,
            CategoryId=2,
            Category = new Category
            {
                Id = 2,
                Name = "Frenos",
            }
        },
        new SparePart 
        {
            Id=1001,
            Name="Balancines",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=200,
            Image="balancines_motor.jpg",
            Price=250.0,
            CategoryId=1,
            Category = new Category
            {
                Id = 1,
                Name = "Motor",
            }
        },
        new SparePart
        {
            Id=1002,
            Name="Bomba de aceite",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=500,
            Image="bomba_aceite_motor.jpg",
            Price=200.0,
            CategoryId=1,
            Category = new Category
            {
                Id = 1,
                Name = "Motor",
            }
        },
        new SparePart
        {
            Id=1003,
            Name="Cigueñal",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=350,
            Image="ciguenal_motor.jpg",
            Price=50.0,
            CategoryId=1,
            Category = new Category
            {
                Id = 1,
                Name = "Motor",
            }
        },
        new SparePart
        {
            Id=1004,
            Name="Bomba de agua",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=500,
            Image="bomba_agua_refrigeracion.jpg",
            Price=30.0,
            CategoryId=3,
            Category = new Category
            {
                Id = 3,
                Name = "Refrigeracion",
            }
        },
        new SparePart
        {
            Id=1,
            Name="Árbol de levas",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=125,
            Image="arbol_levas_motor.jpg",
            Price=40.0,
            CategoryId=1,
            Category = new Category
            {
                Id = 1,
                Name = "Motor",
            }
        },
        new SparePart
        {
            Id=1005,
            Name="Discos de freno",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=500,
            Image="disco_freno.jpg",
            Price=15.0,
            CategoryId=2,
            Category = new Category
            {
                Id = 2,
                Name = "Frenos",
            }

        },
        new SparePart
        {
            Id=1006,
            Name="Zapatas",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=60,
            Image="zapatas_freno.jpg",
            Price=25.0,
            CategoryId=2,
            Category = new Category
            {
                Id = 2,
                Name = "Frenos",
            }
        },
        new SparePart
        {
            Id=1007,
            Name="Termostato",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=20,
            Image="termostato_refrigeracion.jpg",
            Price=150.0,
            CategoryId=3,
            Category = new Category
            {
                Id = 3,
                Name = "Refrigeracion",
            }
        },
        new SparePart
        {
            Id=1008,
            Name="Radiador",
            Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Facile est hoc cernere in primis puerorum aetatulis. Cur, nisi quod turpis oratio est?",
            Stock=100,
            Image="radiador_refrigeracion.jpg",
            Price=100.0,
            CategoryId=3,
            Category = new Category
            {
                Id = 3,
                Name = "Refrigeracion",
            }
        }
    };*/
    public ObservableCollection<Category> Categories { get; set; }/* = new ObservableCollection<Category>
    {
        new Category { Id = 0, Name = "All" },
        new Category { Id = 1, Name = "Motor" },
        new Category { Id = 2, Name = "Frenos" },
        new Category { Id = 3, Name = "Refrigeracion" }
    };*/
    public ObservableCollection<SparePart> FilteredCategories { get; set; }

}