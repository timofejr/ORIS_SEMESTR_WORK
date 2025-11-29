namespace MiniTemplateEngine.UnitTests;

[TestClass]
public sealed class HtmlTemplateRendererTests
{
    [TestMethod]
    public void RenderFromString_WhenOnlyVariablesWithObjectPropertyStyle_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = "<li>$( user.name )</li> <li>$( user.surname )</li>";
        var data = new
        {
            User = new {
                Name = "Timur",
                Surname = "Roger"
            }
        };

        var expectedResult = "<li>Timur</li> <li>Roger</li>";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }
    
    [TestMethod]
    public void RenderFromString_WhenOnlyVariablesWithPropertyStyle_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = "<li>$( name )</li> <li>$( surname )</li>";
        var data = new
        {
            Name = "Timur",
            Surname = "Roger"
        };

        var expectedResult = "<li>Timur</li> <li>Roger</li>";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }
    
    [TestMethod]
    public void RenderFromString_WhenOnlyVariablesWithPropertyStyleWithMultipleNesting_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = "<li>$( user.name )</li> <li>$( user.surname )</li> <li>$( user.location.country )</li> " +
                           "<li>$( user.location.city )</li>";
        var data = new
        {
            User = new {
                Name = "Timur",
                Surname = "Roger",
                
                Location = new
                {
                    Country = "Russia",
                    City = "Moscow"
                }
            }
        };

        var expectedResult = "<li>Timur</li> <li>Roger</li> <li>Russia</li> " +
                             "<li>Moscow</li>";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    public void RenderFromString_WhenForeach_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = "$foreach(var item in user.Items)\n<li>$(item.Name)</li>\n$endfor\n";
        var data = new
        {
            User = new {
                Name = "Timur",
                Surname = "Roger",
                
                Location = new
                {
                    Country = "Russia",
                    City = "Moscow"
                },
                            
                Items = new[]
                {
                    new { Name = "Petya" },
                    new { Name = "Vanya" },
                    new { Name = "Huesos"}
                }
            }
        };

        var expectedResult = @"<li>Petya</li>
<li>Vanya</li>
<li>Huesos</li>
";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult);
    }

    [TestMethod]
    public void RenderFromString_WhenForeachAndVariables_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = @"$foreach(var item in user.Items)
<li>$(item.Name )</li>
$endfor
<h1>$(user.Surname )</h1>
<h1>$(user.Location.Country )</h1>
";
        var data = new
        {
            User = new {
                Name = "Timur",
                Surname = "Roger",
                
                Location = new
                {
                    Country = "Russia",
                    City = "Moscow"
                },
                            
                Items = new[]
                {
                    new { Name = "Petya" },
                    new { Name = "Vanya" },
                    new { Name = "Huesos"}
                }
            }
        };

        var expectedResult = @"<li>Petya</li>
<li>Vanya</li>
<li>Huesos</li>
<h1>Roger</h1>
<h1>Russia</h1>
";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
    
    [TestMethod]
    public void RenderFromString_WhenMultipleForeach_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = @"$foreach(var user in user.Items)
<li>$(user.Name)</li>
$endfor
$foreach(var car in car.Items)
<li>$(car.Name)</li>
$endfor
";
        var data = new
        {
            User = new {
                Items = new[]
                {
                    new { Name = "Petya" },
                    new { Name = "Vanya" },
                    new { Name = "Huesos"}
                }
            },
            Car = new
            {
                Items = new[]
                {
                    new { Name = "KIA" },
                    new { Name = "Renault" },
                    new { Name = "Huinday" }
                }
            }
        };

        var expectedResult = @"<li>Petya</li>
<li>Vanya</li>
<li>Huesos</li>
<li>KIA</li>
<li>Renault</li>
<li>Huinday</li>
";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }

    [TestMethod]
    public void RenderFromString_WhenConditionIsTrue_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = @"$if(user.IsActive)
<p>User is active</p>
$else
<p>User is not active</p>
$endif
";
        var data = new
        {
            User = new {
                IsActive = true
            }
        };

        var expectedResult = @"<p>User is active</p>
";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
    
    [TestMethod]
    public void RenderFromString_WhenConditionIsFalse_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = @"$if(user.IsActive)
<p>User is active</p>
$else
<p>User is not active</p>
$endif
";
        var data = new
        {
            User = new {
                IsActive = false
            }
        };

        var expectedResult = @"<p>User is not active</p>
";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
    
    [TestMethod]
    public void RenderFromString_WhenConditionWithOperator_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = @"$if(user.Name != 'Vladimir')
<p>User is president</p>
$else
<p>User is not president</p>
$endif
";
        var data = new
        {
            User = new {
                Name = "Vladimir"
            }
        };

        var expectedResult = @"<p>User is not president</p>
";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
    
    [TestMethod]
    public void RenderFromString_WhenForeachAndCondition_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtml = @"
$foreach(var item in users.Items)
$if(item.IsActive)
<li>$( item.Name ) - active</li>
$else
<li>$( item.Name ) - nonactive</li>
$endif
$endfor
";
        var data = new
        {
            Users = new {
                Items =  new[]
                {
                    new 
                    {
                        IsActive = true,
                        Name = "Timur"
                    },
                
                    new
                    {
                        IsActive = true,
                        Name = "Timerkhan"                    
                    },
                
                    new
                    {
                        IsActive = false,
                        Name = "Putin"
                    },
                
                    new
                    {
                        IsActive = true,
                        Name = "Stalin"
                    }
                }
            },
        };

        var expectedResult = @"
<li>Timur - active</li>
<li>Timerkhan - active</li>
<li>Putin - nonactive</li>
<li>Stalin - active</li>
";
        
        // Act
        var actualResult = renderer.RenderFromString(templateHtml, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }

    [TestMethod]
    public void RenderFromFile_WhenSingleVariable_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtmlPath = "Static/WhenSingleVariable/inputTestFile.html";
        var expectedResult = File.ReadAllText("Static/WhenSingleVariable/outputTestFile.html");
            
        var data = new
        {
            User = new {
                Name = "Vanya"
            }
        };
        
        // Act
        var actualResult = renderer.RenderFromFile(templateHtmlPath, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
    
    [TestMethod]
    public void RenderFromFile_WhenMultipleVariables_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtmlPath = "Static/WhenMultipleVariables/inputTestFile.html";
        var expectedResult = File.ReadAllText("Static/WhenMultipleVariables/outputTestFile.html");
            
        var data = new
        {
            User = new {
                Name = "Vanya"
            },
            Car = new
            {
                Label = "Lada Vesta"
            }
        };
        
        // Act
        var actualResult = renderer.RenderFromFile(templateHtmlPath, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
    
    [TestMethod]
    public void RenderFromFile_WhenSingleForeach_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtmlPath = "Static/WhenSingleForeach/inputTestFile.html";
        var expectedResult = File.ReadAllText("Static/WhenSingleForeach/outputTestFile.html");
            
        var data = new
        {
            Users = new {
                Items = new[]
                {
                    new { Name = "Vanya" },
                    new { Name = "Petya" },
                    new { Name = "Vladimir" },
                }
            },
        };
        
        // Act
        var actualResult = renderer.RenderFromFile(templateHtmlPath, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
    
    [TestMethod]
    public void RenderFromFile_WhenMultipleForeach_ShouldReturnCorrectString()
    {
        // Arrange
        var renderer = new HtmlTemplateRenderer();
        var templateHtmlPath = "Static/WhenMultipleForeach/inputTestFile.html";
        var expectedResult = File.ReadAllText("Static/WhenMultipleForeach/outputTestFile.html");
            
        var data = new
        {
            Users = new {
                Items = new[]
                {
                    new { Name = "Vanya" },
                    new { Name = "Petya" },
                    new { Name = "Vladimir" },
                }
            },
            
            Cars = new
            {
                Items = new[]
                {
                    new { Label = "Vesta" },
                    new { Label = "Jeely" },
                    new { Label = "Mercedes" },
                }
            }
        };
        
        // Act
        var actualResult = renderer.RenderFromFile(templateHtmlPath, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }

    [TestMethod]
    public void RenderFromFile_WhenCondition_ShouldReturnCorrectString()
    {
        var renderer = new HtmlTemplateRenderer();
        var templateHtmlPath = "Static/WhenCondition/inputTestFile.html";
        var expectedResult = File.ReadAllText("Static/WhenCondition/outputTestFile.html");
            
        var data = new
        {
            IsAvailable = false
        };
        
        // Act
        var actualResult = renderer.RenderFromFile(templateHtmlPath, data);
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }

    [TestMethod]
    public void RenderToFile()
    {
        var renderer = new HtmlTemplateRenderer();
        var inputTemplateHtmlPath = "Static/WhenCondition/inputTestFile.html";
        var outputTemplateHtmlPath = "Static/WhenCondition/outputTestFile.html";
        var expectedResult = File.ReadAllText("Static/WhenCondition/outputTestFile.html");
            
        var data = new
        {
            IsAvailable = false
        };
        
        // Act
        var renderedHtmlTemplate = renderer.RenderToFile(inputTemplateHtmlPath, outputTemplateHtmlPath, data);
        var actualResult = File.ReadAllText("Static/WhenCondition/outputTestFile.html");
        
        // Assert
        Assert.AreEqual(expectedResult, actualResult); 
    }
}