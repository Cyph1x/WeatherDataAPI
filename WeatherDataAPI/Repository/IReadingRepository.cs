using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WeatherDataAPI.Models.WeatherReadings;
using WeatherDataAPI.Models.WeatherReadings.DTO;

namespace WeatherDataAPI.Repository
{
    public interface IReadingRepository
    {
        //Standard CRUD methods
        public List<WeatherReading> GetAll(WeatherReadingFilter readingFilter);
        public WeatherReading GetById(string id);
        public WeatherReading CreateWeatherReading(WeatherReadingBase weatherReading);
        public bool ReplaceWeatherReading(WeatherReading weatherReading);
        public long UpdateWeatherReading(WeatherReadingUpdateDTO weatherReading, string id);
        public bool DeleteWeatherReading(string id);
        //Multiple methods
        public long ReplaceWeatherReadings(List<WeatherReading> weatherReadings);
        public long UpdateWeatherReadings(List<WeatherReadingUpdateManyDTO> weatherReadings);
        public long DeleteWeatherReadings(List<string> ids);
        public long DeleteWeatherReadings(WeatherReadingFilter readingFilter);
        public List<WeatherReading> CreateWeatherReadings(List<WeatherReadingBase> weatherReadings);
        public List<WeatherReadingMaxTempDTO> MaxTempBetweenTimes(DateTime startTime, DateTime endTime);
        public List<WeatherReadingHourDTO> ReadingsAtSpecificHourForStation(string deviceName, [FromQuery, Required] DateTime hour);



    }
}
