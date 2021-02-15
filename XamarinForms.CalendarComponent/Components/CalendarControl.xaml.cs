using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public partial class CalendarControl : ContentView
    {
        public List<DayControl> Days { get; } = new List<DayControl>();
        
        public event EventHandler<DayControlTappedEventArgs> DayTapped;
        public event EventHandler<DayControlAddedEventArgs> DayAdded;

        #region WeekDayHeaderControlTemplateProperty

        public static readonly BindableProperty WeekDayControlTemplateProperty =
            BindableProperty.Create(
                propertyName: nameof(WeekDayControlTemplateProperty),
                returnType: typeof(ControlTemplate),
                declaringType: typeof(CalendarControl),
                propertyChanged: OnWeekDayControlTemplateChanged);

        private static void OnWeekDayControlTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendarControl = bindable as CalendarControl;
            calendarControl.InitializeView();
        }
        
        public ControlTemplate WeekDayHeaderControlTemplate
        {
            get => (ControlTemplate) GetValue(WeekDayControlTemplateProperty);
            set => SetValue(WeekDayControlTemplateProperty, value);
        }
        
        #endregion

        #region DayControlTemplateProperty

        public static readonly BindableProperty DayControlTemplateProperty =
            BindableProperty.Create(
                propertyName: nameof(DayControlTemplate),
                returnType: typeof(ControlTemplate),
                declaringType: typeof(CalendarControl),
                propertyChanged: OnDayControlTemplateChanged);

        private static void OnDayControlTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendarControl = bindable as CalendarControl;
            calendarControl.InitializeView();
        }

        public ControlTemplate DayControlTemplate
        {
            get => (ControlTemplate) GetValue(DayControlTemplateProperty);
            set => SetValue(DayControlTemplateProperty, value);
        }
            
        #endregion
        
        #region SelectedDaysProperty
        
        public static readonly BindableProperty SelectedDaysProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectedDays),
                returnType: typeof(IReadOnlyCollection<DateTime>),
                declaringType: typeof(CalendarControl),
                propertyChanged: OnSelectedDaysChanged);

        private static void OnSelectedDaysChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var daysSelected = newValue as IList<DateTime>;
            var calendarControl = bindable as CalendarControl;
            
            if (calendarControl.SelectionMode == CalendarControlSelectionMode.SingleSelect)
            {
                if (daysSelected != null && 
                    daysSelected.Count > 1)
                {
                    // throw exception?
                }
            }

            calendarControl.SelectDayControls(daysSelected);
        }

        public IReadOnlyCollection<DateTime> SelectedDays
        {
            get => (IReadOnlyCollection<DateTime>) GetValue(SelectedDaysProperty);
            set => SetValue(SelectedDaysProperty, value);
        }

        #endregion
        
        #region DateProperty

        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                propertyName: nameof(Date),
                returnType: typeof(DateTime),
                declaringType: typeof(CalendarControl),
                propertyChanged: OnDateChanged);

        private static void OnDateChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var calendarControl = bindable as CalendarControl;
            calendarControl.InitializeView();
        }

        public DateTime Date
        {
            get => (DateTime) GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        #endregion
        
        #region SelectionModeProperty

        public static readonly BindableProperty SelectionModeProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectionMode),
                returnType: typeof(CalendarControlSelectionMode),
                declaringType: typeof(CalendarControl));

        public CalendarControlSelectionMode SelectionMode
        {
            get => (CalendarControlSelectionMode) GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        #endregion

        public CalendarControl()
        {
            InitializeComponent();
        }

        private void InitializeView()
        {
            SelectedDays = new List<DateTime>().AsReadOnly();
            LayoutRoot.Children.Clear();

            InitializeGridDefinitions();
            InitializeCalendarHeaders();
            InitializeCalendarDays();
        }

        private void SelectDayControls(IEnumerable<DateTime> daysSelected)
        {
            Days.ForEach(dayControl => dayControl.IsSelected = false);
        }

        private void InitializeGridDefinitions()
        {
            var gridColumns = new ColumnDefinitionCollection();
            var gridRows = new RowDefinitionCollection();
            
            for (var i = 0; i < DayControl.DaysInWeek; i++)
            {
                gridColumns.Add(new ColumnDefinition());
            }

            for (var i = 0; i <= Date.GetWeekCountInMonth(); i++)
            {
                gridRows.Add(new RowDefinition());
            }

            LayoutRoot.RowDefinitions = gridRows;
            LayoutRoot.ColumnDefinitions = gridColumns;
        }

        private void InitializeCalendarDays()
        {
            if (DayControlTemplate == null)
            {
                return;
            }

            if (Days.Count > 0)
            {
                foreach (var dayControl in Days)
                {
                    dayControl.Tapped -= DayComponent_OnTapped;
                }
            }

            for (var i = 1; i <= Date.GetDayCountInMonth(); i++)
            {
                var date = new DateTime(Date.Year, Date.Month, i);

                var dayControl = new DayControl
                {
                    Date = date,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions =  LayoutOptions.CenterAndExpand,
                };

                dayControl.ControlTemplate = DayControlTemplate;

                Grid.SetColumn(dayControl, date.GetDayNumberOfWeek() - 1);
                Grid.SetRow(dayControl, date.GetWeekNumberOfMonth());
                
                dayControl.Tapped += DayComponent_OnTapped;
                
                Days.Add(dayControl);
                LayoutRoot.Children.Add(dayControl);

                DayAdded?.Invoke(this, new DayControlAddedEventArgs(dayControl));
            }
            
            if (SelectedDays?.Count > 0)
            {
                SelectDayControls(SelectedDays);
            }
        }

        private void InitializeCalendarHeaders()
        {
            if (WeekDayHeaderControlTemplate == null)
            {
                return;
            }

            void AddWeekDayControl(DayOfWeek dayOfWeek, int weekDayNumber)
            {
                var weekDayControl = new WeekDayHeaderControl();
                weekDayControl.ControlTemplate = WeekDayHeaderControlTemplate;
                weekDayControl.DayOfWeek = dayOfWeek;

                Grid.SetRow(weekDayControl, 0);
                Grid.SetColumn(weekDayControl, weekDayNumber - 1);

                LayoutRoot.Children.Add(weekDayControl);
            }

            AddWeekDayControl(DayOfWeek.Monday, weekDayNumber: 1);
            AddWeekDayControl(DayOfWeek.Tuesday, weekDayNumber: 2);
            AddWeekDayControl(DayOfWeek.Wednesday, weekDayNumber: 3);
            AddWeekDayControl(DayOfWeek.Thursday, weekDayNumber: 4);
            AddWeekDayControl(DayOfWeek.Friday, weekDayNumber: 5);
            AddWeekDayControl(DayOfWeek.Saturday, weekDayNumber: 6);
            AddWeekDayControl(DayOfWeek.Sunday, weekDayNumber: 7);
        }

        private void DayComponent_OnTapped(object sender, EventArgs e)
        {
            var dayControl = sender as DayControl;

            DayTapped?.Invoke(this, new DayControlTappedEventArgs(dayControl));
        }
    }
    public class DayControlAddedEventArgs : EventArgs
    {
        public DayControl DayControl { get; }
        
        public DayControlAddedEventArgs(DayControl dayControl)
        {
            DayControl = dayControl;
        }
    }
    
    public class DayControlTappedEventArgs : EventArgs
    {
        public DayControl DayControl { get; }

        public DayControlTappedEventArgs(DayControl dayControl)
        {
            DayControl = dayControl;
        }
    }

    public enum CalendarControlSelectionMode
    {
        SingleSelect,
        MultiSelect
    }
}