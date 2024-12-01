using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSMobile.Data.ViewModels
{
    internal class ImagesViewModel
    {
        public ObservableCollection<String> Image { get; set; }
        public ImagesViewModel() 
        {
            Image = new ObservableCollection<string>
            {
                "freno.jpeg",
                "motor.jpeg",
                "radiador.webp"
            };
        }
    }
}
