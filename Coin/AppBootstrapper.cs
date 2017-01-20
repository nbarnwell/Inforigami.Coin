using System.Linq;
using Coin.Data;
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
        private SimpleContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            ConfigureCaliburn();
            RegisterDatabase();
            RegisterCommandHandlers();
            RegisterViewModels();
        }

        private void ConfigureCaliburn()
        {
            LogManager.GetLog = type => new CaliburnLog();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IViewModelFactory, ViewModelFactory>();

            ViewLocatorConfig.ConfigureViewLocator();
        }

        private void RegisterDatabase()
        {
            _container.PerRequest<IDatabase, Database>();
        }

        private void RegisterViewModels()
        {
            _container.Singleton<IShell, ShellViewModel>();
            _container.Singleton<WorkspaceHostViewModel>();

            var viewModelTypes =
                GetType().Assembly
                         .GetTypes()
                         .Where(x => x.Name.EndsWith("ViewModel"));
            foreach (var viewModelType in viewModelTypes)
            {
                if (!_container.HasHandler(viewModelType, null))
                {
                    _container.RegisterPerRequest(viewModelType, null, viewModelType);
                }
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
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}