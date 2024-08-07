using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CoordinateSharp;
using NuGet.Frameworks;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoordinateSharp_UnitTests
{
    [TestClass]
    public class Coordinate_Initialization_Tests
    {
        /// <summary>
        /// Test base coordinate initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initializes_Without_Exceptions()
        {

            Coordinate c = new Coordinate();
            c = new Coordinate(25, 25);
            c = new Coordinate(25, 25, new DateTime(2018, 8, 5, 10, 10, 0));

            EagerLoad eg = new EagerLoad();

            eg.Cartesian = false;
            eg.Celestial = false;
            eg.UTM_MGRS = false;

            c = new Coordinate(eg);
            c = new Coordinate(25, 25, eg);
            c = new Coordinate(25, 25, new DateTime(2018, 8, 5, 10, 10, 0), eg);

        }

        /// <summary>
        /// Test base coordinate part initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Initializes_Without_Exceptions()
        {

            Coordinate c = new Coordinate();
            CoordinatePart cp = new CoordinatePart(CoordinateType.Lat);
            cp = new CoordinatePart(CoordinateType.Long);
            cp = new CoordinatePart(25, CoordinateType.Lat);
            cp = new CoordinatePart(25, CoordinateType.Long);
            cp = new CoordinatePart(25, 25, CoordinatesPosition.N);
            cp = new CoordinatePart(25, 25, CoordinatesPosition.E);
            cp = new CoordinatePart(25, 25, CoordinatesPosition.S);
            cp = new CoordinatePart(25, 25, CoordinatesPosition.W);
            cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.N);
            cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.E);
            cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.S);
            cp = new CoordinatePart(25, 25, 25, CoordinatesPosition.W);

        }

        //Test has high chance to catch random exception issues in circumpolar region.
        //Long running however, so comment out until final tests ran if needed
        [TestMethod]
        public void CoordinateInitExceptionCheckNPole()
        {
            List<Task> tasks = new List<Task>();

            for (int i1 = -180; i1 < 180; i1++)
            {
                DateTime dateTime1 = new DateTime(2024, 1, 1);

                for (int i = 0; i < 365; i++)
                {
                    int currentI1 = i1;
                    DateTime currentDate = dateTime1.AddDays(i);

                    // Initialize the Coordinate object within a Task
                    Task coordinateTask = Task.Run(() =>
                    {                      
                        var coordinate = new Coordinate(90, currentI1, currentDate);
                        coordinate.Offset = 2;
                        return coordinate;
                    });

                    tasks.Add(coordinateTask);
                }
            }

            try
            {
                Task.WhenAll(tasks).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Task threw an exception: {ex}");
            }
        }

        /// <summary>
        /// Test UTM initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void UTM_Initializes_Without_Exceptions()
        {

            UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8);
            utm = new UniversalTransverseMercator("Q", 14, 581943.5, 2111989.8, 6378160.000, 298.25);
            utm = new UniversalTransverseMercator("14Q", 581943.5, 2111989.8);
            utm = new UniversalTransverseMercator("14Q", 581943.5, 2111989.8, 6378160.000, 298.25);
            //ADD UPS

        }
        [TestMethod]
        public void UTM_Initializes_Out_Of_Bounds()
        {

            UniversalTransverseMercator utm = new UniversalTransverseMercator("Q", 0, 581943.5, 2111989.8);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("Q", 61, 581943.5, 2111989.8);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14I", 581943.5, 2111989.8);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14Q", 15999, 2111989.8);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14Q", 834001, 2111989.8);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14Q", 832000, -1);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14Q", 832000, 10000001);
            Assert.IsTrue(utm.Out_Of_Bounds);

            //UPS
            utm = new UniversalTransverseMercator("14A", 886999, 3113000);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14A", 3113001, 3113000);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14A", 887000, 3113001);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("14A", 887000, 886999);
            Assert.IsTrue(utm.Out_Of_Bounds);

            utm = new UniversalTransverseMercator("Q", 45, 581943.5, 2111989.8);
            Assert.IsFalse(utm.Out_Of_Bounds);

        }
        /// <summary>
        /// Test MGRS initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void MGRS_Initializes_Without_Exceptions()
        {
            MilitaryGridReferenceSystem mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264);
            mgrs = new MilitaryGridReferenceSystem("T", 19, "CE", 51307, 93264, 6378160.000, 298.25);
            mgrs = new MilitaryGridReferenceSystem("19T", "CE", 51307, 93264);
            mgrs = new MilitaryGridReferenceSystem("19T", "CE", 51307, 93264, 6378160.000, 298.25);
            //ADD MGRS POLAR

        }
        /// <summary>
        /// Test Cartesian initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Cartesian_Initializes_Without_Exceptions()
        {
            Coordinate c = new Coordinate();
            Cartesian cart = new Cartesian(c);
            cart = new Cartesian(345, -123, 2839);
        }
        /// <summary>
        /// Test ECEF initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void ECEF_Initializes_Without_Exceptions()
        {
            Coordinate c = new Coordinate();
            ECEF ecef = new ECEF(c);
            ecef = new ECEF(3194.469, 3194.469, 4487.419);
        }
        
        /// <summary>
        /// Test GEOREF initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void GEOREF_Initializes_Without_Exceptions()
        {
            Coordinate c = new Coordinate();
            GEOREF geo = new GEOREF(c.GEOREF.Quad_15, c.GEOREF.Quad_1, c.GEOREF.Easting, c.GEOREF.Northing);
        }

        /// <summary>
        /// Test Web Mercator initialization to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Web_Mercator_Initializes_Without_Exceptions()
        {
            WebMercator wmc = new WebMercator(581943.5, 2111989.8);   
        }

       
        /// <summary>
        /// Tests to ensure coordinate cannot initialize outside of allowed system ranges.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initialize_Within_Allowed_Ranges()
        {
            EagerLoad eg = new EagerLoad();
            Coordinate c = new Coordinate(90, 180);
            c = new Coordinate(-90, -180);
            c = new Coordinate(90, 180, new DateTime());
            c = new Coordinate(-90, -180, new DateTime());
            c = new Coordinate(90, 180, eg);
            c = new Coordinate(-90, -180, eg);
            c = new Coordinate(90, 180, new DateTime(), eg);
            c = new Coordinate(-90, -180, new DateTime(), eg);
        }
        /// <summary>
        /// Tests to ensure coordinate throws exceptions when limits are exceeded.
        /// </summary>
        [TestMethod]
        public void Coordinate_Throws_ArguementOutOfRangeException_When_Ranges_Exceed()
        {

            Coordinate c;
            EagerLoad eg = new EagerLoad();
            string failMsg = "Coordinate initialized with exceeded limitations.";

            //Should fail as arguments are out of range.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(90, 181), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-91, -180), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-90, -181), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(90, 181, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-91, -180, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-90, -181, new DateTime()), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(91, 180, new DateTime(), eg), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(90, 181, new DateTime(), eg), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-91, -180, new DateTime(), eg), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => c = new Coordinate(-90, -181, new DateTime(), eg), failMsg);

        }
        /// <summary>
        /// Tests to ensure Web Mercator throws exceptions when limits are exceeded.
        /// </summary>
        [TestMethod]
        public void Web_Mercator_Throws_ArguementOutOfRangeException_When_Ranges_Exceed()
        {

            WebMercator wmc;
            EagerLoad eg = new EagerLoad();
            string failMsg = "Web Mercator initialized with exceeded limitations.";

            //Should fail as arguments are out of range.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => wmc = new WebMercator(-20037508.342789, -20037508.34279), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => wmc = new WebMercator(-20037508.34279, -20037508.342789), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => wmc = new WebMercator(20037508.342789, 20037508.34279), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => wmc = new WebMercator(20037508.34279, 20037508.342789), failMsg);
        
        }
        /// <summary>
        /// Tests to ensure GEOREF throws exceptions when limits are exceeded.
        /// </summary>
        [TestMethod]
        public void GEOREF_Throws_ArguementOutOfRangeException_When_Ranges_Exceed()
        {

            GEOREF geo;
            EagerLoad eg = new EagerLoad();
            string failMsg = "GEOREF initialized with exceeded limitations.";

            //Should fail as arguments are out of range.
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SZ", "FB", "145200", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "FZ", "145200", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("!A", "FB", "145200", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "!B", "145200", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "!B", "Z", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "!B", "145200", "Z"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("", "FB", "145200", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", " ", "145200", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "FB", "", "367200"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "FB", "145200", " "), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "FB", "145200", "3672"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "FB", "745200", "3672"), failMsg);
            Assert.ThrowsException<FormatException>(() => geo = new GEOREF("SA", "FB", "145200", "7672"), failMsg);

        }
        /// <summary>
        /// Tests GEOREF String Precision calls
        /// </summary>
        [TestMethod]
        public void GEOREF_String_Precision()
        {

            GEOREF geo = new GEOREF("SH", "FB", "145200", "367200");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => geo.ToString(-1), "Precision may not be less than 0");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => geo.ToString(11), "Precision may not be greater than 10");
            Assert.AreEqual(geo.ToString(0).Length, 4);
            Assert.AreEqual(geo.ToString(10).Length, 24);
            Assert.AreEqual(geo.ToString(8).Length, 20);
        }
        /// <summary>
        /// Tests to ensure coordinate part cannot initialize outside of allowed system ranges.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Initialize_Within_Allowed_Ranges()
        {
            Coordinate c = new Coordinate();
            CoordinatePart cp = new CoordinatePart(90, CoordinateType.Lat);
            cp = new CoordinatePart(-90, CoordinateType.Lat);
            cp = new CoordinatePart(89, 59, CoordinatesPosition.N);
            cp = new CoordinatePart(89, 59, CoordinatesPosition.S);
            cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.N);
            cp = new CoordinatePart(89, 59, 59, CoordinatesPosition.S);
            cp = new CoordinatePart(180, CoordinateType.Long);
            cp = new CoordinatePart(-180, CoordinateType.Long);
            cp = new CoordinatePart(179, 59, CoordinatesPosition.E);
            cp = new CoordinatePart(179, 59, CoordinatesPosition.W);
            cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.E);
            cp = new CoordinatePart(179, 59, 59, CoordinatesPosition.W);
        }
        /// <summary>
        /// Tests to ensure coordinate part throws exceptions when limits are exceeded.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Throws_ArguementOutOfRangeException_When_Ranges_Exceed()
        {
            CoordinatePart cp;
            string failMsg = "CoordinatePart initialized with exceeded limitations.";

            //Should fail as arguments are out of range.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, CoordinateType.Lat), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-91, CoordinateType.Lat), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, CoordinateType.Long), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-181, CoordinateType.Long), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, CoordinatesPosition.N), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, CoordinatesPosition.S), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.N), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.N), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(91, 0, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(90, 0, 1, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 59, 60, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-90, 0, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, -1, 0, CoordinatesPosition.S), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(89, 1, -1, CoordinatesPosition.S), failMsg);


            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, CoordinatesPosition.E), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, CoordinatesPosition.W), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.E), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.E), failMsg);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(181, 0, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(180, 0, 1, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 59, 60, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(-180, 0, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, -1, 0, CoordinatesPosition.W), failMsg);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => cp = new CoordinatePart(179, 1, -1, CoordinatesPosition.W), failMsg);

        }
        
        /// <summary>
        /// Tests to ensures properties change when a new CoordinatePart is initialized and assigned to a Coordinate object.
        /// </summary>
        [TestMethod]
        public void CoordinatePart_Constructor_Property_Checks()
        {
            Coordinate coord = new Coordinate();

            double lat = coord.Latitude.DecimalDegree;
            double lng = coord.Longitude.DecimalDegree;
            string MGRS = coord.MGRS.ToString();
            string UTM = coord.UTM.ToString();
            string ECEF = coord.ECEF.ToString();
            string Cartesian = coord.Cartesian.ToString();
            DateTime? cel = coord.CelestialInfo.MoonSet;

            CoordinatePart cpLat = new CoordinatePart(25, CoordinateType.Lat);
            CoordinatePart cpLng = new CoordinatePart(25, CoordinateType.Long);

            //PROP CHANGE ERROR CHECK

            cpLat.DecimalDegree = 27;
            cpLng.Seconds = 24;

            coord.Latitude = cpLat;
            Assert.AreNotEqual(lat, coord.Latitude.ToDouble());
           
            coord.Longitude = cpLng;
            Assert.AreNotEqual(lng, coord.Longitude.ToDouble());

            Assert.AreNotEqual(MGRS, coord.MGRS.ToString()); 
            Assert.AreNotEqual(UTM, coord.UTM.ToString());
            Assert.AreNotEqual(ECEF, coord.ECEF.ToString());
            Assert.AreNotEqual(Cartesian, coord.Cartesian.ToString());
            Assert.AreNotEqual(cel, coord.CelestialInfo.MoonSet);
        }

       
        /// <summary>
        /// Tests to Coordinate object initializes as specified in the Global Settings.
        /// </summary>
        [TestMethod]
        public void Global_Settings_Initialization_Check()
        {
            GlobalSettings.Default_CoordinateFormatOptions = new CoordinateFormatOptions() { Format = CoordinateFormatType.Decimal };
            GlobalSettings.Default_EagerLoad = new EagerLoad(false);
            GlobalSettings.Default_Cartesian_Type = CartesianType.ECEF;
            GlobalSettings.Default_Parsable_Formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.MGRS;
            GlobalSettings.Set_DefaultDatum(Earth_Ellipsoid_Spec.Airy_1830);

            Earth_Ellipsoid ee = Earth_Ellipsoid.Get_Ellipsoid(Earth_Ellipsoid_Spec.WGS84_1984);

            Coordinate c = new Coordinate(25, 25);

            //Ensure setting took
            Assert.AreEqual("25 25", c.ToString());
            Assert.AreEqual(null, c.CelestialInfo);
            Assert.AreNotEqual(ee.Equatorial_Radius, c.Equatorial_Radius);
            Assert.AreNotEqual(ee.Inverse_Flattening, c.Inverse_Flattening);
            Assert.ThrowsException<FormatException>(()=> { Coordinate.Parse("10T 573104mE 5249221mN"); });
           
            //Reset to continue tests
            GlobalSettings.Default_CoordinateFormatOptions = new CoordinateFormatOptions() { Format = CoordinateFormatType.Degree_Minutes_Seconds };
            GlobalSettings.Default_EagerLoad = new EagerLoad(true);
            GlobalSettings.Default_Parsable_Formats = Allowed_Parse_Format.Lat_Long | Allowed_Parse_Format.MGRS | Allowed_Parse_Format.UTM |
            Allowed_Parse_Format.Cartesian_ECEF | Allowed_Parse_Format.Cartesian_Spherical |
            Allowed_Parse_Format.WebMercator | Allowed_Parse_Format.GEOREF;
            GlobalSettings.Set_DefaultDatum(Earth_Ellipsoid_Spec.WGS84_1984);
            GlobalSettings.Default_Cartesian_Type = CartesianType.Cartesian; //If not reverted other test will fail.
        }
        /// <summary>
        /// Tests Coordinate object initializes as specified in the Global Settings that specify to run in local environments time.
        /// </summary>
        [TestMethod]
        public void Global_Settings_Local_Environment_Time_Settings()
        {
            GlobalSettings.Allow_Coordinate_DateTimeKind_Specification = true;
            Coordinate c = new Coordinate(47.36, -122.06, new DateTime(2023, 03, 02));
            double delta = 100;
         
            TimeSpan sr = new DateTime(2023, 3, 2, 6, 48, 30) - c.CelestialInfo.SunRise.Value;
            Assert.AreEqual(0,sr.TotalSeconds, delta);

            TimeSpan ss = new DateTime(2023, 3, 2, 17, 55, 16) - c.CelestialInfo.SunSet.Value;
            Assert.AreEqual(0, ss.TotalSeconds, delta);

            Assert.ThrowsException<InvalidOperationException>(() => c.Offset = c.Offset + 1);

            GlobalSettings.Allow_Coordinate_DateTimeKind_Specification = false;


        }
        /// <summary>
        /// Test base coordinate initialization with new date to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initializes_With_New_Date_Without_Exceptions()
        {

            Coordinate c = new Coordinate();
            c = new Coordinate(25, 25, new DateTime());

        }

        /// <summary>
        /// Test base coordinate initialization with high date to ensure no exceptions are thrown.
        /// </summary>
        [TestMethod]
        public void Coordinate_Initializes_With_High_Date_Without_Exceptions()
        {
            Coordinate c = new Coordinate();
            c = new Coordinate(25, 25, new DateTime(3999, 8, 5, 10, 10, 0));        
        }


    }
}
