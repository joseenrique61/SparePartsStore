using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class MainViewModel
{
    //private readonly IUnitOfWork _unitOfWork;i
    public ObservableCollection<string> Images { get; set; }
    //public ObservableCollection<SparePart> SpareParts { get; set; }
    public MainViewModel()
    {
        //_unitOfWork = unitOfWork;

        //SpareParts = new SparepartsViewModel(_unitOfWork).SpareParts;
        Images = new ObservableCollection<string>
        {
            "freno.jpeg",
            "motor.jpeg",
            "radiador.webp"
        };

    }
}