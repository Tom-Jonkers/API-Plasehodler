namespace Models.Models;

public class Status
{
    public const int POISON_ID = 1;
    public const int STUNNED_ID = 2;
    public const int CHARM_ID = 3;
    
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Icone { get; set; }
}