using SPSMobile.Data.UnitOfWork;
using SPSMobile.Data.ViewModels;
using SPSModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSMobile.Data.ViewModels;
public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<string> Images { get; set; } = new ObservableCollection<string> {"freno.jpeg","motor.jpeg","radiador.webp"};
    public ObservableCollection<SparePart> SpareParts { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<Category> FilteredCategories { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add
        {
            ((INotifyPropertyChanged)SpareParts).PropertyChanged += value;
        }

        remove
        {
            ((INotifyPropertyChanged)SpareParts).PropertyChanged -= value;
        }
    }
}