using SPSMobile.Data.ViewModels;
using SPSModels.Models;
using System.Collections.ObjectModel;

namespace SparePartsStoreWeb.Data.UnitOfWork
{
    public class ClientViewModel
    {
        public ObservableCollection<SparePart> SparePartsInCart { get; set; } = new ObservableCollection<SparePart>()
        {
            new() {
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
            new() {
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
        };
        public Client Client { get; set; } = new Client()
        {
            Id = 10000,
            Name = "Ariel Anchapaxi",
            City = "Quito",
            Country = "Ecuador",
            Address = "Av. NNUU & Av. Eloy Alfaro"
        };

        public User User { get; set; } = new User() 
        {
            Id = 20000,
            Email = "ariel.anchapaxi@gmail.com",
            Password = "Password"
        };
        public ICollection<Order> Orders { get; set; } = new List<Order>()
        {
            // First Order
            new()
            {
                Id = 40000,
                SparePart = new SparePart
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
                SparePartId = 1000,
                Amount=50,
            },

            // Second Order
            new()
            {
                Id = 40001,
                Amount=30,
                SparePart = new SparePart
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
                SparePartId = 1001,
            },

            // Third Order
            new()
            {
                Id = 40002,
                Amount=10,
                SparePart = new SparePart
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
                SparePartId = 1000,
            }
        };
        public PurchaseOrder PurchaseOrder = new PurchaseOrder
        {
            Id = 30000,
            PurchaseCompleted = false
        };

        public ClientViewModel() 
        { 
            // Client
            Client.User = User;
            Client.UserId = User.Id;

            // Orders
            PurchaseOrder.Client = Client;
            PurchaseOrder.ClientId = Client.Id;

            PurchaseOrder.Orders = Orders;
        }
    }
}
