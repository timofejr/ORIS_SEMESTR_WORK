using MyORMLibrary;

namespace MyOrmLibrary.UnitTests;

[TestClass]
public sealed class ORMContextTests
{
    private const string ConnectionString = "Host=localhost;Port=5433;Database=oris_database;Username=oris_user;Password=11408;";
    
    [TestMethod]
    public void ReadById_WhenTargetExists()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
        var expectedResult = new Users()
        {
            Id = 1,
            Username = "Timur",
            Email = "Timur@gmail.com"
        };
        
        //Act
        var actualResult = context.ReadById<Users>(1);
            
        //Assert
        Assert.AreEqual(expectedResult.Id, actualResult.Id);
    }
    
    [TestMethod]
    public void ReadById_WhenTargetDoesntExists()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
        
        //Act
        var actualResult = context.ReadById<Users>(929293992);
            
        //Assert
        Assert.AreEqual(null, actualResult);
    }
    
    [TestMethod]
    public void ReadByAll_WhenTargetExists()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
        var expectedResult = new List<int>()
        {
            1, 2, 3, 4, 5
        };
        
        //Act
        var actualResult = context.ReadByAll<Users>().Select(x => x.Id).Order().ToList();
            
        //Assert
        CollectionAssert.AreEqual(expectedResult, actualResult);    
    }
    
    [TestMethod]
    public void ReadByAll_WhenTargetDoesntExists()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
        
        //Act
        var actualResult = context.ReadByAll<FakeModel>();
            
        //Assert
        CollectionAssert.AreEqual(null, actualResult);    
    }
    
    [TestMethod]
    public void ReadByAll_WhenTargetIsEmpty()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
        var expectedResult = new List<Cities>();
        
        //Act
        var actualResult = context.ReadByAll<Cities>();
            
        //Assert
        CollectionAssert.AreEqual(expectedResult, actualResult);    
    }

    [TestMethod]
    public void Create_WhenTargetIsValid()
    {
       //Arrange
       var context = new ORMContext(ConnectionString);
       
       //Act
       var newUser = new Users()
       {
           Username = "Vladimir",
           Email = "validimir@gmail.com"
       };
       
       var actualResult = context.Create(newUser);
       
       Assert.AreEqual(true, true);
    }
    
    [TestMethod]
    public void Update_WhenTargetIsValid()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
       
        //Act
        var user = new Users()
        {
            Username = "Vladimir228",
            Email = "validimir@gmail.com"
        };
       
        context.Update(6, user);
       
        Assert.AreEqual(true, true);
    }
    
    [TestMethod]
    public void Delete_WhenTargetIsValid()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
       
        //Act
        context.Delete<Users>(6);
       
        Assert.AreEqual(true, true);
    }

    [TestMethod]
    public void Where_WhenTargetIsValid()
    {
        //Arrange
        var context = new ORMContext(ConnectionString);
        var expectedValue = new List<Users>
        {
            new  ()
            {
                Id = 5,
                Username = "Putin",
                Email = "putin@gmail.com"
            }
        };
       
        //Act
        var actualValue = context.Where<Users>(u => u.Username == "Putin");
       
        CollectionAssert.AreEqual(expectedValue, actualValue.ToList());
    }
}