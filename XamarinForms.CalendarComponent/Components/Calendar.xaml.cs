using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace XamarinForms.CalendarComponent.Components
{
    public partial class Calendar : ContentView
    {
        private readonly List<CalendarDay> _days = new List<CalendarDay>();
    
        public IReadOnlyCollection<CalendarDay> Days => new ReadOnlyCollection<CalendarDay>(_days);

        public event EventHandler<CalendarDayTappedEventArgs> DayTapped;
        public event EventHandler<CalendarDayAddedEventArgs> DayAdded;

        #region ShowDaysFromOtherMonthsProperty

        public static readonly BindableProperty ShowDaysFromOtherMonthsProperty =
            BindableProperty.Create(
                propertyName: nameof(ShowDaysFromOtherMonths),
                returnType: typeof(bool),
                declaringType: typeof(Calendar),
                defaultValue: true,
                propertyChanged: OnShowDaysFromOtherMonthsPropertyChanged);

        private static void OnShowDaysFromOtherMonthsPropertyChanged(BindableObject bindable, object oldValue,
            object newValue)
        {
            var calendar = bindable as Calendar;
            calendar.InitializeCalendarDays();
        }

        public bool ShowDaysFromOtherMonths
        {
            get => (bool) GetValue(ShowDaysFromOtherMonthsProperty);
            set => SetValue(ShowDaysFromOtherMonthsProperty, value);
        }

        #endregion

        #region ShowWeekendsProperty

        public static readonly BindableProperty ShowWeekendsProperty =
            BindableProperty.Create(
                propertyName: nameof(ShowWeekends),
                returnType: typeof(bool),
                declaringType: typeof(Calendar),
                defaultValue: true,
                propertyChanged: OnShowWeekendsChanged);

        private static void OnShowWeekendsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = bindable as Calendar;

            if (!calendar.ShowWeekends &&
                (calendar.FirstDayOfWeek == DayOfWeek.Saturday ||
                 calendar.FirstDayOfWeek == DayOfWeek.Sunday))
            {
                calendar.FirstDayOfWeek = DayOfWeek.Monday;
            }
            else
            {
                calendar.InitializeWeekDayHeaders();
                calendar.InitializeCalendarDays();
            }
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
                propertyName: nameof(WeekDayHeaderControlTemplateProperty),
                returnType: typeof(DayOfWeek),
                declaringType: typeof(Calendar),
                defaultValue: DayOfWeek.Monday,
                propertyChanged: OnFirstDayOfWeekChanged);

        private static void OnFirstDayOfWeekChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = bindable as Calendar;
            calendar.InitializeWeekDayHeaders();
            calendar.InitializeCalendarDays();
        }

        public DayOfWeek FirstDayOfWeek
        {
            get => (DayOfWeek) GetValue(FirstDayOfWeekProperty);
            set => SetValue(FirstDayOfWeekProperty, value);
        }

        #endregion

        #region WeekDayHeaderControlTemplateProperty

        public static readonly BindableProperty WeekDayHeaderControlTemplateProperty =
            BindableProperty.Create(
                propertyName: nameof(WeekDayHeaderControlTemplateProperty),
                returnType: typeof(ControlTemplate),
                declaringType: typeof(Calendar),
                propertyChanged: OnWeekDayHeaderControlTemplateChanged);

        private static void OnWeekDayHeaderControlTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = bindable as Calendar;
            calendar.InitializeWeekDayHeaders();
        }

        public ControlTemplate WeekDayHeaderControlTemplate
        {
            get => (ControlTemplate) GetValue(WeekDayHeaderControlTemplateProperty);
            set => SetValue(WeekDayHeaderControlTemplateProperty, value);
        }

        #endregion

        #region DayControlTemplateProperty

        public static readonly BindableProperty DayControlTemplateProperty =
            BindableProperty.Create(
                propertyName: nameof(DayControlTemplate),
                returnType: typeof(ControlTemplate),
                declaringType: typeof(Calendar),
                propertyChanged: OnDayControlTemplateChanged);

        private static void OnDayControlTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = bindable as Calendar;
            calendar.InitializeCalendarDays();
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
                declaringType: typeof(Calendar),
                defaultValue: new List<DateTime>().AsReadOnly(),
                propertyChanged: OnSelectedDaysChanged);

        private static void OnSelectedDaysChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var daysToSelect = newValue as IList<DateTime>;
            var calendar = bindable as Calendar;

            if (calendar.SelectionMode == CalendarSelectionMode.SingleSelect)
            {
                if (daysToSelect != null &&
                    daysToSelect.Count > 1)
                {
                    throw new InvalidOperationException(
                        "Trying to select more than one day when working in SingleSelect mode");
                }
            }

            calendar.SelectDays(daysToSelect);
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
                declaringType: typeof(Calendar),
                propertyChanged: OnDateChanged);

        private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = bindable as Calendar;
            calendar.InitializeCalendarDays();
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
                returnType: typeof(CalendarSelectionMode),
                propertyChanged: OnModeChanged,
                declaringType: typeof(Calendar));

        private static void OnModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = bindable as Calendar;
            calendar.SelectedDays = new List<DateTime>().AsReadOnly();
        }

        public CalendarSelectionMode SelectionMode
        {
            get => (CalendarSelectionMode) GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        #endregion

        public Calendar()
        {
            InitializeComponent();
        }

        private void InitializeCalendarDays()
        {
            InitializeGridForCalendarDays();
            AddCalendarDays();

            SelectDays(SelectedDays);
        }

        private void SelectDays(IEnumerable<DateTime> daysToSelect)
        {
            _days.ForEach(calendarDay => calendarDay.IsSelected = false);

            foreach (var dayToSelect in daysToSelect)
            {
                var calendarDay = _days.FirstOrDefault(x => x.Date == dayToSelect.Date);
                if (calendarDay != null)
                {
                    calendarDay.IsSelected = true;
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
                gridRows.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });
            }

            GridDays.RowDefinitions = gridRows;
        }

        private void AddCalendarDays()
        {
            if (DayControlTemplate == null)
            {
                return;
            }

            if (_days.Count > 0)
            {
                foreach (var calendarDay in _days)
                {
                    calendarDay.GestureRecognizers.Clear();
                }

                _days.Clear();
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
                    
                    if (ShowDaysFromOtherMonths)
                    {
                        AddCalendarDaysFromPreviousOrNextMonths(week, date, weeksInMonth, daysInMonth);
                    }

                    day++;

                    if (!ShowWeekends &&
                        (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
                    {
                        continue;
                    }

                    AddDay(date, week);
                }
            }

            if (SelectedDays?.Count > 0)
            {
                SelectDays(SelectedDays);
            }
        }

        private void AddCalendarDaysFromPreviousOrNextMonths(int week, DateTime date, int weeksInMonth,
            int daysInMonth)
        {
            if (week == 1 &&
                date.Day == 1 &&
                date.DayOfWeek != FirstDayOfWeek)
            {
                var newDate = date;

                do
                {
                    newDate = newDate.AddDays(-1);

                    if (!ShowWeekends &&
                        (newDate.DayOfWeek == DayOfWeek.Saturday || newDate.DayOfWeek == DayOfWeek.Sunday))
                    {
                        continue;
                    }

                    AddDay(newDate, week);
                } while (newDate.DayOfWeek != FirstDayOfWeek);
            }
            else if (week == weeksInMonth &&
                     date.Day == daysInMonth &&
                     date.DayOfWeek != FirstDayOfWeek.PreviousOrFirst())
            {
                var newDate = date;
                var lastDayOfWeek = FirstDayOfWeek.PreviousOrFirst();

                do
                {
                    newDate = newDate.AddDays(1);

                    if (!ShowWeekends &&
                        (newDate.DayOfWeek == DayOfWeek.Saturday || newDate.DayOfWeek == DayOfWeek.Sunday))
                    {
                        continue;
                    }

                    AddDay(newDate, week);
                } while (newDate.DayOfWeek != lastDayOfWeek);
            }
        }

        private void AddDay(DateTime date, int week)
        {
            var calendarDay = new CalendarDay(date)
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ControlTemplate = DayControlTemplate
            };

            var column = date.DayOfWeek(FirstDayOfWeek, includeWeekends: ShowWeekends) - 1;
            Grid.SetColumn(calendarDay, column);

            var row = week - 1;
            Grid.SetRow(calendarDay, row);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += CalendarDay_OnTapped;
            calendarDay.GestureRecognizers.Add(tapGestureRecognizer);

            _days.Add(calendarDay);
            GridDays.Children.Add(calendarDay);

            DayAdded?.Invoke(this, new CalendarDayAddedEventArgs(calendarDay));
        }

        private void CalendarDay_OnTapped(object sender, EventArgs e)
        {
            var calendarDay = sender as CalendarDay;

            if (calendarDay.IsSelectable)
            {
                if (SelectionMode == CalendarSelectionMode.SingleSelect)
                {
                    SelectedDays = new[] {calendarDay.Date};
                }
                else if (SelectionMode == CalendarSelectionMode.MultiSelect)
                {
                    var newSelectedDays = SelectedDays.ToList();

                    if (calendarDay.IsSelected)
                    {
                        newSelectedDays.Remove(calendarDay.Date);
                    }
                    else
                    {
                        newSelectedDays.Add(calendarDay.Date);
                    }

                    SelectedDays = new ReadOnlyCollection<DateTime>(newSelectedDays);
                }
            }

            DayTapped?.Invoke(this, new CalendarDayTappedEventArgs(calendarDay));
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
                var weekDayControl = new CalendarWeekDayHeader(dayOfWeek)
                {
                    ControlTemplate = WeekDayHeaderControlTemplate
                };

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
                AddWeekDayHeaderControl(currentDayOfWeek, weekDayNumber: i);

                currentDayOfWeek = currentDayOfWeek.NextOrFirst();

                if (!ShowWeekends &&
                    (currentDayOfWeek == DayOfWeek.Saturday || currentDayOfWeek == DayOfWeek.Sunday))
                {
                    currentDayOfWeek = DayOfWeek.Monday;
                }
            }
        }
    }

    public class CalendarDayAddedEventArgs : EventArgs
    {
        public CalendarDay CalendarDay { get; }

        public CalendarDayAddedEventArgs(CalendarDay calendarDay)
        {
            CalendarDay = calendarDay;
        }
    }

    public class CalendarDayTappedEventArgs : EventArgs
    {
        public CalendarDay CalendarDay { get; }

        public CalendarDayTappedEventArgs(CalendarDay calendarDay)
        {
            CalendarDay = calendarDay;
        }
    }

    public enum CalendarSelectionMode
    {
        SingleSelect,
        MultiSelect
    }
}