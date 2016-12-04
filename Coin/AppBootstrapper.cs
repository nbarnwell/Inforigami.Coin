using System.Linq;
using Coin.Infrastructure;
using Coin.Shell;
using Inforigami.Regalo.Core;

namespace Coin
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;

    public class AppBootstrapper : BootstrapperBase
    {
        SimpleContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IViewModelFactory, ViewModelFactory>();

            RegisterCommandHandlers();
            RegisterViewModels();
        }

        private void RegisterViewModels()
        {
            _container.PerRequest<IShell, ShellViewModel>();

            var viewModelTypes =
                GetType().Assembly
                         .GetTypes()
                         .Where(x => x.Name.EndsWith("ViewModel"))
                         .Where(x => x != typeof(ShellViewModel));
            foreach (var viewModelType in viewModelTypes)
            {
                _container.RegisterPerRequest(viewModelType, viewModelType.FullName, viewModelType);
            }
        }

        private void RegisterCommandHandlers()
        {
            _container.Singleton<ICommandProcessor, CommandProcessor>();

            var commandHandlerTypes =
                GetType().Assembly
                         .GetTypes()
                         .Where(x => typeof(ICommandHandler<>).IsAssignableFrom(x));
            foreach (var handlerType in commandHandlerTypes)
            {
                _container.RegisterPerRequest(handlerType.GetInterfaces()[0], handlerType.FullName, handlerType);
            }
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}