﻿// ***********************************************************************
// Assembly         : IronyModManager.Localization
// Author           : Mario
// Created          : 01-18-2020
//
// Last Modified By : Mario
// Last Modified On : 02-04-2020
// ***********************************************************************
// <copyright file="DIPackage.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using IronyModManager.Localization.Attributes.Handlers;
using IronyModManager.Shared;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace IronyModManager.Localization
{
    /// <summary>
    /// Class DIPackage.
    /// Implements the <see cref="SimpleInjector.Packaging.IPackage" />
    /// </summary>
    /// <seealso cref="SimpleInjector.Packaging.IPackage" />
    [ExcludeFromCoverage("Should not test external DI.")]
    internal class DIPackage : IPackage
    {
        #region Methods

        /// <summary>
        /// Registers the set of services in the specified <paramref name="container" />.
        /// </summary>
        /// <param name="container">The container the set of services is registered into.</param>
        public void RegisterServices(Container container)
        {
            container.Register<ILocalizationManager, LocalizationManager>();
            container.Collection.Register(typeof(ILocalizationAttributeHandler), typeof(ILocalizationAttributeHandler).Assembly);
            container.Collection.Register(typeof(ILocalizationRefreshHandler), typeof(ILocalizationRefreshHandler).Assembly);
        }

        #endregion Methods
    }
}
