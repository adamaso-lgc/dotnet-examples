using FieldUp.Domain.ValueObjects;
using Microsoft.VisualBasic.FileIO;

namespace FieldUp.Domain;

public class Field
{
    public Guid FieldId { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public SportType Type { get; private set; }
    public bool IsActive { get; private set; }
    
    public Field(Guid fieldId, string name, Address address, SportType type)
    {
        if (fieldId == Guid.Empty)
            throw new ArgumentException("Field ID cannot be empty.", nameof(fieldId));
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Field name cannot be null or empty.", nameof(name));
        
        ArgumentNullException.ThrowIfNull(address);
        
        FieldId = fieldId;
        Name = name;
        Address = address;
        Type = type;
    }
    
    public void SetStatus(bool status) => IsActive = status;
}