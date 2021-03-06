﻿// ***********************************************************************
// Assembly         : IronyModManager.DI
// Author           : Mario
// Created          : 06-10-2020
//
// Last Modified By : Mario
// Last Modified On : 06-11-2020
// ***********************************************************************
// <copyright file="MessageBus.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IronyModManager.Shared.MessageBus;

namespace IronyModManager.DI.MessageBus
{
    /// <summary>
    /// Class MessageBus.
    /// Implements the <see cref="IronyModManager.Shared.MessageBus.IMessageBus" />
    /// </summary>
    /// <seealso cref="IronyModManager.Shared.MessageBus.IMessageBus" />
    internal class MessageBus : IMessageBus
    {
        #region Fields

        /// <summary>
        /// The message bus
        /// </summary>
        private readonly SlimMessageBus.IMessageBus messageBus;

        /// <summary>
        /// The registered types
        /// </summary>
        private readonly HashSet<Type> registeredTypes;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBus" /> class.
        /// </summary>
        /// <param name="messageBus">The message bus.</param>
        /// <param name="registeredTypes">The registered types.</param>
        public MessageBus(SlimMessageBus.IMessageBus messageBus, HashSet<Type> registeredTypes)
        {
            this.messageBus = messageBus;
            // For crying out loud this is something that SlimMessageBus should handle internally
            this.registeredTypes = registeredTypes;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            messageBus?.Dispose();
        }

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the t message.</typeparam>
        /// <param name="message">The message.</param>
        public void Publish<TMessage>(TMessage message) where TMessage : IMessageBusEvent
        {
            PublishAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Publishes the asynchronous.
        /// </summary>
        /// <typeparam name="TMessage">The type of the t message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        public Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessageBusEvent
        {
            if (!registeredTypes.Contains(typeof(TMessage)))
            {
                return Task.FromResult(false);
            }
            if (!message.IsFireAndForget)
            {
                return PublishAwaitableAsync(message);
            }
            return PublishFireAndForgetAsync(message);
        }

        /// <summary>
        /// Publishes the awaitable asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        private Task PublishAwaitableAsync<T>(T message) where T : IMessageBusEvent
        {
            return messageBus.Publish(message);
        }

        /// <summary>
        /// publish fire and forget as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        private async Task PublishFireAndForgetAsync<T>(T message) where T : IMessageBusEvent
        {
            await Task.Factory.StartNew(async () =>
            {
                await messageBus.Publish(message);
            });
        }

        #endregion Methods
    }
}
