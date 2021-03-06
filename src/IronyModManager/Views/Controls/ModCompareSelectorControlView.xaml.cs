﻿// ***********************************************************************
// Assembly         : IronyModManager
// Author           : Mario
// Created          : 03-24-2020
//
// Last Modified By : Mario
// Last Modified On : 12-14-2020
// ***********************************************************************
// <copyright file="ModCompareSelectorControlView.xaml.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using IronyModManager.Common.Views;
using IronyModManager.DI;
using IronyModManager.Shared;
using IronyModManager.Shared.Models;
using IronyModManager.ViewModels.Controls;

namespace IronyModManager.Views.Controls
{
    /// <summary>
    /// Class ModCompareSelectorControlView.
    /// Implements the <see cref="IronyModManager.Common.Views.BaseControl{IronyModManager.ViewModels.Controls.ModCompareSelectorControlViewModel}" />
    /// </summary>
    /// <seealso cref="IronyModManager.Common.Views.BaseControl{IronyModManager.ViewModels.Controls.ModCompareSelectorControlViewModel}" />
    [ExcludeFromCoverage("This should be tested via functional testing.")]
    public class ModCompareSelectorControlView : BaseControl<ModCompareSelectorControlViewModel>
    {
        #region Fields

        /// <summary>
        /// The mod compare class
        /// </summary>
        private const string ModCompareClass = "ModCompare";

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModCompareSelectorControlView" /> class.
        /// </summary>
        public ModCompareSelectorControlView()
        {
            logger = DIResolver.Get<ILogger>();
            this.InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Binds the ListBox pointer.
        /// </summary>
        /// <param name="listBox">The list box.</param>
        protected virtual void BindListBoxPointer(IronyModManager.Controls.ListBox listBox)
        {
            listBox.ContextMenuOpening += (sender, args) =>
            {
                List<MenuItem> menuItems = null;
                var hoveredItem = listBox.GetLogicalChildren().Cast<ListBoxItem>().FirstOrDefault(p => p.IsPointerOver);
                if (hoveredItem != null)
                {
                    ViewModel.SetParameters(hoveredItem.Content as IDefinition);
                    if (!string.IsNullOrWhiteSpace(ViewModel.ConflictPath))
                    {
                        menuItems = new List<MenuItem>()
                        {
                            new MenuItem()
                            {
                                Header = ViewModel.OpenFile,
                                Command = ViewModel.OpenFileCommand
                            }
                        };
                        if (!ViewModel.ConflictPath.EndsWith(Shared.Constants.ZipExtension, StringComparison.OrdinalIgnoreCase) &&
                            !ViewModel.ConflictPath.EndsWith(Shared.Constants.BinExtension, StringComparison.OrdinalIgnoreCase))
                        {
                            menuItems.Add(new MenuItem()
                            {
                                Header = ViewModel.OpenDirectory,
                                Command = ViewModel.OpenDirectoryCommand
                            });
                        }
                    }
                }
                listBox.SetContextMenu(menuItems);
            };
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        /// <param name="disposables">The disposables.</param>
        protected override void OnActivated(CompositeDisposable disposables)
        {
            static void appendClass(ListBox listBox)
            {
                var children = listBox.GetLogicalChildren().Cast<ListBoxItem>();
                foreach (var item in children)
                {
                    if (!item.Classes.Contains(ModCompareClass))
                    {
                        item.Classes.Add(ModCompareClass);
                    }
                }
            }
            var left = this.FindControl<IronyModManager.Controls.ListBox>("leftSide");
            var right = this.FindControl<IronyModManager.Controls.ListBox>("rightSide");
            LayoutUpdated += (sender, args) =>
            {
                try
                {
                    left.InvalidateArrange();
                    right.InvalidateArrange();
                    appendClass(left);
                    appendClass(right);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            };
            BindListBoxPointer(left);
            BindListBoxPointer(right);
            base.OnActivated(disposables);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #endregion Methods
    }
}
