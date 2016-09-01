﻿using Microsoft.Practices.Unity;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismUnityDemo.Contracts;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace PrismUnityDemo.ViewModel
{
    public class ProductDetailViewModel : BindableBase, INavigationAware, IActiveAware, IProductDetailViewModel
    {
        public ProductDetailViewModel()
        {
            this.PrintCommand = new DelegateCommand(
                () => Debug.WriteLine($"Printing {this.Product.ProductName}"));
            this.CloseCommand = new DelegateCommand(
                () => this.eventAggregator.GetEvent<CloseProductDetailEvent>().Publish(this));
        }

        private IEventAggregator eventAggregator { get; set; }

        private GlobalCommands globalCommands { get; set; }
        public DelegateCommand PrintCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        private Product ProductValue;
        public Product Product
        {
            get { return this.ProductValue; }
            set { this.SetProperty(ref this.ProductValue, value); }
        }

        public IRegionManager SubRegionManager { get; private set; }

        private Repository repository { get; set; }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (this.Product != null)
            {
                var productNumber = Int32.Parse(navigationContext.Parameters["ProductNumber"].ToString());
                return this.Product.ProductNumber == productNumber;
            }

            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var productNumber = Int32.Parse(navigationContext.Parameters["ProductNumber"].ToString());
            this.Product = this.repository
                .SelectAllProducts()
                .Where(p => p.ProductNumber == productNumber)
                .FirstOrDefault();
        }

        private bool IsActiveValue;
        public bool IsActive
        {
            get
            {
                return this.IsActiveValue;
            }

            set
            {
                this.SetProperty(ref this.IsActiveValue, value);
                this.IsActiveChanged?.Invoke(this, new EventArgs());
                this.PrintCommand.IsActive = this.CloseCommand.IsActive = this.IsActive;
            }
        }

        public event EventHandler IsActiveChanged;

        [InjectionMethod]
        public void OnInitialization(IEventAggregator eventAggregator, GlobalCommands globalCommands,
            Repository repository)
        {
            this.eventAggregator = eventAggregator;
            this.globalCommands = globalCommands;
            this.repository = repository;

            this.globalCommands.Print.RegisterCommand(this.PrintCommand);
            this.globalCommands.PrintAll.RegisterCommand(this.PrintCommand);
            this.globalCommands.Close.RegisterCommand(this.CloseCommand);
        }
    }
}
