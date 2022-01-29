using DebugFramework.DataTypes.SubTypes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DebugGui.ViewModels
{
    public class ComponentsViewModel : ViewModelBase
    {
        public int ComponentCount => Components.Count;
        public ObservableCollection<ComponentViewModel> Components { get; set; }

        public ComponentViewModel SelectedComponentView
        {
            get => selectedComponentView;
            set => this.RaiseAndSetIfChanged(ref selectedComponentView, value);
        }
        private ComponentViewModel selectedComponentView;

        public ICommand CollapseAllCommand { get; }
        public ICommand ExpandAllCommand { get; }


        public ComponentsViewModel(List<Component> components)
        {
            Components = new ObservableCollection<ComponentViewModel>(components.Select(component => new ComponentViewModel(component)));
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

        public void UpdateComponents(List<Component> components)
        {

        }
    }
}
