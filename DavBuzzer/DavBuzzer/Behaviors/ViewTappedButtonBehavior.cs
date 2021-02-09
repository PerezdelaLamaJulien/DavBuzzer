using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DavBuzzer.Behaviors
{
    public class ViewTappedButtonBehavior : Behavior<View>, INotifyPropertyChanged
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ViewTappedButtonBehavior), null, defaultBindingMode: BindingMode.TwoWay);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ViewTappedButtonBehavior), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public View AssociatedObject { get; private set; }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;

            if (bindable is Button myButton)
            {
                myButton.Clicked += View_Tapped;
            }
            else if (bindable is Switch mySwitch)
            {
                mySwitch.Toggled += View_Tapped;
            }
            else
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += View_Tapped;
                bindable.GestureRecognizers.Add(tapGestureRecognizer);
            }

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;

            var exists = bindable.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;

            if (exists != null)
                exists.Tapped -= View_Tapped;

            base.OnDetachingFrom(bindable);
        }

        bool _isAnimating = false;

        void View_Tapped(object sender, EventArgs e)
        {
            if (_isAnimating)
                return;

            _isAnimating = true;

            var view = (View)sender;

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Command != null)
                {
                    if (Command.CanExecute(CommandParameter))
                    {
                        Command.Execute(CommandParameter);
                    }
                }

                await view.ScaleTo(1.2, 170, easing: Easing.Linear);
                await view.ScaleTo(1, 170, easing: Easing.Linear);

                _isAnimating = false;
            });
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}