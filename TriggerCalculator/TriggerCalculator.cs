using System;
using System.Activities;

using System.ComponentModel;

namespace TriggerCalculator
{
    public class TriggerCalculator : CodeActivity
    {
        [Category("Input")]
        [Description("Initial Date(new DateTime(year, month, day, hour, minute, second))")]
        [RequiredArgument]
        public InArgument<DateTime> InitialDate { get; set; }

        [Category("Input")]
        [Description("Duration Type (ex : DurationTypes.Year, DurationTypes.Day etc.)")]
        [RequiredArgument]
        public InArgument<DurationTypes> DurationType { get; set; }

        [Category("Input")]
        [Description("Duration (Ex : If Duration is 5 and DurationType is DurationTypes.Minute then it waits for 5 minutes to run it again.)")]
        [RequiredArgument]
        public InArgument<int> Duration { get; set; }

        [Category("Input")]
        [Description("TimeZoneInfo.Id list can be found online (An example of TimeZoneInfo.Id : \"GTB Standard Time\")")]

        public InArgument<string> TimeZoneId { get; set; }

        [Category("Output")]
        [Description("Runnable status of process")]
        public OutArgument<bool> IsRunnable { get; set; }

        public bool triggerCalculator(DateTime initialDate, DurationTypes durationType, string zone, int counter)
        {
            DateTime date = DateTime.Now;
            Boolean triggerFlag = false;

            if (zone != null)
            {
                TimeZoneInfo timeZoneInfo;

                try
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zone);
                }
                catch (Exception ex)
                {
                    timeZoneInfo = TimeZoneInfo.Local;
                }
                initialDate = TimeZoneInfo.ConvertTime(initialDate, timeZoneInfo);
                date = TimeZoneInfo.ConvertTime(date, timeZoneInfo);
            }

            while (date > initialDate)
            {
                switch (durationType)
                {

                    case DurationTypes.Year:
                        {
                            initialDate = initialDate.AddYears(counter);
                            break;
                        }

                    case DurationTypes.Month:
                        {
                            initialDate = initialDate.AddMonths(counter);
                            break;
                        }
                    case DurationTypes.Day:
                        {
                            initialDate = initialDate.AddDays(counter);
                            break;
                        }
                    case DurationTypes.Hour:
                        {
                            initialDate = initialDate.AddHours(counter);
                            break;
                        }
                    case DurationTypes.Minute:
                        {
                            initialDate = initialDate.AddMinutes(counter);
                            break;
                        }
                }
            }

            counter = counter * (-1);

            switch (durationType)
            {

                case DurationTypes.Year:
                    {
                        initialDate = initialDate.AddYears(counter);
                        break;
                    }

                case DurationTypes.Month:
                    {
                        initialDate = initialDate.AddMonths(counter);
                        break;
                    }
                case DurationTypes.Day:
                    {
                        initialDate = initialDate.AddDays(counter);
                        break;
                    }
                case DurationTypes.Hour:
                    {
                        initialDate = initialDate.AddHours(counter);
                        break;
                    }
                case DurationTypes.Minute:
                    {
                        initialDate = initialDate.AddMinutes(counter);
                        break;
                    }
            }

            if (date.Subtract(initialDate).TotalSeconds <= 60)
                triggerFlag = true;

            return triggerFlag;
        }

        protected override void Execute(CodeActivityContext context)

        {

            var date = InitialDate.Get(context);

            var type = DurationType.Get(context);
            var counter2 = Duration.Get(context);
            var zone = TimeZoneId.Get(context);

            var runnable2 = triggerCalculator(date, type, zone, counter2);

            IsRunnable.Set(context, runnable2);





        }
    }

}