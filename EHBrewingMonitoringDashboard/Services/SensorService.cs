namespace EHBrewingMonitoringDashboard.Services;

using EHBrewingMonitoringDashboard.Enums;
using EHBrewingMonitoringDashboard.Models;

public class SensorService
{
    public EStatus GetStatusByValue(Sensor sensor, decimal? value)
    {
        if (!value.HasValue) return EStatus.NoData;

        if (sensor.IdealMaxValue.HasValue)
        {
            if (value > (sensor.IdealMaxValue + (sensor.WarningTolerance ?? 0)))
                return EStatus.Critical;

            if (value > sensor.IdealMaxValue)
                return EStatus.Warning;
        }

        if (sensor.IdealMinValue.HasValue)
        {
            if (value < (sensor.IdealMinValue - (sensor.WarningTolerance ?? 0)))
                return EStatus.Critical;

            if (value < sensor.IdealMinValue)
                return EStatus.Warning;
        }

        return EStatus.Ok;
    }
}