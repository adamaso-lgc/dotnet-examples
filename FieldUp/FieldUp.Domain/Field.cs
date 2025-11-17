using FieldUp.Domain.ValueObjects;
using Microsoft.VisualBasic.FileIO;

namespace FieldUp.Domain.Entities;

public class Field
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public FieldType Type { get; private set; }
    public bool IsActive { get; private set; }
}