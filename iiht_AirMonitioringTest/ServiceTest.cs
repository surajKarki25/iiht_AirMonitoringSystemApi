using iiht_AirMonitoringSystemApi.Controllers;
using iiht_AirMonitoringSystemApi.Data;
using iiht_AirMonitoringSystemApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace iiht_AirMonitioringTest
{
    public class ServiceTest
    {
        // Variables for the options of in-memory database
        private static DbContextOptions<SensorDbContext> dbContextOptions = new DbContextOptionsBuilder<SensorDbContext>()
            .UseInMemoryDatabase(databaseName: "AQMSDbTest")
            .Options;

        // Declaring variables for context and controller
        SensorDbContext context;
        AirQController AirQController;

        // Variable for mock logger
        private readonly Mock<ILogger<AirQController>> loggermock = new Mock<ILogger<AirQController>>();

        // One-time setup method that sets up the context and seed data
        [OneTimeSetUp]
        public void Setup()
        {
            context = new SensorDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            // Seed the database with sample data
            SeedDatabase();

            // Initialize the AirQController
            AirQController = new AirQController(context, loggermock.Object);
        }

        // Test method to check if the GET request returns a list of all sensors
        [Test, Order(1)]
        public void HttpGET_GetAllSensors()
        {
            // Get the result of the GET request
            IActionResult result = AirQController.Get();

            // Assert that the result is of type OkObjectResult and contains a list of sensors
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var ResultData = (result as OkObjectResult).Value as IEnumerable<Sensor>;
            Assert.That(ResultData.Count, Is.EqualTo(6));
            Assert.That(ResultData.First().Id, Is.EqualTo(1));
        }

        // Test method to check if the PUT request updates a sensor by its Id
        [Test, Order(2)]
        public void HttpPUT_UpdateSensors_byId()
        {
            // Create a new sensor with updated values
            var Sensor = new Sensor()
            {
                Id = 1,
                Mapped_Floor = 1,
                Dust_Parcticle = 24,
                Co2 = 35,
                Co = 30,
                No2 = 321,
                So2 = 11
            };

            // Call the PUT request to update the sensor
            AirQController.Put(1, Sensor);

            // Get the updated list of all sensors
            IActionResult GetAll1 = AirQController.Get();
            var Getalldata1 = (GetAll1 as OkObjectResult).Value as IEnumerable<Sensor>;

            // Assert that the first sensor's Id is 1
            Assert.That(Getalldata1.First().Id, Is.EqualTo(1));
        }
        //Test case to verify Http GET method to retrieve all sensors by id 
        [Test, Order(3)]
        public void HttpGET_GetAllSensorsById()
        {
            //Call the Get method to retrieve sensor data with id 1
            IActionResult result2 = AirQController.Get(1);
            //Check the result type is of OKObjectResult
            Assert.That(result2, Is.TypeOf<OkObjectResult>());
            //Cast the result to type Sensor
            var resultdata2 = (result2 as OkObjectResult).Value as Sensor;
            //Assert that the sensor id is equal to 1
            Assert.That(resultdata2.Id, Is.EqualTo(1));
        }

        //Test case to verify Http DELETE method to delete sensor by id
        [Test, Order(4)]
        public void HttpDelete_DeleteSensorsById()
        {
            //Set the id to delete
            int Id = 1;
            //Call the delete method with id
            IActionResult Result3 = AirQController.Delete(Id);
            //Check the result type is of OKObjectResult
            Assert.That(Result3, Is.TypeOf<OkObjectResult>());
        }

        //Test case to verify Http POST method to add sensor data
        [Test, Order(5)]
        public void HttpPost_AddSensorData()
        {
            //Create new Sensor object
            var Sensor = new Sensor()
            {
                Id = 1,
                Mapped_Floor = 2,
                Dust_Parcticle = 44,
                Co2 = 54,
                Co = 64,
                No2 = 74,
                So2 = 84

            };
            //Call the post method to add sensor data
            AirQController.Post(Sensor);
            //Call the get method to retrieve all sensor data
            IActionResult GetAll = AirQController.Get();
            //Cast the result to type IEnumerable<Sensor>
            var Getalldata = (GetAll as OkObjectResult).Value as IEnumerable<Sensor>;
            //Assert that the last sensor id is equal to 6
            Assert.That(Getalldata.Last().Id, Is.EqualTo(6));
        }

        //One time method to clean up the database after all tests have run
        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        //Method to seed the database with initial data
        private void SeedDatabase()
        {
            var Sensors = new List<Sensor>
            {
                new Sensor()
                {
                    Id = 1,
                    Mapped_Floor = 1,
                    Dust_Parcticle =56,
                    Co2 = 66,
                    Co = 76,
                    No2 = 86,
                    So2 = 96
                },
                 new Sensor()
                {
                    Id = 2,
                    Mapped_Floor = 2,
                    Dust_Parcticle =44,
                    Co2 = 54,
                    Co = 64,
                    No2 = 74,
                    So2 = 84
                },
                  new Sensor()
                {

                      Id = 3,
                    Mapped_Floor = 3,
                    Dust_Parcticle =33,
                    Co2 = 44,
                    Co = 55,
                    No2 = 66,
                    So2 = 77
                },
                   new Sensor()
                {
                    Id = 4,
                    Mapped_Floor = 1,
                    Dust_Parcticle =11,
                    Co2 = 12,
                    Co = 13,
                    No2 = 14,
                    So2 = 15
                },
                    new Sensor()
                {
                    Id = 5,
                    Mapped_Floor = 2,
                    Dust_Parcticle = 98,
                    Co2 = 88,
                    Co = 78,
                    No2 = 45,
                    So2 = 66
                },
                     new Sensor()
                {
                    Id = 6,
                    Mapped_Floor = 3,
                    Dust_Parcticle =65,
                    Co2 = 75,
                    Co = 85,
                    No2 = 55,
                    So2 = 67
                }
            };
            context.Sensors.AddRange(Sensors);


            context.SaveChanges();
        }
    }
}