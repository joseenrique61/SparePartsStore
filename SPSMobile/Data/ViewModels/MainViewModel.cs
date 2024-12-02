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
    private readonly IUnitOfWork _unitOfWork;
    public ObservableCollection<string> Images { get; set; }
    public ObservableCollection<SparePart> SpareParts { get ; set; }
    public MainViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        var sparePartsViewModel = new SparepartsViewModel(_unitOfWork);
        SpareParts = sparePartsViewModel.SpareParts;
        //FillCollection();

        var imageViewModel = new ImagesViewModel();
        Images = imageViewModel.Image;
    }

    /*
     public async void FillCollection() 
    {
        SpareParts = new ObservableCollection<SparePart>(await _unitOfWork.SparePart.GetAll());
    }*/
}