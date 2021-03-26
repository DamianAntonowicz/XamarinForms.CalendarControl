# Calendar control for Xamarin.Forms

This repository contains Calendar control with following features:
- customizing specific day with ControlTemplate
- customizing week day header with ControlTemplate
- single or multi select of days
- show / hide days from other months
- show / hide weekends
- select first day of a week

## Code

```xml
<components:Calendar x:Name="Calendar"
                     DayAdded="Calendar_OnDayAdded"
                     DayTapped="Calendar_OnDayTapped"
                     Date="01/01/21"
                     ShowWeekends="True"
                     ShowDaysFromOtherMonths="True"
                     FirstDayOfWeek="Sunday"
                     SelectionMode="SingleSelect"
                     DayControlTemplate="{StaticResource CalendarDayControlTemplate}"
                     WeekDayHeaderControlTemplate="{StaticResource CalendarWeekDayHeaderControlTemplate}"/>
```

## Screenshots

### iOS

![iOS](docs/iOS_2.png)
![iOS](docs/iOS_1.png)

### Android

![iOS](docs/Android_2.png)
![iOS](docs/Android_1.png)
