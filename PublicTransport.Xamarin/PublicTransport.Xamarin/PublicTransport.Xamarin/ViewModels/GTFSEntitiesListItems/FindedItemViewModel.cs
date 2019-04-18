using GTFS.Entities;
using PublicTransport.Xamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PublicTransport.Xamarin.ViewModels.GTFSEntitiesListItems
{
    public abstract class FindedItemViewModel : BaseViewModel
    {
        private GTFSEntity _entity;
        protected GTFSEntity Entity
        {
            get
            {
                return _entity;
            }
            set
            {
                _entity = value;
            }
        }

        public abstract ImageSource ImagePath { get; }
        public abstract string Title { get; }

        public FindedItemViewModel(GTFSEntity entity)
        {
            _entity = entity;
        }

        public abstract ICommand OpenDetailsCommand
        {
            get;
        }
    }
}
