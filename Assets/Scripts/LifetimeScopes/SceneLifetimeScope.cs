using DefaultNamespace;
using Models.Interfaces;
using ScriptableObjects;
using Services;
using Services.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace LifetimeScopes {
    public class SceneLifetimeScope : LifetimeScope {
        [SerializeField]
        private Variables variables;

        [SerializeField]
        private Transform cellsParent;
        
        protected override void Configure(IContainerBuilder builder) {
            builder.RegisterInstance(this.variables);
            builder.RegisterInstance(this.cellsParent);

            builder.Register<IGrid>(_ => new Models.Grid(this.variables.width, this.variables.height), Lifetime.Scoped);

            builder.Register<IGridSimulationService, GridSimulationService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<IPopulationService, PopulationService>(Lifetime.Scoped);
            
            builder.RegisterComponentInHierarchy<InputService>().As<IInputService>();

            builder.RegisterEntryPoint<EntryPoint>();
        } 
    }
}