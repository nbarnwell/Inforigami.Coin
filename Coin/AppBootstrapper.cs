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
        private static SimpleContainer _container;

        public static SimpleContainer Container => _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            RegisterLogging();
            ConfigureCaliburn();
            RegisterDatabase();
            RegisterCommandHandlers();
            RegisterViewModels();
        }

        private void RegisterLogging()
        {
            _container.Singleton<ILogger, RegaloLoggingAdapter>();

            var logger = _container.GetInstance<ILogger>();

            Application.DispatcherUnhandledException += (sender, args) =>
            {
                logger.Error(sender, args.Exception, args.Exception.Message);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = (Exception)args.ExceptionObject;
                logger.Error(sender, exception, exception.Message);
            };

            logger.Info(this, "Logging initialised");
        }

        private void ConfigureCaliburn()
        {
            _container.Singleton<ILog, CaliburnLog>();
            LogManager.GetLog = type => _container.GetInstance<ILog>();

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
                         .Where(x =>
                             !x.IsAbstract
                             && x.Name.EndsWith("ViewModel")
                             || x.Name.EndsWith("Workspace")
                             || (x.Name.EndsWith("Screen") && x != typeof(Screen)));

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
            _container.Singleton<ICommandProcessor, CommandProcessor>("CommandProcessor");

            var commandHandlerTypes =
                GetType().Assembly
                         .GetTypes()
                         .Where(IsCommandHandler)
                         .ToList();

            foreach (var handlerType in commandHandlerTypes)
            {
                _container.RegisterPerRequest(handlerType.GetInterfaces()[0], handlerType.FullName, handlerType);
            }

            Resolver.Configure(
                type => _container.GetInstance(type, ""),
                type => _container.GetAllInstances(type),
                o => {});
        }

        private static bool IsCommandHandler(Type x)
        {
            return !x.IsAbstract && ImplementsCommandHandlerInterface(x);
        }

        private static bool ImplementsCommandHandlerInterface(Type x)
        {
            return x.GetInterfaces()
                    .Where(y => y.IsGenericType)
                    .Any(y => typeof(ICommandHandler<>).IsAssignableFrom(y.GetGenericTypeDefinition()));
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