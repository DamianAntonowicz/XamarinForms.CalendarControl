using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public partial class CalendarControl : ContentView
    {
        public List<DayControl> Days { get; } = new List<DayControl>();

        public event EventHandler<DayControlTappedEventArgs> DayTapped;
        public event EventHandler<DayControlAddedEventArgs> DayAdded;

        #region ShowWeekendsProperty

        public static readonly BindableProperty ShowWeekendsProperty =
            BindableProperty.Create(
                propertyName: nameof(ShowWeekends),
                returnType: typeof(bool),
                declaringType: typeof(CalendarControl),
                defaultValue: true,
                propertyChanged: OnShowWeekendsChanged);

        private static void OnShowWeekendsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendarControl = bindable as CalendarControl;
            calendarControl.InitializeWeekDayHeaders();
            calendarControl.InitializeCalendarDays();
        }

        public bool ShowWeekends
        {
            get => (bool) GetValue(ShowWeekendsProperty);
            set => SetValue(ShowWeekendsProperty, value);
        }

        #endregion

        #region FirstDayOfWeekProperty

        public static readonly BindableProperty FirstDayOfWeekProperty =
            BindableProperty.Create(
                propertyName: nameof(WeekDayControlTemplateProperty),
                returnType: typeof(DayOfWeek),
                declaringType: typeof(CalendarControl),
                defaultValue: DayOfWeek.Monday,
                propertyChanged: OnFirstDayOfWeekChanged);

        private static void OnFirstDayOfWeekChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendarControl = bindable as CalendarControl;
            calendarControl.InitializeWeekDayHeaders();
            calendarControl.InitializeCalendarDays();
        }

        public DayOfWeek FirstDayOfWeek
        {
            get => (DayOfWeek) GetValue(FirstDayOfWeekProperty);
            set => SetValue(FirstDayOfWeekProperty, value);
        }

        #endregion

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
            calendarControl.InitializeWeekDayHeaders();
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
            calendarControl.InitializeCalendarDays();
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
                defaultValue: new List<DateTime>().AsReadOnly(),
                propertyChanged: OnSelectedDaysChanged);

        private static void OnSelectedDaysChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var daysToSelect = newValue as IList<DateTime>;
            var calendarControl = bindable as CalendarControl;

            if (calendarControl.SelectionMode == CalendarControlSelectionMode.SingleSelect)
            {
                if (daysToSelect != null &&
                    daysToSelect.Count > 1)
                {
                    throw new InvalidOperationException(
                        "Trying to select more than one day when working in SingleSelect mode");
                }
            }

            calendarControl.SelectDayControls(daysToSelect);
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

        private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendarControl = bindable as CalendarControl;
            calendarControl.InitializeCalendarDays();
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
                propertyChanged: OnModeChanged,
                declaringType: typeof(CalendarControl));

        private static void OnModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendarControl = bindable as CalendarControl;
            calendarControl.SelectedDays = new List<DateTime>().AsReadOnly();
        }

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

        private void InitializeCalendarDays()
        {
            InitializeGridForCalendarDays();
            AddCalendarDays();

            SelectDayControls(SelectedDays);
        }

        private void SelectDayControls(IEnumerable<DateTime> daysToSelect)
        {
            Days.ForEach(dayControl => dayControl.IsSelected = false);

            foreach (var dayToSelect in daysToSelect)
            {
                var dayControl = Days.FirstOrDefault(x => x.Date == dayToSelect.Date);
                if (dayControl != null)
                {
                    dayControl.IsSelected = true;
                }
            }
        }

        private void InitializeGridForCalendarDays()
        {
            GridDays.Children.Clear();

            if (!ShowWeekends &&
                GridDays.ColumnDefinitions.Count == 7)
            {
                GridDays.ColumnDefinitions.RemoveAt(GridDays.ColumnDefinitions.Count - 1);
                GridDays.ColumnDefinitions.RemoveAt(GridDays.ColumnDefinitions.Count - 1);
            }
            else if (ShowWeekends &&
                     GridDays.ColumnDefinitions.Count < 7)
            {
                GridDays.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});
                GridDays.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});
            }

            var gridRows = new RowDefinitionCollection();

            for (var i = 1; i <= Date.WeeksInMonth(FirstDayOfWeek); i++)
            {
                gridRows.Add(new RowDefinition());
            }

            GridDays.RowDefinitions = gridRows;
        }

        private void AddCalendarDays()
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

                Days.Clear();
            }

            var daysInWeek = 7;
            var weeksInMonth = Date.WeeksInMonth(FirstDayOfWeek);
            var daysInMonth = Date.DaysInMonth();
            var day = 1;

            for (var week = 1; week <= weeksInMonth; week++)
            {
                for (var dayInWeek = 1; dayInWeek <= daysInWeek; dayInWeek++)
                {
                    if (day > daysInMonth)
                    {
                        break;
                    }

                    var date = new DateTime(Date.Year, Date.Month, day);

                    if (date.WeekOfMonth(FirstDayOfWeek) != week)
                    {
                        break;
                    }

                    day++;

                    if (!ShowWeekends &&
                        (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        continue;
                    }

                    var dayControl = new DayControl
                    {
                        Date = date,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                    };

                    dayControl.ControlTemplate = DayControlTemplate;

                    var column = date.DayOfWeek(FirstDayOfWeek, includeWeekends: ShowWeekends) - 1;
                    Grid.SetColumn(dayControl, column);

                    var row = week - 1;
                    Grid.SetRow(dayControl, row);

                    dayControl.Tapped += DayComponent_OnTapped;

                    Days.Add(dayControl);
                    GridDays.Children.Add(dayControl);

                    DayAdded?.Invoke(this, new DayControlAddedEventArgs(dayControl));
                }
            }

            if (SelectedDays?.Count > 0)
            {
                SelectDayControls(SelectedDays);
            }
        }

        private void DayComponent_OnTapped(object sender, EventArgs e)
        {
            var dayControl = sender as DayControl;

            if (dayControl.IsSelectable)
            {
                if (SelectionMode == CalendarControlSelectionMode.SingleSelect)
                {
                    SelectedDays = new[] {dayControl.Date};
                }
                else if (SelectionMode == CalendarControlSelectionMode.MultiSelect)
                {
                    var newSelectedDays = SelectedDays.ToList();

                    if (dayControl.IsSelected)
                    {
                        newSelectedDays.Remove(dayControl.Date);
                    }
                    else
                    {
                        newSelectedDays.Add(dayControl.Date);
                    }

                    SelectedDays = new ReadOnlyCollection<DateTime>(newSelectedDays);
                }
            }

            DayTapped?.Invoke(this, new DayControlTappedEventArgs(dayControl));
        }

        private void InitializeWeekDayHeaders()
        {
            if (WeekDayHeaderControlTemplate == null)
            {
                return;
            }

            GridWeekDayHeaders.Children.Clear();

            if (!ShowWeekends &&
                GridWeekDayHeaders.ColumnDefinitions.Count == 7)
            {
                GridWeekDayHeaders.ColumnDefinitions.RemoveAt(GridWeekDayHeaders.ColumnDefinitions.Count - 1);
                GridWeekDayHeaders.ColumnDefinitions.RemoveAt(GridWeekDayHeaders.ColumnDefinitions.Count - 1);
            }
            else if (ShowWeekends &&
                     GridWeekDayHeaders.ColumnDefinitions.Count < 7)
            {
                GridWeekDayHeaders.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});
                GridWeekDayHeaders.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});
            }

            void AddWeekDayHeaderControl(DayOfWeek dayOfWeek, int weekDayNumber)
            {
                var weekDayControl = new WeekDayHeaderControl();
                weekDayControl.ControlTemplate = WeekDayHeaderControlTemplate;
                weekDayControl.DayOfWeek = dayOfWeek;

                Grid.SetColumn(weekDayControl, weekDayNumber - 1);

                GridWeekDayHeaders.Children.Add(weekDayControl);
            }

            var currentDayOfWeek = FirstDayOfWeek;
            var daysInWeek = 7;

            if (!ShowWeekends)
            {
                daysInWeek = 5;
            }

            for (var i = 1; i <= daysInWeek; i++)
            {
                if (ShowWeekends)
                {
                    AddWeekDayHeaderControl(currentDayOfWeek, weekDayNumber: i);
                    currentDayOfWeek = currentDayOfWeek.NextOrFirst();
                }
                else if (currentDayOfWeek != DayOfWeek.Saturday &&
                         currentDayOfWeek != DayOfWeek.Sunday)
                {
                    AddWeekDayHeaderControl(currentDayOfWeek, weekDayNumber: i);

                    currentDayOfWeek = currentDayOfWeek.NextOrFirst();

                    if (currentDayOfWeek == DayOfWeek.Saturday ||
                        currentDayOfWeek == DayOfWeek.Sunday)
                    {
                        currentDayOfWeek = DayOfWeek.Monday;
                    }
                }
            }
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