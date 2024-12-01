using SPSMobile.Data.UnitOfWork;
using SPSModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPSMobile.Data.ViewModels
{
    class SparepartsViewModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public ObservableCollection<SparePart> SpareParts { get; set; }
        public SparepartsViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            LoadModel();
        }

        public async void LoadModel ()
        {
            var _spareParts = await _unitOfWork.SparePart.GetAll();
            if (_spareParts != null) 
            {
                SpareParts = new ObservableCollection<SparePart>(_spareParts.ToArray());
            } 
        }
    }
}
