using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherApplication.HuntingModel;
using WeatherApplication;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class HuntingDataTests
    {
        // Arrange
        List<HuntingSeason> expectedHuntingSeasons = new List<HuntingSeason> //the list is being reuse, that is why it moved here
            {
                new HuntingSeason("Waterfowl", "May","June (North Island)", "Mallard; Canada Geese; Australian Shoveler; Paradise Shelduck; Pacific Black Duck; Black Swan."),
                new HuntingSeason("Waterfowl", "May","July (South Island)", "Mallard; Canada Geese; Australian Shoveler; Paradise Shelduck; Pacific Black Duck; Black Swan."),
                new HuntingSeason("Turkey", "All Year","", "August through December spring turkey hunting with no competition; pairs well with trout fishing.  North Island."),
                new HuntingSeason("Upland Gamebirds", "May","July", "Pheasant; Quail; Pukeko; Peacock.  North Island."),
                new HuntingSeason("Red Stag", "February", "August (Rut March to April)", "Combines easily with waterfowl; fallow deer and other big game hunts.  North Island & South Island."),
                new HuntingSeason("Fallow Deer", "March", "September (Rut in April)", "Commonly taken during Red Deer hunts.  North Island."),
                new HuntingSeason("Himalayan Tahr", "April","August (Rut in April and May)", "May be hunted all year but designated dates are when capes are prime.  Commonly paired with Chamois hunting.  North Island & South Island."),
                new HuntingSeason("European Chamois", "April","August (Rut in April and May)", "Dates indicate prime capes.  Commonly hunted in conjunction with Tahr.  North Island & South Island."),
                new HuntingSeason("Sika Stag", "Late February","May (Rut in April)", "Winter hunting through September can be rewarding; but snow is likely.  North Island."),
                new HuntingSeason("Sambar Stag", "Mid August","September", "For 6 weekends only.  Average 50% on trophy stags. North Island."),
                new HuntingSeason("Rusa Stag", "Rut July","August", "Popularly hunted in conjunction with winter hunt for Tahr; Sambar; or Sika.  North Island."),
                new HuntingSeason("Trout Fishing", "All Year","", "October through April peak trophy fishing.")
            };

        [Test]
        public void DeserializeHuntingData_ReturnsHuntingDataObject()
        {

            // Act
            List<HuntingSeason> actualHuntingSeasons = HuntingModel.ParseHuntingSeasonData("hunting_season_data.txt");

            // Assert
            Assert.That(actualHuntingSeasons, Is.Not.Null);
            Assert.That(actualHuntingSeasons.Count, Is.EqualTo(expectedHuntingSeasons.Count));

            for (int i = 0; i < expectedHuntingSeasons.Count; i++)
            {
                Assert.That(actualHuntingSeasons[i].Species, Is.EqualTo(expectedHuntingSeasons[i].Species));
                Assert.That(actualHuntingSeasons[i].HuntingStarts, Is.EqualTo(expectedHuntingSeasons[i].HuntingStarts));
                Assert.That(actualHuntingSeasons[i].HuntingEnds, Is.EqualTo(expectedHuntingSeasons[i].HuntingEnds));
                Assert.That(actualHuntingSeasons[i].Notes, Is.EqualTo(expectedHuntingSeasons[i].Notes));
            }
        }
        [Test]
        public void ExtractingMonthTest()
        {
            string stringToExtract1 = "dog May bird April season";
            string stringToExtract2 = "All Year";
            string result1 = HuntingModel.ExtractMonth(stringToExtract1);
            string result2 = HuntingModel.ExtractMonth(stringToExtract2);
            Assert.That(result1, Is.EqualTo("May"));
            Assert.That(result1, Is.Not.EqualTo("April"));
            Assert.That(result2, Is.EqualTo(string.Empty));
        }
        [Test]
        public void FilterHuntingListTest()
        {
            List<HuntingSeason> allSeasonsMock = new List<HuntingSeason> //this list is easier to manage for the particular test
            {
                new HuntingSeason("Sambar Stag", "Mid August","September", "For 6 weekends only.  Average 50% on trophy stags. North Island."),
                new HuntingSeason("Rusa Stag", "Rut July","August", "Popularly hunted in conjunction with winter hunt for Tahr; Sambar; or Sika.  North Island."),
                new HuntingSeason("Trout Fishing", "All Year","", "October through April peak trophy fishing.")
            };
            List<HuntingSeason> filteredList1 = FilterHuntingSeasons(allSeasonsMock, "January"); //Should give you only 3d line
            List<HuntingSeason> filteredList2 = FilterHuntingSeasons(allSeasonsMock, "July"); //Should give2nd and 3d lines
            List<HuntingSeason> filteredList3 = FilterHuntingSeasons(allSeasonsMock, "September");//Should give 1st and 3d lines
            List<HuntingSeason> filteredList4 = FilterHuntingSeasons(allSeasonsMock, "August");//Should give all lines
            Assert.That(filteredList1.Count, Is.EqualTo(1));
            Assert.That(filteredList2.Count, Is.EqualTo(2));
            Assert.That(filteredList3.Count, Is.EqualTo(2));
            Assert.That(filteredList4.Count, Is.EqualTo(3));
        }
        [Test]
        public void WrongInputTest()
        {
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            HuntingModel.FilterHuntingSeasons(expectedHuntingSeasons, "MisspeltInput");
            Assert.That(consoleOutput.ToString(), Is.EqualTo("The input was misspelt\r\n"));
            //\r\n" characters represent a newline in Windows-style line endings
        }
        [Test]
        public void EmptyInputTest()
        {
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            HuntingController huntingController = new HuntingController();
            huntingController.RefreshHuntingSeasonData("hunting_season_data.txt", "");
            Assert.That(consoleOutput.ToString(), Is.EqualTo("The input was empty\r\n"));
            //\r\n" characters represent a newline in Windows-style line endings
        }
    }

}
