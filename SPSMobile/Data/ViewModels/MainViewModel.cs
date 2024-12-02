using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSMobile.Data.ViewModels;
public class MainViewModel
{
    public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string>
    {
        "freno.jpeg",
            "motor.jpeg",
            "radiador.webp"
    };

    public ObservableCollection<SparePart> SpareParts { get; set; }
}