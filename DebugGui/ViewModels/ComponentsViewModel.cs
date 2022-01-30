using Avalonia.Threading;
using DebugFramework.DataTypes.SubTypes;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace DebugGui.ViewModels
{
    public class ComponentsViewModel : ViewModelBase
    {
        public int ComponentCount => Components.Count;
        public ReadOnlyObservableCollection<ComponentViewModel> Components => components;

        private readonly ReadOnlyObservableCollection<ComponentViewModel> components;

        private readonly SourceList<ComponentViewModel> allComponents;

        public ComponentViewModel SelectedComponentView
        {
            get => selectedComponentView;
            set => this.RaiseAndSetIfChanged(ref selectedComponentView, value);
        }
        private ComponentViewModel selectedComponentView;

        public ICommand CollapseAllCommand { get; }
        public ICommand ExpandAllCommand { get; }


        public ComponentsViewModel(List<ComponentData> components)
        {
            allComponents = new SourceList<ComponentViewModel>();
            allComponents.Connect()
                  .Sort(SortExpressionComparer<ComponentViewModel>.Ascending(viewModel => viewModel.ComponentName))
                  .ObserveOn(AvaloniaScheduler.Instance)
                  .Bind(out this.components)
                  .Subscribe();

            foreach (ComponentViewModel viewModel in components.Select(component => new ComponentViewModel(component)))
            {
                allComponents.Add(viewModel);
            }

            this.WhenAnyValue(x => x.SelectedComponentView)
                .Subscribe(view =>
                {
                    if (view == null)
                    {
                        return;
                    }
                    view.Selected = !view.Selected;
                });

            CollapseAllCommand = ReactiveCommand.Create(() =>
            {
                foreach (ComponentViewModel component in Components)
                {
                    component.Selected = false;
                }
            });


            ExpandAllCommand = ReactiveCommand.Create(() =>
            {
                foreach (ComponentViewModel component in Components)
                {
                    component.Selected = true;
                }
            });
        }

        public void UpdateComponents(IEnumerable<ComponentData> components)
        {
            IEnumerable<ComponentData> newComponents = components.Where(component => !Components.Any(cComponent => cComponent.ComponentName == component.ComponentType));
            IEnumerable<string> removedComponents = Components.Where(cComponent => !components.Any(nComponent => nComponent.ComponentType == cComponent.ComponentName)).Select(component => component.ComponentName);
            IEnumerable<ComponentViewModel> updatedComponents = Components.Where(cComponent => components.Any(nComponent => nComponent.ComponentType == cComponent.ComponentName));


            for (int i = Components.Count; i > 0; i--)
            {
                if (removedComponents.Contains(Components[0].ComponentName))
                {
                    allComponents.RemoveAt(i);
                }
            }

            foreach (ComponentViewModel newView in newComponents.Select(component => new ComponentViewModel(component)))
            {
                allComponents.Add(newView);
            }

            foreach (ComponentViewModel updatedComponent in updatedComponents)
            {
                ComponentData component = components.FirstOrDefault(component => component.ComponentType == updatedComponent.ComponentName);
                updatedComponent.UpdateArguments(component?.Arguments);
            }
        }
    }
}
