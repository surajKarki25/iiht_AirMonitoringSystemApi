using iiht_AirMonitoringSystemApi.Data;
using iiht_AirMonitoringSystemApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iiht_AirMonitoringSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
        public class AirQController : ControllerBase // defining the class and its inheritance
        {
            private readonly SensorDbContext _dbContext; // private readonly variable to hold the context of the database
            private readonly ILogger<AirQController> _logger; // private readonly variable to hold the logger object

            // constructor for initializing the database context and logger
            public AirQController(SensorDbContext dbContext, ILogger<AirQController> logger)
            {
                _dbContext = dbContext;
                _logger = logger;
            }

            // HTTP Get action to fetch all the sensors data
            [HttpGet]
            [Authorize] // applying authorization attribute to the action
            public IActionResult Get()
            {
                try
                {
                    _logger.LogInformation("---------*********Get All SensorsData*********-----------"); // logging the information

                    var data = _dbContext.Sensors.Skip(_dbContext.Sensors.Count() - 10).Take(10); // fetching the last 10 data records from the database

                    return Ok(data); // returning the data as an HTTP OK response
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message); // logging the exception message
                    return BadRequest(ex.Message); // returning the exception message as an HTTP Bad Request response
                }
            }

            // HTTP Get action to fetch the sensor data by Id
            [HttpGet("{id}")]
            public IActionResult Get(long id)
            {
                try
                {
                    _logger.LogInformation("-----------**********Get sensors Data by Id**********------------"); // logging the information
                    var Sensor = _dbContext.Sensors.Find(id); // fetching the sensor data from the database by its id
                    if (Sensor == null)
                    {
                        return NotFound("No data against the id"); // returning the HTTP Not Found response if the sensor data is not found
                    }
                    return Ok(Sensor); // returning the sensor data as an HTTP OK response
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message); // logging the exception message
                    return BadRequest(ex.Message); // returning the exception message as an HTTP Bad Request response
                }
            }
            [HttpPost]
            public IActionResult Post([FromBody] Sensor sensor)
            {
                try
                {
                    // Log information about posting sensor data
                    _logger.LogInformation("-----------**********Posting the sensors Data**********------------");

                    // Add the new sensor to the database context
                    _dbContext.Sensors.Add(sensor);

                    // Save changes to the database
                    _dbContext.SaveChanges();

                    // Return 201 status code (created)
                    return StatusCode(StatusCodes.Status201Created);
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    _logger.LogInformation(ex.Message);

                    // Return a bad request with the exception message
                    return BadRequest(ex.Message);
                }
            }

            [HttpPut("{id}")]
            public IActionResult Put(long id, [FromBody] Sensor sensorObj)
            {
                try
                {
                    // Log information about updating sensor data
                    _logger.LogInformation("-----------**********Updating the sensors Data**********------------");

                    // Find the sensor in the database by id
                    var Sensor = _dbContext.Sensors.Find(id);

                    // Update the sensor's properties
                    Sensor.Mapped_Floor = sensorObj.Mapped_Floor;
                    Sensor.Dust_Parcticle = sensorObj.Dust_Parcticle;
                    Sensor.Co2 = sensorObj.Co2;
                    Sensor.Co = sensorObj.Co;
                    Sensor.No2 = sensorObj.No2;
                    Sensor.So2 = sensorObj.So2;

                    // Save changes to the database
                    _dbContext.SaveChanges();

                    // Return success message
                    return Ok("Record Updated Successfully");
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    _logger.LogInformation(ex.Message);

                    // Return a bad request with the exception message
                    return BadRequest(ex.Message);
                }
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(long id)
            {
                try
                {
                    // Log information about deleting sensor data
                    _logger.LogInformation("-----------**********Deleting the sensors Data**********------------");

                    // Find the sensor in the database by id
                    var Sensor = _dbContext.Sensors.Find(id);

                    // Remove the sensor from the database
                    _dbContext.Sensors.Remove(Sensor);

                    // Save changes to the database
                    _dbContext.SaveChanges();

                    // Return success message
                    return Ok("Record Deleted Successfully");
                }
                catch (Exception ex)
                {
                    // Log the exception message
                    _logger.LogInformation(ex.Message);

                    // Return a bad request with the exception message
                    return BadRequest(ex.Message);
                }
            }

        
    }
}
