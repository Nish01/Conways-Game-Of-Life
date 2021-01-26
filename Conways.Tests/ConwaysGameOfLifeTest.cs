using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Conways.Tests
{
    [TestClass]
    public class ConwaysGameOfLifeTest
    {
        private ConwaysGameOfLife _ConwaysGameOfLifeUnderTest;

        [TestInitialize]
        public void Initialise()
        {
            _ConwaysGameOfLifeUnderTest = new ConwaysGameOfLife(6);
        }

        [TestMethod]
        public void InitialiseTheGameOfLife()
        {
            _ConwaysGameOfLifeUnderTest = new ConwaysGameOfLife(5);

            Assert.AreEqual(5, _ConwaysGameOfLifeUnderTest.BoardSize);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[4,4]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[4,4]);
        }

        [TestMethod]
        public void ToggleCell_True_False()
        {
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 5);

            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 5]);

            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 5);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 5]);
        }

        [TestMethod]
        public void BeginGeneration_ForStartOfLife()
        {
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 2);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 3);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 4);
            _ConwaysGameOfLifeUnderTest.BeginGeneration();

            Assert.AreEqual(0, _ConwaysGameOfLifeUnderTest.Generation);

            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 2]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 3]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 1]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 5]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 4]);

            //000000
            //000000
            //010000
            //010000
            //010000
            //000000
        }

        [TestMethod]
        public void Update_Next_Generation_Cell_StayingAlive()
        {
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 2);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 3);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 4);

            _ConwaysGameOfLifeUnderTest.ToggleCell(3, 3);
            _ConwaysGameOfLifeUnderTest.ToggleCell(3, 4);
            _ConwaysGameOfLifeUnderTest.ToggleCell(3, 5);
            _ConwaysGameOfLifeUnderTest.ToggleCell(4, 5);

            _ConwaysGameOfLifeUnderTest.BeginGeneration();
            _ConwaysGameOfLifeUnderTest.Wait();
            _ConwaysGameOfLifeUnderTest.ProcessNextGeneration();

            Assert.AreEqual(1, _ConwaysGameOfLifeUnderTest.Generation);

            //2 Main checks of the cells with two or three live neighbours lives, unchanged
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 3]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[3, 4]);


            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 1]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 5]);

            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[2, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 3]);

            //000000
            //000000
            //010000
            //010100
            //010100
            //000110

            //000000
            //000000
            //011000
            //0X0100
            //010X00
            //001110
        }

        [TestMethod]
        public void Update_Next_Generation_Cell_Death_By_Overcrowding()
        {
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 1);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 2);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 3);

            _ConwaysGameOfLifeUnderTest.ToggleCell(2, 1);
            _ConwaysGameOfLifeUnderTest.ToggleCell(0, 2);

            _ConwaysGameOfLifeUnderTest.BeginGeneration();
            _ConwaysGameOfLifeUnderTest.Wait();
            _ConwaysGameOfLifeUnderTest.ProcessNextGeneration();

            Assert.AreEqual(1, _ConwaysGameOfLifeUnderTest.Generation);
           
            //1 cell with more than three live neighbours dies
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 2]);
           
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 4]);

            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[2, 1]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[0, 2]);

            //000000
            //011000
            //110000
            //010000
            //000000
            //000000

            //000000
            //011000
            //1X0000
            //010000
            //000000
            //000000
        }


        [TestMethod]
        public void Update_Next_Generation_Cell_Bring_Back_To_Life()
        {
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 2);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 3);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 4);
            _ConwaysGameOfLifeUnderTest.BeginGeneration();
            _ConwaysGameOfLifeUnderTest.Wait();
            _ConwaysGameOfLifeUnderTest.ProcessNextGeneration();

            Assert.AreEqual(1, _ConwaysGameOfLifeUnderTest.Generation);

            //Main checks - Brought back to life due to 3 contact points
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[0, 3]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[2, 3]);


            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 2]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 4]);

            //000000
            //000000
            //010000
            //010000
            //010000
            //000000

            //000000
            //000000
            //000000
            //X1X000
            //000000
            //000000    
        }

        [TestMethod]
        public void Update_Next_Generation_Cell_Dies_Of_loneliness()
        {
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 2);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 3);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 4);
            _ConwaysGameOfLifeUnderTest.BeginGeneration();
            _ConwaysGameOfLifeUnderTest.Wait();
            _ConwaysGameOfLifeUnderTest.ProcessNextGeneration();

            Assert.AreEqual(1, _ConwaysGameOfLifeUnderTest.Generation);

            //2 Deaths by loneliness
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 2]);       
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 4]);

            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 3]);


            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 1]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 5]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 2]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[2, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 4]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[0, 3]);

            //000000
            //000000
            //010000
            //010000
            //010000
            //000000
            
            //000000
            //000000
            //0X0000
            //111000
            //0X0000
            //000000
        }

        [TestMethod]
        public void Update_Next_Generation_Cell_Multiple_Generations()
        {
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 2);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 3);
            _ConwaysGameOfLifeUnderTest.ToggleCell(1, 4);

            _ConwaysGameOfLifeUnderTest.ToggleCell(4, 5);

            _ConwaysGameOfLifeUnderTest.BeginGeneration();
            _ConwaysGameOfLifeUnderTest.Wait();
            _ConwaysGameOfLifeUnderTest.ProcessNextGeneration();

            Assert.AreEqual(1, _ConwaysGameOfLifeUnderTest.Generation);

            //000000
            //000000
            //010000
            //010000
            //010000
            //000010

            //000000
            //000000
            //000000
            //111000
            //000000
            //000000

            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 3]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[2, 3]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[0, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[4, 5]);


            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 1]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 5]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 3]);
      
            //Process next (second) generation
            _ConwaysGameOfLifeUnderTest.ProcessNextGeneration();
            _ConwaysGameOfLifeUnderTest.Wait();

            Assert.AreEqual(2, _ConwaysGameOfLifeUnderTest.Generation);

            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 2]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 3]);
            Assert.IsTrue(_ConwaysGameOfLifeUnderTest[1, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 1]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[1, 5]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 3]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[2, 4]);

            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 2]);
            Assert.IsFalse(_ConwaysGameOfLifeUnderTest[3, 3]);

            //000000
            //000000
            //000000
            //111000
            //000000
            //000000

            //000000
            //000000
            //010000
            //010000
            //010000
            //000000
        }

    }
}
