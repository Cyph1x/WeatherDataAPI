using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.ComponentModel.DataAnnotations;
using WeatherDataAPI.Models.WeatherReadings;
using WeatherDataAPI.Models.WeatherReadings.DTO;
using WeatherDataAPI.Services;

namespace WeatherDataAPI.Repository
{
    public class MongoReadingRepository : IReadingRepository
    {

        private readonly IMongoCollection<WeatherReading> _WeatherReadings;
        public MongoReadingRepository(MongoConnection connection)
        {
            _WeatherReadings = connection.GetDatabase().GetCollection<WeatherReading>("WeatherData");
        }

        public WeatherReading CreateWeatherReading(WeatherReadingBase weatherReading)
        {
            WeatherReading newReading = weatherReading.ToReading();
            _WeatherReadings.InsertOne(newReading);
            return newReading;
        }

        public List<WeatherReading> CreateWeatherReadings(List<WeatherReadingBase> weatherReadings)
        {
            var newReadings = weatherReadings.Select(weatherReading => new WeatherReading
            {
                AtmosphericPressure = weatherReading.AtmosphericPressure,
                DeviceName = weatherReading.DeviceName,
                Humidity = weatherReading.Humidity,
                Latitude = weatherReading.Latitude,
                Longitude = weatherReading.Longitude,
                MaxWindSpeed = weatherReading.MaxWindSpeed,
                Precipitation = weatherReading.Precipitation,
                SolarRadiation = weatherReading.SolarRadiation,
                Temperature = weatherReading.Temperature,
                Time = weatherReading.Time,
                VaporPressure = weatherReading.VaporPressure,
                WindDirection = weatherReading.WindDirection
            }).ToList();
            _WeatherReadings.InsertMany(newReadings);
            return newReadings;
        }

        public bool DeleteWeatherReading(string id)
        {
            return _WeatherReadings.DeleteOne(reading => reading._id == new ObjectId(id)).DeletedCount >= 1;
        }

        public long DeleteWeatherReadings(List<string> ids)
        {
            return _WeatherReadings.DeleteMany(reading => ids.Contains(reading._id.ToString())).DeletedCount;
        }

        public long DeleteWeatherReadings(WeatherReadingFilter readingFilter)
        {
            //get all ids to delete
            IEnumerable<ObjectId> ids = GetAll(readingFilter).Select(x => x._id);

            //delete the ids
            return _WeatherReadings.DeleteMany(reading => ids.Contains(reading._id)).DeletedCount;
        }

        public List<WeatherReading> GetAll(WeatherReadingFilter readingFilter)
        {
            return _WeatherReadings.FindSync(readingFilter.BuildFilter(), readingFilter.BuildFindOptions()).ToList();
        }

        public WeatherReading GetById(string id)
        {
            var result = _WeatherReadings.Find(reading => reading._id == new ObjectId(id)).FirstOrDefault();
            return result;
        }

        public bool ReplaceWeatherReading(WeatherReading weatherReading)
        {
            var result = _WeatherReadings.ReplaceOne(reading => reading._id == weatherReading._id, weatherReading);
            return result.ModifiedCount >= 1;
        }

        public long ReplaceWeatherReadings(List<WeatherReading> weatherReadings)
        {
            var result = _WeatherReadings.BulkWrite(weatherReadings.Select(reading => new ReplaceOneModel<WeatherReading>(new BsonDocument("_id", reading._id), reading)));
            return result.ModifiedCount;
        }

        public long UpdateWeatherReading(WeatherReadingUpdateDTO weatherReading, string id)
        {

            var update = weatherReading.ToUpdateDefinition();
            //if update equals null then no valid fields were provided
            if (update == null)
            {
                return -1; // bad fields
            }

            UpdateResult result;
            //because mongodb is stupid and wont let us modify the value of a shard key without using a equality statement in the filter, we need to get the wind direction from the original weather reading if we are modifying the weather reading.
            if (weatherReading.WindDirection != null)
            {
                var original = _WeatherReadings.Find(reading => reading._id == new ObjectId(id)).FirstOrDefault();
                if (original != null)
                {
                    result = _WeatherReadings.UpdateOne(
                       reading => reading._id == new ObjectId(id) && reading.WindDirection == original.WindDirection,
                       update);
                }
                else
                {
                    result = _WeatherReadings.UpdateOne(reading => reading._id == new ObjectId(id), update);
                }
            }
            else
            {
                result = _WeatherReadings.UpdateOne(reading => reading._id == new ObjectId(id), update);
            }

            if (result.MatchedCount > 0)
            {
                return 1; // worked
            }
            else
            {
                return 0; // nothing matched

            }
        }

        public long UpdateWeatherReadings(List<WeatherReadingUpdateManyDTO> weatherReadings)
        {
            //speedy bulk write magic
            //we need to create a list of models as each model will define its own filter and update definition
            var updates = new UpdateManyModel<WeatherReading>[weatherReadings.Count];
            for (int index = 0; index < weatherReadings.Count; index++)
            {
                var update = weatherReadings[index].Update.ToUpdateDefinition();
                var filter = Builders<WeatherReading>.Filter.Eq("_id", weatherReadings[index]._id);

                if (weatherReadings[index].Update.WindDirection != null)
                {
                    var original = _WeatherReadings.Find(filter).FirstOrDefault();
                    if (original != null)
                    {
                        filter &= Builders<WeatherReading>.Filter.Eq(WeatherReadingNames.WindDirection, original.WindDirection);
                    }

                }
                var model = new UpdateManyModel<WeatherReading>(filter, update);

                updates[index] = model;
            }
            //do the bulk write / bulk update using the created models
            var result = _WeatherReadings.BulkWrite(updates);
            return result.MatchedCount;
        }

        //special stuff
        public List<WeatherReadingMaxTempDTO> MaxTempBetweenTimes(DateTime startTime, DateTime endTime)
        {
            var pipeline = new BsonDocument[]
{
    new BsonDocument("$sort",
    new BsonDocument(WeatherReadingNames.Temperature, -1)),
    new BsonDocument("$match",
    new BsonDocument("$and",
    new BsonArray
            {
                new BsonDocument(WeatherReadingNames.Time,
                new BsonDocument("$gt",
                startTime)),
                new BsonDocument(WeatherReadingNames.Time,
                new BsonDocument("$lt",
                endTime))
            })),
    new BsonDocument("$group",
    new BsonDocument
        {
            { "_id", "$"+WeatherReadingNames.DeviceName },
            { WeatherReadingNames.Temperature,
    new BsonDocument("$first", "$"+ WeatherReadingNames.Temperature) },
            { "Id",
    new BsonDocument("$first", "$_id") },
            { WeatherReadingNames.Time,
    new BsonDocument("$first", "$" + WeatherReadingNames.Time) }
        }),
    new BsonDocument("$project",
    new BsonDocument
        {

            { "_id", "$Id" },
            { WeatherReadingNames.DeviceName, "$_id" },
            { WeatherReadingNames.Temperature, 1 },
            { WeatherReadingNames.Time, 1 }
        })
};
            var result = _WeatherReadings.Aggregate<WeatherReadingMaxTempDTO>(pipeline).ToList();
            return result;
        }

        public List<WeatherReadingHourDTO> ReadingsAtSpecificHourForStation(string deviceName, [FromQuery, Required] DateTime hour)
        {
            var filter = new WeatherReadingFilter();
            var startTime = new DateTime(hour.Year, hour.Month, hour.Day, hour.Hour, 0, 0, 0);
            var endTime = new DateTime(hour.Year, hour.Month, hour.Day, hour.Hour, 59, 59, 999);
            filter.StartTime = startTime;
            filter.EndTime = endTime;
            filter.DeviceName = deviceName;
            filter.Projection = new WeatherReadingProjection
            {
                Temperature = true,
                AtmosphericPressure = true,
                SolarRadiation = true,
                Precipitation = true
            };
            var readings = GetAll(filter);
            //we now have all readings from that station for that hour
            //change to WeatherReadingHourDTO type
            List<WeatherReadingHourDTO> result = readings.Select(reading => new WeatherReadingHourDTO
            {
                _id = reading._id,
                Temperature = reading.Temperature,
                AtmosphericPressure = reading.AtmosphericPressure,
                SolarRadiation = reading.SolarRadiation,
                Precipitation = reading.Precipitation
            }).ToList();
            return result;

        }
    }
}
