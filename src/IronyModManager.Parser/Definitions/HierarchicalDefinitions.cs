﻿// ***********************************************************************
// Assembly         : IronyModManager.Parser
// Author           : Mario
// Created          : 03-23-2020
//
// Last Modified By : Mario
// Last Modified On : 03-24-2020
// ***********************************************************************
// <copyright file="HierarchicalDefinitions.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodexMicroORM.Core.Collections;
using IronyModManager.Parser.Common.Definitions;

namespace IronyModManager.Parser.Definitions
{
    /// <summary>
    /// Class HierarchicalDefinitions.
    /// Implements the <see cref="IronyModManager.Parser.Common.Definitions.IHierarchicalDefinitions" />
    /// </summary>
    /// <seealso cref="IronyModManager.Parser.Common.Definitions.IHierarchicalDefinitions" />
    public class HierarchicalDefinitions : IHierarchicalDefinitions
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalDefinitions" /> class.
        /// </summary>
        public HierarchicalDefinitions()
        {
            Children = new ConcurrentIndexedList<IHierarchicalDefinitions>(nameof(IHierarchicalDefinitions.Name));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public ICollection<IHierarchicalDefinitions> Children { get; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="unwrap">if set to <c>true</c> [unwrap].</param>
        /// <returns>System.Object.</returns>
        public object GetValue(string propName, bool unwrap)
        {
            return propName switch
            {
                nameof(Key) => Key,
                nameof(Children) => Children,
                _ => Name,
            };
        }

        #endregion Methods
    }
}
