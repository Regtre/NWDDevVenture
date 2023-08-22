namespace NWDServerFront.Controllers;

public class DataName
{
    public string name { set; get; }
    
    public DataName(){}

    public override string ToString()
    {
        return $"My name is {name} !";
    }
}