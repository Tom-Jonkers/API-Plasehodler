using System.Text.Json.Serialization;
using Super_Cartes_Infinies.Models;

namespace Models.Models;

public class PlayableCardStatus
{
    public int Id { get; set; }

    public int Value { get; set; }
    
    public virtual Status Status { get; set; }
    
    public int StatusId { get; set; }
    
    [JsonIgnore]
    public virtual PlayableCard PlayableCard { get; set; }
    
    public int PlayableCardId { get; set; }

}